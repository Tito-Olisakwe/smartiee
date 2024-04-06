using Smartiee.Models;
using Smartiee.Services;
using System;
using System.Linq;
using System.Timers; // Necessary for the Timer functionality

namespace Smartiee
{
    class Program
    {
        static System.Timers.Timer quizTimer; // Timer for the quiz duration
        static bool timeExpired = false; // Flag to check if time has expired

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Smartiee, the trivia game!");
            Console.WriteLine("Get ready to test your knowledge across various topics and difficulties.\n");
            ShowCategories();
        }

        static void ShowCategories()
        {
            DataService dataService = new DataService();
            var triviaData = dataService.LoadTriviaData();

            Console.WriteLine("Please select a category by entering its number:");
            foreach (var category in triviaData.Categories)
            {
                Console.WriteLine($"{category.Id}: {category.Name}");
            }
            Console.WriteLine("0: Random");

            var selectedCategoryId = Console.ReadLine();

            // If "Random" is selected, skip difficulty selection and go straight to quiz
            if (selectedCategoryId == "0")
            {
                bool isTimed = AskForTimedQuiz();
                StartQuiz(selectedCategoryId, null, isTimed); // Pass null for difficulty as it's irrelevant for Random
            }
            else
            {
                ShowDifficultyLevels(selectedCategoryId);
            }
        }


        static void ShowDifficultyLevels(string categoryId)
        {
            Console.WriteLine("\nSelect a difficulty level:");
            Console.WriteLine("1: Easy");
            Console.WriteLine("2: Medium");
            Console.WriteLine("3: Hard");

            var levelInput = Console.ReadLine();
            string difficulty = levelInput switch
            {
                "1" => "Easy",
                "2" => "Medium",
                "3" => "Hard",
                _ => null
            };

            if (difficulty == null)
            {
                Console.WriteLine("Invalid selection. Please restart the quiz.");
                return;
            }

            bool isTimed = AskForTimedQuiz();
            StartQuiz(categoryId, difficulty, isTimed);
        }

        static bool AskForTimedQuiz()
        {
            Console.WriteLine("Do you want the quiz to be timed? (yes/no)");
            string response = Console.ReadLine().Trim().ToLower();
            return response.StartsWith("y");
        }

        static void StartQuiz(string categoryId, string difficulty, bool isTimed)
        {
            DataService dataService = new DataService();
            var triviaData = dataService.LoadTriviaData();
            IEnumerable<Question> questions;

            int timerLength = isTimed ? (categoryId == "0" ? 900000 : 300000) : 0; // 15 min for random, 5 min otherwise

            if (categoryId == "0") // Check if Random was selected
            {
                questions = triviaData.Questions
                                      .OrderBy(q => Guid.NewGuid())
                                      .Take(30); // Select 30 random questions from any category
            }
            else
            {
                questions = triviaData.Questions
                                      .Where(q => q.CategoryId.ToString() == categoryId && q.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase))
                                      .OrderBy(q => Guid.NewGuid())
                                      .Take(10); // Normally select 10 questions for specific categories
            }

            timeExpired = false; // Reset the timeExpired flag before starting
            if (isTimed)
            {
                InitializeTimer(timerLength);
            }

            int score = 0;
            DateTime startTime = DateTime.Now;

            foreach (var question in questions)
            {
                if (timeExpired)
                {
                    Console.WriteLine("\nTime has run out before you could finish the quiz.");
                    break; // Exit the loop if time has expired
                }

                Console.WriteLine($"\n{question.QuestionText}");
                for (int i = 0; i < question.Options.Length; i++)
                {
                    Console.WriteLine($"{i + 1}: {question.Options[i]}");
                }

                Console.Write("Your answer (number): ");
                var answer = Console.ReadLine();
                int answerIndex;

                if (int.TryParse(answer, out answerIndex) && answerIndex > 0 && answerIndex <= question.Options.Length)
                {
                    answerIndex -= 1; // Adjust for zero-based indexing

                    if (answerIndex == question.CorrectAnswerIndex)
                    {
                        Console.WriteLine("Correct!");
                        score++;
                    }
                    else
                    {
                        Console.WriteLine($"Incorrect! The correct answer is: {question.Options[question.CorrectAnswerIndex]}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid answer. Moving to the next question.");
                }

                Console.WriteLine($"Explanation: {question.Explanation}");
            }

            if (isTimed && quizTimer.Enabled)
            {
                quizTimer.Stop();
            }

            DisplayResults(score, questions.Count(), startTime, isTimed, timeExpired, categoryId, difficulty, isTimed);
        }

        static void InitializeTimer(int timerLength)
        {
            if (quizTimer != null)
            {
                quizTimer.Stop();
            }

            quizTimer = new System.Timers.Timer(timerLength);
            quizTimer.Elapsed += OnTimedEvent;
            quizTimer.AutoReset = false;
            quizTimer.Start();
        }

        static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            timeExpired = true;
            quizTimer.Stop();
        }

        static void DisplayResults(int score, int totalQuestions, DateTime startTime, bool isTimed, bool timeRanOut, string categoryId, string difficulty, bool isTimedOriginal)
        {
            if (timeRanOut)
            {
                double percentage = ((double)score / totalQuestions) * 100;
                Console.WriteLine("\nTime has run out before you could finish the quiz.");
                Console.WriteLine($"You managed to answer {score} out of {totalQuestions} ({percentage:F2}%) correctly before time ran out.");
            }
            else
            {
                TimeSpan timeTaken = DateTime.Now - startTime;
                double percentage = ((double)score / totalQuestions) * 100;
                string feedback = percentage >= 90 ? "Excellent! You're a trivia master!" :
                                  percentage >= 70 ? "Good job! You have a solid grasp on these topics." :
                                  percentage >= 50 ? "Not bad, but there's room for improvement." :
                                  "Looks like you need to brush up on your knowledge. Keep trying!";

                Console.WriteLine($"\nQuiz complete! Your score: {score}/{totalQuestions} ({percentage:F2}%)");
                if (isTimed)
                {
                    Console.WriteLine($"Time taken: {timeTaken.Minutes} minutes and {timeTaken.Seconds} seconds.");
                }
                Console.WriteLine(feedback);
            }

            Console.WriteLine("Would you like to play again with the same settings? (yes/no)");
            string replayResponse = Console.ReadLine().Trim().ToLower();
            if (replayResponse.StartsWith("y"))
            {
                StartQuiz(categoryId, difficulty, isTimedOriginal);
            }
            else
            {
                timeExpired = false; // Reset the flag before restarting
                ShowCategories(); // Restart the quiz
            }
        }
    }
}
