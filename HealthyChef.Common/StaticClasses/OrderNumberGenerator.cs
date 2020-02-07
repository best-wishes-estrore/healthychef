using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace HealthyChef.Common
{
    public static class OrderNumberGenerator
    {
        //Allowed Chars and Allowed Numbers can be edited to only allow certain letters and numbers.  The repeated numbers are used to reduce the total number of loops.
        private const string ALLOWED_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string ALLOWED_NUMBERS = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890";

        public static string GenerateOrderNumber(string pattern)
        {
            int length = pattern.Length;
            RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[length];
            int index = 0;
            int current = 0;
            string orderNumber = string.Empty;

            do
            {
                generator.GetBytes(buffer);

                for (int idx = 0; idx < length; idx++)
                {
                    current = buffer[idx];
                    if (pattern[index] == '?')
                    {
                        if (current < ALLOWED_CHARS.Length)
                        {
                            orderNumber += ALLOWED_CHARS[current];
                            index++;
                        }
                    }
                    else if (pattern[index] == '#')
                    {
                        if (current < ALLOWED_CHARS.Length)
                        {
                            orderNumber += ALLOWED_NUMBERS[current];
                            index++;
                        }
                    }
                    else
                    {
                        orderNumber += pattern[index++];
                    }

                    if (index >= length)
                    {
                        break;
                    }
                }
            }
            while (orderNumber.Length < length);

            return orderNumber;
        }
    }
}
