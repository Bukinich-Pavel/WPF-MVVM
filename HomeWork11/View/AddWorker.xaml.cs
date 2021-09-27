using HomeWork11.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HomeWork11.View
{
    /// <summary>
    /// Логика взаимодействия для AddWorker.xaml
    /// </summary>
    public partial class AddWorker : Window
    {
        public AddWorker()
        {
            InitializeComponent();
            DataContext = new WorkerViewModel();
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public ObservableCollection<string> ListDepartament
        {
            set { listDepartament.ItemsSource = value; }
        }

        public ObservableCollection<string> ListPosition
        {
            set { listPosition.ItemsSource = value; }
        }

        private void listPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string str = ((Selector)sender).SelectedItem.ToString();

            switch (str)
            {
                case "Менеджер":
                    Salary.Visibility = Visibility.Hidden;
                    PriceHour.Visibility = Visibility.Hidden;
                    NumberOfHours.Visibility = Visibility.Hidden;
                    break;
                case "Специалист":
                    Salary.Visibility = Visibility.Hidden;
                    PriceHour.Visibility = Visibility.Visible;
                    NumberOfHours.Visibility = Visibility.Visible;
                    break;
                case "Стажер":
                    Salary.Visibility = Visibility.Visible;
                    PriceHour.Visibility = Visibility.Hidden;
                    NumberOfHours.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }
        }
    }
}
