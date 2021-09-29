using HomeWork11.ViewModel;
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

namespace HomeWork11
{
    class Manager : Worker
    {
        public override string Position
        {
            get
            {
                if (this is Manager)
                {
                    return "Менеджер";
                }
                return "";
            }
        }


        private int salary;
        public new int Salary
        {
            get
            {
                GetAllSalary();
                return salary > 1300 ? salary : 1300;
            }
            set { }
        }


        public void GetAllSalary()
        {
            salary = 0;
            foreach (var item in Departament.AllDepartaments)
            {
                if (item.Id == this.DepartamentId)
                {
                    GetSalaryAllDepartament(item);
                }
            }
        }

        private void GetSalaryAllDepartament(Departament departament)
        {
            ObservableCollection<Worker> Workers = ApplicationViewModel.Workers; //+
            List<Worker> wr = new List<Worker>() { };
            foreach (var item in Workers)
            {
                if (item.DepartamentId == departament.Id) wr.Add(item);
            }

            foreach (var item in wr)
            {
                salary += item.Salary;
            }

            List<Departament> dp = new List<Departament>() { };
            foreach (var item in Departament.AllDepartaments)
            {
                if (item.DepartamentParentId == departament.Id)
                {
                    dp.Add(item);
                }
            }
            if (dp == null) return;

            foreach (var item in dp)
            {
                GetSalaryAllDepartament(item);
            }


        }

    }
}
