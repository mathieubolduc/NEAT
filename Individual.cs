using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class Individual
    {
        private NeuralGraph graph;

        public Individual(NeuralGraph graph)
        {
            this.graph = graph;
        }

        // TODO do we want to return a new individual or modify this one?
        public void mutate(float newNodeProb, float newConnectionProb)
        {

        }

        public void cross(Individual indiv)
        {

        }

    }

}
