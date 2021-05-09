using System;
using System.Linq;

namespace Next_palindrome_388
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"1 => {NextPal(1)}");
            Console.WriteLine($"9 => {NextPal(9)}");
            Console.WriteLine($"808 => {NextPal(808)}");
            Console.WriteLine($"998 => {NextPal(998)}");
            Console.WriteLine($"999 => {NextPal(999)}");
            Console.WriteLine($"1998 => {NextPal(1998)}");
            Console.WriteLine($"2222 => {NextPal(2222)}");
            Console.WriteLine($"9999 => {NextPal(9999)}");
            Console.WriteLine($"3^39 => {NextPal((ulong)Math.Pow(3, 39))}");
            Console.WriteLine($"18446644073709551615 => {NextPal(18446644073709551615)}");
        }

        internal static ulong NextPal(ulong n)
        {
            var numberInts = ConvertUlongToIntArray(n);
            //little cheat here, if all ints are 9 then the next Palindrome will be 10 .. 01
            var isAll9 = numberInts.All(numberInt => numberInt == 9);
            if (isAll9)
            {
                // create new int array
                var valLength = numberInts.Length + 1;
                numberInts = new int[valLength];
                // set first and last index to 1
                numberInts[0] = 1;
                numberInts[^1] = 1;
                // convert int array to uInt64
                return ConvertIntArrayToUlong(numberInts);
            }

            // increment number 
            n++;
            numberInts = ConvertUlongToIntArray(n);
            // if there is only one digit then return
            if (numberInts.Length == 1)
            {
                return n;
            }

            //another cheat, if all values are the same return
            var isAllSame = numberInts.All(numberInt => numberInt == numberInts[0]);
            if (isAllSame)
            {
                return n;
            }

            // split array into 2
            var middle = numberInts.Length / 2;
            // start checking from middle of value
            var leftIndex = middle - 1;
            // check if length is odd
            var isOddNumberOfValues = numberInts.Length % 2 != 0;
            // get right index, we can ignore the middle value if odd
            var rightIndex = isOddNumberOfValues ? middle + 1 : middle;
            // get indexes for when values do not match 
            while (leftIndex >= 0 && numberInts[leftIndex] == numberInts[rightIndex])
            {
                leftIndex--;
                rightIndex++;
            }

            // Is the left side vale smaller than the right side?
            var isLeftSmaller = (leftIndex < 0 || numberInts[leftIndex] < numberInts[rightIndex]);
            if (isLeftSmaller)
            {
                var carry = 1;
                // if the left side is smaller than the right side we will need to increment
                if (isOddNumberOfValues)
                {
                    numberInts[middle] += 1;
                    carry = numberInts[middle] / 10;
                    numberInts[middle] %= 10;
                }
                // reset the indexes
                leftIndex = middle - 1;
                rightIndex = isOddNumberOfValues ? middle + 1 : middle;
                // go through the values with a carry
                while (leftIndex >= 0)
                {
                    numberInts[leftIndex] = numberInts[leftIndex] + carry;
                    carry = numberInts[leftIndex] / 10;
                    numberInts[leftIndex] %= 10;
                    // copy left to right
                    numberInts[rightIndex] = numberInts[leftIndex];
                    leftIndex--;
                    rightIndex++;
                }
            }
            else
            {
                // copy left side to right side if indexes did not match eariler on
                while (leftIndex >= 0)
                {
                    numberInts[rightIndex++] = numberInts[leftIndex--];
                }
            }

            return ConvertIntArrayToUlong(numberInts);
        }

        private static int[] ConvertUlongToIntArray(ulong n)
        {
            // convert ulong to char array
            var numberChars = n.ToString().ToCharArray();

            // convert char array to int array
            var numberInts = new int[numberChars.Length];
            for (var index = 0; index < numberChars.Length; index++)
            {
                var c = numberChars[index];
                numberInts[index] = int.Parse(c.ToString());
            }

            return numberInts;
        }

        private static ulong ConvertIntArrayToUlong(int[] intArray)
        {
            var s = intArray.Aggregate("", (current, num) => current + num);
            return ulong.Parse(s);
        }
    }
}
