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
                Console.WriteLine("Generation " + generation);

                population.newGeneration();
                currentFitness = population.getMaxFitness(fitnessFunc);
                Console.WriteLine(currentFitness.Item1);
                Console.WriteLine(currentFitness.Item2);

                if (generation == 20)
                    break;
                   
            }

            // Print the final population
            foreach (Individual indiv in population.getIndividuals().OrderBy(o => o.getFitness()))
            {
                Console.WriteLine(indiv);
                Console.WriteLine(indiv.getFitness());
            }

            return currentFitness.Item1;
        }
    }
}
