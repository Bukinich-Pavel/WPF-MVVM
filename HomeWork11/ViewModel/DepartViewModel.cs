using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HomeWork11.ViewModel
{
    class DepartViewModel : INotifyPropertyChanged
    {
        private string selecteddepartament;
        public string SelectedDepartament
        {
            get { return selecteddepartament; }
            set
            {
                selecteddepartament = value;
                OnPropertyChanged("SelectedDepartament");
            }
        }





        #region реализация INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        #endregion

    }
}
