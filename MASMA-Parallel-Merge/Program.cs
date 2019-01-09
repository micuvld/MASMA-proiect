using ActressMas;
using MASMA_Parallel_Merge.agents;
using MASMA_proiect.agents;
using MASMA_proiect.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MASMA_Parallel_Merge
{
    static class Program
    {
        static void Main()
        {
            int[] arrayToSort = Utils.GenerateRandomArray(10, 100);
            int noComparatorAgents = 100;
            var env = new ActressMas.Environment();

            int numPhases =(int) Math.Log(arrayToSort.Length, 2);
            AgentsManager agentsManager = new AgentsManager();

            List<WorkerAgent> comparatorAgents = new List<WorkerAgent>();
            for (int i = 0; i < noComparatorAgents; ++i)
            {
                string agentName = AgentType.COMPARATOR.ToString() + i;
                Agent comparatorAgent = new ComparatorAgent();
                env.Add(comparatorAgent, agentName);
                comparatorAgent.Start();
            }

            Dictionary<AgentType, List<WorkerAgent>> agentTypeCounts = new Dictionary<AgentType, List<WorkerAgent>>();
            agentTypeCounts.Add(AgentType.COMPARATOR, comparatorAgents);
            agentsManager.SetAgents(agentTypeCounts);


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

            MasterAgent masterAgent = new MasterAgent(arrayToSort, env);
            env.Add(masterAgent, AgentType.MASTER.ToString());

            Stopwatch sw = new Stopwatch();
            sw.Start();
            masterAgent.Start();

            env.WaitAll();
            sw.Stop();
            Console.WriteLine("\nParallel merge sort took {0} millis\n", sw.ElapsedMilliseconds);
            Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;
            Console.WriteLine("Bytes used: {0}\n", totalBytesOfMemoryUsed);
        }
    }
}
