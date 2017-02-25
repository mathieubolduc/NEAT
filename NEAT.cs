using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class NEAT
    {
        private float c1 = 1, c2 = 1, c3 = 0.4f;
        private float speciesDistance = 3;
        private float newLinkProb = 0.3f;
        private float interspeciesMatingRate = 0.001f;
        private float newNodeProb = 0.03f, newConnectionProb = 0.05f;
        private float disableGeneProb = 0.75f; // If the gene is disabled in either parent
        private float mutationProb = 0.8f; // TODO When this happens each weight is modified?
        private float uniformPerturbationMutationProb = 0.9f; // In the remaining 10%, the weights are assigned a new random value
        private int populationSize = 150;
        private int minSpeciesSizeForChampion = 5;
        private int timeToKill = 15; // If the max fitness of a species does not increase in this number of generation, it goes extinct
        // TODO  "In each generation, 25% of offspring resulted from mutation without crossover"

        private Population population;
        private int generation = 0;

        public NEAT(int nbInputs, int nbOutputs)
        {
            Console.WriteLine("Generating initial population");
            population = new Population(populationSize, nbInputs, nbOutputs);
            Console.WriteLine("Done");
        }

        // TODO constructor allowing to change settings or, alternately, a Config object

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
