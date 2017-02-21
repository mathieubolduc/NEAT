﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    enum NeuronType { Input, Hidden, Output };

    class Neuron
    {
        private static int innovation_cntr = 0; // TODO consider changing this to 'long'

        private double value = 0;
        private NeuronType type;
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

        public Neuron clone()
        {
            Neuron n = new Neuron(type);
            return n;
        }
    }
}
