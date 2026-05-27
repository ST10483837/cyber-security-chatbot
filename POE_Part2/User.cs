using System.Collections.Generic;

namespace POE_Part2
{
    public class User
    {
        public string Name { get; set; }
        public string FavoriteTopic { get; set; }
        public List<string> ConversationHistory { get; set; }

        public User()
        {
            Name = "";
            FavoriteTopic = "";
            ConversationHistory = new List<string>();
        }

        public void AddToHistory(string message)
        {
            ConversationHistory.Add(message);
        }

        public string RecallInfo()
        {
            if (!string.IsNullOrEmpty(FavoriteTopic))
                return $"I remember you're interested in {FavoriteTopic}!";
            return "Tell me what cybersecurity topics interest you!";
        }
    }
}