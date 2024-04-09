using System;

namespace SmartieeWeb.Services
{
    /// <summary>
    /// Manages the application state, specifically the current category name selected in the quiz.
    /// This service allows for the category name to be updated and provides a notification mechanism
    /// to inform components of state changes.
    /// </summary>
    public class AppStateService : IAppStateService
    {
        /// <summary>
        /// Gets the current category name displayed in the quiz.
        /// </summary>
        public virtual string CurrentCategoryName { get; private set; } = "How Smart Are You?";

        /// <summary>
        /// Event that is triggered whenever the state of the application changes.
        /// Components can subscribe to this event to be notified of state changes.
        /// </summary>
        public event Action OnChange;

        /// <summary>
        /// Updates the current category name and notifies subscribers of the change.
        /// </summary>
        /// <param name="name">The new category name to set.</param>
        public void SetCurrentCategoryName(string name)
        {
            CurrentCategoryName = name;
            NotifyStateChanged();
        }

        /// <summary>
        /// Resets the current category name to its default value and notifies subscribers of the change.
        /// </summary>
        public void ResetCategoryNameToDefault()
        {
            SetCurrentCategoryName("How Smart Are You?");
        }

        // Notifies all subscribers about a state change by invoking the OnChange event.
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
