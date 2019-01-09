using MASMA_proiect.agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASMA_proiect.utils
{
    public class Utils
    {
        private static Random random = new Random();

        public static int[] GenerateRandomArray(int length, int maxValue)
        {
            int[] theArray = new int[length];

            for (int i = 0; i < length; ++i)
            {
                theArray[i] = random.Next(maxValue);
            }

            return theArray;
        }

        public static string GenerateMessageContent(Actions action, params string[] messageParts)
        {
            return action.ToString() + "#" + string.Join("#", messageParts);
        }
    }
}
