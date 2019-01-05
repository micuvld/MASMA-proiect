using ActressMas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MASMA_proiect.agents;

namespace MASMA_Enumeration.agents
{
    public class EnumerationAgent : WorkerAgent
    {
        private AgentsManager agentsManager;
        private int totalReplies;
        private int numbersLessThanCurrentElement;
        private int currentElement;
        private int[] theArray;
        private int indexInArray;

        public EnumerationAgent(AgentsManager agentsManager)
        {
            this.agentsManager = agentsManager;
        }

        public override void Act(Message message)
        {
            string[] splittedMessage = message.Content.Split('#');
            string stringAction = splittedMessage[0];
            Enum.TryParse(stringAction, out Actions action);

            switch (action)
            {
                case Actions.ENUMERATE:
                    Idle = false;
                    string[] theArrayAsString = splittedMessage[1].Split(',');
                    string indexAsString = splittedMessage[2];
                    theArray = Array.ConvertAll(theArrayAsString, s => int.Parse(s));
                    indexInArray = int.Parse(indexAsString);
                    currentElement = theArray[indexInArray];
                    sendToWorkers(theArray, indexInArray);
                    break;

                case Actions.COMPARISON_RESULT:
                    string[] comparisonResult = splittedMessage[1].Split(',');
                    processResponse(comparisonResult, currentElement);

                    if (totalReplies == theArray.Length)
                    {
                        string replyMessage = Actions.ENUMARTION_RESULT + "#" + currentElement + "#" + numbersLessThanCurrentElement;
                        this.Send(agentsManager.getUniqueAgent(AgentType.MASTER), replyMessage);
                        cleanup();
                    }

                    break;
            }
        }

        private void processResponse(string[] comparisonResult, int currentElement)
        {
            int[] orderedNumbers = Array.ConvertAll(comparisonResult, s => int.Parse(s));
            totalReplies++;
            if (orderedNumbers[1] == currentElement) //currentElement is the higher number
            {
                numbersLessThanCurrentElement++;
            }
        }

        private void sendToWorkers(int[] theArray, int initialIndex)
        {
            Dictionary<int, int> finalIndexes = new Dictionary<int, int>();

            for (int i = 0; i < theArray.Length; ++i)
            {
                sendForComparison(theArray[initialIndex], theArray[i]);
            }
        }

        private void sendForComparison(int a, int b)
        {
            string comparatorAgent = agentsManager.getIdleAgent(AgentType.COMPARATOR);
            this.Send(comparatorAgent, ComparatorAgent.serialize(a, b));
        }

        private void cleanup()
        {
            totalReplies = 0;
            numbersLessThanCurrentElement = 0;
            currentElement = 0;
            theArray = null;
            indexInArray = 0;
            Idle = true;
        }
    }
}
