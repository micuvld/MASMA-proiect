using MASMA_proiect.agents;
using MASMA_proiect.utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MASMA_Odd_Even
{
    static class Program
    {
        static void Main(string[] args)
        {
            int[] array = Utils.GenerateRandomArray(10, 100);
            int numberOfPhases;
            int numberOfAgents;
            bool isNumberOfElementsEven;

            if (array.Length % 2 == 0)
            {
                isNumberOfElementsEven = true;
            }
            else
            {
                isNumberOfElementsEven = false;
            }

            numberOfPhases = array.Length;
            numberOfAgents = array.Length / 2;

            var env = new ActressMas.Environment();

            List<string> comparatorsList = new List<string>();
            for (int i = 0; i < numberOfAgents; i++)
            {
                ComparatorAgent ComparatorAgent = new MASMA_proiect.agents.ComparatorAgent();
                string agentName = "ComparatorAgent" + i;
                env.Add(ComparatorAgent, agentName);

                comparatorsList.Add(agentName);
                ComparatorAgent.Start();
            }

            Console.WriteLine("Number of comparator agents = " + comparatorsList.Count);
            Console.WriteLine("Number of phases = " + numberOfPhases);

            MasterAgent masterAgent = new MasterAgent(numberOfPhases, array, comparatorsList, isNumberOfElementsEven, env);
            env.Add(masterAgent, "MasterAgent");
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
