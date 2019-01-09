using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActressMas;


using MASMA_proiect.agents;
using MASMA_proiect.utils;

namespace MASMA_Odd_Even
{
    class MasterAgent : WorkerAgent
    {

        private int numberOfPhases;
        private int[] array;
        public List<string> comparatorAgents;
        private int currentPhase = 1;
        private int numberOfReceivedMessages = 0;
        private bool isNumberOfElementsEven;
        private ActressMas.Environment env;

        public MasterAgent(int numberOfPhases, int[] array, List<string> agents, bool isNumberOfElementsEven, ActressMas.Environment env)
        {
            this.numberOfPhases = numberOfPhases;
            this.array = array;
            this.comparatorAgents = agents;
            this.isNumberOfElementsEven = isNumberOfElementsEven;
            this.env = env;
        }

        public override void Setup()
        {
            Console.WriteLine("Array sent to comparator agents!");
            SendArrayToAgentDueToThePhase(currentPhase, isNumberOfElementsEven);
        }

        public override void Act(Message message)
        {
            string agentName = message.Sender;
            int index = Int32.Parse(agentName.Replace("ComparatorAgent", ""));

            numberOfReceivedMessages++;

            string[] splittedMessage = message.Content.Split('#');
            string action = splittedMessage[0];
            string[] parameters = splittedMessage[1].Split(',');

            if (isNumberOfElementsEven)
            {
                if (currentPhase % 2 != 0)
                {
                    array[2 * index] = Int32.Parse(parameters[0]);
                    array[2 * index + 1] = Int32.Parse(parameters[1]);

                    if (numberOfReceivedMessages == array.Length / 2)
                    {
                        SendAndUpdateCurrentPhase();
                        numberOfReceivedMessages = 0;
                    }
                }
                else
                {
                    array[2 * index + 1] = Int32.Parse(parameters[0]);
                    array[2 * index + 2] = Int32.Parse(parameters[1]);

                    if (numberOfReceivedMessages == array.Length / 2 - 1)
                    {
                        SendAndUpdateCurrentPhase();
                        numberOfReceivedMessages = 0;
                    }
                }
            }
            else
            {
                if (currentPhase % 2 != 0)
                {
                    array[2 * index] = Int32.Parse(parameters[0]);
                    array[2 * index + 1] = Int32.Parse(parameters[1]);

                    if (numberOfReceivedMessages == array.Length / 2)
                    {
                        SendAndUpdateCurrentPhase();
                        numberOfReceivedMessages = 0;
                    }
                }
                else
                {
                    array[2 * index + 1] = Int32.Parse(parameters[0]);
                    array[2 * index + 2] = Int32.Parse(parameters[1]);

                    if (numberOfReceivedMessages == array.Length / 2)
                    {
                        SendAndUpdateCurrentPhase();
                        numberOfReceivedMessages = 0;
                    }
                }
            }
        }

        void SendAndUpdateCurrentPhase()
        {
            currentPhase++;

            Console.WriteLine("Current phase: " + currentPhase);
            if (currentPhase <= numberOfPhases)
            {
                SendArrayToAgentDueToThePhase(currentPhase, isNumberOfElementsEven);
            } else
            {
                Console.WriteLine("\nSorted array: " + string.Join(",", array));
                env.StopAll();
            }

            numberOfReceivedMessages = 0;
        }

        void SendArrayToAgentDueToThePhase(int phase, bool isEven)
        {
            if (isEven)
            {
                if (phase % 2 != 0)
                {
                    SendForOddPhase(0);
                }
                else
                {
                    SendForEvenPhase(1);
                }
            }
            else
            {
                if (phase % 2 != 0)
                {
                    SendForOddPhase(0);
                }
                else
                {
                    SendForEvenPhase(0);
                }
            }
        }

        void SendForOddPhase(int offset)
        {
            for (int i = 0; i < comparatorAgents.Count - offset; i++)
            {

                string message = Utils.GenerateMessageContent(MASMA_proiect.agents.Actions.COMPARE, array[2 * i] + "," + array[(2 * i) + 1]);
                Console.WriteLine("Message: " + message + " was sent to : " + comparatorAgents.ElementAt(i));
                this.Send(comparatorAgents.ElementAt(i), message);
            }
        }

        void SendForEvenPhase(int offset)
        {
            for (int i = 0; i < comparatorAgents.Count - offset; i++)
            {
                string message = Utils.GenerateMessageContent(MASMA_proiect.agents.Actions.COMPARE, array[2 * i + 1] + "," + array[(2 * i) + 2]);
                Console.WriteLine("Message: " + message + " was sent to : " + comparatorAgents.ElementAt(i));
                this.Send(comparatorAgents.ElementAt(i), message);
            }
        }
    }
}
