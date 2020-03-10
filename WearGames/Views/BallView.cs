using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Util;
using Android.Views;
using NoXP.Types;
using System;
using System.Collections.Generic;

namespace WearGames
{
    public class BallView : GameView
    {
        public static event Action BallDestroyed;

        public static List<BallView> Instances
        { get; private set; } = new List<BallView>();


        private static Paint DefaultPaint = new Paint(PaintFlags.AntiAlias) { Color = Color.White, StrokeWidth = 0 };
        private static ShapeDrawable DefaultDrawable = new ShapeDrawable(new OvalShape());
        public static Color DefaultColor
        {
            get => DefaultPaint.Color;
            set { DefaultPaint.Color = value; Instances.ForEach((x) => x.Invalidate()); }
        }



        private int _size = 10;
        private int _sizeHalf = 5;
        private Vector2 _direction = new Vector2(0.0f, -1.0f);
        private Vector2 _speed = new Vector2(5, 5);


        public int Size
        {
            get => _size;
            set { _size = (int)TypedValue.ApplyDimension(ComplexUnitType.Px, value, this.Context.Resources.DisplayMetrics); _sizeHalf = _size / 2; this.RefreshSize(); }
        }
        protected int HalfSize
        { get { return _sizeHalf; } }

        public Vector2 Direction
        { get => _direction; }

        public Vector2 Speed
        { get => _speed; }


        #region Ctors
        public BallView(Context context) : base(context)
        { }
        #endregion


        protected override void Setup()
        {
            base.Setup();

            DefaultPaint.SetStyle(Paint.Style.FillAndStroke);
            DefaultDrawable.Paint.Set(DefaultPaint);
            this.RefreshSize();
            this.IsInitialized = true;
        }

        protected override void OnDraw(Canvas canvas)
        {
            //base.OnDraw(canvas);
            if (DefaultDrawable != null)
                DefaultDrawable.Draw(canvas);
        }
        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            this.SetMeasuredDimension(this.Size, this.Size);
        }
        protected override void RefreshSize()
        {
            DefaultDrawable.SetBounds(0, 0, this.Size, this.Size);
            this.LayoutParameters.Width = this.Size;
            this.LayoutParameters.Height = this.Size;
        }

        protected override void Update()
        {
            this.Move();
        }
        protected void Move()
        {
            this.Translate(Direction.X * Speed.Y, Direction.Y * Speed.X);
        }

        protected override void LateUpdate()
        {
            // calculate center position only once per LateUpdate (pass it to methods in following commands
            Vector2 collisionNormal;
            Vector2 centerPosition = this.GetCenterPosition();
            float radius = this.HalfSize;

            Vector2 localPosInPaddleSpace = PaddleView.Instance.TransformToLocalSpace(centerPosition);
            if (PaddleView.Instance.CollisionBounds.IntersectBoundsWithCircle(localPosInPaddleSpace, radius, InsetModes.Grow))
            {
                collisionNormal = PaddleView.Instance.Up();
                _direction = Vector2.Reflect(_direction, collisionNormal);
                this.Move();
                return;
            }
            else
            {
                if (!ArenaView.Instance.IntersectBoundingCircles(centerPosition, radius, true))
                {
                    // Hint: this code is not required to run as the BallView instance is destroyed anyway
                    //collisionNormal = ArenaView.Instance.GetCollisionNormalForCenterOnUnitCircle(centerPosition);
                    //_direction = Vector2.Reflect(_direction, collisionNormal);
                    this.Destroy();
                    return;
                }
            }
            
            for (int i = BlockView.Instances.Count - 1; i >= 0; i--)
            {
                BlockView block = BlockView.Instances[i];
                if ((block as IBoundsProvider).IntersectBoundsWithCircle(centerPosition, radius, InsetModes.Grow, false))
                {
                    block.Hit();
                    // ! ! ! !
                    // somehow the reflected vector sometimes get f*cked up and makes the ball flip it's direction erroneous
                    //  ->  perhabs add up direction reflections and average them
                    _direction = Vector2.Reflect(_direction, -Extensions.LastIntersectionNormal);
                    this.Move();
                    return;
                }
            }

        }

        public override void Destroy(bool invokeEvents = true)
        {
            if (BallView.Instances.Contains(this))
                BallView.Instances.Remove(this);
            if (invokeEvents)
                BallView.OnBallDestroyed();
            base.Destroy(invokeEvents);
        }

        private static void OnBallDestroyed()
        {
            if (BallDestroyed != null)
                BallDestroyed.Invoke();
        }

        public static BallView Create(ViewGroup parent, float x, float y)
        {
            BallView ball = new BallView(parent.Context);
            parent.AddView(ball);
            ball.SetPosition(x - ball.HalfSize, y - ball.HalfSize);
            Instances.Add(ball);
            return ball;
        }

        public static void Clear()
        {
            // clear all remaining blocks if there are any
            for (int i = BallView.Instances.Count - 1; i >= 0; i--)
            {
                BallView view = BallView.Instances[i];
                if (view.Parent != null && view.Parent is ViewGroup)
                    (view.Parent as ViewGroup).RemoveView(view);
                BallView.Instances.RemoveAt(i);
                view.Destroy(false);
            }
            BallView.Instances.Clear();
        }
    }

}