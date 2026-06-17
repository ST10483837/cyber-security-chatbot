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
        private TaskManager taskManager;
        private QuizManager quizManager;

        public MainWindow()
        {
            InitializeComponent();

            chatbot = new Chatbot();
            user = new User();
            audioPlayer = new AudioPlayer();
            taskManager = new TaskManager();
            quizManager = new QuizManager();

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Play voice greeting on startup
            try
            {
                await System.Threading.Tasks.Task.Delay(500);
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

            // Load tasks from database
            LoadTasks();
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

        // ================================================================
        // QUIZ BUTTON CLICK
        // ================================================================
        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            AddMessage("CyberGuard Bot", "🎯 Starting Cybersecurity Quiz!");
            AddMessage("CyberGuard Bot", quizManager.StartQuiz());
        }

        // ================================================================
        // MAIN PROCESSING METHOD
        // ================================================================
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
                AddMessage("CyberGuard Bot", "");
                AddMessage("CyberGuard Bot", "💡 NEW: You can also say:");
                AddMessage("CyberGuard Bot", "  • 'Add a task to enable 2FA'");
                AddMessage("CyberGuard Bot", "  • 'Remind me to update my password'");
                AddMessage("CyberGuard Bot", "  • 'Show activity log'");
                AddMessage("CyberGuard Bot", "  • 'Start quiz' to test your knowledge!");
                return;
            }

            // EXIT COMMAND
            if (userMessage.ToLower() == "exit" || userMessage.ToLower() == "bye" || userMessage.ToLower() == "quit")
            {
                AddMessage("CyberGuard Bot", $"Goodbye, {chatbot.UserName}! Stay safe online! 👋🛡️");
                return;
            }

            // ================================================================
            // CHECK FOR QUIZ INTENT
            // ================================================================
            if (userMessage.ToLower().Contains("start quiz") ||
                userMessage.ToLower().Contains("take quiz") ||
                userMessage.ToLower().Contains("quiz me") ||
                userMessage.ToLower().Contains("play the game") ||
                userMessage.ToLower().Contains("test my knowledge"))
            {
                AddMessage("CyberGuard Bot", "🎯 Starting Cybersecurity Quiz!");
                AddMessage("CyberGuard Bot", quizManager.StartQuiz());
                return;
            }

            // ================================================================
            // CHECK FOR QUIZ ANSWER
            // ================================================================
            if (quizManager.IsQuizActive && !quizManager.IsQuizComplete())
            {
                string input = userMessage.ToUpper().Trim();
                if (input == "A" || input == "B" || input == "C" || input == "D")
                {
                    string result = quizManager.SubmitAnswer(input);
                    AddMessage("CyberGuard Bot", result);

                    if (quizManager.IsQuizComplete())
                    {
                        AddMessage("CyberGuard Bot", "🎉 Thanks for playing! Type 'start quiz' to play again.");
                    }
                    return;
                }
                else
                {
                    AddMessage("CyberGuard Bot", "Please answer with A, B, C, or D.");
                    AddMessage("CyberGuard Bot", quizManager.GetCurrentQuestion());
                    return;
                }
            }

            // ================================================================
            // CHECK FOR TASK INTENT (NLP)
            // ================================================================
            string taskResponse = ProcessTaskIntent(userMessage);
            if (taskResponse != null)
            {
                AddMessage("CyberGuard Bot", taskResponse);
                LoadTasks();
                return;
            }

            // ================================================================
            // CHECK FOR ACTIVITY LOG INTENT
            // ================================================================
            if (userMessage.ToLower().Contains("activity log") ||
                userMessage.ToLower().Contains("what have you done") ||
                userMessage.ToLower().Contains("show log") ||
                userMessage.ToLower().Contains("recent actions"))
            {
                AddMessage("CyberGuard Bot", ActivityLogger.GetRecentLog(10));
                return;
            }

            // CHECK FOR "SHOW MORE" LOG
            if (userMessage.ToLower().Contains("show more") && userMessage.ToLower().Contains("log"))
            {
                AddMessage("CyberGuard Bot", ActivityLogger.GetFullLog());
                return;
            }

            // ================================================================
            // FOLLOW-UP RESPONSES ("tell me more", "another tip")
            // ================================================================
            string followUp = chatbot.HandleFollowUp(userMessage);
            if (followUp != null)
            {
                AddMessage("CyberGuard Bot", followUp);
                return;
            }

            // ================================================================
            // NORMAL CYBERSECURITY RESPONSE
            // ================================================================
            string response = chatbot.GetResponse(userMessage);

            // SENTIMENT DETECTION
            string sentiment = chatbot.DetectSentiment(userMessage);
            string finalResponse = chatbot.GetSentimentAdjustedResponse(sentiment, response);

            AddMessage("CyberGuard Bot", finalResponse);

            // REMEMBER FAVORITE TOPIC
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

        // ================================================================
        // PROCESS TASK INTENT (NLP)
        // ================================================================
        private string ProcessTaskIntent(string userMessage)
        {
            string input = userMessage.ToLower().Trim();

            if (input.Contains("add task") || input.Contains("add a task") ||
                input.Contains("create task") || input.Contains("new task"))
            {
                string taskDescription = userMessage;
                string[] keywords = { "add task", "add a task", "create task", "new task" };
                foreach (string keyword in keywords)
                {
                    if (input.Contains(keyword))
                    {
                        int index = input.IndexOf(keyword) + keyword.Length;
                        if (index < userMessage.Length)
                        {
                            taskDescription = userMessage.Substring(index).Trim();
                            if (taskDescription.StartsWith("-") || taskDescription.StartsWith(":"))
                                taskDescription = taskDescription.Substring(1).Trim();
                            break;
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(taskDescription) || taskDescription.Length < 3)
                {
                    return "What task would you like to add? Please describe it! 📋";
                }

                string result = taskManager.AddTask(taskDescription, "Added via chat", "");
                ActivityLogger.Log($"NLP recognised task intent: '{taskDescription}'");
                return result + " Would you like to set a reminder? (Say 'Remind me in X days')";
            }

            if (input.Contains("remind me") || input.Contains("set a reminder") ||
                input.Contains("remind me to") || input.Contains("don't forget"))
            {
                string taskName = "";
                string days = "";

                if (input.Contains("in") && input.Contains("day"))
                {
                    string[] words = input.Split(' ');
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (words[i] == "in" && i + 1 < words.Length)
                        {
                            days = words[i + 1];
                            if (i + 2 < words.Length && (words[i + 2].Contains("day") || words[i + 2].Contains("days")))
                            {
                                break;
                            }
                        }
                    }
                }

                if (input.Contains("remind me to"))
                {
                    int index = input.IndexOf("remind me to") + 12;
                    if (index < userMessage.Length)
                    {
                        taskName = userMessage.Substring(index).Trim();
                        if (taskName.ToLower().Contains("in") && taskName.ToLower().Contains("day"))
                        {
                            int dayIndex = taskName.ToLower().IndexOf("in");
                            if (dayIndex > 0)
                                taskName = taskName.Substring(0, dayIndex).Trim();
                        }
                    }
                }
                else if (input.Contains("remind me about"))
                {
                    int index = input.IndexOf("remind me about") + 15;
                    if (index < userMessage.Length)
                    {
                        taskName = userMessage.Substring(index).Trim();
                    }
                }
                else
                {
                    taskName = userMessage;
                }

                if (string.IsNullOrWhiteSpace(taskName) || taskName.Length < 3)
                {
                    return "What would you like me to remind you about? 🤔";
                }

                string reminder = string.IsNullOrEmpty(days) ? "" : $"In {days} days";
                string result = taskManager.AddTask(taskName, $"Reminder task: {reminder}", reminder);
                ActivityLogger.Log($"NLP recognised reminder intent: '{taskName}' for {reminder}");

                if (string.IsNullOrEmpty(days))
                {
                    return result + " I've saved it. Would you like me to remind you in a specific number of days?";
                }
                else
                {
                    return result + $" ✅ I'll remind you in {days} days!";
                }
            }

            return null;
        }

        // ================================================================
        // TASK ASSISTANT METHODS
        // ================================================================
        private void LoadTasks()
        {
            try
            {
                var tasks = taskManager.GetAllTasks();
                TaskListBox.Items.Clear();

                if (tasks.Count == 0)
                {
                    TaskListBox.Items.Add("📭 No tasks yet. Add one above!");
                    return;
                }

                foreach (var task in tasks)
                {
                    string status = task.IsComplete ? "✅" : "⏳";
                    TaskListBox.Items.Add($"{status} {task.Title} - {task.Description} (Reminder: {task.Reminder ?? "None"})");
                }
            }
            catch (Exception ex)
            {
                AddMessage("System", $"Error loading tasks: {ex.Message}");
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleTextBox.Text.Trim();
            string description = TaskDescriptionTextBox.Text.Trim();
            string reminder = TaskReminderTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                AddMessage("CyberGuard Bot", "Please enter a task title! 📋");
                return;
            }

            string result = taskManager.AddTask(title, description, reminder);
            AddMessage("CyberGuard Bot", result);
            ActivityLogger.Log($"Task added via GUI: '{title}'");

            TaskTitleTextBox.Clear();
            TaskDescriptionTextBox.Clear();
            TaskReminderTextBox.Clear();

            LoadTasks();
        }

        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedIndex == -1)
            {
                AddMessage("CyberGuard Bot", "Please select a task to complete! ✅");
                return;
            }

            try
            {
                var tasks = taskManager.GetAllTasks();
                var selectedTask = tasks[TaskListBox.SelectedIndex];

                string result = taskManager.MarkAsComplete(selectedTask.Id);
                AddMessage("CyberGuard Bot", result);
                LoadTasks();
            }
            catch (Exception ex)
            {
                AddMessage("System", $"Error: {ex.Message}");
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedIndex == -1)
            {
                AddMessage("CyberGuard Bot", "Please select a task to delete! 🗑️");
                return;
            }

            try
            {
                var tasks = taskManager.GetAllTasks();
                var selectedTask = tasks[TaskListBox.SelectedIndex];

                string result = taskManager.DeleteTask(selectedTask.Id);
                AddMessage("CyberGuard Bot", result);
                LoadTasks();
            }
            catch (Exception ex)
            {
                AddMessage("System", $"Error: {ex.Message}");
            }
        }

        private void TaskListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Optional
        }

        // ================================================================
        // ADD MESSAGE TO CHAT
        // ================================================================
        private void AddMessage(string sender, string message)
        {
            ChatDisplay.Items.Add($"{sender}: {message}");

            if (ChatDisplay.Items.Count > 0)
            {
                ChatDisplay.ScrollIntoView(ChatDisplay.Items[ChatDisplay.Items.Count - 1]);
            }
        }
    }
}