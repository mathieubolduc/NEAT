using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class Population
    {
        private NEATConfig config;
        private Individual[] individuals;
        private int generation;

        public Population(int nbInputs, int nbOutputs, NEATConfig config)
        {
            this.config = config;

            // Initialize a population with the input and output layers fully connected
            individuals = new Individual[config.populationSize];

            for (int i = 0; i < individuals.Length; i++)
            {
                individuals[i] = new Individual(NeuralGraph.generateFullyConnected(nbInputs + 1, nbOutputs)); // Add a bias input
            }
        }

        public Tuple<Individual, double> getMaxFitness(Func<Individual, double> fitnessFunc) {
            double maxFitness = double.MinValue;
            Individual champion = null;

            foreach(Individual indiv in individuals) {
                double fitness = fitnessFunc(indiv);
                indiv.setFitness(fitness); // Cache the result
                if (fitness > maxFitness) {
                    maxFitness = fitness;
                    champion = indiv;
                }
            }

            Console.WriteLine(champion);
            return new Tuple<Individual, double>(champion, maxFitness);
        }

        public void newGeneration() {
            foreach(Individual indiv in individuals) {
                // TODO remove stagnant species (config.timeToKillStagnant)
                // TODO pick champions of each species
                // TODO crossover within and outside of species

                indiv.mutate(config);

                // TODO match innovation numbers between individuals
                // TODO speciate
            }
        }
    }
}
