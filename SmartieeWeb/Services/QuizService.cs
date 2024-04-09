using SmartieeWeb.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;


namespace SmartieeWeb.Services
{
    /// <summary>
    /// Manages quiz operations including loading questions, scoring, and tracking quiz timing.
    /// </summary>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizService"/> with a specified HttpClient.
        /// </summary>
        /// <param name="http">The HttpClient instance used for HTTP requests.</param>
        public QuizService(HttpClient http)
        {
            _http = http;
            _questions = new List<Question>();
            TimeRanOut = false;
        }

        /// <summary>
        /// Asynchronously loads questions based on specified criteria and prepares the quiz.
        /// </summary>
        /// <param name="categoryId">The category ID for filtering questions.</param>
        /// <param name="difficulty">The difficulty level for filtering questions.</param>
        /// <param name="isTimed">Indicates if the quiz should be timed.</param>
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
                if (categoryId == 0)
                {
                    _questions = questionsQuery.OrderBy(_ => random.Next()).Take(20).ToList();
                }
                else
                {
                    _questions = questionsQuery.OrderBy(_ => random.Next()).Take(10).ToList();
                }
            }

            Score = 0;
            TimeRanOut = false;
        }

        /// <summary>
        /// Retrieves a question by its index from the loaded questions.
        /// </summary>
        /// <param name="index">The index of the question to retrieve.</param>
        /// <returns>The question at the specified index.</returns>
        public Question GetQuestion(int index)
        {
            return _questions[index];
        }

        /// <summary>
        /// Submits an answer for a question and updates the score if the answer is correct.
        /// </summary>
        /// <param name="questionIndex">The index of the question being answered.</param>
        /// <param name="answerIndex">The index of the selected answer.</param>
        /// <returns>True if the selected answer is correct; otherwise, false.</returns>
        public bool SubmitAnswer(int questionIndex, int answerIndex)
        {
            if (_questions[questionIndex].CorrectAnswerIndex == answerIndex)
            {
                Score++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Marks the quiz as having run out of time.
        /// </summary>
        public void MarkTimeAsRanOut()
        {
            TimeRanOut = true;
        }

        /// <summary>
        /// Resets the quiz state, including clearing questions and resetting the score.
        /// </summary>
        public void Reset()
        {
            _questions.Clear();
            Score = 0;
            TimeRanOut = false;
        }

        /// <summary>
        /// Determines whether there are more questions to be answered in the quiz.
        /// </summary>
        /// <param name="currentQuestionIndex">The index of the current question.</param>
        /// <returns>True if there are more questions; otherwise, false.</returns>
        public bool HasMoreQuestions(int currentQuestionIndex)
        {
            return currentQuestionIndex < _questions.Count;
        }

        /// <summary>
        /// Starts the timer for a timed quiz session.
        /// </summary>
        public void StartQuizTimer()
        {
            StartTime = DateTime.Now;
        }

        /// <summary>
        /// Ends the timer for a timed quiz session.
        /// </summary>
        public void EndQuizTimer()
        {
            EndTime = DateTime.Now;
        }

        /// <summary>
        /// Calculates the duration of the quiz from start to end.
        /// </summary>
        /// <returns>The duration of the quiz.</returns>
        public TimeSpan GetQuizDuration()
        {
            return EndTime - StartTime;
        }
    }
}
