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
        private bool disabled = false;

        public Connection(Neuron source, Neuron dest) : this(source, dest, innovation_cntr++) { }

        public Connection(Neuron source, Neuron dest, int innovation) {
            this.source = source;
            this.dest = dest;
            this.innovation = innovation;
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

        public bool isDisabled()
        {
            return disabled;
        }

        public void disable()
        {
            disabled = true;
        }

        public void enable() {
            disabled = false;
        }

        public override string ToString() {
            string disabledText = disabled ? "\t(Disabled)" : "";
            return (source.GetHashCode() + " --(" + weight + ")--> " + dest.GetHashCode() + disabledText);
        }
    }
}
