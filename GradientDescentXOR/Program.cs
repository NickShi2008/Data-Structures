using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GradientDescentXOR
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ActivationFunction sigmoid = new ActivationFunction(
                x => 1.0 / (1.0 + Math.Exp(-x)),
                x =>
                {
                    double fx = 1.0 / (1.0 + Math.Exp(-x));
                    return fx * (1 - fx);
                }
            );

           
            ErrorFunction errorFunc = new ErrorFunction(
                (output, target) =>Math.Pow(output - target, 2),
                (output, target) => output - target
            );
            
            NeuralNetwork neuralNetworkXOR = new NeuralNetwork(
                sigmoid, errorFunc, 2, 5, 1
            );
            

            double[][] gateInputs = {
                new double[] { 0, 0 },
                new double[] { 0, 1 },
                new double[] { 1, 0 },
                new double[] { 1, 1 },
            };
            double error = 1;

            double[][] xORExpected =
            {
                new double[1] { 0 },
                new double[1] { 1 },
                new double[1] { 1 },
                new double[1] { 0 },
            };
            double[][] ANDExpected =
            {
                new double[1] { 0 },
                new double[1] { 0 },
                new double[1] { 0 },
                new double[1] { 1 },
            };

            neuralNetworkXOR.Randomize(new Random(), -1, 1);
            double learningRate = 0.03;
            double momentum = 0.9;
            //do
            //{
            //    error = neuralNetworkXOR.Train(gateInputs, xORExpected, learningRate, momentum);
            //    Console.WriteLine(error);
            //    for (int i = 0; i < gateInputs.Length; i++)
            //    {
            //        double[] output = neuralNetworkXOR.Compute(gateInputs[i]);
            //        Console.WriteLine($"Input: {gateInputs[i][0]}, {gateInputs[i][1]} " +
            //            $"=> Output: {output[0]}" + $" Error: {neuralNetworkXOR.GetError(gateInputs[i], xORExpected[i])}");
            //    }
            //} while (error > 0.0001);
            //do
            //{
            //    error = neuralNetworkXOR.Train(gateInputs, ANDExpected, learningRate, 0.9);
            //    Console.WriteLine(error);
            //    for (int i = 0; i < gateInputs.Length; i++)
            //    {
            //        double[] output = neuralNetworkXOR.Compute(gateInputs[i]);
            //        Console.WriteLine($"Input: {gateInputs[i][0]}, {gateInputs[i][1]} " +
            //            $"=> Output: {output[0]}" + $" Error: {neuralNetworkXOR.GetError(gateInputs[i], xORExpected[i])}");
            //    }
            //} while (error > 0.0001);

            ActivationFunction tanH = new ActivationFunction(
              x => (Math.Pow(Math.E, x) - Math.Pow(Math.E, -x)) / (Math.Pow(Math.E, x) + Math.Pow(Math.E, -x)),
              x =>
              {
                  double fx = (Math.Pow(Math.E, x) - Math.Pow(Math.E, -x)) / (Math.Pow(Math.E, x) + Math.Pow(Math.E, -x));
                  return 1 - Math.Pow(fx, 2);
              }
            );

            NeuralNetwork neuralNetworkSin = new NeuralNetwork(tanH, errorFunc, 1, 5,5, 1);

            int sampleCount = 10;

            double[][] waveInputs = new double[sampleCount][];
            double[][] sinWave = new double[sampleCount][];

            for (int i = 0; i < sampleCount; i++)
            {
                double x = (i / 10.0) * Math.PI * 2;

                waveInputs[i] = new double[]
                {
                    x / (Math.PI * 2)
                };

                sinWave[i] = new double[]
                {
                    Math.Sin(x)
                };
            }

            int batchSize = 1;
            neuralNetworkSin.Randomize(new Random(), -1, 1);
            do
            {
                error = neuralNetworkSin.BatchTrain(waveInputs, sinWave, learningRate, momentum, batchSize);
                Console.WriteLine($"Error: {error}");
            } while (error > 0.0000001);

            for (int i = 0; i < 10; i++)
            {
                double x = (double)i /10.0;

                double[] output = neuralNetworkSin.Compute(new double[] { x });

                Console.WriteLine(
                    $"x = {x:F1}  predicted = {output[0]:F4}  actual = {Math.Sin(x*Math.PI*2):F4}"
                );
            }

        }
    }
}
