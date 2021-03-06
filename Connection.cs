﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neat
{
    class Connection
    {
        // Use an 'innovation number' to keep track of historical markings
        private static int innovation_cntr = 0; // TODO consider changing this to 'long'

        private Neuron source, dest;
        private double weight;
        private int innovation;
        private bool disabled = false;

        private bool innovationChecked = false;
        private int parentInnovation = -1;

        public Connection(Neuron source, Neuron dest, int parentInnovation) : this(source, dest, parentInnovation, innovation_cntr + 1, false) { }

        public Connection(Neuron source, Neuron dest, int parentInnovation, int innovation, bool innovationChecked) {
            this.source = source;
            this.dest = dest;
            this.parentInnovation = parentInnovation;
            this.innovation = innovation;
            if (innovation_cntr < innovation) {
                innovation_cntr = innovation;
            }
            this.innovationChecked = innovationChecked;
            weight = Utils.nextGaussian(0, 1);
        }

        public double getWeight()
        {
            return weight;
        }

        public void setWeight(double weight)
        {
            this.weight = weight;
        }

        public Neuron getSource()
        {
            return source;
        }

        public Neuron getDest()
        {
            return dest;
        }

        public int getInnovation()
        {
            return innovation;
        }

        public bool isDisabled()
        {
            return disabled;
        }

        public void setDisabled(bool disabled) {
            this.disabled = disabled;
        }

        public void disable()
        {
            disabled = true;
        }

        public void enable() {
            disabled = false;
        }

        public bool isInnovationChecked() {
            return innovationChecked;
        }

        public void setInnovationChecked(bool innovationChecked) {
            this.innovationChecked = innovationChecked;
        }

        public int getParentInnovation() {
            return parentInnovation;
        }

        public override string ToString() {
            string disabledText = disabled ? "\t(Disabled)" : "";
            string inputText = (source.getType() == NeuronType.Input) ? "(Input)\t" : "\t";
            string outputText = (dest.getType() == NeuronType.Output) ? "\t(Output)" : "\t";
            return (inputText + source.GetHashCode() + " --(" + Math.Round(weight, 2) + ",\t" + innovation + ",\t" + innovationChecked + ",\t" + parentInnovation + ")-->\t" + dest.GetHashCode() + outputText + disabledText);
        }
    }
}
