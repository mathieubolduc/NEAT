using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class Species
    {
        private List<Individual> individuals = new List<Individual>();
        
        public List<Individual> getIndividuals() {
            return individuals;
        }

        public void addIndividual(Individual indiv) {
            individuals.Add(indiv);
        }

        public Individual getRandomIndividual() {
            if (individuals.Count > 0)
                return individuals[Utils.rand.Next(individuals.Count)];
            else
                return null;
        }

        public void clear() {
            individuals.Clear();
        }

    }

}
