using Smartiee.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Smartiee.Services
{
    public class QuizService
    {
        private List<Question> Questions;
        private int Score = 0;

        // Constructor
        public QuizService()
        {
            LoadQuestions();
        }

        private void LoadQuestions()
        {
            DataService dataService = new DataService();
            TriviaData triviaData = dataService.LoadTriviaData();

            // Assuming triviaData is correctly populated, assign its questions to this service's Questions list
            Questions = triviaData.Questions;
        }

        // Add other methods to start the quiz, check answers, etc.
    }
}
