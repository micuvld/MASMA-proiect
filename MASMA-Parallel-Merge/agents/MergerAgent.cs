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
    public class MergerAgent : WorkerAgent
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
            string[] splittedMessage = message.Content.Split('#');
            string stringAction = splittedMessage[0];
            
            Enum.TryParse(stringAction, out Actions action);
            
            string[] arrayOfString = splittedMessage[1].Split(',');
            int[] responseArray = Array.ConvertAll(arrayOfString, s => int.Parse(s));

            switch (action)
            {
                case Actions.MERGE:
                    if (message.Sender[0] == 'P') //sender is a partitioner
                    {
                        ComputeSimplePartition(responseArray);
                        return;
                    }

                    UpdateLocalPartitions(responseArray);
                    if (firstPartition != null && secondPartition != null)
                    {
                        StartMerging();
                    }

                    break;

                case Actions.COMPARISON_RESULT:
                    DoMergingStep(responseArray);
                    //send next two values for comparison
                    this.Send(agentsManager.GetIdleAgent(AgentType.COMPARATOR), 
                        ComparatorAgent.serialize(firstPartition[firstPartitionIndex], secondPartition[secondPartitionIndex]));
                    break;

                default:
                    return;
            }


        }

        private void StartMerging()
        {
            this.Send(agentsManager.GetIdleAgent(AgentType.COMPARATOR), ComparatorAgent.serialize(firstPartition[0], secondPartition[0]));
            this.finalPartition = new int[firstPartition.Length + secondPartition.Length];
        }

        private void UpdateLocalPartitions(int[] partition)
        {
            if (this.firstPartition == null)
            {
                this.firstPartition = partition;
            }
            else
            {
                this.secondPartition = partition;

            }
        }

        private void ComputeSimplePartition(int[] partition)
        {
            if (partition.Length == 2)
            {
                int firstValue = partition[0];
                int secondValue = partition[1];
                if (firstValue >= secondValue)
                {
                    partition = new int[] { secondValue, firstValue };
                }
            }

            this.Send(GetParentMerger(), Utils.GenerateMessageContent(Actions.MERGE, string.Join(",", partition)));
        }

        private void DoMergingStep(int[] comparisonResult)
        {
            SetLowestElementInTheSortedArray(comparisonResult);

            if (firstPartitionIndex == firstPartition.Length)
            {
                AppendRestOfPartition(secondPartitionIndex, secondPartition);
                ContinueOrSendToMaster();
                return;
            }

            if (secondPartitionIndex == secondPartition.Length)
            {
                AppendRestOfPartition(firstPartitionIndex, firstPartition);
                ContinueOrSendToMaster();
                return;
            }
        }

        private void AppendRestOfPartition(int startIndex, int[] partition)
        {
            for (int i = startIndex; i < partition.Length; i++)
            {
                finalPartition[finalPartitionIndex++] = partition[i];
            }
        }

        private void SetLowestElementInTheSortedArray(int[] comparisonResult)
        {
            if (firstPartition[firstPartitionIndex].Equals(comparisonResult[0]))
            {
                firstPartitionIndex++;
            }
            else
            {
                secondPartitionIndex++;
            }

            finalPartition[finalPartitionIndex++] = comparisonResult[0];
        }

        private void ContinueOrSendToMaster()
        {
            if (this.Name.Equals("M0"))
            {
                this.Send(AgentType.MASTER.ToString(), Utils.GenerateMessageContent(Actions.FINISH_MERGE, string.Join(",", finalPartition)));
                return;
            }
            this.Send(GetParentMerger(), Utils.GenerateMessageContent(Actions.MERGE, string.Join(",", finalPartition)));
        }

        public string GetParentMerger()
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
