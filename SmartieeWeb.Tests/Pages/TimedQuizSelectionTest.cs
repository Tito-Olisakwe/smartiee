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
    public class TimedQuizSelectionTests : TestContext
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void QuizTimingButtons_NavigateCorrectly(bool isTimed)
        {
            // Arrange
            var mockNavManager = new MockNavigationManager();
            Services.AddSingleton<NavigationManager>(mockNavManager);

            var parameters = new ComponentParameter[]
            {
                ComponentParameter.CreateParameter("CategoryId", 1),
                ComponentParameter.CreateParameter("Difficulty", "Easy")
            };
            var component = RenderComponent<TimedQuizSelection>(parameters);

            // Act
            component.Find($"button[onclick='() => StartQuiz({isTimed.ToString().ToLower()})']").Click();

            // Assert
            string expectedUri = $"/quiz/1/Easy/{isTimed}";
            Assert.Equal(expectedUri, mockNavManager.NavigatedUri);
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
