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
    public class IndexTests : TestContext
    {
        /// <summary>
        /// Test ID: TC014
        /// Description: Verify that clicking the continue button on the Index page navigates the user to the Categories page.
        /// Expected Outcome: The application navigates to "/categories" upon clicking the continue button.
        /// </summary>
        [Fact]
        public void ContinueButton_NavigatesToCategories()
        {
            // Arrange
            var mockNavManager = new MockNavigationManager();
            Services.AddSingleton<NavigationManager>(mockNavManager);

            var component = RenderComponent<Pages.Index>();

            // Act
            component.Find("button.custom-btn").Click();

            // Assert
            Assert.Equal("/categories", mockNavManager.NavigatedUri);
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
