using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                if (i == 0)
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
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].Randomize(random, min, max);
            }
        }
        public double[] Compute(double[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                layers[0].Neurons[i].Output = inputs[i];
            }
            double[] outputs = new double[layers.Length];
            for (int i = 1; i < layers.Length; i++)
            {
                outputs = layers[i].Compute();
            }
            return outputs;
        }

        public double GetError(double[] inputs, double[] desiredOutputs)
        {
            double[] outputs = Compute(inputs);
            double totalError = 0;
            for (int i = 0; i < outputs.Length; i++)
            {
                totalError += errorFunc.Function(outputs[i], desiredOutputs[i]);
            }
            return totalError;
        }

        public void ApplyUpdates(double momentum)
        {
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].ApplyUpdates(momentum);
            }
        }

        public void BackProp(double learningRate, double[] desiredOutputs)
        {
            int n = layers.Length;

            for (int j = 0; j < layers[n - 1].Neurons.Length; j++)
            {
                //only one output for xOr
                layers[n - 1].Neurons[j].Delta = errorFunc.Derivative(layers[n - 1].Neurons[j].Output, desiredOutputs[j]);
            }
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                layers[i].BackProp(learningRate);
            }
        }

        public double Train(double[][] inputs, double[][] desiredOutputs, double learningRate, double momentum)
        {
            double[] errorPerRow = new double[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                errorPerRow[i] = GetError(inputs[i], desiredOutputs[i]);
                BackProp(learningRate, desiredOutputs[i]);
            }
            ApplyUpdates(momentum);
            return errorPerRow.Average();
        }

        public double BatchTrain(double[][] inputs, double[][] desiredOutputs, double learningRate, double momentum, int batchSize)
        {
            double[] errorPerRow = new double[inputs.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                errorPerRow[i] = GetError(inputs[i], desiredOutputs[i]);
                BackProp(learningRate, desiredOutputs[i]);

                ApplyUpdates(momentum);

            }
            ApplyUpdates(momentum);
            return errorPerRow.Average();
        }
    }
}
