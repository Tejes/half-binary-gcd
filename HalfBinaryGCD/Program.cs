using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Numerics;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace HalfBinaryGCD
{
    public static class Program
    {
        private static readonly BigInteger Two = new BigInteger(2);

        public static (BigInteger, BigInteger[,]) HalfBinaryGcd(BigInteger a, BigInteger b, int k)
        {
            if (Nu(b) > k)
            {
                return (0, new BigInteger[,] {{1, 0}, {0, 1}});
            }

            var k1 = k >> 1;
            var a1 = a % BigInteger.Pow(Two, 2 * k1 + 1);  // TODO refact
            var b1 = b % BigInteger.Pow(Two, 2 * k1 + 1);
            var (j1, R) = HalfBinaryGcd(a1, b1, k1);

            var at = BigInteger.Pow(Two, -2 * (int) j1) * (R[0, 0] * a + R[0, 1] * b);
            var bt = BigInteger.Pow(Two, -2 * (int) j1) * (R[1, 0] * a + R[1, 1] * b);

            var j0 = Nu(bt);
            if (j0 + j1 > k)
            {
                return (j1, R);
            }

            var (q, r) = BinaryDivide(at, bt);

            var k2 = k - (j0 + j1);
            var a2 = ModuloArithmetic.Divide(bt, BigInteger.One << j0, BigInteger.One << 2 * (int) k2 + 1);
            var b2 = ModuloArithmetic.Divide(r, BigInteger.One << j0, BigInteger.One << 2 * (int) k2 + 1);

            var (j2, S) = HalfBinaryGcd(a2, b2, (int)k2);

            return (j1 + j0 + j2,
                    MatrixArithmetic.Product(
                        MatrixArithmetic.Product(S,
                            new BigInteger[,] {{0, BigInteger.One << j0}, {BigInteger.One << j0, q}}),
                        R)
                );
        }
        
        public static (BigInteger, BigInteger) BinaryDivide(BigInteger a, BigInteger b)
        {
            int j = Nu(b) - Nu(a);
            if (j <= 0)
            {
                throw new ArgumentException("ν(b) > ν(a) is not true");
            }
            var bt = b >> j; //  Math.Pow(2, -j) * b
            var twoToTheJPlusOne = BigInteger.One << (j + 1);
            var q = BigInteger.Remainder(ModuloArithmetic.Negate(a, twoToTheJPlusOne) * ModuloArithmetic.Invert(bt, twoToTheJPlusOne), twoToTheJPlusOne);
            if (q >= twoToTheJPlusOne >> 1)
            {
                q -= twoToTheJPlusOne;
            }

            var r = a + q * (b >> j);
            return (q, r);
        }

        /// <summary>
        /// 2-valuation
        /// </summary>
        /// <param name="n"></param>
        /// <returns>Largest k such that 2^k divides n. (nu(0) == inf)</returns>
        public static int Nu(BigInteger n)
        {
            var array = n.ToByteArray(isBigEndian: false);
            int k = 0;
            //for (int i = array.Length - 1; i >= 0; --i)
            for (int i = 0; i < array.Length; ++i)
            {
                int j;
                for (j = 0; j < 8 && (array[i] & 0b00000001) == 0; ++j)
                {
                    array[i] >>= 1;
                    ++k;
                }

                if (j < 8)
                {
                    break;
                }
            }
            
            return k;
        }
        
        public static void Main(string[] args)
        {
//            BigInteger a = 1432604672;
//            BigInteger b = 245760;
//            BigInteger a = 935;
//            BigInteger b = 714;
            BigInteger a = 1889826700059L;
            BigInteger b =  421872857844L;
            Console.WriteLine("{0} / {1} = {2}", a, b, BinaryDivide(a, b));
            Console.WriteLine("GCD({0}, {1}) = {2}", a, b, ModuloArithmetic.SlowGcd((long)a, (long)b));
            Console.WriteLine("GCD({0}, {1}) = {2}", a, b, HalfBinaryGcd(a, b, 20));
        }
    }
}
