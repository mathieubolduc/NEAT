using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class NEAT
    {
        private NEATConfig config;

        private Population population;
        private int generation = 0;

        public NEAT(int nbInputs, int nbOutputs, NEATConfig config)
        {
            this.config = config;

            Console.WriteLine("Generating initial population");
            population = new Population(nbInputs, nbOutputs, config);
            Console.WriteLine("Done");
        }

        public Individual runUntil(double desiredFitness, Func<Individual, double> fitnessFunc)
        {
            Tuple<Individual, double> currentFitness = population.getMaxFitness(fitnessFunc);
            Console.WriteLine(currentFitness.Item2);
            while (currentFitness.Item2 < desiredFitness) {
                // Create a new generation
                generation++;
                population.newGeneration();
                currentFitness = population.getMaxFitness(fitnessFunc);
                Console.WriteLine(currentFitness.Item2);

                if (generation == 20)
                    break;
            }
            return null;
        }
    }
}
