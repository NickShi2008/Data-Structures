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
                (output, target) => 0.5 * Math.Pow(output - target, 2),
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

            neuralNetworkXOR.Randomize(new Random(), -10, 10);
            double learningRate = 0.01;
            do
            {
                error = neuralNetworkXOR.Train(gateInputs, xORExpected, learningRate);
                Console.WriteLine(error);
                for(int i = 0; i < gateInputs.Length; i++)
                {
                    double[] output = neuralNetworkXOR.Compute(gateInputs[i]);
                    Console.WriteLine($"Input: {gateInputs[i][0]}, {gateInputs[i][1]} " +
                        $"=> Output: {neuralNetworkXOR.GetError(gateInputs[i], xORExpected[i])}");
                }
            } while (error > 0.1);
        }
    }
}
