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

            // Simulate time running out
            component.Instance.timeExpired = true;
            component.Render();

            // Assert
            Assert.Contains("Time's up! Your final score:", component.Markup);
        }

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
            component.Instance.timeExpired = true; // Assuming this triggers navigation internally
            component.Instance.ShowResults(); // Directly invoke the method or simulate the condition leading to its call

            // Assert
            mockNavigationManager.Verify(nm => nm.NavigateTo(It.Is<string>(uri => uri == expectedUri), It.IsAny<bool>()), Times.Once());
        }


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