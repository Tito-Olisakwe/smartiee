using SmartieeWeb.Models;
using System;
using System.Threading.Tasks;

namespace SmartieeWeb.Services
{
    /// <summary>
    /// Defines a contract for managing quiz operations, including question loading, scoring, and timing.
    /// </summary>
    public interface IQuizService
    {
        int Score { get; }
        int TotalQuestions { get; }
        bool TimeRanOut { get; }
        int LastCategoryId { get; set; }
        string LastDifficulty { get; set; }
        bool LastIsTimed { get; set; }
        DateTime StartTime { get; }
        DateTime EndTime { get; }

        /// <summary>
        /// Asynchronously loads questions based on specified criteria and prepares the quiz.
        /// </summary>
        Task LoadQuestionsAsync(int categoryId, string difficulty, bool isTimed);

        /// <summary>
        /// Retrieves a question by its index.
        /// </summary>
        Question GetQuestion(int index);

        /// <summary>
        /// Submits an answer for a question and updates the score based on correctness.
        /// </summary>
        bool SubmitAnswer(int questionIndex, int answerIndex);

        /// <summary>
        /// Marks the quiz as having run out of time.
        /// </summary>
        void MarkTimeAsRanOut();

        /// <summary>
        /// Resets the quiz state to default.
        /// </summary>
        void Reset();

        /// <summary>
        /// Determines whether more questions are available to be answered.
        /// </summary>
        bool HasMoreQuestions(int currentQuestionIndex);

        /// <summary>
        /// Starts the timer for a timed quiz session.
        /// </summary>
        void StartQuizTimer();

        /// <summary>
        /// Ends the timer for a timed quiz session.
        /// </summary>
        void EndQuizTimer();

        /// <summary>
        /// Calculates the total duration of the quiz.
        /// </summary>
        TimeSpan GetQuizDuration();
    }
}
