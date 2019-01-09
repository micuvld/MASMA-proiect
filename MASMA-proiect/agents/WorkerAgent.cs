using ActressMas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MASMA_proiect.agents
{
    public class WorkerAgent : Agent
    {
        private bool idle = true;

        public bool Idle { get => idle; set => idle = value; }

        public void Send(string receiver, string content, string conversationId = "")
        {
            Console.WriteLine(this.Name + " -> " + receiver + ": " + content);
            base.Send(receiver, content, conversationId);
        }
    }
}
