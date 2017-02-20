﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class NeuralGraph
    {
        private List<Neuron> hiddenNeurons = new List<Neuron>();
        private Neuron[] inputNeurons;
        private Neuron[] outputNeurons;
        private List<Connection> connections = new List<Connection>();

        public NeuralGraph(int nbInputs, int nbOutputs) {
            inputNeurons = new Neuron[nbInputs];
            outputNeurons = new Neuron[nbOutputs];

            for(int i = 0; i < inputNeurons.Length; i++)
            {
                inputNeurons[i] = new Neuron(NeuronType.Input);
            }
            for (int i = 0; i < outputNeurons.Length; i++)
            {
                outputNeurons[i] = new Neuron(NeuronType.Output);
            }
        }

        public NeuralGraph(Neuron[] inputNeurons, Neuron[] outputNeurons, List<Neuron> hiddenNeurons, List<Connection> connections)
        {
            this.inputNeurons = inputNeurons;
            this.outputNeurons = outputNeurons;

            if (hiddenNeurons != null)
            {
                foreach (Neuron neuron in hiddenNeurons)
                {
                    addHiddenNeuron(neuron);
                }
            }
            
            if (connections != null)
            {
                foreach (Connection connection in connections)
                {
                    addConnection(connection);
                }
            }
            
        }

        public static NeuralGraph generateFullyConnected(int nbInputs, int nbOutputs)
        {
            NeuralGraph graph = new NeuralGraph(nbInputs, nbOutputs);

            foreach (Neuron input in graph.getInputNeurons())
            {
                foreach (Neuron output in graph.getOutputNeurons())
                {
                    graph.addConnection(new Connection(input, output));
                }
            }

            return graph;
        }

        public double[] eval()
        {
            double[] outputs = new double[outputNeurons.Length];
            int counter = 0;
            foreach (Neuron outputNeuron in outputNeurons)
            {
                outputs[counter++] = evalNeuron(outputNeuron); // Note that calling this method multiple times is not inefficient since the 'visited' attribute is not reset
            }

            // Clear the 'visited' attribute on all neurons
            foreach (Neuron outputNeuron in outputNeurons)
            {
                clearVisitedNeurons(outputNeuron);
            }

            return outputs;
        }

        private double evalNeuron(Neuron neuron)
        {
            HashSet<Connection> inputs = neuron.getInputs();
            if (inputs.Count == 0) // This means the neuron is of type 'Input'
            {
                return neuron.getValue();
            }

            neuron.visit();

            double sum = 0;
            foreach (Connection connection in inputs)
            {
                Neuron source = connection.getSource();
                if (!source.isVisited())
                    sum += evalNeuron(source) * connection.getWeight();
                else
                    sum += source.getValue() * connection.getWeight(); // In the case where there is a recurrent connection, the value won't be evaluated yet and thus the returned value will be the one from t-1
            }

            if (neuron.getType() != NeuronType.Input) // Should activation only be applied to hidden neurons?
                sum = Utils.sigmoid(sum);

            neuron.setValue(sum);

            return sum;
        }

        private void clearVisitedNeurons(Neuron neuron)
        {
            if (neuron.isVisited())
                return;

            neuron.clearVisited();
            foreach (Connection connection in neuron.getInputs())
            {
                clearVisitedNeurons(connection.getSource());
            }
        }

        public void addHiddenNeuron(Neuron neuron)
        {
            hiddenNeurons.Add(neuron);
        }

        public void addConnection(Connection connection)
        {
            connections.Add(connection);
            connection.getDest().addInput(connection);
        }

        public Neuron[] getOutputNeurons()
        {
            return outputNeurons;
        }

        public Neuron[] getInputNeurons()
        {
            return inputNeurons;
        }

        public List<Neuron> getHiddenNeurons()
        {
            return hiddenNeurons;
        }

        public List<Connection> getConnections()
        {
            return connections;
        }
    }
}
