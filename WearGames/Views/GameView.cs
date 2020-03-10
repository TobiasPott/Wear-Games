using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;
using System;

namespace WearGames
{

    public interface IBoundsProvider
    {
        RectF Bounds { get; }
    }

    public abstract class InitViewBase : View
    {
        protected bool IsInitialized { get; set; }


        public override float Rotation
        {
            get { return base.Rotation; }
            set { base.Rotation = value % 360.0f; }
        }


        #region Ctors
        public InitViewBase(Context context) :
            base(context)
        { }
        public InitViewBase(Context context, IAttributeSet attrs) :
            base(context, attrs)
        { }
        public InitViewBase(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        { }
        #endregion

        protected virtual void Initialize()
        {
            if (!IsInitialized)
            {
                this.Setup();
                this.IsInitialized = true;
            }
        }
        protected virtual void Setup()
        { }

        public virtual void SetPosition(float x, float y)
        {
            this.SetX(x);
            this.SetY(y);
        }
        public virtual void Translate(float x, float y)
        {
            if (x != 0.0f)
                this.SetX(this.GetX() + x);
            if (y != 0.0f)
                this.SetY(this.GetY() + y);
        }

    }

    public abstract class StaticView : InitViewBase, IBoundsProvider
    {
        protected RectF _bounds = new RectF();

        public RectF Bounds
        { get => _bounds; }

        public override ViewGroup.LayoutParams LayoutParameters
        {
            get { return base.LayoutParameters; }

            set
            {
                base.LayoutParameters = value;
                _bounds.Right = _bounds.Left + value.Width;
                _bounds.Bottom = _bounds.Top + value.Height;
            }
        }

        #region Ctors
        public StaticView(Context context) :
            base(context)
        { }
        public StaticView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        { }
        public StaticView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        { }
        #endregion


        public override void SetX(float x)
        {
            base.SetX(x);
            _bounds.Left = x;
        }
        public override void SetY(float y)
        {
            base.SetY(y);
            _bounds.Top = y;
        }

        protected abstract void RefreshSize();
        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            if (!this.IsInitialized)
                this.Initialize();
            _bounds.Set(this.GetX(), this.GetY(), this.GetX() + this.Width, this.GetY() + this.Height);
            this.RefreshSize();
        }

    }


    public abstract class GameView : StaticView
    {

        #region Ctors
        public GameView(Context context) :
            base(context)
        { }
        public GameView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        { }
        public GameView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        { }
        #endregion

        protected override void Setup()
        {
            this.AttachEvents();
        }

        private void AttachEvents()
        {
            GameUpdateTask.EarlyUpdate += EarlyUpdate;
            GameUpdateTask.Update += Update;
            GameUpdateTask.LateUpdate += LateUpdate;
        }
        private void DetachEvents()
        {
            GameUpdateTask.EarlyUpdate -= EarlyUpdate;
            GameUpdateTask.Update -= Update;
            GameUpdateTask.LateUpdate -= LateUpdate;
        }

        protected virtual void EarlyUpdate()
        { }
        protected virtual void Update()
        { }
        protected virtual void LateUpdate()
        { }

        public virtual void Destroy(bool invokeEvents = true)
        {
            this.Visibility = ViewStates.Gone;
            if (this.Parent != null && this.Parent is ViewGroup)
                (this.Parent as ViewGroup).RemoveView(this);
            this.DetachEvents();
        }

    }

}