using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Support.Wearable.Input;
using Android.Util;
using Android.Views;
using NoXP.Types;
using System;

namespace WearGames
{

    public class PaddleView : GameView
    {
        public static PaddleView Instance { get; protected set; }

        private ShapeDrawable _drawableArc = new ShapeDrawable();
        private Paint _paintArc = new Paint(PaintFlags.AntiAlias) { Color = Color.White, StrokeCap = Paint.Cap.Round };

        private ShapeDrawable _drawableRect = new ShapeDrawable();
        private Paint _paintRect = new Paint() { Color = Color.White };

        private ShapeDrawable _drawableDebug = new ShapeDrawable();
        private Paint _paintDebug = new Paint() { Color = Color.Red, StrokeWidth = 1.0f };

        public static float AngleLimit
        { get; set; } = 361;
        public static float RotationLimit
        { get; set; } = 15.0f;

        public RectF CollisionBounds { get; } = new RectF();

        public bool DrawDebug
        { get; set; } = false;


        public float Delta
        { get; set; }


        #region Ctors
        public PaddleView(Context context) : base(context)
        { }
        public PaddleView(Context context, IAttributeSet attrs) : base(context, attrs)
        { }
        public PaddleView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        { }
        #endregion


        protected override void Setup()
        {
            base.Setup();

            if (Instance == null)
                Instance = this;

            this.KeyPress += PongPaddleView_KeyPress;
            int shapeSize = Math.Min(this.Width, this.Height);
            int shapeSizeThird = shapeSize / 3;
            int strokeWidth = 10;
            int strokeWidthHalf = strokeWidth / 2;

            // setup details on paint used for arc
            _paintArc.SetStyle(Paint.Style.Stroke);
            _paintArc.StrokeWidth = strokeWidth;
            // setup details on paint used for rect
            _paintRect.SetStyle(Paint.Style.Fill);
            // setup details on paint used for rect
            _paintDebug.SetStyle(Paint.Style.Stroke);


            Path pathArc = new Path();
            // add the arc slightly smaller then the drawable size will be to prevent clipping
            pathArc.AddArc(0, 0, shapeSize - strokeWidthHalf, shapeSize - strokeWidthHalf, 75, 30);
            _drawableArc.Shape = new PathShape(pathArc, shapeSize, shapeSize);
            _drawableArc.Paint.Set(_paintArc);

            // set collision bounds
            pathArc.ComputeBounds(CollisionBounds, true);
            CollisionBounds.Inset(-strokeWidthHalf, -strokeWidthHalf);

            RectF arcBounds = new RectF();
            pathArc.ComputeBounds(arcBounds, true);
            arcBounds.Top -= strokeWidthHalf;
            arcBounds.Bottom -= strokeWidthHalf;

            Path pathRect = new Path();
            pathRect.AddRect(arcBounds, Path.Direction.Cw);
            _drawableRect.Shape = new PathShape(pathRect, shapeSize, shapeSize);
            _drawableRect.Paint.Set(_paintRect);

            // setup debug drawable
            pathArc.ComputeBounds(arcBounds, true);
            arcBounds.Inset(-strokeWidthHalf, -strokeWidthHalf);
            arcBounds.Bottom -= strokeWidthHalf;
            Path pathDebug = new Path();
            pathDebug.AddRect(arcBounds, Path.Direction.Cw);
            _drawableDebug.Shape = new PathShape(pathDebug, shapeSize, shapeSize);
            _drawableDebug.Paint.Set(_paintDebug);
        }

        protected override void EarlyUpdate()
        {
            this.ApplyRotation(this.Delta);
            this.Delta = 0.0f;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            _drawableRect.Draw(canvas);
            _drawableArc.Draw(canvas);
            if (this.DrawDebug)
                _drawableDebug.Draw(canvas);
        }

        protected override void RefreshSize()
        {
            int widthHalf = this.Width / 2;
            int heightHalf = this.Height / 2;
            int halfSize = Math.Max(this.Width, this.Height) / 2;
            Rect bounds = new Rect(widthHalf - halfSize, heightHalf - halfSize,
                            widthHalf + halfSize, heightHalf + halfSize);

            _drawableArc.SetBounds(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
            _drawableRect.SetBounds(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
            _drawableDebug.SetBounds(bounds.Left, bounds.Top, bounds.Right, bounds.Bottom);
        }

        private void PongPaddleView_KeyPress(object sender, KeyEventArgs e)
        {
            // ! ! ! !
            // move this to the "Update" method and ponder about a way to also do so with the rotary dial input

            if (e.KeyCode == Keycode.A)
                this.ApplyRotation(5);
            if (e.KeyCode == Keycode.D)
                this.ApplyRotation(-5);
        }

        public override bool OnGenericMotionEvent(MotionEvent ev)
        {
            if (ev.Action == MotionEventActions.Scroll
                && RotaryEncoder.IsFromRotaryEncoder(ev))
            {
                float delta = -RotaryEncoder.GetRotaryAxisValue(ev) * RotaryEncoder.GetScaledScrollFactor(this.Context);
                this.Delta = Math.Clamp(delta, -PaddleView.RotationLimit, PaddleView.RotationLimit);
                return true;
            }
            return base.OnGenericMotionEvent(ev);
        }

        private void ApplyRotation(float amount)
        {
            this.Rotation = Math.Clamp(this.Rotation + amount, -PaddleView.AngleLimit, PaddleView.AngleLimit);
        }

    }

}