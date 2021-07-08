using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GF_256_
{
    
    class GaloisField
    {
        public byte Add(byte poly_a, byte poly_b)
        {
            return (byte)(poly_a ^ poly_b);
        }
        public byte Degree(ushort multiply_polynomials)
        {
            byte degree = 0;
            for (int i = 14; i >= 0; i--)
            {
                if ((multiply_polynomials & (1 << i)) > 0 && i > degree)
                {
                    degree = (byte)i;
                }
            }
            return degree;
        }
        public byte Division(ushort poly_a, byte poly_b)
        {
            byte    degree_a = Degree(poly_a);
            byte    degree_b = Degree(poly_b);

            while (degree_a >= degree_b)
            {
                int i = degree_a - degree_b;
                poly_a ^= (ushort)((poly_b << i));
                degree_a = Degree(poly_a);
            }
            return (byte)poly_a;
        }
        public byte Multiply(byte poly_a, byte poly_b, ushort modulo)
        {
            ushort  multiply_polynomials = 0;
            byte    degree;

            for (int i = 14; i >= 0; i--)
            {
                if ((poly_a & (1 << i)) > 0)
                {
                    multiply_polynomials ^= (ushort)((poly_b << i));
                }
            }

            degree = Degree(multiply_polynomials);

            while (degree > 7)
            {
                int i = degree - 8;
                multiply_polynomials ^= (ushort)((modulo << i));
                degree = Degree(multiply_polynomials);
            }
            return (byte)multiply_polynomials;
        }
        public byte Inverse(byte poly_a, ushort modulo)
        {
            byte    res = Multiply(1, poly_a, modulo);  //254 = [1]1111110

            for (int i = 6; i >= 0; i--)                // 1[1111110]
            {
                if ((254 & (1 << i)) > 0)               //1
                {
                    res = Multiply(res, res, modulo);
                    res = Multiply(res, poly_a, modulo);
                }
                else                                    //0
                {
                    res = Multiply(res, res, modulo);
                }
            }
            return res;
        }
        public void AllIrreduciblePolynomials()
        {
            int     count = 0;
            bool    flag = true;
            for (ushort i = 257; i < 512; i += 2)
            {
                flag = true;
                for (byte j = 3; j < 32; j+=2)
                {
                    if (Division(i, j) == 0)
                    {
                        flag = false;                                            
                        j = 34;
                    }                       
                }
                if (flag && Monom_odd_check(i))
                {
                    count++;
                    Console.WriteLine(count + ". " + i);
                }
            }
        }
        public bool Monom_odd_check(ushort poly_a)
        {
            int     count = 0;

            for (int i = 8; i >= 0; i--)
            {
                if ((poly_a & (1 << i)) > 0)
                    count++;
            }
            if (count % 2 != 0) return true;
            else return false;   
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            GaloisField T = new GaloisField();
            byte    a = 50;
            byte    b = 249;
            ushort  modulo = 283;

            Console.WriteLine($"{a} + {b} = {T.Add(a, b)}");
            Console.WriteLine($"{a} * {b} = {T.Multiply(a, b, modulo)}");
            Console.WriteLine($"{a} ^(-1) = {T.Inverse(a, modulo)}");
            Console.WriteLine($"{a} ^(-1) * {a} = {T.Multiply(a, T.Inverse(a, modulo), modulo)}");

            T.AllIrreduciblePolynomials();
        }
    }
}
