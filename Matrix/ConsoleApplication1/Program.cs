using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main()
        {
            var arr1 = new[,] { { false, true, false, false, false, false},
                                  { false, false, true, false, false, false},
                                  { false, false, false, true, true, false},
                                  { false, true, false, false, false, true},
                                  { true, false, false, false, false, true},
                                  { false, false, false, false, false, false}
            };

            var arr2 = new[,] { { false, true, false, false, false, false},
                                  { false, false, true, false, false, false},
                                  { false, false, false, true, true, false},
                                  { false, true, false, false, false, true},
                                  { true, false, false, false, false, true},
                                  { false, false, false, false, false, false} };

            var result = Mul(arr1, arr2);

            for (var i = 0; i < result.GetLength(0); i++)
            {
                for (var j = 0; j < result.GetLength(1); j++)
                    Console.Write("{0} ", result[i, j]);
                Console.WriteLine();
            }
        }

        private static bool[,] Mul(bool[,] left, bool[,] right)
        {
            if (left.GetLength(1) != right.GetLength(0))
                throw new ArgumentException();

            var result = new bool[left.GetLength(0), right.GetLength(1)];
            for (var i = 0; i < result.GetLength(0); i++)
                for (var j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = false;
                    for (var k = 0; k < left.GetLength(1); k++)
                        result[i, j] |= left[i, k] && right[k, j];
                }

            return result;
        }
    }
}
