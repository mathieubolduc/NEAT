using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    enum NeuronType { Input, Hidden, Output };

    class Neuron
    {
        private double value = 0;
        private NeuronType type;
        private bool visited = false;

        public Neuron(NeuronType type)
        {
            this.type = type;
        }

        public void setValue(double value)
        {
            this.value = value;
        }

        public double getValue()
        {
            return value;
        }

        public NeuronType getType()
        {
            return type;
        }

        public void visit()
        {
            visited = true;
        }

        public bool isVisited()
        {
            return visited;
        }

        public void clear()
        {
            visited = false;
            value = 0;
        }

        public Neuron clone()
        {
            Neuron n = new Neuron(type);
            return n;
        }
    }
}
