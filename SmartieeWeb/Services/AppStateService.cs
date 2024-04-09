using System;

namespace SmartieeWeb.Services
{
    public class AppStateService
    {
        public string CurrentCategoryName { get; private set; } = "How Smart Are You?";

        public event Action OnChange;

        public void SetCurrentCategoryName(string name)
        {
            CurrentCategoryName = name;
            NotifyStateChanged();
        }

        public void ResetCategoryNameToDefault()
        {
            SetCurrentCategoryName("How Smart Are You?");
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
