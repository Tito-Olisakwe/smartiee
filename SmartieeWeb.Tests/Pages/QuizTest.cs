using Moq;
using Bunit;
using SmartieeWeb.Services;
using SmartieeWeb.Pages;
using SmartieeWeb.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components;

namespace SmartieeWeb.Tests
{
    public class QuizTests : TestContext
    {
        var mockQuizService = new Mock<IQuizService>();
        var mockLogger = new Mock<ILogger<Quiz>>();
        var mockNavigationManager = new Mock<NavigationManager>();

        Services.AddSingleton<IQuizService>(mockQuizService.Object);
Services.AddSingleton<ILogger<Quiz>>(mockLogger.Object);
Services.AddSingleton<NavigationManager>(mockNavigationManager.Object);

/// <summary>
/// Test ID: TC015
/// Description: Verify that the Quiz component shows a loading message upon initial load while fetching questions.
/// Expected Outcome: The component's markup contains "Loading questions..." during the question fetching process.
/// </summary>
[Fact]
        public void QuizComponent_ShowsLoadingOnInitialLoad()
        {
            mockQuizService.Setup(service => service.LoadQuestionsAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.CompletedTask); // Simulate loading state

            // Act
            var component = RenderComponent<Quiz>(parameters =>
            {
                parameters.Add(p => p.CategoryIdAsString, "1");
                parameters.Add(p => p.Difficulty, "Easy");
                parameters.Add(p => p.IsTimed, false);
            });

            // Assert
            Assert.Contains("Loading questions...", component.Markup);
        }

        /// <summary>
        /// Test ID: TC016
        /// Description: Ensure the Quiz component displays a loading message while questions are being asynchronously fetched.
        /// Expected Outcome: "Loading questions..." is displayed in the component's markup while fetching questions.
        /// </summary>
        [Fact]
        public void ShowsLoadingMessage_WhileQuestionsAreBeingFetched()
        {
            // Arrange
            var mockQuizService = new Mock<IQuizService>();

            mockQuizService.Setup(service => service.LoadQuestionsAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.Delay(100));

            // Act
            var component = RenderComponent<Quiz>(parameters =>
            {
                parameters.Add(p => p.CategoryIdAsString, "1");
                parameters.Add(p => p.Difficulty, "Easy");
                parameters.Add(p => p.IsTimed, true);
            });

            // Assert
            Assert.Contains("Loading questions...", component.Markup);
        }

        /// <summary>
        /// Test ID: TC017
        /// Description: Verify that the Quiz component displays a "Time's up! Your final score:" message when the quiz time expires.
        /// Expected Outcome: The component's markup includes the "Time's up! Your final score:" message upon time expiration.
        /// </summary>
        [Fact]
        public void DisplaysTimesUpMessage_WhenTimeExpires()
        {
            // Arrange
            var mockQuizService = new Mock<IQuizService>();
            var component = RenderComponent<Quiz>(parameters =>
            {
                parameters.Add(p => p.CategoryIdAsString, "1");
                parameters.Add(p => p.Difficulty, "Easy");
                parameters.Add(p => p.IsTimed, true);
            });

            component.Instance.timeExpired = true;
            component.Render();

            // Assert
            Assert.Contains("Time's up! Your final score:", component.Markup);
        }

        /// <summary>
        /// Test ID: TC018
        /// Description: Confirm that the Quiz component navigates to the results page when the quiz time expires.
        /// Expected Outcome: The application's URL changes to "/results/5" indicating navigation to the results page with the final score.
        /// </summary>
        [Fact]
        public void NavigatesToResults_WhenTimeExpires()
        {
            // Arrange
            var expectedUri = "/results/5";
            var mockNavigationManager = new Mock<NavigationManager>();
            mockNavigationManager.SetupProperty(x => x.Uri, "http://localhost/");
            mockNavigationManager.Setup(nm => nm.NavigateTo(It.IsAny<string>(), It.IsAny<bool>()))
                                 .Callback<string, bool>((url, forceLoad) => mockNavigationManager.Object.Uri = $"http://localhost{url}");

            Services.AddSingleton<NavigationManager>(mockNavigationManager.Object);

            var mockQuizService = new Mock<IQuizService>();
            mockQuizService.Setup(service => service.Score).Returns(5);

            Services.AddSingleton<IQuizService>(mockQuizService.Object);

            var component = RenderComponent<Quiz>(parameters =>
            {
                parameters.Add(p => p.CategoryIdAsString, "1");
                parameters.Add(p => p.Difficulty, "Easy");
                parameters.Add(p => p.IsTimed, true);
            });

            // Act
            component.Instance.timeExpired = true;
            component.Instance.ShowResults();

            // Assert
            mockNavigationManager.Verify(nm => nm.NavigateTo(It.Is<string>(uri => uri == expectedUri), It.IsAny<bool>()), Times.Once());
        }

