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

        [Fact]
        public void CategoriesPage_LoadsAndDisplaysCategories()
        {
            // Act
            var component = RenderComponent<Categories>();

            // Assert
            component.Markup.Should().ContainAll("Science", "Math");
            component.FindAll("button.custom-btn").Count.Should().Be(3); 
        }

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
