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

        /// <summary>
        /// Test ID: TC003
        /// Description: Verify that the NavMenu component displays categories as fetched from the DataService after initialization.
        /// Expected Outcome: The component's markup contains the category name "Science".
        /// </summary>
        [Fact]
        public void ComponentDisplaysCategoriesAfterInitialization()
        {
            // Act
            var component = ctx.RenderComponent<NavMenu>();

            // Assert
            component.Markup.Should().Contain("Science");
        }

        /// <summary>
        /// Test ID: TC004
        /// Description: Ensure the NavMenu component displays an error message when categories data is unavailable.
        /// Expected Outcome: The component's markup contains the text "Categories Unavailable".
        /// </summary>
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

        /// <summary>
        /// Test ID: TC005
        /// Description: Verify that clicking the navbar toggler button changes the visibility of the navigation menu.
        /// Expected Outcome: The navigation menu's class list toggles the "collapse" class, indicating a change in visibility.
        /// </summary>
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

        /// <summary>
        /// Test ID: TC006
        /// Description: Confirm that clicking a category button navigates to the correct category page.
        /// Expected Outcome: The application's URI contains "/difficulty/" indicating navigation to the category's difficulty selection page.
        /// </summary>
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
