using Moq;
using Bunit;
using SmartieeWeb.Services;
using SmartieeWeb.Layout;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace SmartieeWeb.Tests
{
    public class MainLayoutTests
    {
        /// <summary>
        /// Test ID: TC001
        /// Description: Verify the MainLayout component initializes with the default category name from the AppStateService.
        /// Expected Outcome: The component's markup contains the "Default Category" text, indicating the default category name is displayed.
        /// </summary>
        [Fact]
        public void ComponentInitializesWithDefaultCategoryName()
        {
            // Arrange
            using var ctx = new TestContext();
            var mockAppStateService = new Mock<IAppStateService>();
            var mockDataService = new Mock<IDataService>();

            mockAppStateService.Setup(service => service.CurrentCategoryName).Returns("Default Category");
            ctx.Services.AddSingleton<IAppStateService>(mockAppStateService.Object);
            ctx.Services.AddSingleton<IDataService>(mockDataService.Object);

            // Act
            var component = ctx.RenderComponent<MainLayout>();

            // Assert
            component.Markup.Contains("Default Category").Should().BeTrue();
        }

        /// <summary>
        /// Test ID: TC002
        /// Description: Verify the MainLayout component updates its display when the category name changes in the AppStateService.
        /// Expected Outcome: The component's markup updates to contain "New Category" when the AppStateService's category name is changed.
        /// </summary>
        [Fact]
        public void ComponentUpdatesCategoryNameOnAppStateServiceChange()
        {
            // Arrange
            using var ctx = new TestContext();
            var mockAppStateService = new Mock<IAppStateService>();
            var mockDataService = new Mock<IDataService>();
            var newCategoryName = "New Category";
            ctx.Services.AddSingleton<IAppStateService>(mockAppStateService.Object);
            ctx.Services.AddSingleton<IDataService>(mockDataService.Object);

            var component = ctx.RenderComponent<MainLayout>();
            mockAppStateService.Setup(service => service.CurrentCategoryName).Returns(newCategoryName);

            // Act
            mockAppStateService.Raise(m => m.OnChange += null);

            // Assert
            component.Markup.Contains(newCategoryName).Should().BeTrue();
        }
    }
}
