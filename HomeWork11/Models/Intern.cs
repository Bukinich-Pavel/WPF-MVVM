using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork11
{
    class Intern : Worker
    {
        public override string Position
        {
            get
            {
                if (this is Intern)
                {
                    return "Стажер";
                }
                return "";
            }
        }
    }
}
