using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovarianceProject
{  
    class Program
    {
        class A { }
        class B : A {}
        delegate void ContrvariantDelegate<in T>(T item);
        delegate T CovariantDelegate<out T>();

        interface ICovariant<out T>
        {
            T Process();
        }
        
        interface IContrvariant<in T> 
        {
            void Process(T v);
        }

        interface IGenericInterface<in K, out V>
        {
            V Process(K value);
        }
        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/covariance-contravariance/using-variance-in-delegates
        //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/covariance-contravariance/using-variance-for-func-and-action-generic-delegates
        
        static void Main(string[] args)
        {
            //Delegate Covariance / Contrvariance
            {
                Action<A> f1 = ProcessA;
                //Contrvariance - function which accepts less derived type, can be assigned to more derived type delegate
                //All Action delegate are contrvariant
                Action<B> f2 = f1;
                
                ContrvariantDelegate<A> f3 = ProcessA;
                //Direct contrvariant assignment is always supported (because it is basically wrapping in new Action())
                ContrvariantDelegate<B> f5 = ProcessA;
                //This won't compile if "in" keyword is missing in Delegate definition
                ContrvariantDelegate<B> f6 = f3;

                CovariantDelegate<B> f7 = ReturnB;
                //Direct covariant assignment is always supported (because it is basically wrapping in new Action())
                CovariantDelegate<A> f8 = ReturnB;
                //This won't compile if "out" keyword is missing in Delegate definition
                CovariantDelegate<A> f9 = f7;
            }
            
            //Collection Invariance
            //Those lines won't compile
            //List<A> list = new List<B>();
            //IList<A> list1 = new List<B>();
            //IEnumerable though supports covariance because you can not change it
            var list2 = new List<B>();
            IEnumerable<A> list3 = list2;
            
            //Arrays support variance but can emmit runtime exception
            A[] array = new B[1];
            //Like in this case
            array[0] = new A();
            
            //Interface Covariance Contrvariance
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/covariance-contravariance/creating-variant-generic-interfaces
            ICovariant<B> interfaceExample = null;
            //This won't compile without "out" keyword
            ICovariant<A> i = interfaceExample;

            IContrvariant<A> interfaceExample2 = null;
            //This won't compile without "in" keyword
            IContrvariant<B> i2 = interfaceExample2;
            
            //Accepts A, returns B
            IGenericInterface<A, B> interfaceExample3 = null;
            //Accepts B, returns A
            IGenericInterface<B, A> i3 = interfaceExample3;
            //Basically Contrvariance allows to accept more derived arguments and covariance to return less derived arguments
        }
             

        static void ProcessA(A value)
        {
            
        }

        static B ReturnB()
        {
            return new B();
        }
    }
}
