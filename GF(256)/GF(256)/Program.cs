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
        public byte Division(ushort poly_a, ushort poly_b)
        {
            byte    degree_a = Degree(poly_a);
            byte    degree_b = Degree(poly_b);

            while (degree_a > degree_b - 1)
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

            for (int i = 14; i >= 0; i--)
            {
                if ((poly_a & (1 << i)) > 0)
                {
                    multiply_polynomials ^= (ushort)((poly_b << i));
                }
            }
            return Division(multiply_polynomials, modulo);
        }
        public byte Inverse(byte poly_a, ushort modulo)
        {
            byte    res = Multiply(1, poly_a, modulo);  //254 = [1]1111110

            for (int i = 6; i >= 0; i--)                // 1[1111110]
            {
                res = Multiply(res, res, modulo);
                if ((254 & (1 << i)) > 0)                                  
                    res = Multiply(res, poly_a, modulo);
 
            }
            return res;
        }
        public List<ushort> AllIrreduciblePolynomials()
        {
            bool            flag;
            List<ushort>    polynomials = new List<ushort>();

            for (ushort i = 257; i < 512; i += 2)
            {              
                if (!Monom_odd_check(i)) continue;

                flag = true;
                for (byte j = 3; j < 32; j+=2)
                {
                    if (Division(i, j) == 0)
                    {
                        flag = false;
                        break; ;
                    }                       
                }
                if (flag) polynomials.Add(i);
            }
            return polynomials;
        }

        public void PrintAllIrreduciblePolynomials(List<ushort> Polynomials)
        {
            Console.WriteLine("\nAll irreducible polynomials: ");
            for (int i = 0; i < Polynomials.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + Polynomials[i]);
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
            byte        a = 50;
            byte        b = 249;
            byte        c = 15;
            ushort      modulo = 283;
            GaloisField T = new GaloisField();

            Console.WriteLine($"{a} + {b} = {T.Add(a, b)}");
            Console.WriteLine($"{a} * {b} = {T.Multiply(a, b, modulo)}");

            Console.WriteLine($"{c} ^(-1) = {T.Inverse(c, modulo)}");
            Console.WriteLine($"{c} ^(-1) * {c} = {T.Multiply(c, T.Inverse(c, modulo), modulo)}");
           
            T.PrintAllIrreduciblePolynomials(T.AllIrreduciblePolynomials());
        }
    }
}