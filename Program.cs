using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class Program
    {
        static void Main(string[] args)
        {
           // NEAT neat = new NEAT(2, 1);

            Individual i1 = new Individual(NeuralGraph.generateFullyConnected(2, 1));
            Individual i2 = new Individual(NeuralGraph.generateFullyConnected(2, 1));
            Individual child = Individual.cross(i1, i2, 0.75f);

            Console.WriteLine("Individual 1:");
            Console.WriteLine(i1);
            Console.WriteLine("Individual 2:");
            Console.WriteLine(i2);
            Console.WriteLine("Child:");
            Console.WriteLine(child);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.Write("Press a key to terminate:");
                Console.ReadLine();
            }
                    
        }
    }
}
