using HomeWork11.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HomeWork11
{
    /// <summary>
    /// Логика взаимодействия для AddDepartament.xaml
    /// </summary>
    public partial class AddDepartament : Window
    {
        public AddDepartament()
        {
            InitializeComponent();
            DataContext = new DepartViewModel();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string NameDepartament
        {
            get { return nameDepartament.Text; }
        }

        public ObservableCollection<string> ListDepartament
        {
            set { listDepartament.ItemsSource = value; }
        }
    }
}
