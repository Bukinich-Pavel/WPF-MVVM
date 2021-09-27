using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork11
{
    abstract class Worker : INotifyPropertyChanged
    {
        public static int IdMax = 0;
        public static void GetMaxId(Worker dp)
        {
            IdMax = IdMax < dp.Id ? dp.Id : IdMax;
        }

        public int Id { get; set; }
        public virtual string Position { get; }

        private string nameWorker;
        public string NameWorker
        {
            get { return nameWorker; }
            set
            {
                nameWorker = value;
                OnPropertyChanged("NameWorker");
            }

        }


        private int departamentId;
        public int DepartamentId
        {
            get { return departamentId; }
            set
            {
                departamentId = value;
                OnPropertyChanged("DepartamentId");
            }
        }


        private int salary;
        public int Salary
        {
            get { return salary; }
            set
            {
                salary = value;
                OnPropertyChanged("Salary");
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
