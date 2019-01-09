using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActressMas;
using MASMA_proiect.utils;

namespace MASMA_proiect.agents
{
    public class ComparatorAgent : WorkerAgent
    {
        public override void Act(Message message)
        {
            string[] splittedMessage = message.Content.Split('#');
            string action = splittedMessage[0];
            string[] parameters = splittedMessage[1].Split(',');

            if (action.Equals(Actions.COMPARE.ToString()))
            {
                int firstValue = Int32.Parse(parameters[0]);
                int secondValue = Int32.Parse(parameters[1]);
                string sortedValues;

                if (firstValue <= secondValue)
                {
                    sortedValues = firstValue + "," + secondValue;
                } else
                {
                    sortedValues = secondValue + "," + firstValue;
                }

                this.Send(message.Sender, Utils.GenerateMessageContent(Actions.COMPARISON_RESULT, sortedValues));
            }
        }

        public static string serialize(int a, int b)
        {
            return Utils.GenerateMessageContent(MASMA_proiect.agents.Actions.COMPARE, + a + "," + b);
        }

        public static string serialize(string a, string b)
        {
            return Utils.GenerateMessageContent(MASMA_proiect.agents.Actions.COMPARE, a + "," + b);
        }
    }
}