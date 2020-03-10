using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace NoXP.Types
{
    [Serializable()]
    [StructLayout(LayoutKind.Explicit)]
    public struct Vector3Int
    {
        // constant for the default size in memory of the struct
        public const byte SizeInBytes = 12;

        #region Fields
        // public readonly static
        /// <summary>
        /// Shorthand for writing Int3(0, 0, 0)
        /// </summary>
        public readonly static Vector3Int Zero = new Vector3Int(0, 0, 0);
        /// <summary>
        /// Shorthand for writing Int3(1, 1, 1)
        /// </summary>
        public readonly static Vector3Int One = new Vector3Int(1, 1, 1);
        /// <summary>
        /// Shorthand for writing Int3(0, 0, 1)
        /// </summary>
        public readonly static Vector3Int Front = new Vector3Int(0, 0, 1);
        /// <summary>
        /// Shorthand for writing Int3(0, 0, -1)
        /// </summary>
        public readonly static Vector3Int Back = new Vector3Int(0, 0, -1);
        /// <summary>
        /// Shorthand for writing Int3(0, 1, 0)
        /// </summary>
        public readonly static Vector3Int Up = new Vector3Int(0, 1, 0);
        /// <summary>
        /// Shorthand for writing Int3(0, -1, 0)
        /// </summary>
        public readonly static Vector3Int Down = new Vector3Int(0, -1, 0);
        /// <summary>
        /// Shorthand for writing Int3(1, 0, 0)
        /// </summary>
        public readonly static Vector3Int Right = new Vector3Int(1, 0, 0);
        /// <summary>
        /// Shorthand for writing Int3(-1, 0, 0)
        /// </summary>
        public readonly static Vector3Int Left = new Vector3Int(-1, 0, 0);
        /// <summary>
        /// Shorthand for writing Int3(int.MaxValue, int.MaxValue, int.MaxValue)
        /// </summary>
        public readonly static Vector3Int Max = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        /// <summary>
        /// Shorthand for writing Int3(int.MinValue, int.MinValue, int.MinValue)
        /// </summary>
        public readonly static Vector3Int Min = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);


        // private
        // X, R
        [FieldOffset(0)]
        private int _x;
        // Y, G
        [FieldOffset(4)]
        private int _y;
        // Z, B
        [FieldOffset(8)]
        private int _z;

        #endregion

        // vector property mapping (_x=X, _y=Y, _z=Z)
        #region Property - int - X
        /// <summary>
        /// gets or sets the X value (equivalent with R)
        /// </summary>
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }
        #endregion

        #region Property - int - Y
        /// <summary>
        /// gets or sets the Y value (equivalent with G)
        /// </summary>
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }
        #endregion

        #region Property - int - Z
        /// <summary>
        /// gets or sets the Z value (equivalent with B)
        /// </summary>
        public int Z
        {
            get { return _z; }
            set { _z = value; }
        }
        #endregion

        // color property mapping (_x=R, _y=G, _z=B)        
        #region Property - int - R
        /// <summary>
        /// gets or sets the R value (equivalent with X)
        /// </summary>
        public int R
        {
            get { return _x; }
            set { _x = value; }
        }
        #endregion

        #region Property - int - G
        /// <summary>
        /// gets or sets the G value (equivalent with Y)
        /// </summary>
        public int G
        {
            get { return _y; }
            set { _y = value; }
        }
        #endregion

        #region Property - int - B
        /// <summary>
        /// gets or sets the B value (equivalent with Z)
        /// </summary>
        public int B
        {
            get { return _z; }
            set { _z = value; }
        }
        #endregion


        #region Ctors

        #region Ctor(double, double, double)
        /// <summary>
        /// creates a new instance of Int3
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        /// <param name="z">value of the z component</param>
        public Vector3Int(double x, double y, double z)
        {
            _x = (int)x;
            _y = (int)y;
            _z = (int)z;
        }
        #endregion

        #region Ctor(float, float, float)
        /// <summary>
        /// creates a new instance of Int3
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        /// <param name="z">value of the z component</param>
        public Vector3Int(float x, float y, float z)
        {
            _x = (int)x;
            _y = (int)y;
            _z = (int)z;
        }
        #endregion

        #region Ctor(int, int, int)
        /// <summary>
        /// creates a new instance of Int3
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        /// <param name="z">value of the z component</param>
        public Vector3Int(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        #endregion

        #region Ctor(byte, byte, byte)
        /// <summary>
        /// creates a new instance of Int3
        /// </summary>
        /// <param name="x">value of the x component</param>
        /// <param name="y">value of the y component</param>
        /// <param name="z">value of the z component</param>
        public Vector3Int(byte x, byte y, byte z)
        {
            _x = x;
            _y = y;
            _z = z;
        }
        #endregion

        #endregion

        public void Set(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        #region Public Static - Vector3Int[] - Convert(int[])
        /// <summary>
        /// Converts an array of int to an array of Int3
        /// </summary>
        /// <param name="source">the source array of int</param>
        /// <returns>an array of Int3 containing the sources data</returns>
        public static Vector3Int[] Convert(int[] source)
        {
            if (source.Length % 3 != 0)
                throw new ArgumentException("Can't convert source array to Int3. Make sure you pass an array with a multiple of 3.", "source");

            Vector3Int[] result = new Vector3Int[source.Length / 3];
            for (int i = 0; i < result.Length; i++)
            {
                int srcI = i * 3;
                result[i] = new Vector3Int(source[srcI], source[srcI + 1], source[srcI + 2]);
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
            return String.Format("{0:0} {1:0} {2:0} ", new object[] { _x, _y, _z });
        }
        #endregion



        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            return this == ((Vector3Int)obj);
        }

        public bool Equals(Vector3Int value)
        {
            if (this._x != value._x
                || this._y != value._y
                || this._z != value._z)
                return false;
            return true;
        }

        public static bool operator ==(Vector3Int lh, Vector3Int rh)
        {
            if (lh._x != rh._x
                || lh._y != rh._y
                || lh._z != rh._z)
                return false;

            return true;
        }

        public static bool operator !=(Vector3Int lh, Vector3Int rh)
        {
            if (lh._x != rh._x
                || lh._y != rh._y
                || lh._z != rh._z)
                return true;

            return false;
        }

        #region Operator - Greater
        /// <summary>
        /// checks if the left operand is greater than the right operand
        /// </summary>
        /// <param name="lh">value to check if it is greater than the right operand</param>
        /// <param name="rh">value to check against</param>
        /// <returns>true if left value is greater than the right operand</returns>
        public static bool operator >(Vector3Int lh, Vector3Int rh)
        {
            if ((lh._x > rh._x && lh._y >= rh._y && lh._z >= rh._z)
                || (lh._y > rh._y && lh._x >= rh._x && lh._z >= rh._z)
                || (lh._z > rh._z && lh._x >= rh._x && lh._y >= rh._y))
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
        public static bool operator <(Vector3Int lh, Vector3Int rh)
        {
            if ((lh._x < rh._x && lh._y <= rh._y && lh._z <= rh._z)
                || (lh._y < rh._y && lh._x <= rh._x && lh._z <= rh._z)
                || (lh._z < rh._z && lh._x <= rh._x && lh._y <= rh._y))
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
        public static bool operator >=(Vector3Int lh, Vector3Int rh)
        {
            if (lh._x >= rh._x && lh._y >= rh._y && lh._z >= rh._z)
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
        public static bool operator <=(Vector3Int lh, Vector3Int rh)
        {
            if (lh._x <= rh._x && lh._y <= rh._y && lh._z <= rh._z)
                return true;

            return false;
        }
        #endregion



        #region Mathematical Operator Overload
        // math operator overloading
        // multiply operator
        public static Vector3Int operator *(Vector3Int lh, double rh)
        {
            return new Vector3Int(lh._x * rh, lh._y * rh, lh._z * rh);
        }

        public static Vector3Int operator *(Vector3Int lh, float rh)
        {
            return new Vector3Int(lh._x * rh, lh._y * rh, lh._z * rh);
        }

        public static Vector3Int operator *(Vector3Int lh, int rh)
        {
            return new Vector3Int(lh._x * rh, lh._y * rh, lh._z * rh);
        }

        public static Vector3Int operator *(Vector3Int lh, byte rh)
        {
            return new Vector3Int(lh._x * rh, lh._y * rh, lh._z * rh);
        }

        // divide operator        
        public static Vector3Int operator /(Vector3Int lh, double rh)
        {
            double invRh = 1.0 / rh;
            return new Vector3Int(lh._x * invRh, lh._y * invRh, lh._z * invRh);
        }

        public static Vector3Int operator /(Vector3Int lh, float rh)
        {
            float invRh = 1.0f / rh;
            return new Vector3Int(lh._x * invRh, lh._y * invRh, lh._z * invRh);
        }

        public static Vector3Int operator /(Vector3Int lh, int rh)
        {
            float invRh = 1.0f / rh;
            return new Vector3Int(lh._x * invRh, lh._y * invRh, lh._z * invRh);
        }

        public static Vector3Int operator /(Vector3Int lh, byte rh)
        {
            float invRh = 1.0f / rh;
            return new Vector3Int(lh._x * invRh, lh._y * invRh, lh._z * invRh);
        }

        // add operator
        //public static Vector3Int operator +(Vector3Int lh, Double3 rh)
        //{
        //    return new Vector3Int(lh._x + rh.X, lh._y + rh.Y, lh._z + rh.Z);
        //}

        public static Vector3Int operator +(Vector3Int lh, Vector3Int rh)
        {
            return new Vector3Int(lh._x + rh._x, lh._y + rh._y, lh._z + rh._z);
        }

        public static Vector3Int operator +(Vector3Int lh, Vector3 rh)
        {
            return new Vector3Int(lh._x + rh.X, lh._y + rh.Y, lh._z + rh.Z);
        }

        //public static Vector3Int operator +(Vector3Int lh, Byte3 rh)
        //{
        //    return new Vector3Int(lh._x + rh.X, lh._y + rh.Y, lh._z + rh.Z);
        //}

        // substract operator
        public static Vector3Int operator -(Vector3Int val)
        {
            return new Vector3Int(-val._x, -val._y, -val._z);
        }

        //public static Vector3Int operator -(Vector3Int lh, Double3 rh)
        //{
        //    return new Vector3Int(lh._x - rh.X, lh._y - rh.Y, lh._z - rh.Z);
        //}

        public static Vector3Int operator -(Vector3Int lh, Vector3Int rh)
        {
            return new Vector3Int(lh._x - rh._x, lh._y - rh._y, lh._z - rh._z);
        }

        public static Vector3Int operator -(Vector3Int lh, Vector3 rh)
        {
            return new Vector3Int(lh._x - rh.X, lh._y - rh.Y, lh._z - rh.Z);
        }

        //public static Vector3Int operator -(Vector3Int lh, Byte3 rh)
        //{
        //    return new Vector3Int(lh._x - rh.X, lh._y - rh.Y, lh._z - rh.Z);
        //}

        #endregion





        public override int GetHashCode()
        {
            return this._x.GetHashCode() + this._y.GetHashCode() + this._z.GetHashCode();
        }




        public int[] ToIntArray()
        {
            return new int[] { this._x, this._y, this._z };
        }

        public int CompareTo(Vector3Int other)
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


        #region Public Static - int[] - ToIntArray(Int3[])
        /// <summary>
        /// converts an array of Int3 to a sequential int array containing all position data (x0, y0, z0, x1, y1, z1, x2, y2, etc.)
        /// </summary>
        /// <param name="source">source array containing Int3 elements</param>
        /// <returns>array containing a sequential list of the source's data</returns>
        public static int[] ToIntArray(Vector3Int[] source)
        {
            int[] result = new int[source.Length * 3];

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

        #region Public Static - int[] - ToIntArray(IList<Int3>)
        /// <summary>
        /// converts a IList of Int3 to a sequential int array containing all position data (x0, y0, z0, x1, y1, z1, x2, y2, etc.)
        /// </summary>
        /// <param name="source">source IList containing Int3 elements</param>
        /// <returns>array containing a sequential list of the source's data</returns>
        public static int[] ToIntArray(IList<Vector3Int> source)
        {
            int[] result = new int[source.Count * 3];

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
