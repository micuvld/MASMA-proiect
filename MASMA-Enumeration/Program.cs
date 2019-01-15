using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActressMas;
using MASMA_Enumeration.agents;
using MASMA_proiect.agents;
using MASMA_proiect.utils;

namespace MASMA_Enumeration
{
    static class Program
    {
        private static int noEnumAgents = 1000;
        private static int noComparatorAgents = 1000;
        private static int[] arrayToSort = Utils.GenerateRandomArray(10000, 100);

        static void Main()
        {
            string agentName;
            
            var env = new ActressMas.Environment();
            AgentsManager agentsManager = new AgentsManager();

            List<WorkerAgent> enumerationAgents = new List<WorkerAgent>();
            for (int i = 0; i < noEnumAgents; ++i)
            {
                agentName = AgentType.ENUMERATOR.ToString() + i;
                WorkerAgent enumeratorAgent = new EnumerationAgent(agentsManager);
                env.Add(enumeratorAgent, agentName);
                enumerationAgents.Add(enumeratorAgent);
                enumeratorAgent.Start();
            }

            List<WorkerAgent> comparatorAgents = new List<WorkerAgent>();
            for (int i = 0; i < noComparatorAgents; ++i)
            {
                agentName = AgentType.COMPARATOR.ToString() + i;
                WorkerAgent comparatorAgent = new ComparatorAgent();
                env.Add(comparatorAgent, agentName);
                comparatorAgents.Add(comparatorAgent);
                comparatorAgent.Start();
            }

            agentName = AgentType.MASTER.ToString();
            MasterAgent masterAgent = new MasterAgent(agentsManager, arrayToSort, env);
            env.Add(masterAgent, agentName);

            Dictionary<AgentType, List<WorkerAgent>> agentTypeCounts = new Dictionary<AgentType, List<WorkerAgent>>();
            agentTypeCounts.Add(AgentType.ENUMERATOR, enumerationAgents);
            agentTypeCounts.Add(AgentType.COMPARATOR, comparatorAgents);
            agentTypeCounts.Add(AgentType.MASTER, new List<WorkerAgent>() { masterAgent });
            agentsManager.SetAgents(agentTypeCounts);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            masterAgent.Start();

            env.WaitAll();
            sw.Stop();
            Console.WriteLine("\nEnumeration sort took {0} millis\n", sw.ElapsedMilliseconds);
            Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;
            Console.WriteLine("Bytes used: {0}\n", totalBytesOfMemoryUsed);
        }
    }
}
