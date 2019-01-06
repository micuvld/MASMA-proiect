using ActressMas;
using MASMA_Parallel_Merge.agents;
using MASMA_proiect.agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MASMA_Parallel_Merge
{
    static class Program
    {
        static void Main()
        {
            int[] arrayToSort = { 9,8,7,6,5,4,3,2,1,0};
            int noComparatorAgents = 1000;
            var env = new ActressMas.Environment();

            int numPhases =(int) Math.Log(arrayToSort.Length, 2);
            AgentsManager agentsManager = new AgentsManager();

            List<Agent> comparatorAgents = new List<Agent>();
            for (int i = 0; i < noComparatorAgents; ++i)
            {
                string agentName = AgentType.COMPARATOR.ToString() + i;
                Agent comparatorAgent = new ComparatorAgent();
                env.Add(comparatorAgent, agentName);
                comparatorAgent.Start();
            }

            Dictionary<AgentType, List<Agent>> agentTypeCounts = new Dictionary<AgentType, List<Agent>>();
            agentTypeCounts.Add(AgentType.COMPARATOR, comparatorAgents);
            agentsManager.setAgents(agentTypeCounts);


            int partitionerIndex = 0;
            for (int i = 0; i < numPhases; ++i)
            {
                for (int j = 0; j < Math.Pow(2, numPhases); ++j)
                {
                    PartitionerAgent partitionerAgent = new PartitionerAgent();
                    env.Add(partitionerAgent, "P" + partitionerIndex);
                    partitionerAgent.Start();

                    MergerAgent mergerAgent = new MergerAgent(agentsManager);
                    env.Add(mergerAgent, "M" + partitionerIndex++);
                    mergerAgent.Start();
                }
            }

            MasterAgent masterAgent = new MasterAgent(arrayToSort);
            env.Add(masterAgent, AgentType.MASTER.ToString());
            masterAgent.Start();

            env.WaitAll();
        }
    }
}
