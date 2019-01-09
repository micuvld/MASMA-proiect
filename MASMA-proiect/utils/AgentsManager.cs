using ActressMas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASMA_proiect.agents
{
    public class AgentsManager
    {
        //add a mapping between agent type and agent name
        private Dictionary<AgentType, List<WorkerAgent>> agents = new Dictionary<AgentType, List<WorkerAgent>>();
        Random random = new Random();

        public AgentsManager(Dictionary<AgentType, List<WorkerAgent>> agents)
        {
            this.agents = agents;
        }

        public AgentsManager()
        { }

        public string GetIdleAgent(AgentType type)
        {
            int agentNumber;
            do
            {
                agentNumber = random.Next(agents[type].Count);
            } while (!agents[type][agentNumber].Idle);
            return type.ToString() + agentNumber;
        }

        public string GetUniqueAgent(AgentType type)
        {
            return type.ToString();
        }

        public void SetAgents(Dictionary<AgentType, List<WorkerAgent>> agents)
        {
            this.agents = agents;
        }
    }
}
