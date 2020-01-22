using Acr.UserDialogs;

namespace HandyApp.ViewModels
{
    public class SettingsViewModel
    {
        private IUserDialogs Dialogs;

        public SettingsViewModel(IUserDialogs dialog)
        {
            Dialogs = dialog;
        }
    }
}
