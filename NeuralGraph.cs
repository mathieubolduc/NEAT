﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class NeuralGraph
    {

        // Neurons
        private List<Neuron> hiddenNeurons = new List<Neuron>();
        private Neuron[] inputNeurons;
        private Neuron[] outputNeurons;

        // Connections
        private Dictionary<Neuron, Dictionary<Neuron, Connection>> connections;     // 1st key = dest, 2nd key = source
        private List<Connection> connectionList;                                    // sorted by inno #

        // Constructs an empty neuralGraph
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

            connections = new Dictionary<Neuron, Dictionary<Neuron, Connection>>();
            connectionList = new List<Connection>();
        }

        public NeuralGraph(Neuron[] inputNeurons, Neuron[] outputNeurons)
        {
            this.inputNeurons = inputNeurons;
            this.outputNeurons = outputNeurons;
            connections = new Dictionary<Neuron, Dictionary<Neuron, Connection>>();
            connectionList = new List<Connection>();
        }

        public static NeuralGraph generateFullyConnected(int nbInputs, int nbOutputs)
        {
            NeuralGraph graph = new NeuralGraph(nbInputs, nbOutputs);

            // Use specific innovation numbers such that all graphs created through this method are identical
            int innovation = 0;

            foreach (Neuron input in graph.getInputNeurons())
            {
                foreach (Neuron output in graph.getOutputNeurons())
                {
                    graph.addConnection(new Connection(input, output, -1, innovation++, true));
                }
            }

            return graph;
        }

        public double[] eval(double[] inputs)
        {
            if (inputs.Length + 1 != inputNeurons.Length)
                throw new ArgumentException("'inputs' size did not match the number of input neurons");

            for (int i = 0; i < inputs.Length; i++)
            {
                inputNeurons[i].setValue(inputs[i]);
            }
            // Bias input:
            inputNeurons[inputNeurons.Length - 1].setValue(1);

            double[] outputs = new double[outputNeurons.Length];
            int counter = 0;
            foreach (Neuron outputNeuron in outputNeurons)
            {
                outputs[counter++] = evalNeuron(outputNeuron); // Note that calling this method multiple times is not inefficient since the 'visited' attribute is not reset
            }

            // Clear the 'visited' attribute on all neurons
            foreach (Neuron neuron in outputNeurons)
            {
                neuron.clear();
            }

            foreach (Neuron neuron in inputNeurons)
            {
                neuron.clear();
            }

            foreach (Neuron neuron in hiddenNeurons)
            {
                neuron.clear();
            }
            
            return outputs;
        }

        private double evalNeuron(Neuron neuron)
        {
            Dictionary<Neuron, Connection> inputs;
            if (!connections.TryGetValue(neuron, out inputs)) { // This means the neuron is of type 'Input'
                return neuron.getValue();
            }

            neuron.visit();

            double sum = 0;
            foreach (Connection connection in inputs.Values)
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

        public void addHiddenNeuron(Neuron neuron)
        {
            hiddenNeurons.Add(neuron);
        }

        public void addConnection(Connection connection)
        {
            // Update dictionary
            Neuron dest = connection.getDest();
            if (!connections.ContainsKey(dest))
            {
                connections.Add(dest, new Dictionary<Neuron, Connection>());
            }
            connections[dest].Add(connection.getSource(), connection);

            // Update List
            connectionList.Add(connection);

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

        public Dictionary<Neuron, Dictionary<Neuron, Connection>> getConnections()
        {
            return connections;
        }

        public List<Connection> getConnectionList()
        {
            return connectionList;
        }

        public override string ToString() {
            string output = "";

            output += (inputNeurons.Length + " inputs and " + outputNeurons.Length + " outputs\n");
            foreach (Connection connection in connectionList) {
                output += connection.ToString() + "\n";
            }

            return output;
        }

    }
}
