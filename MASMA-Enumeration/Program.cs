using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActressMas;
using MASMA_Enumeration.agents;
using MASMA_proiect.agents;

namespace MASMA_Enumeration
{
    static class Program
    {
        private static int noEnumAgents = 11;
        private static int noComparatorAgents = 1000;
        private static int[] arrayToSort = { 1, 8, 5, -32, -2321, 2, 12, 33, 321, 3, -21321 };

        static void Main()
        {
            string agentName;
            
            var env = new ActressMas.Environment();
            AgentsManager agentsManager = new AgentsManager();

            List<Agent> enumerationAgents = new List<Agent>();
            for (int i = 0; i < noEnumAgents; ++i)
            {
                agentName = AgentType.ENUMERATOR.ToString() + i;
                Agent enumeratorAgent = new EnumerationAgent(agentsManager);
                env.Add(enumeratorAgent, agentName);
                enumerationAgents.Add(enumeratorAgent);
                enumeratorAgent.Start();
            }

            List<Agent> comparatorAgents = new List<Agent>();
            for (int i = 0; i < noComparatorAgents; ++i)
            {
                agentName = AgentType.COMPARATOR.ToString() + i;
                Agent comparatorAgent = new ComparatorAgent();
                env.Add(comparatorAgent, agentName);
                comparatorAgent.Start();
            }

            agentName = AgentType.MASTER.ToString();
            MasterAgent masterAgent = new MasterAgent(agentsManager, arrayToSort);
            env.Add(masterAgent, agentName);

            Dictionary<AgentType, List<Agent>> agentTypeCounts = new Dictionary<AgentType, List<Agent>>();
            agentTypeCounts.Add(AgentType.ENUMERATOR, enumerationAgents);
            agentTypeCounts.Add(AgentType.COMPARATOR, comparatorAgents);
            agentTypeCounts.Add(AgentType.MASTER, new List<Agent>() { masterAgent });
            agentsManager.setAgents(agentTypeCounts);

            masterAgent.Start();
            env.WaitAll();
        }
    }
}
