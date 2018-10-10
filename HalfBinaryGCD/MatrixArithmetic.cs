using System;
using System.Numerics;

namespace HalfBinaryGCD
{
    public static class MatrixArithmetic
    {
        public static BigInteger[,] Product(BigInteger[,] a, BigInteger[,] b)
        {
            if (a.Length != 4 || b.Length != 4)
            {
                throw new ArgumentException("Matrix size not supported");
            }

            return new BigInteger[,]
            {
                {a[0, 0] * b[0, 0] + a[0, 1] * b[1, 0], a[0, 0] * b[0, 1] + a[0, 1] * b[1, 1]},
                {a[1, 0] * b[0, 0] + a[1, 1] * b[1, 0], a[1, 0] * b[0, 1] + a[1, 1] * b[1, 1]}
            };
        }
    }
}
