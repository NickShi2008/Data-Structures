using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradientDescentXOR
{
    public class ErrorFunction
    {
        Func<double, double, double> function;
        Func<double, double, double> derivative;
        public ErrorFunction(Func<double, double, double> function, Func<double, double, double> derivative) 
        {
            this.function = function;
            this.derivative = derivative;
        }

        public double Function(double output, double target)
        {
            return function(output, target);
        }

        public double Derivative(double output, double target)
        {
            return derivative(output, target);
        }
    }
}
