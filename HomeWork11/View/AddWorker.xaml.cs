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

    }
}
