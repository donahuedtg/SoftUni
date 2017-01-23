using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Largest_Common_End
{
    class Largest_Common_End
    {
        static void Main(string[] args)
        {
            //Read two arrays of words and find the length of the largest common end (left or right).

            string[] arr1 = Console.ReadLine().Split(' ');
            string[] arr2 = Console.ReadLine().Split(' ');

            int fromLeft = 0;
            int fromRight = 0;

            int arrLengthMin = Math.Min(arr1.Length, arr2.Length);

            for (int i = 0; i < arrLengthMin; i++)
            {
                if (arr1[i] == arr2[i])
                {
                    fromLeft++;
                }
            }

            for (int i = 0; i < arrLengthMin; i++)
            {
                if (arr1[arr1.Length - 1 - i] == arr2[arr2.Length - 1 - i])
                {
                    fromRight++;
                }
            }



            //if (fromLeft != 0)
            //{
            //    Console.WriteLine("The largest common end is at the left: {0}", fromLeft);
            //}
            //else if (fromRight != 0)
            //{
            //    Console.WriteLine("The largest common end is at the right {0}", fromRight);
            //}
            //else
            //{
            //    Console.WriteLine("No common words at the left and right");
            //}

            int result = Math.Max(fromLeft, fromRight);
            Console.WriteLine(result);

        }
    }
}
