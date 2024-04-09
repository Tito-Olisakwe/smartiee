using Moq;
using Bunit;
using SmartieeWeb.Services;
using SmartieeWeb.Layout;
using SmartieeWeb.Models;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components;

namespace SmartieeWeb.Tests
{
    public class NavMenuTests : IDisposable
    {
        private readonly TestContext ctx;
        private readonly Mock<IDataService> mockDataService;
        private readonly Mock<IAppStateService> mockAppStateService;

        public NavMenuTests()
        {
            ctx = new TestContext();
            mockDataService = new Mock<IDataService>();
            mockDataService.Setup(_ => _.LoadTriviaDataAsync()).ReturnsAsync(new TriviaData
            {
                Categories = new List<Category> { new Category { Id = 1, Name = "Science" } }
            });

            ctx.Services.AddSingleton<IDataService>(mockDataService.Object);

            mockAppStateService = new Mock<IAppStateService>();
            ctx.Services.AddSingleton<IAppStateService>(mockAppStateService.Object);
        }

        [Fact]
        public void ComponentDisplaysCategoriesAfterInitialization()
        {
            // Act
            var component = ctx.RenderComponent<NavMenu>();

            // Assert
            component.Markup.Should().Contain("Science");
        }

        [Fact]
        public void ComponentShowsErrorWhenCategoriesUnavailable()
        {
            // Arrange
            mockDataService.Setup(x => x.LoadTriviaDataAsync()).ReturnsAsync((TriviaData)null);

            // Act
            var component = ctx.RenderComponent<NavMenu>();

            // Assert
            component.WaitForAssertion(() => component.Markup.Should().Contain("Categories Unavailable"), TimeSpan.FromSeconds(2));
        }

        [Fact]
        public void ToggleNavMenuChangesVisibility()
        {
            // Arrange
            var component = ctx.RenderComponent<NavMenu>();

            var navMenuDiv = component.Find("div.nav-scrollable");
            navMenuDiv.ClassList.Contains("collapse").Should().BeTrue();

            // Act
            component.Find("button.navbar-toggler").Click();

            // Assert
            navMenuDiv.ClassList.Contains("collapse").Should().BeFalse();
        }

        [Fact]
        public void NavigateToCategoryCallsCorrectServiceMethod()
        {
            // Arrange
            var navigationManager = ctx.Services.GetRequiredService<NavigationManager>();

            var component = ctx.RenderComponent<NavMenu>();

            component.WaitForState(() => component.FindAll("button.nav-link-button").Count > 0);

            // Act
            var categoryButton = component.Find("button.nav-link-button");
            categoryButton.Click();

            // Assert
            navigationManager.Uri.Should().Contain("/difficulty/");
        }

        public void Dispose()
        {
            ctx.Dispose();
        }
    }
}
