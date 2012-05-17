using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class Vector
    {
        public float x { get;  set; }
        public float y { get;  set; }
        public float z { get;  set; }
        public float w { get; set; }

        

       

        public Vector(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vector(double x, double y, double z, double w)
            : this((float)x, (float)y, (float)z, (float)w)
        {
        }

        public Vector(double x, double y, double z)
            : this((float)x, (float)y, (float)z, 1)
        {
        }

        public Vector(float x, float y, float z) 
            : this(x,y,z,1)
        {
        }

        public Vector(double x, double y)
            : this((float)x, (float)y)
        {
        }

        public Vector(float x, float y)
            : this(x, y, 0, 1)
        {
        }

        public Vector()
            : this(0, 0, 0, 1)
        {
        }

        /// <summary>
        /// Returns the magnitude of a 3-dimensional vector
        /// </summary>
        /// <returns></returns>
        public float Magnitude3()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }

        /// <summary>
        /// Normalizes a 3-dimensional vector
        /// </summary>
        public Vector Normalize3()
        {
            float currentMagnitude = Magnitude3();
            x /= currentMagnitude;
            y /= currentMagnitude;
            z /= currentMagnitude;
            return this;
        }

        public Vector Clamp3()
        {
            x = Math.Min(x, 1);
            y = Math.Min(y, 1);
            z = Math.Min(z, 1);
            return this;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z, v1.w + v2.w);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z, v1.w - v2.w);
        }

        public static Vector operator *(float scalar, Vector v)
        {
            return new Vector(scalar * v.x, scalar * v.y, scalar * v.z, scalar*v.w);
        }

        public static Vector operator *(Vector v, float scalar)
        {
            return new Vector(scalar * v.x, scalar * v.y, scalar * v.z, scalar * v.w);
        }

        public static Vector operator *(Vector v1, Vector v2)
        {
            return new Vector(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z, v1.w * v2.w);
        }

        public static Vector operator /(Vector v, float scalar)
        {
            return new Vector(v.x / scalar, v.y / scalar, v.z / scalar, v.w / scalar);
        }

        public static Vector Cross3(Vector v1, Vector v2)
        {
            Vector crossVec = new Vector();
            crossVec.x = v1.y * v2.z - v2.y * v1.z;
            crossVec.y = v2.x * v1.z - v1.x * v2.z;
            crossVec.z = v1.x * v2.y - v2.x * v1.y;
            crossVec.w = 0.0f;
            return crossVec;
        }

        public override string ToString()
        {
            return x + ", " + y + ", " + z;
        }

        /// <summary>
        /// Dot product of a 3-dimensional vector
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static float Dot3(Vector v1, Vector v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }


        /// <summary>
        /// Override indexer
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public float this[int i]
        {
            get 
            {
                switch (i)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    case 3:
                        return w;
                    default:
                        throw new IndexOutOfRangeException("Class Vector only supports 0, 1, 2 or 3 as index values");
                }
            }
            set
            {
                switch (i)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    case 3:
                        w = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Class Vector only supports 0, 1, 2 or 3 index values");
                }
            
            }
        }


    }
}
