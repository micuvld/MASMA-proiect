using ActressMas;
using MASMA_proiect.agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASMA_Parallel_Merge.agents
{
    public class MasterAgent : ActressMas.Agent
    {
        private int[] arrayToSort;
        private int[] sortedArray;

        public MasterAgent(int[] arrayToSort)
        {
            this.arrayToSort = arrayToSort;
            this.sortedArray = new int[arrayToSort.Length];
        }

        public override void Setup()
        {
            this.Send("P0", Actions.START_PARTITIONING + "#" + string.Join(",", arrayToSort));
        }

        public override void Act(Message message)
        {
            string[] splittedMessage = message.Content.Split('#');
            string stringAction = splittedMessage[0];

            Enum.TryParse(stringAction, out Actions action);

            string[] arrayOfString = splittedMessage[1].Split(',');

            Console.WriteLine("Master received array: " + string.Join(",", arrayOfString));
        }
    }
}
