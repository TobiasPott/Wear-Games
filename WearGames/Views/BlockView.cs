using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Util;
using Android.Views;
using NoXP.Types;
using System;
using System.Collections.Generic;
using WearGames.Breakout;

namespace WearGames
{

    public enum ColorMask
    {
        R = 1,
        G = 2,
        B = 4,
        //A = 8,
        All = R + G + B// + A
    }



    public class BlockView : GameView
    {
        public static event Action BlockDestroyed;

        public static List<BlockView> Instances
        { get; private set; } = new List<BlockView>();


        private static float[] Radii_x5 = new float[] { 5, 5, 5, 5, 5, 5, 5, 5 };
        private static float[] Radii_x6 = new float[] { 6, 6, 6, 6, 6, 6, 6, 6 };
        private static float[] Radii_x3 = new float[] { 3, 3, 3, 3, 3, 3, 3, 3 };

        private static RoundRectShape ShapeRR_x6 = new RoundRectShape(Radii_x6, null, null);
        private static RoundRectShape ShapeRR_x3 = new RoundRectShape(Radii_x3, null, null);
        private static RoundRectShape ShapeRR_x0 = new RoundRectShape(null, null, null);





        private byte _structure = 1;
        private byte _structureColorDecrement = 255;
        private Vector3Int _structureMaskModifier = new Vector3Int(-1, -1, -1);
        private ShapeDrawable _drawable = new ShapeDrawable();
        private Paint _paint = new Paint(PaintFlags.AntiAlias) { Color = Color.White, StrokeWidth = 0 };

        public Color Color
        {
            get => _paint.Color;
            set { _paint.Color = value; this.Invalidate(); }
        }

        public byte Structure
        {
            get => _structure;
            set
            {
                _structure = value;
                if (value > 0) _structureColorDecrement = (byte)(255 / value);
                else _structureColorDecrement = 255;
            }
        }
        public ColorMask ColorMask { get; set; } = ColorMask.All;
        public Vector3Int ColorMaskModifier
        {
            get => _structureMaskModifier;
            set { _structureMaskModifier = value; }
        }


        public int BlockWidth
        {
            get => this.LayoutParameters.Width;
            set { this.LayoutParameters.Width = value; this.RefreshSize(); }
        }
        public int BlockHeight
        {
            get => this.LayoutParameters.Height;
            set { this.LayoutParameters.Height = value; this.RefreshSize(); }
        }


        #region Ctors
        public BlockView(Context context) : base(context)
        { }
        #endregion


        protected override void Setup()
        {
            base.Setup();

            _paint.SetStyle(Paint.Style.FillAndStroke);
            _drawable.Paint.Set(_paint);
            this.RefreshSize();
            this.IsInitialized = true;
        }

        protected override void OnDraw(Canvas canvas)
        {
            if (_drawable != null)
                _drawable.Draw(canvas);
        }
        protected override void RefreshSize()
        {
            _drawable.SetBounds(0, 0, this.Width, this.Height);
        }

        public void Hit()
        {
            if (_structure > 0)
            {
                _structure--;

                this._paint.Color = this._paint.Color.ModifyColor(_structureColorDecrement, ColorMask, _structureMaskModifier);
                this._drawable.Paint.Set(_paint);

                // ! ! ! !
                // add changing corner radius with every structure point lost
                //  might randomize for the range of every 4 points lost
                this.Invalidate();
            }
            if (_structure == 0)
                this.Destroy();
        }


        public override void Destroy(bool invokeEvents = true)
        {
            if (BlockView.Instances.Contains(this))
                BlockView.Instances.Remove(this);
            // invoke 'BlockDestroyed' event
            if (invokeEvents)
                BlockView.OnBlockDestroyed();
            base.Destroy(invokeEvents);
        }

        private static void OnBlockDestroyed()
        {
            if (BlockDestroyed != null)
                BlockDestroyed.Invoke();
        }



        public static BlockView Create(ViewGroup parent, float x, float y, int width, int height, Color color)
        {
            BlockView ball = new BlockView(parent.Context);
            parent.AddView(ball);
            ball.SetPosition(x, y);
            ball.BlockWidth = width;
            ball.BlockHeight = height;
            ball.Color = color;
            Instances.Add(ball);
            return ball;
        }

        public static void Clear()
        {
            // clear all remaining blocks if there are any
            for (int i = BlockView.Instances.Count - 1; i >= 0; i--)
            {
                BlockView view = BlockView.Instances[i];
                if (view.Parent != null && view.Parent is ViewGroup)
                    (view.Parent as ViewGroup).RemoveView(view);
                BlockView.Instances.RemoveAt(i);
                view.Destroy(false);
            }
            BlockView.Instances.Clear();
        }


        // ! ! ! !
        // add a method to create block grid based on text, CSV or other file
        //  ->  should allow definition by numerical and easy other values 
        //      e.g. [5][#FFF], [2][#FFF] etc.
    }

}
