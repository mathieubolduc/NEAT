using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class Population
    {
        private NEATConfig config;
        private Individual[] individuals;
        private List<Species> species = new List<Species>();

        public Population(int nbInputs, int nbOutputs, NEATConfig config)
        {
            this.config = config;

            // Initialize a population with the input and output layers fully connected
            individuals = new Individual[config.populationSize];

            for (int i = 0; i < individuals.Length; i++)
            {
                individuals[i] = new Individual(NeuralGraph.generateFullyConnected(nbInputs + 1, nbOutputs)); // Add a bias input
            }
        }

        public Individual getMaxFitness(Func<Individual, double> fitnessFunc) {
            double maxFitness = double.MinValue;
            Individual champion = null;

            foreach(Individual indiv in individuals) {
                double fitness = fitnessFunc(indiv);
                indiv.setFitness(fitness); // Cache the result
                if (fitness > maxFitness) {
                    maxFitness = fitness;
                    champion = indiv;
                }
            }
            
            return champion;
        }

        public void newGeneration() {
            speciate();
            // TODO remove stagnant species (config.timeToKillStagnant)
            int nbChampions = pickChampions();
            Console.WriteLine(nbChampions);

            // For the remaining free spots in the population, perform mutations and crossovers
            for (int i = nbChampions; i < individuals.Length; i++) {
                if (Utils.rand.NextDouble() < config.interspeciesMatingRate) { // TODO missing config for matingRate?
                    // This is currently unaware of species...
                    Individual i1 = getRandomIndividual(nbChampions);
                    Individual i2 = getRandomIndividual(nbChampions);
                    individuals[i] = Individual.cross(i1, i2, config);
                }
                else {
                    individuals[i] = getRandomIndividual(nbChampions).mutate(config);
                }
            }

            // match innovation numbers between individuals
            matchInnovationNumbers();
        }

        public int pickChampions() {
            int index = 0;
            foreach(Species singleSpecies in species) {
                Individual[] champions = singleSpecies.getChampions(config.nbChampionsPerSpecies);
                foreach (Individual champion in champions) {
                    individuals[index] = champion;
                    index++;
                }
            }

            return index; // Return the total number of champions
        }

        public void matchInnovationNumbers() {
            foreach(Individual indiv in individuals) {
                foreach(Connection conn in indiv.getGraph().getConnectionList()) {

                }
            }
        }

        public void speciate() {
            Dictionary<Individual, Species> representatives = new Dictionary<Individual, Species>();
            int counter = 0;
            // Generate representatives for each species and clear species from the previous generation
            foreach (Species singleSpecies in species) {
                Individual randomIndiv = singleSpecies.getRandomIndividual();
                if (randomIndiv == null) {
                    // TODO delete the species as it is empty
                    continue;
                }
                representatives.Add(randomIndiv, singleSpecies);
                singleSpecies.clear();
                counter++;
            }

            // Put each individual in a species
            foreach (Individual indiv in individuals) {
                bool speciesFound = false;
                foreach (KeyValuePair<Individual, Species> representative in representatives) {
                    if (representative.Key.distanceFrom(indiv, config) <= config.speciesDistance) {
                        representative.Value.addIndividual(indiv);
                        speciesFound = true;
                        break;
                    }
                }
                
                // If no existing species fits the individual, create a new one with the individual as a representative
                if (!speciesFound) {
                    Species newSpecies = new Species();
                    newSpecies.addIndividual(indiv);
                    representatives.Add(indiv, newSpecies);
                    species.Add(newSpecies);
                }
            }
        }

        public Individual getRandomIndividual(int maxIndex) {
            return individuals[Utils.rand.Next(maxIndex)];
        }

        public Individual[] getIndividuals() {
            return individuals;
        }
    }
}
