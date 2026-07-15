using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlankApp.ViewModels
{
    internal class APISettingViewModel : BindableBase, IDialogAware
    {
        public DelegateCommand SaveCommand { get; }

        public event Action<IDialogResult> RequestClose;

        private string apiKey;
        public string APIKey
        {
            get => apiKey;
            set => SetProperty(ref apiKey, value);
        }

        public string Title => "API设置";

        public APISettingViewModel()
        {
            SaveCommand = new DelegateCommand(ExecuteSave, CanExecuteSave)
                .ObservesProperty(() => APIKey);
        }

        private bool CanExecuteSave()
        {
            return !string.IsNullOrEmpty(APIKey);
        }

        private void ExecuteSave()
        {
            var apiKey = new DialogParameters { { "APIKey", APIKey } };
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, apiKey));
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
