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
    public class DifficultyTests : TestContext
    {
        /// <summary>
        /// Test ID: TC012
        /// Description: Verify the Difficulty selection component renders correctly with all difficulty level buttons.
        /// Expected Outcome: The component renders three buttons labeled "Easy", "Medium", and "Hard".
        /// </summary>
        [Fact]
        public void DifficultySelectionComponent_RendersCorrectly()
        {
            // Arrange
            Services.AddScoped<NavigationManager, MockNavigationManager>();

            // Act
            var component = RenderComponent<Difficulty>();

            // Assert
            Assert.Equal(3, component.FindAll("button.custom-btn").Count);
            Assert.Contains("Easy", component.Markup);
            Assert.Contains("Medium", component.Markup);
            Assert.Contains("Hard", component.Markup);
        }

        /// <summary>
        /// Test ID: TC013
        /// Description: Ensure that clicking on any difficulty level button navigates to the correct URL with the chosen difficulty.
        /// Expected Outcome: Clicking a difficulty button navigates to "/timed/1/{difficulty}", where {difficulty} is the difficulty level of the button clicked.
        /// </summary>
        [Theory]
        [InlineData("Easy")]
        [InlineData("Medium")]
        [InlineData("Hard")]
        public void DifficultyButtons_NavigateCorrectly(string difficulty)
        {
            // Arrange
            var mockNavManager = new Mock<NavigationManager>();
            mockNavManager.SetupProperty(nm => nm.Uri, "http://localhost/");
            Services.AddSingleton<NavigationManager>(mockNavManager.Object);
            var component = RenderComponent<Difficulty>(parameters => parameters.Add(p => p.CategoryId, 1));

            // Act
            component.Find($"button[onclick='Start{difficulty}Quiz']").Click();

            // Assert
            mockNavManager.Verify(nav => nav.NavigateTo($"/timed/1/{difficulty}", true), Times.Once());
        }

        public class MockNavigationManager : NavigationManager
        {
            public MockNavigationManager() => Initialize("http://localhost/", "http://localhost/");

            protected override void NavigateToCore(string uri, bool forceLoad) { /* Mock implementation */ }
        }
    }
}
