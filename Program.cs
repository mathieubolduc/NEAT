using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class Program
    {
        static void Main(string[] args) {

            // NEAT XOR Test
            NEATConfig config = new NEATConfig();
            NEAT neat = new NEAT(2, 1, config);

            Individual champion = neat.runUntil(15.95, (indiv) => {
                double[][] inputs = {
                    new double[] { 0, 0 },
                    new double[] { 0, 1 },
                    new double[] { 1, 0 },
                    new double[] { 1, 1 }
                };
                double[] expectedOutput = new double[] { 0, 1, 1, 0 };

                double fitness = 0;

                for (int i = 0; i < 4; i++) {
                    fitness += Math.Abs(expectedOutput[i] - indiv.eval(inputs[i])[0]);
                }

                return Math.Pow(4 - fitness, 2);
            });

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.Write("Press a key to terminate:");
                Console.ReadLine();
            }
                    
        }
    }
}
