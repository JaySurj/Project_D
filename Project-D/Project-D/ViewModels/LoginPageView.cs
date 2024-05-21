using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_D.ViewModels
{
    public partial class LoginPageView : BaseView
    {
        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _password;

        [ICommand]
        public async void Login()
        {
            // Login logic here
        }
    }
}
