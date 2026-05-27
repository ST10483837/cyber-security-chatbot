using System;
using System.Collections.Generic;
using System.Threading;

namespace CyberGuard_Bot
{
    public class Chatbot
    {
        private Dictionary<string, string> responses;
        private Random random;

        public Chatbot()
        {
            responses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            random = new Random();
            LoadResponses();
        }

        private void LoadResponses()
        {
            // Basic responses required by rubric
            responses["how are you"] = "I'm functioning perfectly! Ready to help you learn about cybersecurity.";
            responses["what is your purpose"] = "My purpose is to educate you about staying safe online! I can teach you about passwords, phishing, and safe browsing.";
            responses["what can i ask you about"] = "You can ask me about: password safety, phishing attacks, and safe browsing habits!";
            responses["what can you help me with"] = "I specialize in cybersecurity topics. Try asking: 'What is phishing?' or 'How do I create a strong password?'";

            // Cybersecurity topics (password safety, phishing, safe browsing)
            responses["password"] = "🔐 PASSWORD SAFETY: Use a unique password for each account. Make it at least 12 characters with uppercase, lowercase, numbers, and symbols. Consider using a password manager!";
            responses["phishing"] = "🎣 PHISHING: Phishing is when scammers pretend to be legitimate companies to steal your info. Never click suspicious links or share personal info via email!";
            responses["safe browsing"] = "🌐 SAFE BROWSING: Always check for 'https://' in URLs, avoid clicking pop-up ads, and never download files from untrusted websites.";
            responses["strong password"] = "A strong password has: 12+ characters, uppercase letters, lowercase letters, numbers, and symbols. Example: G8!kLp#2mQ$9";
            responses["2fa"] = "Two-Factor Authentication adds an extra layer of security. Even if someone steals your password, they need your phone to log in!";
            responses["virus"] = "Protect against viruses by keeping software updated, using antivirus software, and avoiding suspicious downloads.";
        }

        // String manipulation (required by rubric)
        public string ProcessUserInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return "I didn't quite understand that. Could you rephrase?";
            }

            // String manipulation: Trim, lower case
            string processedInput = input.Trim().ToLower();

            // Check for keywords
            foreach (var key in responses.Keys)
            {
                if (processedInput.Contains(key))
                {
                    return responses[key];
                }
            }

            return "I didn't quite understand that. Could you rephrase? Try asking about passwords, phishing, or safe browsing!";
        }

        // Typing effect for better UI
        public void TypeText(string text, int delay = 25)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }

        // Colored text method
        public void WriteColored(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }

        // Display ASCII art (cybersecurity themed logo)
        public void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string art = @"
    ╔══════════════════════════════════════════════════════════════════╗
    ║                                                                  ║
    ║      ██████╗██╗   ██╗██████╗ ███████╗██████╗  ██████╗ ████████╗ ║
    ║     ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗██╔═══██╗╚══██╔══╝ ║
    ║     ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝██║   ██║   ██║    ║
    ║     ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗██║   ██║   ██║    ║
    ║     ╚██████╗   ██║   ██████╔╝███████╗██║  ██║╚██████╔╝   ██║    ║
    ║      ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝ ╚═════╝    ╚═╝    ║
    ║                                                                  ║
    ║           ██████╗  █████╗ ██╗    ██╗ █████╗ ██████╗ ███████╗    ║
    ║          ██╔══██╗██╔══██╗██║    ██║██╔══██╗██╔══██╗██╔════╝    ║
    ║          ██████╔╝███████║██║ █╗ ██║███████║██████╔╝███████╗    ║
    ║          ██╔══██╗██╔══██║██║███╗██║██╔══██║██╔══██╗╚════██║    ║
    ║          ██████╔╝██║  ██║╚███╔███╔╝██║  ██║██████╔╝███████║    ║
    ║          ╚═════╝ ╚═╝  ╚═╝ ╚══╝╚══╝ ╚═╝  ╚═╝╚═════╝ ╚══════╝    ║
    ║                                                                  ║
    ║              CYBERSECURITY AWARENESS CHATBOT                     ║
    ║                                                                  ║
    ╚══════════════════════════════════════════════════════════════════╝";
            Console.WriteLine(art);
            Console.ResetColor();
            Console.WriteLine();
        }

        // Border method for visual structure
        public void DrawBorder(string title = "")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("┌" + new string('─', 78) + "┐");
            if (!string.IsNullOrEmpty(title))
            {
                int padding = (78 - title.Length) / 2;
                Console.WriteLine("│" + new string(' ', padding) + title + new string(' ', 78 - padding - title.Length) + "│");
                Console.WriteLine("├" + new string('─', 78) + "┤");
            }
            Console.ResetColor();
        }

        public void DrawBottomBorder()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("└" + new string('─', 78) + "┘");
            Console.ResetColor();
        }
    }
}
  