using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace NoXP.Types
{
    [Serializable()]
    [StructLayout(LayoutKind.Explicit)]
    public struct Vector2
    {
        // constant for the default size in memory of the struct
        public const byte SizeInBytes = 8;

        #region Fields
        // public readonly static
        /// <summary>
        /// Shorthand for writing Vector2(0, 0)
        /// </summary>
        public readonly static Vector2 Zero = new Vector2(0, 0);
        /// <summary>
        /// Shorthand for writing Vector2(1, 1)
        /// </summary>
        public readonly static Vector2 One = new Vector2(1, 1);
        /// <summary>
        /// Shorthand for writing Vector2(0, 1)
        /// </summary>
        public readonly static Vector2 Up = new Vector2(0, 1);
        /// <summary>
        /// Shorthand for writing Vector2(0, -1)
        /// </summary>
        public readonly static Vector2 Down = new Vector2(0, -1);
        /// <summary>
        /// Shorthand for writing Vector2(1, 0)
        /// </summary>
        public readonly static Vector2 Right = new Vector2(1, 0);
        /// <summary>
        /// Shorthand for writing Vector2(-1, 0)
        /// </summary>
        public readonly static Vector2 Left = new Vector2(-1, 0);
        /// <summary>
        /// Shorthand for writing Vector2(float.MaxValue, float.MaxValue)
        /// </summary>
        public readonly static Vector2 Max = new Vector2(float.MaxValue, float.MaxValue);
        /// <summary>
        /// Shorthand for writing Vector2(float.MinValue, float.MinValue)
        /// </summary>
        public readonly static Vector2 Min = new Vector2(float.MinValue, float.MinValue);

        // provate
        // X or U
        [FieldOffset(0)]
        private float _x;

        // Y or V
        [FieldOffset(4)]
        private float _y;
        #endregion

        // vector property mapping (_x=X, _y=Y)
        #region Property - float - X
        /// <summary>
        /// gets or sets the X value
        /// </summary>
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        #endregion

        #region Property - float - Y
        /// <summary>
        /// gets or sets the Y value
        /// </summary>
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        #endregion

        // texture coordinate property mapping (_x=U, _y=V)
        #region Property - float - U
        /// <summary>
        /// gets or sets the U value
        /// </summary>
        public float U
        {
            get { return _x; }
            set { _x = value; }
        }
        #endregion

        #region Property - float - V
        /// <summary>
        /// gets or sets the V value
        /// </summary>
        public float V
        {
            get { return _y; }
            set { _y = value; }
        }
        #endregion


        #region Ctors

        #region Ctor(float, float)
        /// <summary>
        /// creates a new instance of Vector2
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        public Vector2(float x, float y)
        {
            _x = x;
            _y = y;
        }
        #endregion

        #region Ctor(double, double)
        /// <summary>
        /// creates a new instance of Vector2
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        public Vector2(double x, double y)
        {
            _x = (float)x;
            _y = (float)y;
        }
        #endregion

        #region Ctor(int, int)
        /// <summary>
        /// creates a new instance of Vector2
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        private Vector2(int x, int y)
        {
            _x = x;
            _y = y;
        }
        #endregion

        #region Ctor(byte, byte)
        /// <summary>
        /// creates a new instance of Vector2
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        private Vector2(byte x, byte y)
        {
            _x = x;
            _y = y;
        }
        #endregion

        #endregion


        public void Set(float x, float y)
        {
            _x = x;
            _y = y;
        }


        #region Public Static - Vector2[] - Convert(float[])
        /// <summary>
        /// Converts an array of float to an array of Vector2
        /// </summary>
        /// <param name="source">the source array of float</param>
        /// <returns>an array of Vector2 containing the sources data</returns>
        public static Vector2[] Convert(float[] source)
        {
            if (source.Length % 2 != 0)
                throw new ArgumentException("Can't convert source array to Vector2. Make sure you pass an array with a multiple of 2.", "source");
            Vector2[] result = new Vector2[source.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                int srcI = i * 2;
                result[i] = new Vector2(source[srcI], source[srcI + 1]);
            }
            return result;
        }
        #endregion


        public float LengthSquared()
        {
            return X * X + Y * Y;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        public void Normalize()
        {
            float length = this.Length();
            this /= length;
        }

        public Vector2 Normalized
        {
            get
            {
                float length = this.Length();
                return this / length;
            }
        }


        public static Vector2 Lerp(Vector2 lh, Vector2 rh, float blend)
        {
            return lh + (rh - lh) * blend;
        }

        public static Vector2 Cross(Vector2 lh)
        {
            return new Vector2(lh.Y, -lh.X);
        }
        public static float Cross(Vector2 lh, Vector2 rh)
        {
            return (lh.X * rh.Y) - (lh.Y * rh.X);

        }
        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="normal">The normal of the surface being reflected off.</param>
        /// <returns>The reflected vector.</returns>
        public static Vector2 Reflect(Vector2 vector, Vector2 normal)
        {
            float dot = Vector2.Dot(vector, normal);
            return vector - (2 * dot * normal);
        }

        public static float Dot(Vector2 lh, Vector2 rh)
        { return lh.X * rh.X + lh.Y * rh.Y; }



        public static bool IsPointOnLine(Vector2 p, Vector2 begin, Vector2 end, out float distToStart)
        {
            Vector2 direction = (end - begin).Normalized;
            float length = direction.Length();
            float absDist = (begin - p).Length();
            distToStart = absDist / length;

            if (absDist == 0.0f)
                return true;
            else if (absDist > length)
                return false;
            else
            {
                if (begin + direction * absDist == p)
                    return true;
            }

            return false;
        }

        public static Vector2 PointOnLine(Vector2 start, Vector2 end, float distance)
        {
            Vector2 dir = (end - start);
            return start + (dir * distance);
        }


        // complete this assembly to provide basic mathematic stuff (Cross, Dot, Normalize, PointOnLine, DistanceToPoint, etc.)


        #region Public Override - string - ToString()
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object. </returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:0.0#####} {1:0.0#####} ", new object[] { _x, _y });
        }
        #endregion

        #region Public Override - bool - Equals(object)
        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false. </returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            return this == ((Vector2)obj);
        }
        #endregion

        #region Public - bool - Equals(Vector2)
        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false. </returns>
        public bool Equals(Vector2 value)
        {
            if (this._x == value._x
                && this._y == value._y)
                return true;
            return false;
        }
        #endregion


        #region Operator - Equal
        /// <summary>
        /// Tests if the object on the left side of the operator is equal to object on the right side.
        /// </summary>
        /// <param name="lh">an object of type Vector2</param>
        /// <param name="rh">an object of type Vector2</param>
        /// <returns>true if the values of its operands are equal, false otherwise.</returns>
        public static bool operator ==(Vector2 lh, Vector2 rh)
        {
            if (lh._x != rh._x
                || lh._y != rh._y)
                return false;

            return true;
        }
        #endregion

        #region Operator - NotEqual
        /// <summary>
        /// Tests if the object on the left side of the operator is not equal to object on the right side.
        /// </summary>
        /// <param name="lh">an object of type Vector2</param>
        /// <param name="rh">an object of type Vector2</param>
        /// <returns>true if the values of its operands are not equal, false otherwise.</returns>
        public static bool operator !=(Vector2 lh, Vector2 rh)
        {
            if (lh._x != rh._x
                || lh._y != rh._y)
                return true;

            return false;
        }
        #endregion

        #region Operator - Greater
        /// <summary>
        /// checks if the left operand is greater than the right operand
        /// </summary>
        /// <param name="lh">value to check if it is greater than the right operand</param>
        /// <param name="rh">value to check against</param>
        /// <returns>true if left value is greater than the right operand</returns>
        public static bool operator >(Vector2 lh, Vector2 rh)
        {
            if ((lh._x > rh._x && lh._y >= rh._y)
                || (lh._x >= rh._x && lh._y > rh._y))
                return true;

            return false;
        }
        #endregion

        #region Operator - Lesser
        /// <summary>
        /// checks if the left operand is lesser than the right operand
        /// </summary>
        /// <param name="lh">value to check if it is lesser than the right operand</param>
        /// <param name="rh">value to check against</param>
        /// <returns>true if left value is lesser than the right operand</returns>
        public static bool operator <(Vector2 lh, Vector2 rh)
        {
            if ((lh._x < rh._x && lh._y <= rh._y)
                || (lh._x <= rh._x && lh._y < rh._y))
                return true;

            return false;
        }
        #endregion

        #region Operator - GreaterEqual
        /// <summary>
        /// checks if the left operand is greater or equals the right operand
        /// </summary>
        /// <param name="lh">value to check if it is greater or equals the right operand</param>
        /// <param name="rh">value to check against</param>
        /// <returns>true if left value is greater or equals the right operand</returns>
        public static bool operator >=(Vector2 lh, Vector2 rh)
        {
            if (lh._x >= rh._x && lh._y >= rh._y)
                return true;

            return false;
        }
        #endregion

        #region Operator - LesserEqual
        /// <summary>
        /// checks if the left operand is lesser or equals the right operand
        /// </summary>
        /// <param name="lh">value to check if it is lesser or equals the right operand</param>
        /// <param name="rh">value to check against</param>
        /// <returns>true if left value is lesser or equals the right operand</returns>
        public static bool operator <=(Vector2 lh, Vector2 rh)
        {
            if (lh._x <= rh._x && lh._y <= rh._y)
                return true;

            return false;
        }
        #endregion


        #region Mathematical Operator Overload
        // math operator overloading
        // multiply operator
        #region Operator - Vector2 - *(Vector2, float)
        /// <summary>
        /// Multiplies the left operands components by the right operand
        /// </summary>
        /// <param name="lh">an object of type Vector2</param>
        /// <param name="rh">an object of type float</param>
        /// <returns>a new Vector2 with the left operands components multiplied with the right operand</returns>
        public static Vector2 operator *(Vector2 lh, float rh)
        { return new Vector2(lh._x * rh, lh._y * rh); }
        public static Vector2 operator *(float lh, Vector2 rh)
        { return rh * lh; }
        #endregion
        // divide operator
        #region Operator - Vector2 - /(Vector2, float)
        /// <summary>
        /// Divides the left operands components by the right operand
        /// </summary>
        /// <param name="lh">an object of type Vector2</param>
        /// <param name="rh">an object of type float</param>
        /// <returns>a new Vector2 with the left operands components divided by the right operand</returns>
        public static Vector2 operator /(Vector2 lh, float rh)
        {
            float invRh = 1.0f / rh;
            return new Vector2(lh._x * invRh, lh._y * invRh);
        }
        #endregion
        // add operator
        #region Operator - Vector2 - +(Vector2, Vector2)
        /// <summary>
        /// Adds the right operand to the left operand
        /// </summary>
        /// <param name="lh">an object of type Vector2</param>
        /// <param name="rh">an object of type Vector2</param>
        /// <returns>a new Vector2 representing the sum of both operands</returns>
        public static Vector2 operator +(Vector2 lh, Vector2 rh)
        { return new Vector2(lh._x + rh._x, lh._y + rh._y); }
        #endregion
        // substract operator
        #region Operator - Vector2 - -(Vector2)
        /// <summary>
        /// Inverts the sign of the operands components
        /// </summary>
        /// <param name="val">an object of type Vector2</param>
        /// <returns>a new Vector2 representing the inverted operand</returns>
        public static Vector2 operator -(Vector2 val)
        { return new Vector2(-val._x, -val._y); }
        #endregion
        #region Operator - Vector2 - -(Vector2, Vector2)
        /// <summary>
        /// Substract the right operand to the left operand
        /// </summary>
        /// <param name="lh">an object of type Vector2</param>
        /// <param name="rh">an object of type Vector2</param>
        /// <returns>a new Vector2 representing the left operand minus the right operands</returns>
        public static Vector2 operator -(Vector2 lh, Vector2 rh)
        { return new Vector2(lh._x - rh._x, lh._y - rh._y); }
        #endregion
        #endregion


        #region Public Override - int - GetHashCode()
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return this._x.GetHashCode() + this._y.GetHashCode();
        }
        #endregion


        #region Public - float[] - ToArray()
        /// <summary>
        /// converts the instance to an array of float
        /// </summary>
        /// <returns>an array of float containing the instances components values</returns>
        public float[] ToArray()
        {
            return new float[] { this._x, this._y };
        }
        #endregion

        #region Public - int - CompareTo(Vector2)
        /// <summary>
        /// Compares this instance to a specified object and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the specified object
        /// </summary>
        /// <param name="other">An object to compare, or null</param>
        /// <returns>A signed number indicating the relative values of this instance and other.</returns>
        public int CompareTo(Vector2 other)
        {
            int cmpX = this._x.CompareTo(other._x);
            int cmpY = this._y.CompareTo(other._y);
            if (cmpX == 0)
            {
                if (cmpY == 0)
                    return 0;
                return cmpY;
            }
            return cmpX;
        }
        #endregion


        #region Public Static - float[] - ToFloatArray(Vector2[])
        /// <summary>
        /// converts an array of Vector2 to a sequential float array containing all position data (x0, y0, x1, y1, x2, y2, etc.)
        /// </summary>
        /// <param name="source">source array containing Vector2 elements</param>
        /// <returns>array containing a sequential list of the source's data</returns>
        public static float[] ToFloatArray(Vector2[] source)
        {
            float[] result = new float[source.Length * 2];

            for (int i = 0; i < source.Length; i++)
            {
                int resI = i * 2;
                result[resI] = source[i].X;
                result[resI + 1] = source[i].Y;
            }

            return result;
        }
        #endregion

        #region Public Static - float[] - ToFloatArray(IList<Vector2>)
        /// <summary>
        /// converts a IList of Vector2 to a sequential float array containing all position data (x0, y0, x1, y1, x2, y2, etc.)
        /// </summary>
        /// <param name="source">source IList containing Vector2 elements</param>
        /// <returns>array containing a sequential list of the source's data</returns>
        public static float[] ToFloatArray(IList<Vector2> source)
        {
            float[] result = new float[source.Count * 2];

            for (int i = 0; i < source.Count; i++)
            {
                int resI = i * 2;
                result[resI] = source[i].X;
                result[resI + 1] = source[i].Y;
            }

            return result;
        }
        #endregion


        #region Public Static - double[] - ToDoubleArray(Vector2[])
        /// <summary>
        /// converts an array of Vector2 to a sequential double array containing all position data (x0, y0, x1, y1, x2, y2, etc.)
        /// </summary>
        /// <param name="source">source array containing Vector2 elements</param>
        /// <returns>array containing a sequential list of the source's data</returns>
        public static double[] ToDoubleArray(Vector2[] source)
        {
            double[] result = new double[source.Length * 2];

            for (int i = 0; i < source.Length; i++)
            {
                int resI = i * 2;
                result[resI] = source[i].X;
                result[resI + 1] = source[i].Y;
            }

            return result;
        }
        #endregion

        #region Public Static - double[] - ToDoubleArray(IList<Vector2>)
        /// <summary>
        /// converts a IList of Vector2 to a sequential double array containing all position data (x0, y0, x1, y1, x2, y2, etc.)
        /// </summary>
        /// <param name="source">source IList containing Vector2 elements</param>
        /// <returns>array containing a sequential list of the source's data</returns>
        public static double[] ToDoubleArray(IList<Vector2> source)
        {
            double[] result = new double[source.Count * 2];

            for (int i = 0; i < source.Count; i++)
            {
                int resI = i * 2;
                result[resI] = source[i].X;
                result[resI + 1] = source[i].Y;
            }

            return result;
        }
        #endregion

    }
}
