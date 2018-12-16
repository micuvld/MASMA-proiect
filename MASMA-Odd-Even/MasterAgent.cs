using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActressMas;


using MASMA_proiect.agents;

namespace MASMA_Odd_Even
{
    class MasterAgent : ActressMas.Agent
    {

        int NumberOfPhases;
        int[] array;
        public List<string> comparatorAgents;
        int currentPhase = 1;
        int numberOfReceivedMessages = 0;
        bool isNumberOfElementsEven;

        public MasterAgent(int numberOfPhases, int[] array, List<string> agents, bool isNumberOfElementsEven)
        {
            this.NumberOfPhases = numberOfPhases;
            this.array = array;
            this.comparatorAgents = agents;
            this.isNumberOfElementsEven = isNumberOfElementsEven;
        }

        public override void Setup()
        {
            Console.WriteLine("Array sent to comparator agents!");
            sendArrayToAgentDueToThePhase(currentPhase, isNumberOfElementsEven);
        }

        public override void Act(Message message)
        {
            string agentName = message.Sender;
            int index = Int32.Parse(agentName.Replace("comparatorAgent", ""));

            //Console.WriteLine("Message received from :" + agentName + " calculated index = " + index);

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
                        sendAndUpdateCurrentPhase();
                        numberOfReceivedMessages = 0;
                    }
                }
                else
                {
                    array[2 * index + 1] = Int32.Parse(parameters[0]);
                    array[2 * index + 2] = Int32.Parse(parameters[1]);

                    if (numberOfReceivedMessages == array.Length / 2 - 1)
                    {
                        sendAndUpdateCurrentPhase();
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
                        sendAndUpdateCurrentPhase();
                        numberOfReceivedMessages = 0;
                    }
                }
                else
                {
                    array[2 * index + 1] = Int32.Parse(parameters[0]);
                    array[2 * index + 2] = Int32.Parse(parameters[1]);

                    if (numberOfReceivedMessages == array.Length / 2)
                    {
                        sendAndUpdateCurrentPhase();
                        numberOfReceivedMessages = 0;
                    }
                }
            }
        }

        void sendAndUpdateCurrentPhase()
        {
            currentPhase++;

            Console.WriteLine("Current phase: " + currentPhase);
            Console.WriteLine(string.Join(",", array));
            if (currentPhase <= NumberOfPhases)
            {
                sendArrayToAgentDueToThePhase(currentPhase, isNumberOfElementsEven);
            }

            numberOfReceivedMessages = 0;
        }

        void sendArrayToAgentDueToThePhase(int phase, bool isEven)
        {
            if (isEven)
            {
                if (phase % 2 != 0)
                {
                    sendForOddPhase(0);
                }
                else
                {
                    sendForEvenPhase(1);
                }
            }
            else
            {
                if (phase % 2 != 0)
                {
                    sendForOddPhase(0);
                }
                else
                {
                    sendForEvenPhase(0);
                }
            }
        }

        void sendForOddPhase(int offset)
        {
            for (int i = 0; i < comparatorAgents.Count - offset; i++)
            {
                string message = MASMA_proiect.agents.Actions.COMPARE + "#" + array[2 * i] + "," + array[(2 * i) + 1];
                Console.WriteLine("Message: " + message + " was sent to : " + comparatorAgents.ElementAt(i));
                this.Send(comparatorAgents.ElementAt(i), message);
            }
        }

        void sendForEvenPhase(int offset)
        {
            for (int i = 0; i < comparatorAgents.Count - offset; i++)
            {
                string message = MASMA_proiect.agents.Actions.COMPARE + "#" + array[2 * i + 1] + "," + array[2 * i + 2];
                Console.WriteLine("Message: " + message + " was sent to : " + comparatorAgents.ElementAt(i));
                this.Send(comparatorAgents.ElementAt(i), message);
            }
        }
    }
}