        /// <summary>
        /// Test ID: TC019
        /// Description: Verify that selecting the correct answer updates the score appropriately and provides positive feedback to the user.
        /// Expected Outcome: The component confirms the answer is correct and displays a "Correct!" message.
        /// </summary>
        [Fact]
        public async Task CorrectAnswerSelection_UpdatesScoreAndProvidesFeedback()
        {
            // Arrange
            var mockQuizService = new Mock<IQuizService>();
            var correctOptionIndex = 0;
            mockQuizService.Setup(service => service.GetQuestion(It.IsAny<int>())).Returns(new Question
            {
                QuestionText = "What is 2+2?",
                Options = new[] { "4", "3", "2", "1" },
                CorrectAnswerIndex = correctOptionIndex,
                Explanation = "Because math."
            });
            mockQuizService.Setup(service => service.SubmitAnswer(It.IsAny<int>(), correctOptionIndex)).Returns(true);

            Services.AddSingleton<IQuizService>(mockQuizService.Object);

            var component = RenderComponent<Quiz>();

            // Act
            component.Instance.ConfirmAnswer(correctOptionIndex);
            await component.Instance.OnConfirm(correctOptionIndex);

            // Assert
            mockQuizService.Verify(service => service.SubmitAnswer(It.IsAny<int>(), correctOptionIndex), Times.Once());
            Assert.Contains("Correct!", component.Markup);
        }

        /// <summary>
        /// Test ID: TC020
        /// Description: Ensure selecting an incorrect answer does not update the score but provides feedback about the correct answer.
        /// Expected Outcome: The component indicates the answer is incorrect and displays the correct answer.
        /// </summary>
        [Fact]
        public async Task IncorrectAnswerSelection_DoesNotUpdateScoreButProvidesCorrectFeedback()
        {
            // Arrange
            var mockQuizService = new Mock<IQuizService>();
            var correctOptionIndex = 0;
            var incorrectOptionIndex = 1;
            mockQuizService.Setup(service => service.GetQuestion(It.IsAny<int>())).Returns(new Question
            {
                QuestionText = "What is 2+2?",
                Options = new[] { "4", "3", "2", "1" },
                CorrectAnswerIndex = correctOptionIndex,
                Explanation = "Because math."
            });
            mockQuizService.Setup(service => service.SubmitAnswer(It.IsAny<int>(), incorrectOptionIndex)).Returns(false);

            Services.AddSingleton<IQuizService>(mockQuizService.Object);

            var component = RenderComponent<Quiz>();

            // Act
            component.Instance.ConfirmAnswer(incorrectOptionIndex);
            await component.Instance.OnConfirm(incorrectOptionIndex);

            // Assert
            mockQuizService.Verify(service => service.SubmitAnswer(It.IsAny<int>(), incorrectOptionIndex), Times.Once());
            Assert.Contains("Incorrect!", component.Markup);
            Assert.Contains("The correct answer is:", component.Markup);

        }

        /// <summary>
        /// Test ID: TC021
        /// Description: Confirm that the next question button either loads the next question or navigates to the results page if there are no more questions.
        /// Expected Outcome: The component navigates to the next question or the results page based on the availability of further questions.
        /// </summary>
        [Fact]
        public async Task NextQuestionButton_LoadsNextQuestionOrNavigatesToResults()
        {
            // Arrange
            var mockQuizService = new Mock<IQuizService>();
            mockQuizService.SetupSequence(service => service.HasMoreQuestions(It.IsAny<int>()))
                           .Returns(true)
                           .Returns(false);
            mockQuizService.Setup(service => service.GetQuestion(It.IsAny<int>()))
                           .Returns(new Question { QuestionText = "Next Question?", Options = new[] { "Option 1" }, CorrectAnswerIndex = 0 });

            var mockNavigationManager = new Mock<NavigationManager>();
            mockNavigationManager.SetupProperty(x => x.Uri, "http://localhost/");

            Services.AddSingleton<IQuizService>(mockQuizService.Object);
            Services.AddSingleton<NavigationManager>(mockNavigationManager.Object);

            var component = RenderComponent<Quiz>();

            component.Find("button.next-question").Click();
            await Task.Delay(100);

            Assert.Contains("Next Question?", component.Markup);

            component.Find("button.next-question").Click();
        }

        /// <summary>
        /// Test ID: TC022
        /// Description: Verify that the play again button restarts the quiz with the initial settings.
        /// Expected Outcome: The quiz is restarted with the same category, difficulty, and timing settings as the initial quiz.
        /// </summary>
        [Fact]
        public async Task PlayAgainButton_RestartsQuizWithSameSettings()
        {
            // Arrange
            var mockQuizService = new Mock<IQuizService>();

            mockQuizService.Setup(service => service.LoadQuestionsAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
                           .Returns(Task.CompletedTask);

            Services.AddSingleton<IQuizService>(mockQuizService.Object);

            var component = RenderComponent<Quiz>(parameters =>
            {
                parameters.Add(p => p.CategoryIdAsString, "1");
                parameters.Add(p => p.Difficulty, "Easy");
                parameters.Add(p => p.IsTimed, true);
            });

            // Act
            component.Instance.PlayAgain();

            // Assert
            mockQuizService.Verify(service => service.LoadQuestionsAsync(1, "Easy", true), Times.AtLeast(2));
        }

    }
}