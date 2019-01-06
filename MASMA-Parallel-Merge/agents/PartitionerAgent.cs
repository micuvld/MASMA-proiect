using ActressMas;
using MASMA_proiect.agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASMA_Parallel_Merge
{
    public class PartitionerAgent : ActressMas.Agent
    {
        public override void Act(Message message)
        {
            string[] splittedMessage = message.Content.Split('#');
            string stringAction = splittedMessage[0];
            Enum.TryParse(stringAction, out Actions action);
            int[] firstPartition;
            int[] secondPartition;
            string[] arrayOfString = splittedMessage[1].Split(',');

            switch (action)
            {
                case Actions.START_PARTITIONING:
                    partition(arrayOfString, out firstPartition, out secondPartition);
                    this.Send("P1", Actions.PARTITION + "#" + string.Join(",", firstPartition));
                    this.Send("P2", Actions.PARTITION + "#" + string.Join(",", secondPartition));
                    break;
                case Actions.PARTITION:
                    if (arrayOfString.Length > 2)
                    {
                        partition(arrayOfString, out firstPartition, out secondPartition);
                        this.Send(getOddPartitioner(), Actions.PARTITION + "#" + string.Join(",", firstPartition));
                        this.Send(getEvenPartitioner(), Actions.PARTITION + "#" + string.Join(",", secondPartition));
                        return;
                    }

                    this.Send("M" + this.Name.Substring(1, this.Name.Length - 1), 
                        Actions.MERGE + "#" + splittedMessage[1]);
                    break;

            }
        }

        public void splitArray(int[] initialArray, out int[] firstPartition, out int[] secondPartition)
        {
            int midIndex = initialArray.Length / 2;
            if (initialArray.Length % 2 != 0)
            {
                midIndex++;
            }

            firstPartition = initialArray.Skip(0).Take(midIndex).ToArray();
            secondPartition = initialArray.Skip(midIndex).Take(midIndex).ToArray();
        }

        public void partition(string[] arrayOfStrings, out int[] firstPartition, out int[] secondPartition)
        {
            int[] theArrayOfInts = Array.ConvertAll(arrayOfStrings, s => int.Parse(s));
            splitArray(theArrayOfInts, out firstPartition, out secondPartition);
        }

        public string getEvenPartitioner()
        {
            return "P" + (int.Parse(this.Name.Substring(1, this.Name.Length - 1)) * 2 + 2);
        }

        public string getOddPartitioner()
        {
            return "P" + (int.Parse(this.Name.Substring(1, this.Name.Length - 1)) * 2 + 1);
        }
    }
}
