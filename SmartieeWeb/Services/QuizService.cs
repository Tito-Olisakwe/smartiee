using SmartieeWeb.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;


namespace SmartieeWeb.Services
{
    public class QuizService
    {
        private readonly HttpClient _http;
        private List<Question> _questions;
        public int Score { get; private set; }
        public int TotalQuestions => _questions?.Count ?? 0;
        public bool TimeRanOut { get; private set; }
        public int LastCategoryId { get; set; }
        public string LastDifficulty { get; set; }
        public bool LastIsTimed { get; set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public QuizService(HttpClient http)
        {
            _http = http;
            _questions = new List<Question>(); // Initialize to an empty list
            TimeRanOut = false; // Initialize to false
        }

        public async Task LoadQuestionsAsync(int categoryId, string difficulty, bool isTimed)
        {
            LastCategoryId = categoryId;
            LastDifficulty = difficulty;
            LastIsTimed = isTimed;

            var triviaData = await _http.GetFromJsonAsync<TriviaData>("TriviaData.json");
            if (triviaData != null)
            {
                var questionsQuery = triviaData.Questions
                    .Where(q => categoryId == 0 || q.CategoryId == categoryId)
                    .Where(q => difficulty.Equals("Any", StringComparison.OrdinalIgnoreCase) || q.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase));

                var random = new Random();
                if (categoryId == 0) // Random category
                {
                    _questions = questionsQuery.OrderBy(_ => random.Next()).Take(20).ToList(); // 20 questions for Random
                }
                else
                {
                    _questions = questionsQuery.OrderBy(_ => random.Next()).Take(10).ToList(); // 10 questions otherwise
                }
            }

            Score = 0;
            TimeRanOut = false;
        }

        public Question GetQuestion(int index)
        {
            return _questions[index];
        }

        public bool SubmitAnswer(int questionIndex, int answerIndex)
        {
            if (_questions[questionIndex].CorrectAnswerIndex == answerIndex)
            {
                Score++;
                return true;
            }
            return false;
        }

        // Method to indicate that time has run out
        public void MarkTimeAsRanOut()
        {
            TimeRanOut = true;
        }

        public void Reset()
        {
            _questions.Clear(); // Consider if you need to clear questions list
            Score = 0;
            TimeRanOut = false; // Reset the time ran out flag
        }

        public bool HasMoreQuestions(int currentQuestionIndex)
        {
            return currentQuestionIndex < _questions.Count;
        }

        public void StartQuizTimer()
        {
            StartTime = DateTime.Now;
        }

        public void EndQuizTimer()
        {
            EndTime = DateTime.Now;
        }

        public TimeSpan GetQuizDuration()
        {
            return EndTime - StartTime;
        }
    }
}
