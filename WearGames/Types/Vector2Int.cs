using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NoXP.Types
{
    [Serializable()]
    [StructLayout(LayoutKind.Explicit)]
    public struct Vector2Int
    {
        // constant for the default size in memory of the struct
        public const byte SizeInBytes = 8;

        #region Fields
        // public readonly static
        /// <summary>
        /// Shorthand for writing Int2(0, 0)
        /// </summary>
        public readonly static Vector2Int Zero = new Vector2Int(0, 0);
        /// <summary>
        /// Shorthand for writing Int2(1, 1)
        /// </summary>
        public readonly static Vector2Int One = new Vector2Int(1, 1);
        /// <summary>
        /// Shorthand for writing Int2(0, 1)
        /// </summary>
        public readonly static Vector2Int Up = new Vector2Int(0, 1);
        /// <summary>
        /// Shorthand for writing Int2(0, -1)
        /// </summary>
        public readonly static Vector2Int Down = new Vector2Int(0, -1);
        /// <summary>
        /// Shorthand for writing Int2(1, 0)
        /// </summary>
        public readonly static Vector2Int Right = new Vector2Int(1, 0);
        /// <summary>
        /// Shorthand for writing Int2(-1, 0)
        /// </summary>
        public readonly static Vector2Int Left = new Vector2Int(-1, 0);
        /// <summary>
        /// Shorthand for writing Int2(int.MaxValue, int.MaxValue)
        /// </summary>
        public readonly static Vector2Int Max = new Vector2Int(int.MaxValue, int.MaxValue);
        /// <summary>
        /// Shorthand for writing Int2(int.MinValue, int.MinValue)
        /// </summary>
        public readonly static Vector2Int Min = new Vector2Int(int.MinValue, int.MinValue);

        // private
        // X or U
        [FieldOffset(0)]
        private int _x;

        // Y or V
        [FieldOffset(4)]
        private int _y;

        #endregion

        // vector property mapping (_x=X, _y=Y)
        #region Property - int - X
        /// <summary>
        /// gets or sets the X value
        /// </summary>
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }
        #endregion

        #region Property - int - Y
        /// <summary>
        /// gets or sets the Y value
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }
        #endregion


        #region Ctors

        #region Ctor(double, double)
        /// <summary>
        /// creates a new instance of Int2
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        public Vector2Int(double x, double y)
        {
            _x = (int)x;
            _y = (int)y;
        }
        #endregion

        #region Ctor(float, float)
        /// <summary>
        /// creates a new instance of Int2
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        public Vector2Int(float x, float y)
        {
            _x = (int)x;
            _y = (int)y;
        }
        #endregion

        #region Ctor(int, int)
        /// <summary>
        /// creates a new instance of Int2
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        public Vector2Int(int x, int y)
        {
            _x = x;
            _y = y;
        }
        #endregion

        #region Ctor(byte, byte)
        /// <summary>
        /// creates a new instance of Int2
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        private Vector2Int(byte x, byte y)
        {
            _x = x;
            _y = y;
        }
        #endregion

        #endregion


        public void Set(int x, int y)
        {
            _x = x;
            _y = y;
        }


        #region Public Static - Vector2Int[] - Convert(int[])
        /// <summary>
        /// Converts an array of int to an array of Int2
        /// </summary>
        /// <param name="source">the source array of int</param>
        /// <returns>an array of Int2 containing the sources data</returns>
        public static Vector2Int[] Convert(int[] source)
        {
            if (source.Length % 2 != 0)
                throw new ArgumentException("Can't convert source array to Int2. Make sure you pass an array with a multiple of 2.", "source");

            Vector2Int[] result = new Vector2Int[source.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                int srcI = i * 2;
                result[i] = new Vector2Int(source[srcI], source[srcI + 1]);
            }
            return result;
        }
        #endregion


        #region Public Override - string - ToString()
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object. </returns>
        public override string ToString()
        {
            return String.Format("{0:0} {1:0} ", new object[] { _x, _y });
        }
        #endregion


        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            return this == ((Vector2Int)obj);
        }


        public bool Equals(Vector2Int value)
        {
            if (this._x == value._x
                && this._y == value._y)
                return true;
            return false;
        }

        public static bool operator ==(Vector2Int lh, Vector2Int rh)
        {
            if (lh._x != rh._x
                || lh._y != rh._y)
                return false;

            return true;
        }

        public static bool operator !=(Vector2Int lh, Vector2Int rh)
        {
            if (lh._x != rh._x
                || lh._y != rh._y)
                return true;

            return false;
        }

        // add greater/lesser operators to all other structs (Int3, Int4, Vector2, Float3, etc.)
        #region Operator - Greater
        /// <summary>
        /// checks if the left operand is greater than the right operand
        /// </summary>
        /// <param name="lh">value to check if it is greater than the right operand</param>
        /// <param name="rh">value to check against</param>
        /// <returns>true if left value is greater than the right operand</returns>
        public static bool operator >(Vector2Int lh, Vector2Int rh)
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
        public static bool operator <(Vector2Int lh, Vector2Int rh)
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
        public static bool operator >=(Vector2Int lh, Vector2Int rh)
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
        public static bool operator <=(Vector2Int lh, Vector2Int rh)
        {
            if (lh._x <= rh._x && lh._y <= rh._y)
                return true;

            return false;
        }
        #endregion


        #region Mathematical Operator Overload
        // math operator overloading
        // multiply operator
        public static Vector2Int operator *(Vector2Int lh, double rh)
        {
            return new Vector2Int(lh._x * rh, lh._y * rh);
        }

        public static Vector2Int operator *(Vector2Int lh, float rh)
        {
            return new Vector2Int(lh._x * rh, lh._y * rh);
        }

        public static Vector2Int operator *(Vector2Int lh, int rh)
        {
            return new Vector2Int(lh._x * rh, lh._y * rh);
        }

        public static Vector2Int operator *(Vector2Int lh, byte rh)
        {
            return new Vector2Int(lh._x * rh, lh._y * rh);
        }

        // divide operator
        public static Vector2Int operator /(Vector2Int lh, double rh)
        {
            double invRh = 1.0 / rh;
            return new Vector2Int(lh._x * invRh, lh._y * invRh);
        }

        public static Vector2Int operator /(Vector2Int lh, float rh)
        {
            float invRh = 1.0f / rh;
            return new Vector2Int(lh._x * invRh, lh._y * invRh);
        }

        public static Vector2Int operator /(Vector2Int lh, int rh)
        {
            float invRh = 1.0f / rh;
            return new Vector2Int(lh._x * invRh, lh._y * invRh);
        }

        public static Vector2Int operator /(Vector2Int lh, byte rh)
        {
            float invRh = 1.0f / rh;
            return new Vector2Int(lh._x * invRh, lh._y * invRh);
        }

        // add operator
        //public static Vector2Int operator +(Vector2Int lh, Double2 rh)
        //{
        //    return new Vector2Int(lh._x + rh.X, lh._y + rh.Y);
        //}

        public static Vector2Int operator +(Vector2Int lh, Vector2Int rh)
        {
            return new Vector2Int(lh._x + rh._x, lh._y + rh._y);
        }

        public static Vector2Int operator +(Vector2Int lh, Vector2 rh)
        {
            return new Vector2Int(lh._x + rh.X, lh._y + rh.Y);
        }

        //public static Vector2Int operator +(Vector2Int lh, Byte2 rh)
        //{
        //    return new Vector2Int(lh._x + rh.X, lh._y + rh.Y);
        //}

        // substract operator
        public static Vector2Int operator -(Vector2Int val)
        {
            return new Vector2Int(-val._x, -val._y);
        }

        //public static Vector2Int operator -(Vector2Int lh, Double2 rh)
        //{
        //    return new Vector2Int(lh._x - rh.X, lh._y - rh.Y);
        //}

        public static Vector2Int operator -(Vector2Int lh, Vector2Int rh)
        {
            return new Vector2Int(lh._x - rh._x, lh._y - rh._y);
        }

        public static Vector2Int operator -(Vector2Int lh, Vector2 rh)
        {
            return new Vector2Int(lh._x - rh.X, lh._y - rh.Y);
        }

        //public static Vector2Int operator -(Vector2Int lh, Byte2 rh)
        //{
        //    return new Vector2Int(lh._x - rh.X, lh._y - rh.Y);
        //}

        #endregion



        public override int GetHashCode()
        {
            return this._x.GetHashCode() + this._y.GetHashCode();
        }


        public int[] ToIntArray()
        {
            return new int[] { this._x, this._y };
        }

        public int CompareTo(Vector2Int other)
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


        #region Public Static - int[] - ToIntArray(Int2[])
        /// <summary>
        /// converts an array of Int2 to a sequential int array containing all position data (x0, y0, x1, y1, x2, y2, etc.)
        /// </summary>
        /// <param name="source">source array containing Int2 elements</param>
        /// <returns>array containing a sequential list of the source's data</returns>
        public static int[] ToIntArray(Vector2Int[] source)
        {
            int[] result = new int[source.Length * 2];

            for (int i = 0; i < source.Length; i++)
            {
                int resI = i * 2;
                result[resI] = source[i].X;
                result[resI + 1] = source[i].Y;
            }

            return result;
        }
        #endregion

        #region Public Static - int[] - ToIntArray(IList<Int2>)
        /// <summary>
        /// converts a IList of Int2 to a sequential int array containing all position data (x0, y0, x1, y1, x2, y2, etc.)
        /// </summary>
        /// <param name="source">source IList containing Int2 elements</param>
        /// <returns>array containing a sequential list of the source's data</returns>
        public static int[] ToIntArray(IList<Vector2Int> source)
        {
            int[] result = new int[source.Count * 2];

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
