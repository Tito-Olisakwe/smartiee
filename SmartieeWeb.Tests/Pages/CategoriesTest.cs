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
    public class CategoriesTests : TestContext, IDisposable
    {
        private readonly Mock<IDataService> mockDataService = new();
        private readonly Mock<IAppStateService> mockAppStateService = new();

        public CategoriesTests()
        {
            mockDataService.Setup(x => x.LoadTriviaDataAsync()).ReturnsAsync(new TriviaData
            {
                Categories = new List<Category>
                {
                    new Category { Id = 1, Name = "Science" },
                    new Category { Id = 2, Name = "Math" }
                }
            });

            Services.AddSingleton<IDataService>(mockDataService.Object);
            Services.AddSingleton<IAppStateService>(mockAppStateService.Object);
        }

        /// <summary>
        /// Test ID: TC007
        /// Description: Verify that the Categories page loads and correctly displays all available categories.
        /// Expected Outcome: The component's markup should contain the names of all categories, "Science" and "Math", and display corresponding buttons for each.
        /// </summary>
        [Fact]
        public void CategoriesPage_LoadsAndDisplaysCategories()
        {
            // Act
            var component = RenderComponent<Categories>();

            // Assert
            component.Markup.Should().ContainAll("Science", "Math");
            component.FindAll("button.custom-btn").Count.Should().Be(3);
        }

        /// <summary>
        /// Test ID: TC008
        /// Description: Ensure that selecting a category on the Categories page navigates to the correct difficulty selection page for that category.
        /// Expected Outcome: The application navigates to "/difficulty/1" upon selecting the "Science" category.
        /// </summary>
        [Fact]
        public void CategoriesPage_NavigatesToCategory_WhenSelected()
        {
            // Arrange
            mockDataService.Setup(x => x.LoadTriviaDataAsync()).ReturnsAsync(new TriviaData
            {
                Categories = new List<Category>
                {
                    new Category { Id = 1, Name = "Science" }
                }
            });

            // Act
            var component = RenderComponent<Categories>();
            component.Find("button.custom-btn").Click();

            // Assert
            var navigationManager = Services.GetRequiredService<NavigationManager>();
            navigationManager.Uri.Should().EndWith("/difficulty/1");

            mockAppStateService.Verify(x => x.SetCurrentCategoryName("Science"), Times.Once());
        }
    }
}
