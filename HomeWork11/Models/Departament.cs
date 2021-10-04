using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork11
{
    public class Departament
    {
        #region Статические поля
        public static int IdMax = 0;
        public static List<string> NameAllDepartaments = new List<string>();
        public static ObservableCollection<Departament> AllDepartaments = new ObservableCollection<Departament>();
        #endregion

        #region Статические методы
        public static void GetMaxId(Departament dp)
        {
            IdMax = IdMax < dp.Id ? dp.Id : IdMax;
        }
        public static void GetDepartament(Departament dp)
        {
            AllDepartaments.Add(dp);
            NameAllDepartaments.Add(dp.NameDepartament);
        }
        #endregion

        
        public Departament(int id, string nameDepartament, int departamentParentId)
        {
            Id = id;
            NameDepartament = nameDepartament;
            DepartamentParentId = departamentParentId;
            
        }

        public int Id { get; set; }

        public string NameDepartament { get; set; }

        public int ManagerId { get; set; }

        public int DepartamentParentId { get; set; }


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
