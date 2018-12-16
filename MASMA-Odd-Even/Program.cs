using MASMA_proiect.agents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MASMA_Odd_Even
{
    static class Program
    {
        static void Main(string[] args)
        {
            int[] array = { 2, 1, 8, 5, -32, -2321, 2,2,12,12,33,12, 321, 3, -21321};
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
                string agentName = "comparatorAgent" + i;
                env.Add(ComparatorAgent, agentName);

                comparatorsList.Add(agentName);
                ComparatorAgent.Start();
            }

            Console.WriteLine("Number of comparator agents = " + comparatorsList.Count);
            Console.WriteLine("Number of phases = " + numberOfPhases);
            comparatorsList.ForEach(ag => Console.Write(ag));


            MasterAgent MasterAgent = new MASMA_Odd_Even.MasterAgent(numberOfPhases, array, comparatorsList, isNumberOfElementsEven);
            
            env.Add(MasterAgent, "masterAgent");
            MasterAgent.Start();

            Console.WriteLine("Main started");
            env.WaitAll();
        }
    }
}
