using ActressMas;
using MASMA_proiect.agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASMA_Parallel_Merge.agents
{
    public class MergerAgent : ActressMas.Agent
    {

        private AgentsManager agentsManager;

        int[] firstPartition;
        int[] secondPartition;
        int[] finalPartition;

        int firstPartitionIndex = 0;
        int secondPartitionIndex = 0;
        int finalPartitionIndex = 0;

        public MergerAgent(AgentsManager agentsManager)
        {
            this.agentsManager = agentsManager;
        }

        public override void Act(Message message)
        {
            Console.WriteLine(message.Content);

          
            string[] splittedMessage = message.Content.Split('#');
            string stringAction = splittedMessage[0];
            
            Enum.TryParse(stringAction, out Actions action);
            
            string[] arrayOfString = splittedMessage[1].Split(',');
            int[] integerArray = Array.ConvertAll(arrayOfString, s => int.Parse(s));

            switch (action)
            {
                case Actions.MERGE:
                    if (message.Sender[0] == 'P')
                    {
                        if (integerArray.Length == 2)
                        {
                            int firstValue = integerArray[0];
                            int secondValue = integerArray[1];
                            if (firstValue >= secondValue)
                            {
                                integerArray = new int[] { secondValue, firstValue };
                            }
                        }

                        this.Send(getParentMerger(), Actions.MERGE + "#" + string.Join(",", integerArray));
                        return;
                    }

                    if (this.firstPartition == null)
                    {
                        this.firstPartition = integerArray;
                    }
                    else
                    {
                        this.secondPartition = integerArray;
                        this.Send(agentsManager.getIdleAgent(AgentType.COMPARATOR), ComparatorAgent.serialize(firstPartition[0], secondPartition[0]));
                        this.finalPartition = new int[firstPartition.Length + secondPartition.Length];
                    }
                    break;

                case Actions.COMPARISON_RESULT:
                    if (firstPartition[firstPartitionIndex].Equals(integerArray[0]))
                    {
                        firstPartitionIndex++;
                    } else
                    {
                        secondPartitionIndex++;
                    }
                    finalPartition[finalPartitionIndex++] = integerArray[0];

                    if(firstPartitionIndex == firstPartition.Length)
                    {
                        for(int i = secondPartitionIndex; secondPartitionIndex < secondPartition.Length; secondPartitionIndex++)
                        {
                            finalPartition[finalPartitionIndex++] = secondPartition[secondPartitionIndex];
                        }

                        if (this.Name.Equals("M0"))
                        {
                            this.Send(AgentType.MASTER.ToString(), Actions.FINISH_MERGE + "#" + string.Join(",", finalPartition));
                            return;
                        }
                            this.Send(getParentMerger(), Actions.MERGE + "#" + string.Join(",", finalPartition));
                        return;
                    }

                    if (secondPartitionIndex == secondPartition.Length)
                    {
                        for (int i = firstPartitionIndex; firstPartitionIndex < firstPartition.Length; firstPartitionIndex++)
                        {
                            finalPartition[finalPartitionIndex++] = firstPartition[firstPartitionIndex];
                        }

                        if (this.Name.Equals("M0"))
                        {
                            this.Send(AgentType.MASTER.ToString(), Actions.FINISH_MERGE + "#" + string.Join(",", finalPartition));
                            return;
                        }
                        this.Send(getParentMerger(), Actions.MERGE + "#" + string.Join(",", finalPartition));
                        return;
                    }
                    this.Send(agentsManager.getIdleAgent(AgentType.COMPARATOR), ComparatorAgent.serialize(firstPartition[firstPartitionIndex], secondPartition[secondPartitionIndex]));


                    break;

                default:
                    return;
            }


        }

        public string getParentMerger()
        {
            int numberOfCurrentMerger = int.Parse(this.Name.Substring(1, this.Name.Length - 1));
            if(numberOfCurrentMerger % 2 == 0)
            {
                return "M" + (numberOfCurrentMerger / 2 - 1 );

            } else
            {
                return "M" + (numberOfCurrentMerger / 2);
            }
        }

    }
}
