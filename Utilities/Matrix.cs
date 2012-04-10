using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class Matrix
    {
        public float M11 {get; set;} public float M12 { get; set; } public float M13 { get; set; } public float M14 { get; set; }
        public float M21 { get; set; } public float M22 { get; set; } public float M23 { get; set; } public float M24 { get; set; }
        public float M31 { get; set; } public float M32 { get; set; } public float M33 { get; set; } public float M34 { get; set; }
        public float M41 { get; set; } public float M42 { get; set; } public float M43 { get; set; } public float M44 { get; set; }

        public static Vector operator *(Matrix M, Vector v)
        {
            float x = M.M11 * v.x + M.M12 * v.y + M.M13 * v.z + M.M14 * v.w;
            float y = M.M21 * v.x + M.M22 * v.y + M.M23 * v.z + M.M24 * v.w;
            float z = M.M31 * v.x + M.M32 * v.y + M.M33 * v.z + M.M34 * v.w;
            float w = M.M41 * v.x + M.M42 * v.y + M.M43 * v.z + M.M44 * v.w;

            Vector transformedVector = new Vector(x, y, z, w);
            return transformedVector;
        }

        public static Matrix operator *(float scalar, Matrix M)
        {
            Matrix result = new Matrix();
            result.M11 = M.M11 * scalar;
            result.M12 = M.M12 * scalar;
            result.M13 = M.M13 * scalar;
            result.M14 = M.M14 * scalar;
            result.M21 = M.M21 * scalar;
            result.M22 = M.M22 * scalar;
            result.M23 = M.M23 * scalar;
            result.M24 = M.M24 * scalar;
            result.M31 = M.M31 * scalar;
            result.M32 = M.M32 * scalar;
            result.M33 = M.M33 * scalar;
            result.M34 = M.M34 * scalar;
            result.M41 = M.M41 * scalar;
            result.M42 = M.M42 * scalar;
            result.M43 = M.M43 * scalar;
            result.M44 = M.M44 * scalar;
            return result;
        }

        public static Matrix operator *(Matrix M1, Matrix M2)
        {
            Vector v1 = new Vector(M2.M11, M2.M21, M2.M31, M2.M41);
            Vector v2 = new Vector(M2.M12, M2.M22, M2.M32, M2.M42);
            Vector v3 = new Vector(M2.M13, M2.M23, M2.M33, M2.M43);
            Vector v4 = new Vector(M2.M14, M2.M24, M2.M34, M2.M44);

            Vector v1Result = M1 * v1;
            Vector v2Result = M1 * v2;
            Vector v3Result = M1 * v3;
            Vector v4Result = M1 * v4;

            Matrix result = new Matrix();
            result.M11 = v1Result.x;
            result.M12 = v2Result.x;
            result.M13 = v3Result.x;
            result.M14 = v4Result.x;
            result.M21 = v1Result.y;
            result.M22 = v2Result.y;
            result.M23 = v3Result.y;
            result.M24 = v4Result.y;
            result.M31 = v1Result.z;
            result.M32 = v2Result.z;
            result.M33 = v3Result.z;
            result.M34 = v4Result.z;
            result.M41 = v1Result.w;
            result.M42 = v2Result.w;
            result.M43 = v3Result.w;
            result.M44 = v4Result.w; 
            return result;
        }

    }
}
