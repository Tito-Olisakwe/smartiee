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
