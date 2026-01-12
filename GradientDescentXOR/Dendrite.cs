using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradientDescentXOR
{
    public class Dendrite
    {
        public double Weight { get; set; }
        public Neuron Previous { get; }
        public Neuron Next { get; }
        public double WeightUpdate { get; set; }

        public Dendrite(Neuron previous, Neuron next, double weight)
        {
            Previous = previous;
            Next = next;
            Weight = weight;
        }

        public double Compute()
        {
            return Previous.Output* Weight;
        }

        public void ApplyUpdates()
        {
            Weight += WeightUpdate;
            WeightUpdate = 0;
        }
    }
}
