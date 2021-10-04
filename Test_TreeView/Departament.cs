using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Test_TreeView
{
    public class Departament : INotifyPropertyChanged
    {
        public static int colDep = 0;
        public Departament()
        {
            colDep++;
        }
        public string Name { get; set; }

        private ObservableCollection<Departament> departaments;
        public ObservableCollection<Departament> Departaments
        {
            get { return departaments; }
            set
            {
                departaments = value;
                OnPropertyChanged("Departaments");

            }
        }



        #region реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion


    }
}
