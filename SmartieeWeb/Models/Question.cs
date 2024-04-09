namespace SmartieeWeb.Models
{
    /// <summary>
    /// Represents a single quiz question, including details like category ID, difficulty, options, and the correct answer.
    /// </summary>
    public class Question
    {
        public int CategoryId { get; set; }
        public string Difficulty { get; set; }
        public string QuestionText { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public string[] Options { get; set; }
        public string Explanation { get; set; }
    }
}
