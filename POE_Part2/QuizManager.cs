using System;
using System.Collections.Generic;

namespace POE_Part2
{
    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex;
        private int _score;
        private bool _quizCompleted;

        public bool IsQuizActive { get; private set; }

        public QuizManager()
        {
            _questions = new List<QuizQuestion>();
            _currentIndex = 0;
            _score = 0;
            _quizCompleted = false;
            IsQuizActive = false;
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            _questions.Add(new QuizQuestion
            {
                Question = "What should you do if you receive an email asking for your password?",
                Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                CorrectAnswer = "C",
                Explanation = "Reporting phishing emails helps prevent scams.",
                Topic = "Phishing"
            });

            _questions.Add(new QuizQuestion
            {
                Question = "Which of the following is a strong password?",
                Options = new List<string> { "A) password123", "B) MyBirthday1990", "C) G8!kLp#2mQ$9", "D) qwerty" },
                CorrectAnswer = "C",
                Explanation = "A strong password uses a mix of uppercase, lowercase, numbers, and symbols.",
                Topic = "Password Safety"
            });

            _questions.Add(new QuizQuestion
            {
                Question = "What is Two-Factor Authentication (2FA)?",
                Options = new List<string> { "A) A type of virus", "B) An extra layer of security requiring two verification methods", "C) A password manager", "D) A phishing scam" },
                CorrectAnswer = "B",
                Explanation = "2FA adds an extra layer of security requiring a second verification method.",
                Topic = "2FA"
            });

            _questions.Add(new QuizQuestion
            {
                Question = "What should you look for in a website URL to ensure it's secure?",
                Options = new List<string> { "A) http://", "B) https://", "C) www.", "D) .com" },
                CorrectAnswer = "B",
                Explanation = "https:// indicates the website uses encryption to protect your data.",
                Topic = "Safe Browsing"
            });

            _questions.Add(new QuizQuestion
            {
                Question = "What is social engineering in cybersecurity?",
                Options = new List<string> { "A) Building social networks", "B) Manipulating people to reveal confidential information", "C) Engineering social media platforms", "D) Creating social media posts" },
                CorrectAnswer = "B",
                Explanation = "Social engineering manipulates people into revealing sensitive information.",
                Topic = "Social Engineering"
            });

            _questions.Add(new QuizQuestion
            {
                Question = "How can you protect your privacy on social media?",
                Options = new List<string> { "A) Share everything publicly", "B) Use strong privacy settings and limit what you share", "C) Post your phone number", "D) Share your location" },
                CorrectAnswer = "B",
                Explanation = "Review your privacy settings and be mindful of what you share online.",
                Topic = "Privacy"
            });

            _questions.Add(new QuizQuestion
            {
                Question = "Which of these is a sign that your computer may have malware?",
                Options = new List<string> { "A) Computer is running faster", "B) Sudden pop-ups and slow performance", "C) You have more storage space", "D) Your internet is faster" },
                CorrectAnswer = "B",
                Explanation = "Sudden pop-ups and slow performance are common signs of malware.",
                Topic = "Malware"
            });

            _questions.Add(new QuizQuestion
            {
                Question = "What should you avoid doing on public Wi-Fi?",
                Options = new List<string> { "A) Browsing the news", "B) Checking the weather", "C) Accessing online banking", "D) Reading emails" },
                CorrectAnswer = "C",
                Explanation = "Never access sensitive information like banking on public Wi-Fi unless using a VPN.",
                Topic = "Safe Browsing"
            });

            _questions.Add(new QuizQuestion
            {
                Question = "Why are software updates important for cybersecurity?",
                Options = new List<string> { "A) They add new features", "B) They fix security vulnerabilities", "C) They make your computer faster", "D) They change the design" },
                CorrectAnswer = "B",
                Explanation = "Software updates contain critical security patches.",
                Topic = "Updates"
            });

            _questions.Add(new QuizQuestion
            {
                Question = "What is ransomware?",
                Options = new List<string> { "A) A type of software that steals passwords", "B) Malware that locks your files and demands payment", "C) A tool to back up data", "D) A type of antivirus" },
                CorrectAnswer = "B",
                Explanation = "Ransomware encrypts your files and demands payment for their release.",
                Topic = "Ransomware"
            });

            _questions.Add(new QuizQuestion
            {
                Question = "What is the 3-2-1 backup rule?",
                Options = new List<string> { "A) 3 backups, 2 days apart, 1 location", "B) 3 copies, 2 different media, 1 off-site backup", "C) 3 passwords, 2 accounts, 1 device", "D) 3 days, 2 weeks, 1 month" },
                CorrectAnswer = "B",
                Explanation = "3 copies, 2 different storage types, 1 off-site backup.",
                Topic = "Backup"
            });

            _questions.Add(new QuizQuestion
            {
                Question = "What should you do if a company you use experiences a data breach?",
                Options = new List<string> { "A) Ignore it", "B) Change your password immediately", "C) Share the news on social media", "D) Wait for someone to tell you" },
                CorrectAnswer = "B",
                Explanation = "Always change your password immediately after a data breach.",
                Topic = "Data Breach"
            });
        }

