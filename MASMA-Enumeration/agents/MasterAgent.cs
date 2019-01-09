using ActressMas;
using MASMA_proiect.agents;
using MASMA_proiect.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASMA_Enumeration.agents
{
    class MasterAgent : WorkerAgent
    {
        private AgentsManager agentsManager;
        private int[] arrayToSort;
        private int numberOfReplies;
        private int[] sortedArray;
        private ActressMas.Environment env;

        public MasterAgent(AgentsManager agentsManager, int[] arrayToSort, ActressMas.Environment env)
        {
            this.agentsManager = agentsManager;
            this.arrayToSort = arrayToSort;
            InitSortedArray(arrayToSort.Length);
            this.env = env;
        }

        void InitSortedArray(int length)
        {
            this.sortedArray = new int[length];
            for (int i = 0; i < length; ++i)
            {
                sortedArray[i] = Int32.MaxValue;
            }
        }

        public override void Setup()
        {
            for (int index = 0; index < arrayToSort.Length; ++index)
            {
                string enumerationAgent = agentsManager.GetIdleAgent(AgentType.ENUMERATOR);
                string message = Utils.GenerateMessageContent(Actions.ENUMERATE, string.Join(",", arrayToSort), index.ToString());
                this.Send(enumerationAgent, message);
            }
        }

        public override void Act(Message message)
        {
            string[] splittedMessage = message.Content.Split('#');
            string stringAction = splittedMessage[0];
            Enum.TryParse(stringAction, out Actions action);

            switch (action)
            {
                case Actions.ENUMARTION_RESULT:
                    int value = int.Parse(splittedMessage[1]);
                    int index = int.Parse(splittedMessage[2]) - 1; //numbersLessThanCurrentElement - 1

                    SetValueInSortedArray(value, index);

                    numberOfReplies++;
                    //the algorithm stops when a reply is received for every value in the array
                    if (numberOfReplies == arrayToSort.Length)
                    {
                        Console.WriteLine("\nSorted array: " + string.Join(",", sortedArray));
                        env.StopAll();
                    }
                    break;
            }
        }

        private void SetValueInSortedArray(int value, int index)
        {
            //if there are duplicate values in the array, the index will be decremented until the next 0 value is found,
            //to avoid overwriting the value on the same index
            while (sortedArray[index] == value)
            {
                index--;
            }
            sortedArray[index] = value;
        }
    }
}
