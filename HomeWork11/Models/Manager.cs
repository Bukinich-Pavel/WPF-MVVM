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
        private int salary;
        public new int Salary
        {
            get
            {
                GetAllSalary();
                return salary;
            }
        }

        public void GetAllSalary()
        {
            salary = 0;
            //var selectedDepartaments = Departament.AllDepartaments.Where(u => u.Id == this.DepartamentId);
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
            List<Worker> wr = GetWorkersDepartament(departament);
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

        private static List<Worker> GetWorkersDepartament(Departament departament)
        {
            List<Worker> Workers = new List<Worker> { };

            string str = File.ReadAllText("interns.json");
            List<Intern> Interns = JsonConvert.DeserializeObject<List<Intern>>(str);
            foreach (var item in Interns)
            {
                Workers.Add(item);
            }

            str = File.ReadAllText("managers.json");
            List<Manager> Managers = JsonConvert.DeserializeObject<List<Manager>>(str);
            foreach (var item in Managers)
            {
                Workers.Add(item);
            }

            List<Worker> wr = new List<Worker>() { };
            foreach (var item in Workers)
            {
                if (item.DepartamentId == departament.Id) wr.Add(item);
            }

            return wr;
        }
    }
}
