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
    class Departament
    {
        #region Статические поля

        public static int IdMax = 0;
        public static ObservableCollection<string> NameAllDepartaments = new ObservableCollection<string>();
        public static ObservableCollection<Departament> AllDepartaments = new ObservableCollection<Departament>();

        #endregion


        #region Статические методы

        /// <summary>
        /// Дабвить департамент в стат поля 
        /// </summary>
        /// <param name="dp"></param>
        public static void SetDepartament(Departament dp)
        {
            AllDepartaments.Add(dp);
            SetMaxId(dp);
            NameAllDepartaments.Add(dp.NameDepartament);

            if (dp.Departaments != null)
            {
                GetAllName(dp.Departaments);
            }
        }

        /// <summary>
        /// Сохранения максимального имеющегося Id среди всех департаментов
        /// </summary>
        /// <param name="dp"></param>
        private static void SetMaxId(Departament dp)
        {
            IdMax = IdMax < dp.Id ? dp.Id : IdMax;
        }

        /// <summary>
        /// Рекурсивный поиск всех имен
        /// </summary>
        /// <param name="dp"></param>
        private static void GetAllName(ObservableCollection<Departament> dp)
        {
            foreach (var item in dp)
            {
                AllDepartaments.Add(item);
                SetMaxId(item);
                NameAllDepartaments.Add(item.NameDepartament);

                if (item.Departaments != null)
                {
                    GetAllName(item.Departaments);
                }
            }
        }

        #endregion

        
        public Departament(int id, string nameDepartament, int departamentParentId)
        {
            Id = id;
            NameDepartament = nameDepartament;
            DepartamentParentId = departamentParentId;
            
        }


        #region Свойства

        public int Id { get; set; }
        public string NameDepartament { get; set; }
        public int DepartamentParentId { get; set; }


        public string manager;
        public string Manager
        {
            get { return manager; }
            set
            {
                manager = value;
                OnPropertyChanged("Manager");
            }
        }


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

        #endregion



        #region реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

    }
}
