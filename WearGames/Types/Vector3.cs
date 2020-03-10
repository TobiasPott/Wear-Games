using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;

namespace NoXP.Types
{
    [Serializable()]
    [StructLayout(LayoutKind.Explicit)]
    public struct Vector3
    {
        // !!!!!!
        // !!!!!!
        // implement -operator and order classes to match the ClassFormat.cs template
        // !!!!!!
        // !!!!!!

        // constant for the default size in memory of the struct
        public const byte SizeInBytes = 12;

        #region Fields
        // public readonly static
        /// <summary>
        /// Shorthand for writing Float3(0, 0, 0)
        /// </summary>
        public readonly static Vector3 Zero = new Vector3(0, 0, 0);
        /// <summary>
        /// Shorthand for writing Float3(1, 1, 1)
        /// </summary>
        public readonly static Vector3 One = new Vector3(1, 1, 1);
        /// <summary>
        /// Shorthand for writing Float3(0, 0, 1)
        /// </summary>
        public readonly static Vector3 Front = new Vector3(0, 0, 1);
        /// <summary>
        /// Shorthand for writing Float3(0, 0, -1)
        /// </summary>
        public readonly static Vector3 Back = new Vector3(0, 0, -1);
        /// <summary>
        /// Shorthand for writing Float3(0, 1, 0)
        /// </summary>
        public readonly static Vector3 Up = new Vector3(0, 1, 0);
        /// <summary>
        /// Shorthand for writing Float3(0, -1, 0)
        /// </summary>
        public readonly static Vector3 Down = new Vector3(0, -1, 0);
        /// <summary>
        /// Shorthand for writing Float3(1, 0, 0)
        /// </summary>
        public readonly static Vector3 Right = new Vector3(1, 0, 0);
        /// <summary>
        /// Shorthand for writing Float3(-1, 0, 0)
        /// </summary>
        public readonly static Vector3 Left = new Vector3(-1, 0, 0);
        /// <summary>
        /// Shorthand for writing Float3(float.MaxValue, float.MaxValue, float.MaxValue)
        /// </summary>
        public readonly static Vector3 Max = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        /// <summary>
        /// Shorthand for writing Float3(float.MinValue, float.MinValue, float.MinValue)
        /// </summary>
        public readonly static Vector3 Min = new Vector3(float.MinValue, float.MinValue, float.MinValue);

        // private
        // X
        [FieldOffset(0)]
        private float _x;
        // Y
        [FieldOffset(4)]
        private float _y;
        // Z
        [FieldOffset(8)]
        private float _z;
        #endregion

        // vector property mapping (_x=X, _y=Y, _z=Z)
        #region Property - float - X
        /// <summary>
        /// gets or sets the X value (equivalent with R)
        /// </summary>
        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        #endregion

        #region Property - float - Y
        /// <summary>
        /// gets or sets the Y value (equivalent with G)
        /// </summary>
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        #endregion

        #region Property - float - Z
        /// <summary>
        /// gets or sets the Z value (equivalent with B)
        /// </summary>
        public float Z
        {
            get { return _z; }
            set { _z = value; }
        }
        #endregion


        // color property mapping (_x=R, _y=G, _z=B)        
        #region Property - float - R
        /// <summary>
        /// gets or sets the R value (equivalent with X)
        /// </summary>
        public float R
        {
            get { return _x; }
            set { _x = value; }
        }
        #endregion

        #region Property - float - G
        /// <summary>
        /// gets or sets the G value (equivalent with Y)
        /// </summary>
        public float G
        {
            get { return _y; }
            set { _y = value; }
        }
        #endregion

        #region Property - float - B
        /// <summary>
        /// gets or sets the B value (equivalent with Z)
        /// </summary>
        public float B
        {
            get { return _z; }
            set { _z = value; }
        }
        #endregion


        #region Ctors

