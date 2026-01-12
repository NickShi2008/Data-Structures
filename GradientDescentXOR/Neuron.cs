using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradientDescentXOR
{
    public class Neuron
    {
        double bias;
        Dendrite[] dendrites;
        public double Output { get; set; }
        public double Input { get; private set; }
        public ActivationFunction Activation { get; set; }

        public double Delta { get; set; }
        double biasUpdate;

        public Neuron(ActivationFunction activation, Neuron[] previousNeurons)
        {
            dendrites = new Dendrite[previousNeurons.Length];
            Activation = activation;
            for (int i = 0; i < previousNeurons.Length; i++)
            {
                dendrites[i] = new Dendrite(previousNeurons[i], this, bias);
            }
        }

        public double Compute()
        {
            double val = bias;
            for(int i = 0; i < dendrites.Length; i++)
            {
                val += dendrites[i].Compute();
            }
            return Activation.Function(val);
        }

        public void Randomize(Random random, double min, double max)
        {
            for(int i = 0; i < dendrites.Length;i ++)
            {
                dendrites[i].Weight = random.NextDouble() * (max - min) + min;
            }
            bias = random.NextDouble() * (max - min) + min;
        }

        public void ApplyUpdates()
        {
            bias += biasUpdate;
            biasUpdate = 0;
            for(int i = 0; i< dendrites.Length; i++)
            {
                dendrites[i].ApplyUpdates();
            }
        }

        public void BackProp(double learningRate)
        {
            for(int i = 0; i < dendrites.Length; i++)
            {
                dendrites[i].WeightUpdate += learningRate * Delta * Activation.Derivative(Input) * dendrites[i].Previous.Output;
                biasUpdate += learningRate * Delta * Activation.Derivative(Input);
               
                
            }
            Delta = 0;
        }
    }
}
