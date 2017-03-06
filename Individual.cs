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
        private double fitness;

        public Individual(NeuralGraph graph)
        {
            this.graph = graph;
        }
        
        public Individual mutate(NEATConfig config)
        {
            Individual newIndiv = clone();
            NeuralGraph newGraph = newIndiv.getGraph();

            if (Utils.rand.NextDouble() < config.newConnectionProb) {
                // Select two random neurons and create (or enable) a link in-between
                int hiddenCount = newGraph.getHiddenNeurons().Count();
                int inputCount = newGraph.getInputNeurons().Count();
                int outputCount = newGraph.getOutputNeurons().Count();

                // Select the input and output neurons
                int sourceIndex = Utils.rand.Next(hiddenCount + inputCount + outputCount);
                int destIndex = Utils.rand.Next(hiddenCount + outputCount);
                Neuron source, dest;

                if (sourceIndex < hiddenCount) {
                    source = newGraph.getHiddenNeurons()[sourceIndex];
                }
                else if (sourceIndex < hiddenCount + inputCount) {
                    source = newGraph.getInputNeurons()[sourceIndex - hiddenCount];
                }
                else {
                    source = newGraph.getOutputNeurons()[sourceIndex - hiddenCount - inputCount];
                }

                if (destIndex < hiddenCount) {
                    dest = newGraph.getHiddenNeurons()[destIndex];
                }
                else {
                    dest = newGraph.getOutputNeurons()[destIndex - hiddenCount];
                }

                Connection connection;
                if (!newGraph.getConnections()[dest].TryGetValue(source, out connection)) {
                    connection = new Connection(source, dest, -1); // TODO what the crap is the parent innovation supposed to be here
                    newGraph.addConnection(connection);
                }
                else {
                    connection.enable();
                }
            }

            if (Utils.rand.NextDouble() < config.newNodeProb) {
                // When adding a node, disable a connection and replace it by a neuron and two connections
                // The new input connection to that neuron has weight of 1, while output has same weight as disabled connection
                Connection connection = newGraph.getConnectionList()[Utils.rand.Next(newGraph.getConnectionList().Count)];
                connection.disable();
                Neuron neuron = new Neuron(NeuronType.Hidden);
                Connection newConn1 = new Connection(connection.getSource(), neuron, connection.getInnovation());
                Connection newConn2 = new Connection(neuron, connection.getDest(), connection.getInnovation());
                newGraph.addHiddenNeuron(neuron);
                newGraph.addConnection(newConn1);
                newGraph.addConnection(newConn2);
            }

            // Mutate each connection weight
            foreach (Connection connection in newGraph.getConnectionList()) {
                if (Utils.rand.NextDouble() < config.mutationProb) {
                    if (Utils.rand.NextDouble() < config.uniformPerturbationMutationProb) {
                        connection.setWeight(connection.getWeight() + Utils.rand.NextDouble() * 2 * config.perturbationStep - config.perturbationStep);
                    }
                    else {
                        connection.setWeight(Utils.nextGaussian(0, 1));
                    }
                }
            }

            return newIndiv;
        }

        public static Individual cross(Individual parent1, Individual parent2, NEATConfig config)
        {
            NeuralGraph graph1 = parent1.getGraph();
            NeuralGraph graph2 = parent2.getGraph();

            Dictionary<Neuron, Neuron> addedNeurons = new Dictionary<Neuron, Neuron>();
            NeuralGraph child = new NeuralGraph(graph1.getInputNeurons(), graph1.getOutputNeurons());

            // Add connections from both parents
            List<Connection>.Enumerator it1 = graph1.getConnectionList().GetEnumerator();
            List<Connection>.Enumerator it2 = graph2.getConnectionList().GetEnumerator();
            bool hasNext1 = it1.MoveNext(), hasNext2 = it2.MoveNext();

            while (hasNext1 || hasNext2)
            {
                if (!hasNext1)
                {
                    addClonedConnection(it2.Current, child, addedNeurons);
                    hasNext2 = it2.MoveNext();
                    continue;
                }
                if (!hasNext2)
                {
                    addClonedConnection(it1.Current, child, addedNeurons);
                    hasNext1 = it1.MoveNext();
                    continue;
                }

                int innov1 = it1.Current.getInnovation();
                int innov2 = it2.Current.getInnovation();
                if (innov1 == innov2)
                {
                    // If either parent's connection is disable, there is a chance for it to be disabled in the child
                    bool disabled = false;
                    if (it1.Current.isDisabled() != it2.Current.isDisabled()) {
                        disabled = (Utils.rand.NextDouble() < config.disableGeneProb);
                    }
                    else if (it1.Current.isDisabled()) {
                        disabled = true;
                    }

                    addClonedConnection(it1.Current, child, addedNeurons, disabled);
                    hasNext1 = it1.MoveNext();
                    hasNext2 = it2.MoveNext();
                }
                else if (innov1 > innov2)
                {
                    addClonedConnection(it2.Current, child, addedNeurons);
                    hasNext2 = it2.MoveNext();
                }
                else
                {
                    addClonedConnection(it1.Current, child, addedNeurons);
                    hasNext1 = it1.MoveNext();
                }
            }   

            return new Individual(child);
        }

        private static void addClonedConnection(Connection connection, NeuralGraph child, Dictionary<Neuron, Neuron> addedNeurons) {
            addClonedConnection(connection, child, addedNeurons, connection.isDisabled());
        }

        private static void addClonedConnection(Connection connection, NeuralGraph child, Dictionary<Neuron, Neuron> addedNeurons, bool disabled)
        {
            Neuron[] oldNeurons = new Neuron[] { connection.getSource(), connection.getDest() };
            Neuron[] newNeurons = new Neuron[oldNeurons.Length];

            for (int i = 0; i < oldNeurons.Length; i++)
            {
                if (oldNeurons[i].getType() != NeuronType.Hidden)
                {
                    // TODO !Important! If we allow recurrent connections on outputs, need to clone the outputs too
                    newNeurons[i] = oldNeurons[i];
                    continue;
                }
                if (!addedNeurons.ContainsKey(oldNeurons[i]))
                {
                    newNeurons[i] = oldNeurons[i].clone();
                    addedNeurons.Add(oldNeurons[i], newNeurons[i]);
                    child.addHiddenNeuron(newNeurons[i]);
                }
                else
                {
                    newNeurons[i] = addedNeurons[oldNeurons[i]];
                }
            }

            Dictionary<Neuron, Dictionary<Neuron, Connection>> childConnections = child.getConnections();
            if (childConnections.ContainsKey(newNeurons[1]) && childConnections[newNeurons[1]].ContainsKey(newNeurons[0])) {
                // If the link already exists, enable it
                if (!disabled)
                    childConnections[newNeurons[1]][newNeurons[0]].enable();
            }
            else {
                Connection newConnection = new Connection(newNeurons[0], newNeurons[1], connection.getParentInnovation(), connection.getInnovation(), true);
                newConnection.setDisabled(disabled);
                child.addConnection(newConnection);
            }
        }

        public double distanceFrom(Individual indiv, NEATConfig config)
        {
            List<Connection> connections = graph.getConnectionList();
            List<Connection> otherConnections = indiv.getGraph().getConnectionList();
            int N = Math.Max(connections.Count, otherConnections.Count);

            int D = 0, E = 0; // Number of Disjoint (D) and Excess (E) connections
            double W = 0; // Total weight difference
            int weightCntr = 0;
            List<Connection>.Enumerator it1 = connections.GetEnumerator();
            List<Connection>.Enumerator it2 = otherConnections.GetEnumerator();
            bool hasNext1 = it1.MoveNext(), hasNext2 = it2.MoveNext();

            while (hasNext1 || hasNext2)
            {
                if (!hasNext1)
                {
                    E++;
                    hasNext2 = it2.MoveNext();
                    continue;
                }
                if (!hasNext2)
                {
                    E++;
                    hasNext1 = it1.MoveNext();
                    continue;
                }

                int innov1 = it1.Current.getInnovation();
                int innov2 = it2.Current.getInnovation();
                if (innov1 == innov2)
                {
                    weightCntr++;
                    W += Math.Abs(it1.Current.getWeight() - it2.Current.getWeight());
                    hasNext1 = it1.MoveNext();
                    hasNext2 = it2.MoveNext();
                }
                else if (innov1 > innov2)
                {
                    D++;
                    hasNext2 = it2.MoveNext();
                }
                else
                {
                    D++;
                    hasNext1 = it1.MoveNext();
                }
            }

            return (config.c1 * E) / N + (config.c2 * D) / N + (config.c3 * W) / weightCntr;
        }

        public double[] eval(double[] inputs) {
            return graph.eval(inputs);
        }

        public double getFitness() {
            return fitness;
        }

        public void setFitness(double fitness) {
            this.fitness = fitness;
        }

        public NeuralGraph getGraph()
        {
            return graph;
        }

        public Individual clone() {
            Dictionary<Neuron, Neuron> addedNeurons = new Dictionary<Neuron, Neuron>();
            NeuralGraph cloneGraph = new NeuralGraph(graph.getInputNeurons(), graph.getOutputNeurons());
            // TODO !Important! If we allow recurrent connections on outputs, need to clone the outputs too

            foreach (Connection connection in graph.getConnectionList()) {
                addClonedConnection(connection, cloneGraph, addedNeurons);
            }

            return new Individual(cloneGraph);
        }

        public override string ToString() {
            return graph.ToString();
        }

    }

}