        #region Ctor(float, float, float)
        /// <summary>
        /// creates a new instance of Float3
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        /// <param name="z">value of the z component</param>
        public Vector3(float x, float y, float z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        #endregion

        #region Ctor(double, double, double)
        /// <summary>
        /// creates a new instance of Float3
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        /// <param name="z">value of the z component</param>
        public Vector3(double x, double y, double z)
        {
            _x = (float)x;
            _y = (float)y;
            _z = (float)z;
        }
        #endregion

        #region Ctor(int, int, int)
        /// <summary>
        /// creates a new instance of Float3
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        /// <param name="z">value of the z component</param>
        public Vector3(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        #endregion

        #region Ctor(byte, byte, byte)
        /// <summary>
        /// creates a new instance of Float3
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        /// <param name="z">value of the z component</param>
        public Vector3(byte x, byte y, byte z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        #endregion

        #endregion


        public void Set(float x, float y, float z)
        {
            this._x = x;
            this._y = y;
            this._z = z;
        }


        #region Public Static - Float3[] - Convert(float[])
        /// <summary>
        /// Converts an array of float to an array of Float3
        /// </summary>
        /// <param name="source">the source array of float</param>
        /// <returns>an array of Float3 containing the sources data</returns>
        public static Vector3[] Convert(float[] source)
        {
            if (source.Length % 3 != 0)
                throw new ArgumentException("Can't convert source array to Float3. Make sure you pass an array with a multiple of 3.", "source");

            Vector3[] result = new Vector3[source.Length / 3];
            for (int i = 0; i < result.Length; i++)
            {
                int srcI = i * 3;
                result[i] = new Vector3(source[srcI], source[srcI + 1], source[srcI + 2]);
            }
            return result;
        }
        #endregion


        public float LengthSquared()
        {
            return X * X + Y * Y + Z * Z;
        }

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        public void Normalize()
        {
            float length = this.Length();
            this /= length;
        }

        public Vector3 Normalized
        {
            get
            {
                float length = this.Length();
                return this / length;
            }
        }

        public static Vector3 Lerp(Vector3 lh, Vector3 rh, float blend)
        {
            return lh + (rh - lh) * blend;
        }

        public static Vector3 Cross(Vector3 lh, Vector3 rh)
        {
            return new Vector3(lh.Y * rh.Z - lh.Z * rh.Y,
                                lh.Z * rh.X - lh.X * rh.Z,
                                lh.X * rh.Y - lh.Y * rh.X);
        }

        public static float Dot(Vector3 lh, Vector3 rh)
        { return lh.X * rh.X + lh.Y * rh.Y + lh.Z * rh.Z; }


        public static bool IsPointOnLine(Vector3 p, Vector3 start, Vector3 end, out float distance)
        {
            float length = (end - start).Length();
            Vector3 direction = (end - start).Normalized;
            float absoluteDistance = (start - p).Length();
            distance = absoluteDistance / length;

            if (absoluteDistance == 0.0f)
                return true;
            else if (absoluteDistance > length)
                return false;
            else
            {
                if (start + direction * absoluteDistance == p)
                    return true;
            }

            return false;
        }


        public static Vector3 PointOnLine(Vector3 start, Vector3 end, float distance)
        {
            Vector3 dir = (end - start);
            return start + (dir * distance);
        }


        #region Public Override - string - ToString()
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object. </returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:0.0#####} {1:0.0#####} {2:0.0#####} ", new object[] { _x, _y, _z });
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
            return this == ((Vector3)obj);
        }
        #endregion

        #region Public - bool - Equals(Float3)
        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false. </returns>
        public bool Equals(Vector3 value)
        {
            if (this._x != value._x
                || this._y != value._y
                || this._z != value._z)
                return false;
            return true;
        }
        #endregion


        #region Operator - bool - ==(Float3, Float3)
        /// <summary>
        /// Tests if the object on the left side of the operator is equal to object on the right side.
        /// </summary>
        /// <param name="lh">an object of type Float3</param>
        /// <param name="rh">an object of type Float3</param>
        /// <returns>true if the values of its operands are equal, false otherwise.</returns>
        public static bool operator ==(Vector3 lh, Vector3 rh)
        {
            if (lh._x != rh._x
                || lh._y != rh._y
                || lh._z != rh._z)
                return false;

            return true;
        }
        #endregion

        #region Operator - bool - !=(Float3, Float3)
        /// <summary>
        /// Tests if the object on the left side of the operator is not equal to object on the right side.
        /// </summary>
        /// <param name="lh">an object of type Float3</param>
        /// <param name="rh">an object of type Float3</param>
        /// <returns>true if the values of its operands are not equal, false otherwise.</returns>
        public static bool operator !=(Vector3 lh, Vector3 rh)
        {
            if (lh._x != rh._x
                || lh._y != rh._y
                || lh._z != rh._z)
                return true;
            return false;
        }
        #endregion

        #region Operator - bool - > (Float3, Float3)
        /// <summary>
        /// checks if the left operand is greater than the right operand
        /// </summary>
        /// <param name="lh">value to check if it is greater than the right operand</param>
        /// <param name="rh">value to check against</param>
        /// <returns>true if left value is greater than the right operand</returns>
        public static bool operator >(Vector3 lh, Vector3 rh)
        {
            if ((lh._x > rh._x && lh._y >= rh._y && lh._z >= rh._z)
                || (lh._y > rh._y && lh._x >= rh._x && lh._z >= rh._z)
                || (lh._z > rh._z && lh._x >= rh._x && lh._y >= rh._y))
                return true;

            return false;
        }
        #endregion

        #region Operator - bool - < (Float3, Float3)
        /// <summary>
        /// checks if the left operand is lesser than the right operand
        /// </summary>
        /// <param name="lh">value to check if it is lesser than the right operand</param>
        /// <param name="rh">value to check against</param>
        /// <returns>true if left value is lesser than the right operand</returns>
        public static bool operator <(Vector3 lh, Vector3 rh)
        {
            if ((lh._x < rh._x && lh._y <= rh._y && lh._z <= rh._z)
                || (lh._y < rh._y && lh._x <= rh._x && lh._z <= rh._z)
                || (lh._z < rh._z && lh._x <= rh._x && lh._y <= rh._y))
                return true;

            return false;
        }
        #endregion

        #region Operator - bool - >= (Float3, Float3)
        /// <summary>
        /// checks if the left operand is greater or equals the right operand
        /// </summary>
        /// <param name="lh">value to check if it is greater or equals the right operand</param>
        /// <param name="rh">value to check against</param>
        /// <returns>true if left value is greater or equals the right operand</returns>
        public static bool operator >=(Vector3 lh, Vector3 rh)
        {
            if (lh._x >= rh._x && lh._y >= rh._y && lh._z >= rh._z)
                return true;

            return false;
        }
        #endregion

        #region Operator - bool - <= (Float3, Float3)
        /// <summary>
        /// checks if the left operand is lesser or equals the right operand
        /// </summary>
        /// <param name="lh">value to check if it is lesser or equals the right operand</param>
        /// <param name="rh">value to check against</param>
        /// <returns>true if left value is lesser or equals the right operand</returns>
        public static bool operator <=(Vector3 lh, Vector3 rh)
        {
            if (lh._x <= rh._x && lh._y <= rh._y && lh._z <= rh._z)
                return true;

            return false;
        }
        #endregion


        #region Mathematical Operator Overload
        // math operator overloading
        // multiply operator
        #region Operator - Float3 - *(Float3, float)
        /// <summary>
        /// Multiplies the left operands components by the right operand
        /// </summary>
        /// <param name="lh">an object of type Float3</param>
        /// <param name="rh">an object of type float</param>
        /// <returns>a new Float3 with the left operands components multiplied with the right operand</returns>
        public static Vector3 operator *(Vector3 lh, float rh)
        { return new Vector3(lh._x * rh, lh._y * rh, lh._z * rh); }
        public static Vector3 operator *(float lh, Vector3 rh)
        { return rh * lh; }
        #endregion
        // divide operator        
        #region Operator - Float3 - /(Float3, float)
        /// <summary>
        /// Divides the left operands components by the right operand
        /// </summary>
        /// <param name="lh">an object of type Float3</param>
        /// <param name="rh">an object of type float</param>
        /// <returns>a new Float3 with the left operands components divided by the right operand</returns>
        public static Vector3 operator /(Vector3 lh, float rh)
        {
            float invRh = 1.0f / rh;
            return new Vector3(lh._x * invRh, lh._y * invRh, lh._z * invRh);
        }
        #endregion
        // add operator
        #region Operator - Float3 - +(Float3, Float3)
        /// <summary>
        /// Adds the right operand to the left operand
        /// </summary>
        /// <param name="lh">an object of type Float3</param>
        /// <param name="rh">an object of type Float3</param>
        /// <returns>a new Float3 representing the sum of both operands</returns>
        public static Vector3 operator +(Vector3 lh, Vector3 rh)
        { return new Vector3(lh._x + rh._x, lh._y + rh._y, lh._z + rh._z); }
        #endregion
        // substract operator
        #region Operator - Float3 - -(Float3)
        /// <summary>
        /// Inverts the sign of the operands components
        /// </summary>
        /// <param name="val">an object of type Float3</param>
        /// <returns>a new Float3 representing the inverted operand</returns>
        public static Vector3 operator -(Vector3 val)
        { return new Vector3(-val._x, -val._y, -val._z); }
        #endregion
        #region Operator - Float3 - -(Float3, Float3)
        /// <summary>
        /// Substract the right operand to the left operand
        /// </summary>
        /// <param name="lh">an object of type Float3</param>
        /// <param name="rh">an object of type Float3</param>
        /// <returns>a new Float3 representing the left operand minus the right operands</returns>
        public static Vector3 operator -(Vector3 lh, Vector3 rh)
        { return new Vector3(lh._x - rh._x, lh._y - rh._y, lh._z - rh._z); }
        #endregion
        #endregion



        #region Public Override - int - GetHashCode()
        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            return this._x.GetHashCode() + this._y.GetHashCode() + this._z.GetHashCode();
        }
        #endregion


        #region Public - float[] - ToArray()
        /// <summary>
        /// converts the instance to an array of float
        /// </summary>
        /// <returns>an array of float containing the instances components values</returns>
        public float[] ToArray()
        {
            return new float[] { this._x, this._y, this._z };
        }
        #endregion

        #region Public - int - CompareTo(Float3)
        /// <summary>
        /// Compares this instance to a specified object and returns an integer that indicates whether the value of this instance is less than, equal to, or greater than the value of the specified object
        /// </summary>
        /// <param name="other">An object to compare, or null</param>
        /// <returns>A signed number indicating the relative values of this instance and other.</returns>
        public int CompareTo(Vector3 other)
        {
            int cmpX = this._x.CompareTo(other._x);
            int cmpY = this._y.CompareTo(other._y);
            int cmpZ = this._z.CompareTo(other._z);
            if (cmpX == 0)
            {
                if (cmpY == 0)
                {
                    if (cmpZ == 0)
                        return 0;
                    return cmpZ;
                }
                return cmpY;
            }
            return cmpX;
        }
        #endregion


        #region Public Static - float[] - ToFloatArray(Float3[])
        /// <summary>
        /// converts an array of Float3 to a sequential float array containing all position data (x0, y0, z0, x1, y1, z1, x2, y2, etc.)
        /// </summary>
        /// <param name="source">source array containing Float3 elements</param>
        /// <returns>array containing a sequential list of the source's data</returns>
        public static float[] ToFloatArray(Vector3[] source)
        {
            float[] result = new float[source.Length * 3];

            for (int i = 0; i < source.Length; i++)
            {
                int resI = i * 3;
                result[resI] = source[i].X;
                result[resI + 1] = source[i].Y;
                result[resI + 2] = source[i].Z;
            }

            return result;
        }
        #endregion

        #region Public Static - float[] - ToFloatArray(List<Float3>)
        /// <summary>
        /// converts a IList of Float3 to a sequential float array containing all position data (x0, y0, z0, x1, y1, z1, x2, y2, etc.)
        /// </summary>
        /// <param name="source">source IList containing Float3 elements</param>
        /// <returns>array containing a sequential list of the source's data</returns>
        public static float[] ToFloatArray(IList<Vector3> source)
        {
            float[] result = new float[source.Count * 3];

            for (int i = 0; i < source.Count; i++)
            {
                int resI = i * 3;
                result[resI] = source[i].X;
                result[resI + 1] = source[i].Y;
                result[resI + 2] = source[i].Z;
            }

            return result;
        }
        #endregion


        #region Public Static - double[] - ToDoubleArray(Float3[])
        /// <summary>
        /// converts an array of Float3 to a sequential double array containing all position data (x0, y0, z0, x1, y1, z1, x2, y2, etc.)
        /// </summary>
        /// <param name="source">source array containing Float3 elements</param>
        /// <returns>array containing a sequential list of the source's data</returns>
        public static double[] ToDoubleArray(Vector3[] source)
        {
            double[] result = new double[source.Length * 3];

            for (int i = 0; i < source.Length; i++)
            {
                int resI = i * 3;
                result[resI] = source[i].X;
                result[resI + 1] = source[i].Y;
                result[resI + 2] = source[i].Z;
            }

            return result;
        }
        #endregion

        #region Public Static - double[] - ToDoubleArray(IList<Float3>)
        /// <summary>
        /// converts a IList of Float3 to a sequential double array containing all position data (x0, y0, z0, x1, y1, z1, x2, y2, etc.)
        /// </summary>
        /// <param name="source">source IList containing Float3 elements</param>
        /// <returns>array containing a sequential list of the source's data</returns>
        public static double[] ToDoubleArray(IList<Vector3> source)
        {
            double[] result = new double[source.Count * 3];

            for (int i = 0; i < source.Count; i++)
            {
                int resI = i * 3;
                result[resI] = source[i].X;
                result[resI + 1] = source[i].Y;
                result[resI + 2] = source[i].Z;
            }

            return result;
        }
        #endregion

    }
}
