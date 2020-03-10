using Android.Graphics;
using NoXP.Types;

namespace WearGames.FX
{
    public class Particle
    {

        private Vector2 _position = Vector2.Zero;
        private Vector2 _speed = Vector2.Zero;
        private Bitmap _bitmap;

        public Particle(Vector2 position, Vector2 speed, Bitmap bitmap)
        {
            this._position = position;
            this._speed = speed;
            this._bitmap = bitmap;
        }


        public void UpdatePhysics(int distChange = 1)
        {
            _position -= distChange * _speed;
        }

        public void Draw(Canvas canvas)
        {
            if (_bitmap != null)
                canvas.DrawBitmap(_bitmap, _position.X, _position.Y, null);
        }

        public bool outOfSight()
        {
            return _position.Y <= -1 * _bitmap.Height
                 || _position.X <= -1 * _bitmap.Width;
        }
    }
}