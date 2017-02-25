using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class NEATConfig
    {
        public float c1 = 1, c2 = 1, c3 = 0.4f;
        public float speciesDistance = 3;
        public float newLinkProb = 0.3f;
        public float interspeciesMatingRate = 0.001f;
        public float newNodeProb = 0.03f, newConnectionProb = 0.05f;
        public float disableGeneProb = 0.75f; // If the gene is disabled in either parent
        public float mutationProb = 0.8f; // TODO When this happens each weight is modified?
        public float uniformPerturbationMutationProb = 0.9f; // In the remaining 10%, the weights are assigned a new random value
        public int populationSize = 150;
        public int minSpeciesSizeForChampion = 5;
        public int timeToKill = 15; // If the max fitness of a species does not increase in this number of generation, it goes extinct
        // TODO  "In each generation, 25% of offspring resulted from mutation without crossover"
    }
}
