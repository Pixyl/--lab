using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Работает только на система из 2^q уравнений, q - натуральное число
//Пример в программе:
// 2  -1  0     x1     16
// -1  2  0  х  x2  =  16
// 0  -1  2     x3     16
//Ответ: 24 32 24

namespace reduction_method
{
    class TridiagMatrix
    {
        private List<double> c;
        private List<double> a;
        private List<double> b;
        private int n;

        public TridiagMatrix(int size, double data_c, double data_a, double data_b)
        {
            n = size;
            c = new List<double>();
            a = new List<double>();
            b = new List<double>();
            for (int i = 0; i < n - 1; i++)
            {
                c.Add(data_c);
                a.Add(data_a);
                b.Add(data_b);
            }
            c.Add(data_c);
        }
        public TridiagMatrix(int size)
        {
            n = size;
            c = new List<double>();
            a = new List<double>();
            b = new List<double>();
            for (int i = 0; i < n - 1; i++)
            {
                c.Add(0);
                a.Add(0);
                b.Add(0);
            }
            c.Add(0);
        }
        public TridiagMatrix(int size, double data_c)
        {
            n = size;
            c = new List<double>();
            a = new List<double>();
            b = new List<double>();
            for (int i = 0; i < n - 1; i++)
            {
                c.Add(data_c);
                a.Add(0);
                b.Add(0);
            }
            c.Add(data_c);
        }

        public TridiagMatrix()
        {
            n = 3;
            c = new List<double>();
            a = new List<double>();
            b = new List<double>();
            for (int i = 0; i < n - 1; i++)
            {
                c.Add(0);
                a.Add(0);
                b.Add(0);
            }
            c.Add(0);
        }

        public TridiagMatrix(List<double> data_c, List<double> data_a, List<double> data_b)
        {
            c = data_c;
            a = data_a;
            b = data_b;
            n = c.Count;
        }

        public TridiagMatrix(List<double> data_c)
        {
            c = data_c;
            n = c.Count;
            a = new List<double>();
            b = new List<double>();
            for (int i = 0; i < n - 1; i++)
            {
                a.Add(0);
                b.Add(0);
            }
        }

        public int size()
        {
            return n;
        }
        public List<double> getA()
        {
            return a;
        }
        public List<double> getB()
        {
            return b;
        }
        public List<double> getC()
        {
            return c;
        }
        public double get(int i, int j)
        {
            if (i < n && j < n)
            {
                if (i > -1 && j > -1)
                {
                    if (i == j) return c[i];
                    if (i == j - 1) return b[i];
                    if (i == j + 1) return a[j];
                    return 0;
                }
            }
            return 0;
        }
        public void setA(List<double> data)
        {
            if (data.Count == n - 1)
                a = data;
        }
        public void setB(List<double> data)
        {
            if (data.Count == n - 1)
                b = data;
        }
        public void setC(List<double> data)
        {
            if (data.Count == n - 1)
                c = data;
        }
        public void set(int i, int j, double data)
        {
            if (i < n && j < n)
            {
                if (i > -1 && j > -1)
                {
                    if (i == j) c[i] = data;
                    if (i == j - 1) b[i] = data;
                    if (i == j + 1) a[j] = data;
                }
            }
        }
        public void consolePrint()
        {
            for (int i = 0; i < size(); i++)
            {
                for (int j = 0; j < size(); j++)
                    Console.Write(get(i, j) + " ");
                Console.WriteLine();
            }
        }

        public static List<double> Solve(TridiagMatrix M, List<double> r)
        {
            List<double> a = new List<double>();
            List<double> b = new List<double>();
            List<double> c = new List<double>();
            a.Add(0);
            a.AddRange(M.getA());
            b.AddRange(M.getC());
            c.AddRange(M.getB());
            c.Add(0);
            List<double> x = r;
            int stride = 1;
            int n = r.Count;
            // 1. прямой ход
            for (int nn = n, low = 2; nn > 1; nn /= 2, low *= 2, stride *= 2)
            {
                for (int i = low - 1; i < n; i += stride * 2)
                {
                    double alpha = -a[i] / b[i - stride];
                    double gamma = -c[i] / b[i + stride];
                    a[i] = alpha * a[i - stride];
                    b[i] = alpha * c[i - stride] + b[i] + gamma * a[i + stride];
                    c[i] = gamma * c[i + stride];
                    r[i] = alpha * r[i - stride] + r[i] + gamma * r[i + stride];

                }
            }

            // 2. обратный ход
            x[n / 2] = r[n / 2] / b[n / 2];
            for (stride /= 2; stride >= 1; stride /= 2)
            {
                for (int i = stride - 1; i < n; i += stride * 2)
                {
                    x[i] = (r[i]
                    - (i - stride > 0 ? a[i] * x[i - stride] : 0.0)
                    - (i + stride < n ? c[i] * x[i + stride] : 0.0)
                    ) / b[i];
                }
            }
            return x;
        }
    };




    class Program
    {
        static void listConsolePrint(List<double> L)
        {
            for (int i = 0; i < L.Count; i++)
                Console.Write(L[i] + " ");
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            TridiagMatrix M = new TridiagMatrix(3, 2, -1, -1);
            List<double> R = new List<double> { 16, 16, 16 };
            M.consolePrint();
            Program.listConsolePrint(R);
            List<double> X = TridiagMatrix.Solve(M,R);
            Program.listConsolePrint(X);
            Console.ReadKey();
        }
    }
}
