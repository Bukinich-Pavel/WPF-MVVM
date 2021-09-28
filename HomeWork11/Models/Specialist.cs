using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork11.Models
{
    class Specialist : Worker
    {
        int numberHours;
        public int NumberHours
        {
            get { return numberHours; }
            set
            {
                numberHours = value;
                OnPropertyChanged("NumberHours");                
            }

        }


        int costOfAnHour;
        public int CostOfAnHour
        {
            get { return costOfAnHour; }
            set
            {
                costOfAnHour = value;
                OnPropertyChanged("CostOfAnHour");
            }

        }


        private int salary;
        public new int Salary
        {
            get { return numberHours * costOfAnHour; }
            set
            {
                salary = numberHours * costOfAnHour;
                OnPropertyChanged("Salary");
            }
        }



    }
}
