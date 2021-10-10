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
        /// <summary>
        /// Выбранный сотрудник
        /// </summary>
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


        /// <summary>
        /// Выбранный департамент
        /// </summary>
        private Departament selecteddepartament;
        public Departament SelectedDepartament
        {
            get { return selecteddepartament; }
            set
            {
                selecteddepartament = value;
                WorkersView = new ObservableCollection<Worker>() { };
                if (value != null)
                {
                    AllNameManagers = new ObservableCollection<string>();
                    foreach (var item in Manager.AllManagers)
                    {
                        if (value.Id == item.DepartamentId) AllNameManagers.Add(item.NameWorker);
                    }
                    foreach (var item in Workers)
                    {
                        if (value.Id == item.DepartamentId) WorkersView.Add(item);
                    }
                }
                OnPropertyChanged("SelectedDepartament");
            }
        }


        /// <summary>
        /// Выбранный менеджер для доавления в департамент
        /// </summary>
        private string selectedNameManager;
        public string SelectedNameManager
        {
            get { return selectedNameManager; }
            set
            {
                selectedNameManager = value;
                OnPropertyChanged("SelectedNameManager");
            }
        }


        /// <summary>
        /// коллекция всех сотрудников
        /// </summary>
        public static ObservableCollection<Worker> Workers { get; set; }


        /// <summary>
        ///  имена менеджеров
        /// </summary>
        private ObservableCollection<string> allNameManagers;
        public ObservableCollection<string> AllNameManagers
        {
            get { return allNameManagers; }
            set
            {
                allNameManagers = value;
                OnPropertyChanged("AllNameManagers");

            }
        }


        /// <summary>
        ///  коллекция департаентов TreeView
        /// </summary>
        private ObservableCollection<Departament> departamentsTreeView;
        public ObservableCollection<Departament> DepartamentsTreeView
        {
            get { return departamentsTreeView; }
            set
            {
                departamentsTreeView = value;
                OnPropertyChanged("DepartamentsTreeView");

            }
        }


        /// <summary>
        ///  сотрудники отображенные в ListBox
        /// </summary>
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



        #region Команды

        /// <summary>
        ///  отобразить всех сотрудников
        /// </summary>
        private RelayCommand viewParentDepartament;
        public RelayCommand ViewParentDepartament
        {
            get
            {
                return viewParentDepartament ?? (viewParentDepartament = new RelayCommand(obj =>
                {
                    WorkersView = Workers;
                }));
            }
        }


        /// <summary>
        ///  команда добавления департамента
        /// </summary>
        private RelayCommand addDepartament;
        public RelayCommand AddDepartament
        {
            get
            {
                return addDepartament ??
                    (addDepartament = new RelayCommand(obj =>
                    {
                        AddDepartament addDepartament = new AddDepartament();
                        addDepartament.ListDepartament = Departament.NameAllDepartaments;
                        addDepartament.ShowDialog();

                        // получает имя и выбранный родительский департамент
                        var selectedItem = addDepartament.listDepartament.SelectedItem;
                        if (selectedItem == null) return;
                        string nameSelectedDepartament = selectedItem.ToString();

                        string nameNewDepartament = addDepartament.NameDepartament;

                        // получает Id выбраного департамента
                        int idSelectDepartament = GetIdSelectedDepartament(nameSelectedDepartament);

                        if (Departament.NameAllDepartaments.IndexOf(nameNewDepartament) == -1)
                        {
                            Departament departament = new Departament(++Departament.IdMax, nameNewDepartament, idSelectDepartament);
                            departament.Departaments = new ObservableCollection<Departament>();
                            if (departament.NameDepartament != "")
                            {
                                AddNewDepartTreeView(DepartamentsTreeView, departament);
                                Departament.SetDepartament(departament);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Департамент с таким именем уже есть");
                        }
                    }));
            }
        }


        /// <summary>
        ///  команда добавления сотрудника
        /// </summary>
        private RelayCommand addWorker;
        public RelayCommand AddWorker
        {
            get
            {
                return addWorker ?? (addWorker = new RelayCommand(obg =>
                {
                    AddWorker addWorker = new AddWorker();
                    addWorker.ListDepartament = GetNameDepartaments(Departament.AllDepartaments);
                    ObservableCollection<string> namePosition = new ObservableCollection<string> { "Менеджер", "Специалист", "Стажер" };
                    addWorker.ListPosition = namePosition;
                    addWorker.ShowDialog();

                    string nameNewWorker, nameSelectedDepartament, nameSelectedPotision;
                    int idSelectDepartament, priceHour, numberOfHours, salary;
                    try
                    {
                        // получение имени сотрудника, имени департамента и позиции
                        nameNewWorker = addWorker.nameWorker.Text;

                        var selectedDepartament = addWorker.listDepartament.SelectedItem;
                        if (selectedDepartament == null) return;
                        nameSelectedDepartament = selectedDepartament.ToString();

                        selectedDepartament = addWorker.listPosition.SelectedItem;
                        if (selectedDepartament == null) return;
                        nameSelectedPotision = selectedDepartament.ToString();

                        // получает Id выбраного департамента
                        idSelectDepartament = GetIdSelectedDepartament(nameSelectedDepartament);


                        switch (nameSelectedPotision)
                        {
                            case "Менеджер":
                                Manager manager = new Manager
                                {
                                    Id = ++Worker.IdMax,
                                    DepartamentId = idSelectDepartament,
                                    NameWorker = nameNewWorker
                                };
                                Workers.Add(manager);
                                if (manager.DepartamentId == SelectedDepartament.Id) WorkersView.Add(manager);
                                Manager.SetAllManagers(manager);
                                break;
                            case "Специалист":
                                // получает количество часов работы и ставку
                                priceHour = Convert.ToInt32(addWorker.priceHour.Text);
                                numberOfHours = Convert.ToInt32(addWorker.numberOfHours.Text);
                                Specialist specialist = new Specialist
                                {
                                    Id = ++Worker.IdMax,
                                    DepartamentId = idSelectDepartament,
                                    NameWorker = nameNewWorker,
                                    NumberHours = numberOfHours,
                                    CostOfAnHour = priceHour
                                };
                                Workers.Add(specialist);
                                if (specialist.DepartamentId == SelectedDepartament.Id) WorkersView.Add(specialist);
                                break;
                            case "Стажер":
                                var r = addWorker.salary.Text;
                                salary = Convert.ToInt32(addWorker.salary.Text);
                                Intern intern = new Intern
                                {
                                    Id = ++Worker.IdMax,
                                    DepartamentId = idSelectDepartament,
                                    NameWorker = nameNewWorker,
                                    Salary = salary
                                };
                                Workers.Add(intern);
                                if (intern.DepartamentId == SelectedDepartament.Id) WorkersView.Add(intern);
                                break;
                        }

                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Неверный ввод!");
                        return;
                    }

                    //ТУТ
                }));
            }
        }


        /// <summary>
        /// команда добавления менеджера в департамент
        /// </summary>
        private RelayCommand addManagerToDepart;
        public RelayCommand AddManagerToDepart
        {
            get
            {
                return addManagerToDepart ?? (addManagerToDepart = new RelayCommand(obg =>
                {
                    SelectedDepartament.Manager = SelectedNameManager;
                }));
            }
        }


        /// <summary>
        ///  команда удаления департамента
        /// </summary>
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
                          Departament.NameAllDepartaments.Remove(departament.NameDepartament);
                          DeleteDepart(DepartamentsTreeView, departament);
                      }
                  },
                  //(obj) => Departaments.Count > 0));
                  (obj) => DepartamentsTreeView.Count > 0));
            }
        }


        /// <summary>
        ///  команда удаления сотрудника
        /// </summary>
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


        // команда SelectItem TreeView 
        private RelayCommand treeViewCommand;
        public RelayCommand TreeViewCommand
        {
            get
            {
                return treeViewCommand ?? (treeViewCommand = new RelayCommand(obj =>
                {
                    Departament departament = obj as Departament;
                    if (departament == null) return;

                    SelectedDepartament = departament;
                }));
            }
        }



        #endregion


        //конструктор
        public ApplicationViewModel()
        {
            if (File.Exists("departamentsTreeView.json") && File.Exists("interns.json") && File.Exists("managers.json") && File.Exists("specialists.json"))
            {
                string str = File.ReadAllText("departamentsTreeView.json"); // считывание всех департаментов из json и десериализация
                DepartamentsTreeView = JsonConvert.DeserializeObject<ObservableCollection<Departament>>(str);


                // Добавляем все департаменты из departamentsTreeView.json в стат поля Departament
                if (DepartamentsTreeView != null)
                {
                    foreach (var item in DepartamentsTreeView)
                    {
                        Departament.SetDepartament(item);
                    }
                }

                
                Workers = new ObservableCollection<Worker> { };
                
                str = File.ReadAllText("interns.json"); // считывание всех стажеров из json и десериализация
                ObservableCollection<Intern> Interns = JsonConvert.DeserializeObject<ObservableCollection<Intern>>(str);
                // добавление всех стажеров в общую коллекцию Worker
                foreach (var item in Interns)
                {
                    Workers.Add(item);
                    Worker.GetMaxId(item);
                }

                
                str = File.ReadAllText("managers.json"); // считывание всех менеджеров из json и десериализация
                ObservableCollection<Manager> Managers = JsonConvert.DeserializeObject<ObservableCollection<Manager>>(str);
                // добавление всех менеджеров в общую коллекцию Worker
                foreach (var item in Managers)
                {
                    Manager.SetAllManagers(item);
                    Workers.Add(item);
                    Worker.GetMaxId(item);
                }

                
                str = File.ReadAllText("specialists.json"); // считывание всех специалистов из json и десериализация
                ObservableCollection<Specialist> Specialists = JsonConvert.DeserializeObject<ObservableCollection<Specialist>>(str);
                // добавление всех специалистов в общую коллекцию Worker
                foreach (var item in Specialists)
                {
                    Workers.Add(item);
                    Worker.GetMaxId(item);
                }

                // отображение всех сотрудников при запуске приложения
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
            ObservableCollection<Intern> Interns = new ObservableCollection<Intern>();
            ObservableCollection<Manager> Managers = new ObservableCollection<Manager>();
            ObservableCollection<Specialist> Specialists = new ObservableCollection<Specialist>();

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
                else if (item is Specialist)
                {
                    Specialists.Add(item as Specialist);
                }
            }
            string json = JsonConvert.SerializeObject(Interns);
            File.WriteAllText("interns.json", json);

            json = JsonConvert.SerializeObject(Specialists);
            File.WriteAllText("specialists.json", json);

            json = JsonConvert.SerializeObject(Managers);
            File.WriteAllText("managers.json", json);

            json = JsonConvert.SerializeObject(DepartamentsTreeView);
            File.WriteAllText("departamentsTreeView.json", json);

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

        /// <summary>
        /// Добавить новый департамент
        /// </summary>
        /// <param name="TreeViewDepart"></param>
        /// <param name="NewDep"></param>
        private void AddNewDepartTreeView(ObservableCollection<Departament> TreeViewDepart, Departament NewDep)
        {
            if (NewDep.DepartamentParentId == -1)
            {
                TreeViewDepart.Add(NewDep);
            }
            foreach (var item in TreeViewDepart)
            {
                if (NewDep.DepartamentParentId == item.Id)
                {
                    if (item.Departaments == null) item.Departaments = new ObservableCollection<Departament>();
                    item.Departaments.Add(NewDep);
                    //return TreeViewDepart;
                }
                else if (item.Departaments != null)
                {
                    //item.Departaments = GetAllDepart(item.Departaments, NewDep);
                    AddNewDepartTreeView(item.Departaments, NewDep);
                }
                else
                {
                    //return TreeViewDepart;
                    return;
                }
            }

            //return TreeViewDepart;
        }

        /// <summary>
        /// Удалить департамент
        /// </summary>
        /// <param name="TreeViewDepart"></param>
        /// <param name="NewDep"></param>
        private void DeleteDepart(ObservableCollection<Departament> TreeViewDepart, Departament NewDep)
        {
            foreach (var item in TreeViewDepart)
            {
                if (item == NewDep)
                {
                    TreeViewDepart.Remove(item);
                    var del = new ObservableCollection<Worker>();
                    foreach (var worker in Workers)
                    {
                        if(worker.DepartamentId == item.Id)
                        {
                            del.Add(worker);
                        }
                    }
                    var r = Workers.Except(del);
                    Workers = new ObservableCollection<Worker>();
                    foreach (var ren in r)
                    {
                        Workers.Add(ren);
                    }
                    return; 

                }
                else if (item.Departaments != null)
                {
                    DeleteDepart(item.Departaments, NewDep);
                }
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
