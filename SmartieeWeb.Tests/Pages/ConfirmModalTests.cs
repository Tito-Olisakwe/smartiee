using Bunit;
using SmartieeWeb.Pages;
using FluentAssertions;
using Microsoft.AspNetCore.Components;

namespace SmartieeWeb.Tests
{
    public class ConfirmModalTests : TestContext
    {
        [Fact]
        public async Task ConfirmButton_ClosesModalAndInvokesOnConfirm()
        {
            // Arrange
            int invokedOptionIndex = -1;
            var component = RenderComponent<ConfirmModal>(parameters =>
                parameters.Add(p => p.OnConfirm, EventCallback.Factory.Create<int>(this, index =>
                {
                    invokedOptionIndex = index;
                })));

            await InvokeAsync(() => component.Instance.Show("Confirm Test", 5));

            // Act
            await InvokeAsync(() => component.Find("button.custom-btn").Click());

            // Assert
            component.Markup.Should().NotContain("display:block;");
            Assert.Equal(5, invokedOptionIndex);
        }

        [Fact]
        public async Task CancelButton_ClosesModalAndInvokesOnCancel()
        {
            // Arrange
            bool onCancelInvoked = false;
            var component = RenderComponent<ConfirmModal>(parameters =>
                parameters.Add(p => p.OnCancel, () => onCancelInvoked = true));

            await InvokeAsync(() => component.Instance.Show("Cancel Test", 5));
            var buttons = component.FindAll("button.custom-btn");

            // Act
            if (buttons.Count > 1) // Assuming there's more than one button and the second is Cancel
            {
                await InvokeAsync(() => buttons[1].Click());
            }

            // Assert
            Assert.True(onCancelInvoked);
            component.Markup.Should().NotContain("display:block;"); // Assuming you check for visibility with markup
        }

        [Fact]
        public async Task ShowMethod_MakesModalVisibleWithCorrectMessage()
        {
            // Arrange
            var component = RenderComponent<ConfirmModal>();

            // Act
            await InvokeAsync(() => component.Instance.Show("Test message", 1));

            // Assert
            component.Markup.Should().Contain("Test message");
            component.Markup.Should().Contain("display:block;"); // Assuming "display:block;" makes it visible
        }
    }
}
