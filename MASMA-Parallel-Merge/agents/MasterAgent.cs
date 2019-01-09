using ActressMas;
using MASMA_proiect.agents;
using MASMA_proiect.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASMA_Parallel_Merge.agents
{
    public class MasterAgent : WorkerAgent
    {
        private int[] arrayToSort;
        private int[] sortedArray;
        private ActressMas.Environment env;

        public MasterAgent(int[] arrayToSort, ActressMas.Environment env)
        {
            this.arrayToSort = arrayToSort;
            this.sortedArray = new int[arrayToSort.Length];
            this.env = env;
        }

        public override void Setup()
        {
            this.Send("P0", Utils.GenerateMessageContent(Actions.START_PARTITIONING, string.Join(",", arrayToSort)));
        }

        public override void Act(Message message)
        {
            string[] splittedMessage = message.Content.Split('#');
            string stringAction = splittedMessage[0];

            Enum.TryParse(stringAction, out Actions action);

            string[] arrayOfString = splittedMessage[1].Split(',');

            Console.WriteLine("\nSorted array: " + string.Join(",", arrayOfString));
            env.StopAll();
        }
    }
}