        public string StartQuiz()
        {
            _currentIndex = 0;
            _score = 0;
            _quizCompleted = false;
            IsQuizActive = true;
            ActivityLogger.Log("Quiz started");
            return GetCurrentQuestion();
        }

        public string GetCurrentQuestion()
        {
            if (_currentIndex >= _questions.Count)
            {
                IsQuizActive = false;
                _quizCompleted = true;
                return GetFinalResults();
            }

            var q = _questions[_currentIndex];
            string questionText = $"📝 Question {_currentIndex + 1} of {_questions.Count} ({q.Topic})\n\n";
            questionText += $"{q.Question}\n\n";
            foreach (var option in q.Options)
            {
                questionText += $"{option}\n";
            }
            return questionText;
        }

        public string SubmitAnswer(string answer)
        {
            if (!IsQuizActive || _quizCompleted)
                return "The quiz is not active. Type 'start quiz' to begin!";

            var q = _questions[_currentIndex];
            string userAnswer = answer.Trim().ToUpper();

            if (userAnswer != "A" && userAnswer != "B" && userAnswer != "C" && userAnswer != "D")
            {
                return $"Please answer with A, B, C, or D.\n\n{GetCurrentQuestion()}";
            }

            bool isCorrect = userAnswer == q.CorrectAnswer;

            if (isCorrect)
            {
                _score++;
                string feedback = $"✅ Correct! {q.Explanation}\n\nScore: {_score}/{_currentIndex + 1}";
                ActivityLogger.Log($"Quiz: Question {_currentIndex + 1} - Correct");
                _currentIndex++;
                return feedback + "\n\n" + GetCurrentQuestion();
            }
            else
            {
                string feedback = $"❌ Incorrect. The correct answer was {q.CorrectAnswer}.\n\n{q.Explanation}\n\nScore: {_score}/{_currentIndex + 1}";
                ActivityLogger.Log($"Quiz: Question {_currentIndex + 1} - Incorrect");
                _currentIndex++;
                return feedback + "\n\n" + GetCurrentQuestion();
            }
        }

        public string GetFinalResults()
        {
            IsQuizActive = false;
            _quizCompleted = true;
            int total = _questions.Count;
            double percentage = (double)_score / total * 100;
            string message = $"🏆 Quiz Complete!\n\n";
            message += $"Your Score: {_score} out of {total}\n";
            message += $"Percentage: {percentage:F1}%\n\n";

            if (percentage >= 90)
                message += "🌟 Excellent! You're a cybersecurity pro!";
            else if (percentage >= 70)
                message += "👏 Good job! You have solid cybersecurity knowledge!";
            else if (percentage >= 50)
                message += "📚 Not bad! Review the topics you missed and try again!";
            else
                message += "💪 Keep learning! Cybersecurity is important. Review the basics and try again!";

            ActivityLogger.Log($"Quiz completed - Score: {_score}/{total} ({percentage:F1}%)");
            return message;
        }

        public bool IsQuizComplete()
        {
            return _quizCompleted;
        }

        public void ResetQuiz()
        {
            _currentIndex = 0;
            _score = 0;
            _quizCompleted = false;
            IsQuizActive = false;
        }
    }

    public class QuizQuestion
    {
        public string Question { get; set; } = string.Empty;
        public List<string> Options { get; set; } = new List<string>();
        public string CorrectAnswer { get; set; } = string.Empty;
        public string Explanation { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
    }
}   