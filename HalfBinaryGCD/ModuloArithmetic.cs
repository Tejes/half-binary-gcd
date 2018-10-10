using System;
using System.Numerics;
using System.Runtime.Serialization;

namespace HalfBinaryGCD
{
    public static class ModuloArithmetic
    {
        public static BigInteger Negate(BigInteger x, BigInteger mod)
        {
            var result = -x % mod;
            
            return result < BigInteger.Zero ? result + mod : result;
        }

        public static BigInteger Invert(BigInteger x, BigInteger mod)
        {
            /*var gcd = BigInteger.GreatestCommonDivisor(x, mod);
            if (!gcd.IsOne)
            {
                throw new ArgumentException("x and mod are not relative primes");
            }*/

            /*x %= mod;
            var inv = x;
            var next = x * x % mod;
            while (!next.IsOne)
            {
                inv = next;
                next *= x;
                next %= mod;
            }

            return inv;*/

            BigInteger t = BigInteger.Zero, newt = BigInteger.One, r = mod, newr = x;
            while (!newr.IsZero)
            {
                var quotient = r / newr;
                (t, newt) = (newt, t - quotient * newt);  // TODO check evaluation order
                (r, newr) = (newr, r - quotient * newr);
            }

            if (r > BigInteger.One)
            {
                throw new ArithmeticException(string.Format("{0} is not invertible mod {1}", x, mod));
            }

            return t < BigInteger.Zero ? t + mod : t;
        }

        public static BigInteger Divide(BigInteger q, BigInteger d, BigInteger mod)
        {
            return q * Invert(d, mod) % mod;
        }

        public static ExtendedGcdResult ExtendedGcd(BigInteger a, BigInteger b)
        {
            BigInteger s = BigInteger.Zero, old_s = BigInteger.One;
            BigInteger t = BigInteger.One, old_t = BigInteger.Zero;
            BigInteger r = b, old_r = a;

            while (!r.IsZero)
            {
                var quotient = old_r / r;
                (old_r, r) = (r, old_r - quotient * r);
                (old_s, s) = (s, old_s - quotient * s);
                (old_t, t) = (t, old_t - quotient * t);
            }
            
            return new ExtendedGcdResult
            {
                BezoutCoefficients = (old_s, old_t),
                Gcd = old_r,
                Quotients = (t, s)
            };
        }

        public static long SlowGcd(long a, long b)
        {
            if (a < b)
            {
                var temp = a;
                a = b;
                b = temp;
            }

            while (a > b)
            {
                a -= b;
                if (a < b)
                {
                    var temp = a;
                    a = b;
                    b = temp;
                }
            }

            return a;
        }
    }

    public struct ExtendedGcdResult
    {
        /// <summary>
        /// Bézout coefficients
        /// </summary>
        public (BigInteger, BigInteger) BezoutCoefficients;
        
        /// <summary>
        /// Greatest Common Divisor
        /// </summary>
        public BigInteger Gcd;
        
        /// <summary>
        /// Quotients by the GCD
        /// </summary>
        public (BigInteger, BigInteger) Quotients;
    }
}
