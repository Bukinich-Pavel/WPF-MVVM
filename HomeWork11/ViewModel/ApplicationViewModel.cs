using HomeWork11.Models;
using HomeWork11.View;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HomeWork11.ViewModel
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        // выбранный сотрудник
        private Worker selectedWorker;
        public Worker SelectedWorker
        {
            get { return selectedWorker; }
            set
            {
                selectedWorker = value;
                OnPropertyChanged("SelectedWorker");
            }
        }


        // выбранный департамент
        private Departament selecteddepartament;
        public Departament SelectedDepartament
        {
            get { return selecteddepartament; }
            set
            {
                selecteddepartament = value;
                WorkersView = new ObservableCollection<Worker>() { };
                if(value != null)
                {
                    foreach (var item in Workers)
                    {
                        if (value.Id == item.DepartamentId) WorkersView.Add(item);
                    }
                }
                OnPropertyChanged("SelectedDepartament");
            }
        }


        public ObservableCollection<Worker> Workers { get; set; }
        public ObservableCollection<Intern> Interns { get; set; }
        public ObservableCollection<Specialist> Specialists { get; set; }
        public ObservableCollection<Manager> Managers { get; set; }
        public ObservableCollection<Departament> Departaments { get; set; }


        // сотрудники отображенные в ListBox
        private ObservableCollection<Worker> workersView;
        public ObservableCollection<Worker> WorkersView
        {
            get { return workersView; }
            set
            {
                workersView = value;
                OnPropertyChanged("WorkersView");
            }
        }


        // департаменты отображенные в ListBox
        private ObservableCollection<Departament> departamentsView;
        public ObservableCollection<Departament> DepartamentsView
        {
            get { return departamentsView; }
            set
            {
                departamentsView = value;
                OnPropertyChanged("DepartamentsView");
            }
        }


        #region Команды

        // отобразить верхний департамент
        private RelayCommand viewParentDepartament;
        public RelayCommand ViewParentDepartament
        {
            get
            {
                return viewParentDepartament ?? (viewParentDepartament = new RelayCommand(obj =>
                {
                    DepartamentsView = new ObservableCollection<Departament> { };
                    foreach (var item in Departament.AllDepartaments)
                    {
                        if (item.DepartamentParentId == -1) DepartamentsView.Add(item);
                    }

                    WorkersView = Workers;
                }));
            }
        }


        // команда добавления департамента
        private RelayCommand addDepartament;
        public RelayCommand AddDepartament
        {
            get
            {
                return addDepartament ??
                    (addDepartament = new RelayCommand(obj =>
                    {
                        AddDepartament addDepartament = new AddDepartament();
                        addDepartament.ListDepartament = GetNameDepartaments(Departament.AllDepartaments);
                        addDepartament.ShowDialog();

                            // получает имя и выбранный родительский департамент
                            string nameSelectedDepartament = ((NewDepViewModel)addDepartament.DataContext).SelectedDepartament;
                        string nameNewDepartament = addDepartament.NameDepartament;

                            // получает Id выбраного департамента
                            int idSelectDepartament = GetIdSelectedDepartament(nameSelectedDepartament);

                        if (Departament.NameAllDepartaments.IndexOf(nameNewDepartament) == -1)
                        {
                            Departament departament = new Departament(++Departament.IdMax, nameNewDepartament, idSelectDepartament);
                            if (departament.NameDepartament != "")
                            {
                                Departaments.Add(departament);
                                Departament.GetDepartament(departament);
                                    //DepartamentsView.Add(departament);
                                }
                        }
                        else
                        {
                            MessageBox.Show("Департамент с таким именем уже есть");
                        }
                    }));
            }
        }


        // команда добавления сотрудника
        private RelayCommand addWorker;
        public RelayCommand AddWorker
        {
            get
            {
                return addWorker ?? (addDepartament = new RelayCommand(obg => 
                {
                    AddWorker addWorker = new AddWorker();
                    addWorker.ListDepartament = GetNameDepartaments(Departament.AllDepartaments);
                    addWorker.ShowDialog();
                }));
            }
        }



        // команда удаления департамента
        private RelayCommand removeDepartament;
        public RelayCommand RemoveDepartament
        {
            get
            {
                return removeDepartament ??
                  (removeDepartament = new RelayCommand(obj =>
                  {
                      Departament departament = obj as Departament;
                      if (departament != null)
                      {
                          Departaments.Remove(departament);
                          Departament.AllDepartaments.Remove(departament);
                          DepartamentsView.Remove(departament);
                      }
                  }, 
                  //(obj) => Departaments.Count > 0));
                  (obj) => Departament.AllDepartaments.Count > 0));
            }
        }


        // команда удаления сотрудника
        private RelayCommand removeWorker;
        public RelayCommand RemoveWorker
        {
            get
            {
                return removeWorker ??
                  (removeWorker = new RelayCommand(obj =>
                  {
                      Worker worker = obj as Worker;
                      if (worker != null)
                      {
                          Workers.Remove(worker);
                          WorkersView.Remove(worker);
                      }
                  }, 
                  (obj) => Workers.Count > 0));
            }
        }


        // команда отобразить вложенные департаменты 
        private RelayCommand doubleCommand;
        public RelayCommand DoubleCommand
        {
            get
            {
                return doubleCommand ?? (doubleCommand = new RelayCommand(obj =>
                {
                    Departament departament = obj as Departament;
                    DepartamentsView = new ObservableCollection<Departament> { };
                    foreach (var item in Departament.AllDepartaments)
                    {
                        if (item.DepartamentParentId == departament.Id) DepartamentsView.Add(item);
                    }

                }));
            }
        }

        #endregion


        //конструктор
        public ApplicationViewModel()
        {
            if (File.Exists("departaments.json") && File.Exists("interns.json"))
            {
                string str = File.ReadAllText("departaments.json");
                Departaments = JsonConvert.DeserializeObject<ObservableCollection<Departament>>(str);

                DepartamentsView = new ObservableCollection<Departament> { };
                if (Departaments != null)
                {
                    foreach (var item in Departaments)
                    {
                        if (item.DepartamentParentId == -1) DepartamentsView.Add(item);
                        Departament.GetMaxId(item);
                        Departament.GetDepartament(item);
                    }
                }


                Workers = new ObservableCollection<Worker> { };
                str = File.ReadAllText("interns.json");
                Interns = JsonConvert.DeserializeObject<ObservableCollection<Intern>>(str);
                foreach (var item in Interns)
                {
                    Workers.Add(item);
                }


                str = File.ReadAllText("managers.json");
                Managers = JsonConvert.DeserializeObject<ObservableCollection<Manager>>(str);
                foreach (var item in Managers)
                {
                    Workers.Add(item);
                }

                WorkersView = Workers;

            }
            else
            {
                MessageBox.Show("Нет данных!");
            }
        }


        //деструктор
        ~ApplicationViewModel()
        {
            string json = JsonConvert.SerializeObject(Departaments);
            File.WriteAllText("departaments.json", json);


            Interns = new ObservableCollection<Intern> { };
            Managers = new ObservableCollection<Manager> { };
            foreach (var item in Workers)
            {
                if (item is Intern)
                {
                    Interns.Add(item as Intern);
                }
                else if (item is Manager)
                {
                    Managers.Add(item as Manager);
                }
            }
            json = JsonConvert.SerializeObject(Interns);
            File.WriteAllText("interns.json", json);

            json = JsonConvert.SerializeObject(Managers);
            File.WriteAllText("managers.json", json);
        }


        /// <summary>
        /// возвращает коллекцию имен департаментов из полученой коллекции департаментов
        /// </summary>
        /// <param name="Departaments"></param>
        /// <returns></returns>
        public static ObservableCollection<string> GetNameDepartaments(ObservableCollection<Departament> Departaments)
        {
            ObservableCollection<string> vs = new ObservableCollection<string>() { };
            foreach (var item in Departaments)
            {
                vs.Add(item.NameDepartament);
            }
            return vs;
        }

        /// <summary>
        /// Возвращает Id департамента по имени
        /// </summary>
        /// <param name="nameSelectedDepartament"></param>
        /// <returns></returns>
        private int GetIdSelectedDepartament(string nameSelectedDepartament)
        {
            ObservableCollection<Departament> departaments = Departament.AllDepartaments;
            var selectedDepartaments = from d in departaments
                                where d.NameDepartament == nameSelectedDepartament
                                select d;
            int i = -1;
            foreach (var item in selectedDepartaments)
            {
                i = item.Id;
            }
            return i;
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
