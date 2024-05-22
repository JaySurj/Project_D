using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_D.ViewModels
{
    public partial class BaseView : ObservableObject
    {
        [ObservableProperty]
        public bool isBusy;

        [ObservableProperty]
        public string _Title;


    }
}
