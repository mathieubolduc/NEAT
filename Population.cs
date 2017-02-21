using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class Population
    {
        private Individual[] individuals;

        public Population(int populationSize, int nbInputs, int nbOutputs)
        {
            // Initialize a population with the input and output layers fully connected
            individuals = new Individual[populationSize];

            for (int i = 0; i < individuals.Length; i++)
            {
                individuals[i] = new Individual(NeuralGraph.generateFullyConnected(nbInputs + 1, nbOutputs)); // Add a bias input
            }
        }
    }
}
