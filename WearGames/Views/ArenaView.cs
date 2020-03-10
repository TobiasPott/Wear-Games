using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Util;
using NoXP.Types;
using System;

namespace WearGames
{
    public class ArenaView : GameView
    {

        public static ArenaView Instance { get; protected set; }

        private ShapeDrawable _drawableBorder = new ShapeDrawable();
        private Paint _paintBorder = new Paint() { Color = Color.White, StrokeWidth = 4, Alpha = 64 };

        private Vector2 _arenaCenter = new Vector2(0, 0);
        private float _arenaRadius = 1;
        private float _arenaRadiusSqr = 1;


        #region Ctors
        public ArenaView(Context context) : base(context)
        { }
        public ArenaView(Context context, IAttributeSet attrs) : base(context, attrs)
        { }
        public ArenaView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        { }
        #endregion


        protected override void Setup()
        {
            base.Setup();

            if (Instance == null)
                Instance = this;
            // add the arc slightly smaller then the drawable size will be to prevent clipping
            _paintBorder.SetStyle(Paint.Style.Stroke);
            _drawableBorder.Shape = new OvalShape();
            _drawableBorder.Paint.Set(_paintBorder);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            _drawableBorder.Draw(canvas);
        }

        protected override void RefreshSize()
        {
            _arenaCenter.X = this.Width / 2;
            _arenaCenter.Y = this.Height / 2;
            _arenaRadius = Math.Min(this.Width, this.Height) / 2;
            _arenaRadiusSqr = _arenaRadius * _arenaRadius;

            _drawableBorder.SetBounds(0, 0, this.Width, this.Height);
        }

    }

}