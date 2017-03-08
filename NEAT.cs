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
            Individual champion = population.getMaxFitness(fitnessFunc);
            Console.WriteLine(champion.getFitness());
            while (champion.getFitness() < desiredFitness) {
                // Create a new generation
                generation++;
                Console.WriteLine("Generation " + generation);

                population.newGeneration();
                champion = population.getMaxFitness(fitnessFunc);
                Console.WriteLine(champion);
                Console.WriteLine(champion.getFitness());

                if (generation == 20)
                    break;
                   
            }

            // Print the final population
            foreach (Individual indiv in population.getIndividuals().OrderBy(o => o.getFitness()))
            {
                Console.WriteLine(indiv);
                Console.WriteLine(indiv.getFitness());
            }

            return champion;
        }
    }
}
