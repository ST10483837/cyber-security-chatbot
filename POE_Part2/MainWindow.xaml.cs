using System;
using System.Windows;
using System.Windows.Input;

namespace POE_Part2
{
    public partial class MainWindow : Window
    {
        private Chatbot chatbot;
        private User user;
        private AudioPlayer audioPlayer;

        public MainWindow()
        {
            InitializeComponent();

            chatbot = new Chatbot();
            user = new User();
            audioPlayer = new AudioPlayer();

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Play voice greeting on startup
            try
            {
                await System.Threading.Tasks.Task.Delay(500); // Small delay for smooth loading
                audioPlayer.PlayGreeting();
            }
            catch (Exception ex)
            {
                AddMessage("System", $"Could not play voice greeting: {ex.Message}");
            }

            // Display ASCII art and welcome message
            AddMessage("CyberGuard Bot", "╔════════════════════════════════════════════════════╗");
            AddMessage("CyberGuard Bot", "║        CYBERSECURITY AWARENESS CHATBOT            ║");
            AddMessage("CyberGuard Bot", "╚════════════════════════════════════════════════════╝");
            AddMessage("CyberGuard Bot", "");
            AddMessage("CyberGuard Bot", "Hello! Welcome to CyberGuard Bot! 🛡️");
            AddMessage("CyberGuard Bot", "I'm your friendly cybersecurity assistant! 🐞");
            AddMessage("CyberGuard Bot", "");
            AddMessage("CyberGuard Bot", "What's your name? 💫");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessUserInput();
        }

        private void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessUserInput();
            }
        }

        private void ProcessUserInput()
        {
            string userMessage = UserInputTextBox.Text.Trim();

            // Input validation - empty message
            if (string.IsNullOrWhiteSpace(userMessage))
            {
                AddMessage("CyberGuard Bot", "Please type something! I'm here to help with cybersecurity. 💬");
                return;
            }

            // Display user message
            AddMessage("You", userMessage);
            UserInputTextBox.Clear();

            // FIRST MESSAGE - Get user's name
            if (string.IsNullOrEmpty(chatbot.UserName))
            {
                chatbot.UserName = userMessage;
                user.Name = userMessage;
                AddMessage("CyberGuard Bot", $"Nice to meet you, {chatbot.UserName}! 🎉");
                AddMessage("CyberGuard Bot", "");
                AddMessage("CyberGuard Bot", "I can help you with cybersecurity topics like:");
                AddMessage("CyberGuard Bot", "  🔐 Passwords");
                AddMessage("CyberGuard Bot", "  🎣 Phishing");
                AddMessage("CyberGuard Bot", "  🛡️ Privacy");
                AddMessage("CyberGuard Bot", "  ⚠️ Scams");
                AddMessage("CyberGuard Bot", "");
                AddMessage("CyberGuard Bot", "Try asking:");
                AddMessage("CyberGuard Bot", "  • 'What is phishing?'");
                AddMessage("CyberGuard Bot", "  • 'Give me a password tip'");
                AddMessage("CyberGuard Bot", "  • 'I'm worried about hackers'");
                return;
            }

            // EXIT COMMAND
            if (userMessage.ToLower() == "exit" || userMessage.ToLower() == "bye" || userMessage.ToLower() == "quit")
            {
                AddMessage("CyberGuard Bot", $"Goodbye, {chatbot.UserName}! Stay safe online! 👋🛡️");
                return;
            }

            // FOLLOW-UP RESPONSES ("tell me more", "another tip")
            string followUp = chatbot.HandleFollowUp(userMessage);
            if (followUp != null)
            {
                AddMessage("CyberGuard Bot", followUp);
                return;
            }

            // NORMAL CYBERSECURITY RESPONSE
            string response = chatbot.GetResponse(userMessage);

            // SENTIMENT DETECTION (adjusts response based on mood)
            string sentiment = chatbot.DetectSentiment(userMessage);
            string finalResponse = chatbot.GetSentimentAdjustedResponse(sentiment, response);

            AddMessage("CyberGuard Bot", finalResponse);

            // REMEMBER FAVORITE TOPIC (Memory feature)
            string lowerMsg = userMessage.ToLower();
            if (lowerMsg.Contains("password"))
            {
                chatbot.FavoriteTopic = "password safety";
                user.FavoriteTopic = "password safety";
            }
            else if (lowerMsg.Contains("phishing"))
            {
                chatbot.FavoriteTopic = "phishing awareness";
                user.FavoriteTopic = "phishing awareness";
            }
            else if (lowerMsg.Contains("privacy"))
            {
                chatbot.FavoriteTopic = "privacy protection";
                user.FavoriteTopic = "privacy protection";
            }
            else if (lowerMsg.Contains("scam"))
            {
                chatbot.FavoriteTopic = "scam detection";
                user.FavoriteTopic = "scam detection";
            }
        }

        private void AddMessage(string sender, string message)
        {
            // Add message to the ListBox
            ChatDisplay.Items.Add($"{sender}: {message}");

            // Auto-scroll to the bottom to show latest message
            if (ChatDisplay.Items.Count > 0)
            {
                ChatDisplay.ScrollIntoView(ChatDisplay.Items[ChatDisplay.Items.Count - 1]);
            }
        }
    }
}