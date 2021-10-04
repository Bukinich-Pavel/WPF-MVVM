using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Test_TreeView
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        ObservableCollection<Departament> departaments;
        public MainWindow()
        {
            InitializeComponent();

            departaments = new ObservableCollection<Departament>
            {
                new Departament
                {
                    Name ="Европа",
                    Departaments = new ObservableCollection<Departament>
                    {
                        new Departament {Name="Германия" },
                        new Departament {Name="Франция" },
                        new Departament
                        {
                            Name ="Великобритания",
                            Departaments = new ObservableCollection<Departament>
                            {
                                new Departament {Name="Англия" },
                                new Departament {Name="Шотландия" },
                                new Departament {Name="Уэльс" },
                                new Departament {Name="Сев. Ирландия" },
                            }
                        }
                    }
                }
            };

            treeView1.ItemsSource = departaments;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var r = treeView1.SelectedItem;
            Departament dep = r as Departament;
            if (dep.Departaments == null)
            {
                dep.Departaments = new ObservableCollection<Departament>() { new Departament { Name = "Беларусь" } };
            }
            else
            {
                dep.Departaments.Add(new Departament { Name = "Беларусь" });
            }
        }

        ~MainWindow()
        {
            string json = JsonConvert.SerializeObject(departaments);
            File.WriteAllText("departaments.json", json);

        }
    }
}
