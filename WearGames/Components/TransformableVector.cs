using Android.Graphics;
using NoXP.Types;

namespace WearGames
{
    public class TransformableVector
    {
        private float[] _value = new float[2];
        private float[] _transformedValue = new float[2];

        public Vector2 Value
        { get => new Vector2(_transformedValue[0], _transformedValue[1]); }


        public TransformableVector(float x, float y)
        { this.Set(x, y); }
        public TransformableVector(TransformableVector vector) : this(vector._value[0], vector._value[1])
        { }
        public TransformableVector(Vector2 vector) : this(vector.X, vector.Y)
        { }


        public void Set(float x, float y)
        {
            _value[0] = _transformedValue[0] = x;
            _value[1] = _transformedValue[1] = y;
        }
        public void Set(Vector2 vector)
        { this.Set(vector.X, vector.Y); }



        public void TransformAsPoint(Matrix matrix)
        {
            matrix.MapPoints(_transformedValue, _value);
        }
        public void TransformAsVector(Matrix matrix)
        {
            matrix.MapVectors(_transformedValue, _value);
        }

    }

}