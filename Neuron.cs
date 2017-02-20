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
        private HashSet<Connection> inputs = new HashSet<Connection>(); // Using a hashset prevents having duplicate connections
        private bool visited = false;

        public Neuron(NeuronType type)
        {
            this.type = type;
        }

        public bool addInput(Connection connection)
        {
            if (type == NeuronType.Input)
                throw new InvalidOperationException("An Input neuron cannot have inputs");

            return inputs.Add(connection); // Returns false if the connection already exists
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

        public HashSet<Connection> getInputs()
        {
            return inputs;
        }

        public void visit()
        {
            visited = true;
        }

        public bool isVisited()
        {
            return visited;
        }

        public void clearVisited()
        {
            visited = false;
        }
    }
}
