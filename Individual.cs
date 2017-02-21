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

        public void cross(Individual indiv, float disableGeneProb)
        {

        }

        public double distanceFrom(Individual indiv, float c1, float c2, float c3)
        {
            List<Connection> connections = graph.getConnections();
            List<Connection> otherConnections = indiv.getGraph().getConnections();
            int N = Math.Max(connections.Count, otherConnections.Count);

            int D = 0, E = 0; // Number of Disjoint (D) and Excess (E) connections
            double W = 0; // Total weight difference
            int weightCntr = 0;
            List<Connection>.Enumerator it1 = connections.GetEnumerator();
            List<Connection>.Enumerator it2 = otherConnections.GetEnumerator();
            bool hasNext1 = true, hasNext2 = true;

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

        public NeuralGraph getGraph()
        {
            return graph;
        }

    }

}
