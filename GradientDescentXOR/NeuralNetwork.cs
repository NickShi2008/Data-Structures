using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradientDescentXOR
{
    public class NeuralNetwork
    {
        Layer[] layers;
        ErrorFunction errorFunc;
        public NeuralNetwork(ActivationFunction activation, ErrorFunction errorFunc,
        params int[] neuronsPerLayer)
        {
            this.errorFunc = errorFunc;
            layers = new Layer[neuronsPerLayer.Length];
            for (int i = 0; i < layers.Length; i++)
            {
                if(i == 0)
                {
                    layers[i] = new Layer(activation, neuronsPerLayer[i]);
                }
                else
                {
                    layers[i] = new Layer(activation, neuronsPerLayer[i], layers[i - 1]);
                }
            }
        }
        public void Randomize(Random random, double min, double max) 
        {
            for(int i = 0; i < layers.Length; i++)
            {
                layers[i].Randomize(random, min, max);
            }
        }
        public double[] Compute(double[] inputs) 
        {
            for(int i = 0; i < inputs.Length; i++)
            {
                layers[0].Neurons[i].Output = inputs[i];
            }
            for(int i = 1; i < layers.Length; i++)
            {
                layers[i].Compute();
            }
            return layers[layers.Length - 1].Outputs;
        }

        public double GetError(double[] inputs, double[] desiredOutputs) 
        {
            double[] outputs = Compute(inputs);
            double totalError = 0;
            for(int i = 0; i < outputs.Length; i++)
            {
                totalError += errorFunc.Function(outputs[i], desiredOutputs[i]);
            }
            return totalError/desiredOutputs.Length;
        }

        public void ApplyUpdates()
        {
            for(int i = 0; i < layers.Length; i++)
            {
                layers[i].ApplyUpdates();
            }
        }

        public void BackProp(double learningRate, double[] desiredOutputs)
        {
            for(int i = layers.Length - 1; i > 0; i--)
            {
                for (int j = 0; j < layers[i].Neurons.Length; j++)
                {
                    if (i == layers.Length - 1)
                    {
                        layers[i].Neurons[j].Delta = errorFunc.Derivative(layers[i].Neurons[j].Output, desiredOutputs[j]);
                    }
                    else
                    {
                        double delta = 0;
                        for (int v = 0; v < layers[i].Neurons.Length; v++)
                        {
                            delta += layers[i].Neurons[v].Delta;
                        }
                            
                        layers[i].Neurons[j].Delta = layers[i + 1].Neurons[j].Delta
                            * layers[i + 1].Neurons[j].Activation.Derivative(layers[i].Neurons[j].Input) * delta 
                            ;
                    }
                    layers[i].Neurons[j].BackProp(learningRate);
                        
                }
            }
        }

        public double Train(double[][] inputs, double[][] desiredOutputs, double learningRate)
        {
            double[] errorPerRow = new double[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                errorPerRow[i] = GetError(inputs[i], desiredOutputs[i]);
                BackProp(learningRate, desiredOutputs[i]);
                ApplyUpdates();
            }
            return errorPerRow.Average();
        }
    }
}
