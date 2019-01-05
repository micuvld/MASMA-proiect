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
        private Dictionary<AgentType, List<Agent>> agents = new Dictionary<AgentType, List<Agent>>();
        Random random = new Random();

        public AgentsManager(Dictionary<AgentType, List<Agent>> agents)
        {
            this.agents = agents;
        }

        public AgentsManager()
        { }

        public string getIdleAgent(AgentType type)
        {
            switch(type)
            {
                case AgentType.COMPARATOR:
                    int agentNumber = random.Next(agents[type].Count);
                    return type.ToString() + agentNumber;
                default:
                    List<WorkerAgent> agentList = agents[type].ConvertAll<WorkerAgent>(a => (WorkerAgent)a);
                    return agentList.Find(agent => agent.Idle).Name;                  
            }
            
        }

        public string getUniqueAgent(AgentType type)
        {
            return type.ToString();
        }

        public void setAgents(Dictionary<AgentType, List<Agent>> agents)
        {
            this.agents = agents;
        }
    }
}
