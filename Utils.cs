using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    abstract class Utils
    {
        private const int SEED = 424242;
        public static readonly Random rand = new Random(SEED);

        public static double nextGaussian(double mean, double stdDev)
        {
            // See http://stackoverflow.com/a/218600
            Random rand = new Random(); //reuse this if you are generating many
            double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
            return mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)
        }

        public static double sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-4.9*x));
        }

        public static Connection get(NeuralGraph neuralGraph, Neuron Source, Neuron dest)
        {
            Dictionary<Neuron, Connection> foo;
            neuralGraph.getConnections().TryGetValue(dest, out foo);
            Connection bar;
            foo.TryGetValue(dest, out bar);
            return bar;
        }
    }
}
