using System.Collections.Generic;

namespace SwansonQuotes
{
    public class FactResource
    {
        public FactResource(string language)
        {
            Language = language;
            Facts = new List<string>();
        }

        public string Language { get; set; }
        public string SkillName { get; set; }
        public List<string> Facts { get; set; }
        public string GetFactMessage { get; set; }
        public string HelpMessage { get; set; }
        public string HelpReprompt { get; set; }
        public string StopMessage { get; set; }
    }
}
