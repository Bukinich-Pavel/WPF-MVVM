using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    interface I1
    {
        void M();
    }

    interface I2
    {
        void M();
    }

    class A : I1, I2
    {
        public void M() { Console.WriteLine("A.M()"); }
        void I1.M() { Console.WriteLine("I1.M()"); }
        void I2.M() { Console.WriteLine("I2.M()"); }
    }

    class B : A, I1, I2
    {
        public new void M() { Console.WriteLine("B.M()"); }
        void I1.M() { Console.WriteLine("I1.M()"); }
        void I2.M() { Console.WriteLine("I2.M()"); }

    }

    class Program
    {
        static void Main(string[] args)
        {
            A a = new A();
            a.M();
            I1 ai1 = new A();
            ai1.M();
            I2 ai2 = new A();
            ai2.M();


            B b = new B();
            b.M();
            I1 bi1 = new B();
            bi1.M();
            I2 bi2 = new B();
            bi2.M();

        }
    }

}
