using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class Connection
    {
        // Use an 'innovation number' to keep track of historical markings
        private static int innovation_cntr = 0; // TODO consider changing this to 'long'

        private Neuron source, dest;
        private double weight;
        private int innovation;
        private Boolean disabled = false;

        public Connection(Neuron source, Neuron dest)
        {
            this.source = source;
            this.dest = dest;
            innovation = innovation_cntr++;
            weight = Utils.nextGaussian(0, 1);
        }

        public double getWeight()
        {
            return weight;
        }

        public void setWeight(double weight)
        {
            this.weight = weight;
        }

        public Neuron getSource()
        {
            return source;
        }

        public Neuron getDest()
        {
            return dest;
        }

        public int getInnovation()
        {
            return innovation;
        }

        public Boolean isDisabled()
        {
            return disabled;
        }

        public void disable()
        {
            disabled = true;
        }
    }
}
