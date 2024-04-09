namespace SmartieeWeb.Models
{
    /// <summary>
    /// Contains all data necessary for running a trivia quiz, including categories and questions.
    /// </summary>
    public class TriviaData
    {
        public List<SmartieeWeb.Models.Category> Categories { get; set; }
        public List<SmartieeWeb.Models.Question> Questions { get; set; }
    }
}