using Android.Content;
using Android.Graphics;
using Android.Views;
using NoXP.Types;
using System;
using System.Collections.Generic;

namespace WearGames.FX
{
    public class ParticleSystem
    {
        private List<Particle> _particles = new List<Particle>();

        private Vector2 _xRange = Vector2.Zero;
        private Vector2 _yRange = Vector2.Zero;
        private Vector2 _xSpeedRange = new Vector2(-1, 1);
        private Vector2 _ySpeedRange = new Vector2(-1, 1);

        private Vector2 _scaleRange = new Vector2(1, 1);

        private Bitmap originalBitmap;


        public bool Enabled
        { get; set; }

        public Vector2 XRange
        { get => _xRange; set => _xRange = value; }
        public Vector2 YRange
        { get => _yRange; set => _yRange = value; }
        public Vector2 XSpeedRange
        { get => _xSpeedRange; set => _xSpeedRange = value; }
        public Vector2 YSpeedRange
        { get => _ySpeedRange; set => _ySpeedRange = value; }



        public ParticleSystem(Vector2 xRange, Vector2 yRange, Bitmap bitmap, int numParticles = 20)
        {
            this._xRange = xRange;
            this._yRange = yRange;

            //this.originalBitmap = bitmap;
            // ! ! ! !
            //  move this to the particle class to use per-particle scaled bitmaps
            float scale = GetRandomScale();
            if (scale != 1.0f)
                this.originalBitmap = Bitmap.CreateScaledBitmap(bitmap, (int)MathF.Ceiling(bitmap.Width * scale), (int)MathF.Ceiling(bitmap.Height * scale), true);

            // create all particles 
            for (int i = 0; i < numParticles; i++)
                _particles.Add(new Particle(GetRandomPosition(), GetRandomVelocity(), this.originalBitmap));
        }


        private Vector2 GetRandomPosition()
        { return new Vector2(GetRandomX(), GetRandomY()); }
        private float GetRandomX()
        { return GetRandomValue(_xRange); }
        private float GetRandomY()
        { return GetRandomValue(_yRange); }
        private Vector2 GetRandomVelocity()
        { return new Vector2(GetRandomXSpeed(), GetRandomYSpeed()); }
        private float GetRandomXSpeed()
        { return GetRandomValue(_xSpeedRange); }
        private float GetRandomYSpeed()
        { return GetRandomValue(_ySpeedRange); }
        private float GetRandomScale()
        { return GetRandomValue(_scaleRange); }

        private float GetRandomValue(Vector2 range)
        { return range.X + ((float)Java.Lang.Math.Random() * (range.Y - range.X)); }


        public void Draw(Canvas canvas)
        {
            if (this.Enabled)
                for (int i = 0; i < _particles.Count; i++)
                {
                    Particle particle = _particles[i];
                    if (particle != null)
                        particle.Draw(canvas);
                }
        }

        public void UpdatePhysics()
        {
            if (this.Enabled)
                for (int i = 0; i < _particles.Count; i++)
                {
                    Particle particle = _particles[i];
                    if (particle != null)
                    {
                        particle.UpdatePhysics();

                        // If this particle is completely out of sight
                        // replace it with a new one.

                        //if (particle.outOfSight())
                        //    _particles[i] = new Particle(GetRandomPosition(), GetRandomVelocity(), this.originalBitmap);
                    }
                }
        }
    }

    public class ParticleSystemView : View
    {
        private ParticleSystem _particleSystem;
        public ParticleSystem ParticleSystem
        { get => _particleSystem; }

        public ParticleSystemView(Context context, ParticleSystem ps) : base(context)
        {
            _particleSystem = ps;
            GameUpdateTask.LateUpdate += GameUpdateTask_LateUpdate;
        }

        private void GameUpdateTask_LateUpdate()
        {
            this.Invalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            // TODO Auto-generated method stub
            base.OnDraw(canvas);
            _particleSystem.Draw(canvas);
            _particleSystem.UpdatePhysics();
        }

        //public static ParticleSystemView Create(Context context, Vector2 xRange, Vector2 yRange, int resourceId)
        //{
        //    Bitmap particleBitmap = BitmapFactory.DecodeResource(context.Resources, Resource.Drawable.solid_8px_square);
        //    return Create(context, xRange, yRange, particleBitmap);
        //}
        public static ParticleSystemView Create(Context context, Vector2 xRange, Vector2 yRange, Bitmap particleBitmap)
        {
            return new ParticleSystemView(context, new ParticleSystem(xRange, yRange, particleBitmap, 20));
        }

    }


}