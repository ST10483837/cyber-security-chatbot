using System;
using System.Collections.Generic;

namespace POE_Part2
{
    public class Chatbot
    {
        private Dictionary<string, string> keywordResponses;
        private Dictionary<string, List<string>> randomResponses;
        private Random random;
        private string lastTopic;

        public string UserName { get; set; } = string.Empty;
        public string FavoriteTopic { get; set; } = string.Empty;
        public Chatbot()
        {
            keywordResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            randomResponses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            random = new Random();
            lastTopic = "";
            LoadKeywords();
            LoadRandomResponses();
        }

        private void LoadKeywords()
        {
            keywordResponses["password"] = "🔐 Create strong passwords with 12+ characters, uppercase, lowercase, numbers, and symbols! Never reuse passwords across different sites.";
            keywordResponses["scam"] = "⚠️ Watch for unsolicited calls, emails, or texts asking for personal info. Legitimate companies will NEVER ask for your password!";
            keywordResponses["privacy"] = "🛡️ Protect your privacy by: using privacy settings on social media, being careful what you share online, and using a VPN on public Wi-Fi.";
            keywordResponses["phishing"] = "🎣 Phishing attacks try to trick you into clicking bad links. Always check the sender's email address and hover over links before clicking!";
            keywordResponses["2fa"] = "🔑 Two-Factor Authentication adds an extra layer of security. Turn it on for all important accounts like email, banking, and social media!";
            keywordResponses["virus"] = "🦠 Keep your antivirus software updated and don't download files from untrusted sources or suspicious websites!";
            keywordResponses["hack"] = "💀 To avoid getting hacked: use strong passwords, enable 2FA, keep software updated, and be careful what you click!";
        }

        private void LoadRandomResponses()
        {
            randomResponses["phishing"] = new List<string>
            {
                "🎣 Tip 1: Always check the sender's email address - scammers use fake addresses that look similar to real ones!",
                "🎣 Tip 2: Hover over links before clicking to see where they really go. Don't click if the URL looks suspicious!",
                "🎣 Tip 3: Never enter personal info after clicking an email link - go directly to the website by typing the URL yourself!",
                "🎣 Tip 4: Look for spelling errors and urgent language like 'Act Now!' - these are common phishing signs!"
            };

            randomResponses["password tips"] = new List<string>
            {
                "🔐 Use a password manager to generate and store unique passwords for each account!",
                "🔐 Enable 2FA on all accounts that offer it - it's the best protection after a strong password!",
                "🔐 Never use 'password123' or your birthday - hackers try these first!",
                "🔐 Change passwords immediately if a site reports a data breach involving your account!"
            };
        }

        public string GetResponse(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please type something! I'm here to help with cybersecurity.";

            string input = userInput.ToLower().Trim();

            if (input.Contains("phishing tip") || (input.Contains("phishing") && input.Contains("tip")))
            {
                lastTopic = "phishing";
                return GetRandomResponse("phishing");
            }

            if (input.Contains("password tip") || (input.Contains("password") && input.Contains("tip")))
            {
                lastTopic = "password tips";
                return GetRandomResponse("password tips");
            }

            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    lastTopic = keyword;
                    return keywordResponses[keyword];
                }
            }

            return "I'm not sure I understand. Can you try rephrasing?\n\nTry asking about:\n• passwords\n• phishing\n• privacy\n• scams\n\nOr ask for a 'phishing tip' or 'password tip'!";
        }

        private string GetRandomResponse(string topic)
        {
            if (randomResponses.ContainsKey(topic) && randomResponses[topic].Count > 0)
            {
                int index = random.Next(randomResponses[topic].Count);
                return randomResponses[topic][index];
            }
            return "I have tips on that topic! Try asking for a 'phishing tip' or 'password tip'.";
        }

        public string GetAnotherTip()
        {
            if (!string.IsNullOrEmpty(lastTopic) && randomResponses.ContainsKey(lastTopic))
            {
                int index = random.Next(randomResponses[lastTopic].Count);
                return $"Here's another one! {randomResponses[lastTopic][index]}";
            }
            return "Ask me for a specific tip like 'phishing tip' or 'password tip' first, then say 'tell me more' or 'another tip'!";
        }

        public string HandleFollowUp(string userInput)
        {
            string input = userInput.ToLower();

            if (input.Contains("tell me more") || input.Contains("another tip") || input.Contains("explain more") || input.Contains("more tips"))
            {
                return GetAnotherTip();
            }

            if (input.Contains("remember me") || input.Contains("what do you know about me"))
            {
                if (!string.IsNullOrEmpty(UserName))
                {
                    string topicInfo = string.IsNullOrEmpty(FavoriteTopic)
                        ? "You haven't told me what cybersecurity topic interests you yet!"
                        : $"You're interested in {FavoriteTopic}!";
                    return $"I remember you! Your name is {UserName}. {topicInfo}";
                }
                return "I don't know much about you yet. Tell me your name and what topics interest you!";
            }

            return null;
        }

        public string DetectSentiment(string userInput)
        {
            string input = userInput.ToLower();

            if (input.Contains("worried") || input.Contains("scared") || input.Contains("nervous") ||
                input.Contains("afraid") || input.Contains("hacked") || input.Contains("stolen"))
                return "worried";

            if (input.Contains("curious") || input.Contains("interested") || input.Contains("want to learn") ||
                input.Contains("tell me") || input.Contains("how do i") || input.Contains("what is"))
                return "curious";

            if (input.Contains("frustrated") || input.Contains("confused") || input.Contains("don't understand") ||
                input.Contains("too hard") || input.Contains("complicated"))
                return "frustrated";

            return "neutral";
        }

        public string GetSentimentAdjustedResponse(string sentiment, string originalResponse)
        {
            switch (sentiment)
            {
                case "worried":
                    return $"Don't worry, I'm here to help you stay safe online! 😊\n\n{originalResponse}\n\nYou're taking the right step by learning about cybersecurity!";

                case "curious":
                    return $"That's great you're curious about cybersecurity! 💡\n\n{originalResponse}\n\nWant another tip about this? Just ask for 'another tip'!";

                case "frustrated":
                    return $"I understand cybersecurity can be confusing at first! Let me explain simply. 🤝\n\n{originalResponse}\n\nTake it one step at a time - you've got this!";

                default:
                    return originalResponse;
            }
        }
    }
}