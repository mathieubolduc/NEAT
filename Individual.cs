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

        // TODO do we want to return a new individual or modify this one?
        public void mutate(float newNodeProb, float newConnectionProb)
        {

        }

        public static Individual cross(Individual parent1, Individual parent2, float disableGeneProb)
        {
            // TODO handle disabled connection
            NeuralGraph graph1 = parent1.getGraph();
            NeuralGraph graph2 = parent2.getGraph();

            Dictionary<Neuron, Neuron> addedNeurons = new Dictionary<Neuron, Neuron>();
            NeuralGraph child = new NeuralGraph(graph1.getInputNeurons(), graph1.getOutputNeurons());

            // Add neurons from both parents
            List<Connection>.Enumerator it1 = graph1.getConnectionList().GetEnumerator();
            List<Connection>.Enumerator it2 = graph2.getConnectionList().GetEnumerator();
            bool hasNext1 = it1.MoveNext(), hasNext2 = it2.MoveNext();

            while (hasNext1 || hasNext2)
            {
                if (!hasNext1)
                {
                    addConnectionInCross(it2.Current, child, addedNeurons);
                    hasNext2 = it2.MoveNext();
                    continue;
                }
                if (!hasNext2)
                {
                    addConnectionInCross(it1.Current, child, addedNeurons);
                    hasNext1 = it1.MoveNext();
                    continue;
                }

                int innov1 = it1.Current.getInnovation();
                int innov2 = it2.Current.getInnovation();
                if (innov1 == innov2)
                {
                    addConnectionInCross(it1.Current, child, addedNeurons);
                    hasNext1 = it1.MoveNext();
                    hasNext2 = it2.MoveNext();
                }
                else if (innov1 > innov2)
                {
                    addConnectionInCross(it2.Current, child, addedNeurons);
                    hasNext2 = it2.MoveNext();
                }
                else
                {
                    addConnectionInCross(it1.Current, child, addedNeurons);
                    hasNext1 = it1.MoveNext();
                }
            }   

            return new Individual(child);
        }

        private static void addConnectionInCross(Connection connection, NeuralGraph child, Dictionary<Neuron, Neuron> addedNeurons)
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
            
            child.addConnection(new Connection(newNeurons[0], newNeurons[1]));
        }

        public double distanceFrom(Individual indiv, float c1, float c2, float c3)
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

            return (c1 * E) / N + (c2 * D) / N + (c3 * W) / weightCntr;
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

        public override string ToString() {
            return graph.ToString();
        }

    }

}
