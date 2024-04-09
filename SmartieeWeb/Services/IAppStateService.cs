using System;

namespace SmartieeWeb.Services
{
    /// <summary>
    /// Defines a contract for managing the application's state related to the current quiz category.
    /// </summary>
    public interface IAppStateService
    {
        string CurrentCategoryName { get; }
        event Action OnChange;

        void SetCurrentCategoryName(string name);
        void ResetCategoryNameToDefault();
    }
}
