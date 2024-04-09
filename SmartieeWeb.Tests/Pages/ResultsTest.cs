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
    public class ResultsTests : TestContext
    {
        private Mock<IQuizService> mockQuizService;
        private Mock<IAppStateService> mockAppStateService;
        private MockNavigationManager mockNavManager;

        public ResultsPageTests()
        {
            mockQuizService = new Mock<IQuizService>();
            mockAppStateService = new Mock<IAppStateService>();
            mockNavManager = new MockNavigationManager();

            mockQuizService.Setup(service => service.TotalQuestions).Returns(10);
            mockQuizService.Setup(service => service.TimeRanOut).Returns(false);


            Services.AddSingleton<NavigationManager>(mockNavManager);
            Services.AddSingleton<IQuizService>(mockQuizService.Object);
            Services.AddSingleton<IAppStateService>(mockAppStateService.Object);
        }

        /// <summary>
        /// Test ID: TC023
        /// Description: Verify the Results page initializes correctly and displays the user's score and percentage.
        /// Expected Outcome: The component's markup includes the user's score out of the total questions and the calculated percentage.
        /// </summary>
        [Fact]
        public void ResultsPage_InitializesCorrectly()
        {
            // Arrange
            var parameters = new ComponentParameter[]
            {
                ComponentParameter.CreateParameter("ScoreAsString", "7")
            };

            // Act
            var component = RenderComponent<Results>(parameters);

            // Assert
            Assert.Contains("Your score: 7/10", component.Markup);
            Assert.Contains("That's 70%", component.Markup);
        }

        /// <summary>
        /// Test ID: TC024
        /// Description: Ensure the "Play Again" button navigates the user back to the quiz with the last used settings.
        /// Expected Outcome: Clicking "Play Again" navigates to the quiz page with the last category, difficulty, and timing settings.
        /// </summary>
        [Fact]
        public void PlayAgainButton_NavigatesCorrectly()
        {
            mockQuizService.Setup(service => service.LastCategoryId).Returns(1);
            mockQuizService.Setup(service => service.LastDifficulty).Returns("Easy");
            mockQuizService.Setup(service => service.LastIsTimed).Returns(true);

            // Arrange & Act
            var component = RenderComponent<Results>(parameters => parameters.Add(p => p.ScoreAsString, "5"));
            component.Find("button[onclick='PlayAgain']").Click();

            // Assert
            var expectedUrl = "/quiz/1/Easy/True";
            Assert.Equal(expectedUrl, mockNavManager.NavigatedUri);
        }

        /// <summary>
        /// Test ID: TC025
        /// Description: Confirm the "Return to Main Menu" button correctly navigates the user back to the application's root.
        /// Expected Outcome: Clicking "Return to Main Menu" navigates to the application's root ("/").
        /// </summary>
        [Fact]
        public void ReturnToMainMenuButton_NavigatesToRoot()
        {
            // Arrange & Act
            var component = RenderComponent<Results>();
            component.Find("button[onclick='ReturnToMainMenu']").Click();

            // Assert
            Assert.Equal("/", mockNavManager.NavigatedUri);
        }

        public class MockNavigationManager : NavigationManager
        {
            public string NavigatedUri { get; private set; }

            public MockNavigationManager()
            {
                Initialize("http://localhost/", "http://localhost/");
            }

            protected override void NavigateToCore(string uri, bool forceLoad)
            {
                NavigatedUri = uri;
            }
        }
    }
}
