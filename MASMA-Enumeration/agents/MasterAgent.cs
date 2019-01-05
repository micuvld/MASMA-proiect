using ActressMas;
using MASMA_proiect.agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASMA_Enumeration.agents
{
    class MasterAgent : ActressMas.Agent
    {
        private AgentsManager agentsManager;
        private int[] arrayToSort;
        private int numberOfReplies;
        private int[] sortedArray;

        public MasterAgent(AgentsManager agentsManager, int[] arrayToSort)
        {
            this.agentsManager = agentsManager;
            this.arrayToSort = arrayToSort;
            this.sortedArray = new int[arrayToSort.Length];
        }

        public override void Setup()
        {
            for (int index = 0; index < arrayToSort.Length; ++index)
            {
                string enumerationAgent = agentsManager.getIdleAgent(AgentType.ENUMERATOR);
                string message = Actions.ENUMERATE + "#" + string.Join(",", arrayToSort) + "#" + index;
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
                    sortedArray[index] = value;

                    numberOfReplies++;
                    if (numberOfReplies == arrayToSort.Length)
                    {
                        Console.WriteLine(string.Join(",", sortedArray));
                    }
                    break;
            }
        }
    }
}
