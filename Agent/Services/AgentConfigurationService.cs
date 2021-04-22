using System;

namespace Agent.Services
{
    public class AgentConfigurationService
    {
        private Pipeline _pipeline;

        public AgentConfigurationService()
        {
            _pipeline = new Pipeline();
        }
        
        public void StartConfiguration()
        {
            Console.WriteLine("Please provide a path to your code file");
            var path = Console.ReadLine();
            
            // haal content op uit die file
            var text = "combat=attackall";
            
            _pipeline.ParseString(text);
        }
    }
}