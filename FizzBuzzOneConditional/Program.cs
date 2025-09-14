namespace FizzBuzzOneConditional
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(FizzBuzzNoConditional(1000));

        }

        string FizzBuzz(int number)
        {
            if (number % 3 == 0 && number % 5 == 0)
                return "FizzBuzz";
            else if (number % 3 == 0)
                return "Fizz";
            else if (number % 5 == 0)
                return "Buzz";
            else
                return number.ToString();
        }
        
        static string FizzBuzzNoConditional(int number)
        {
            string result = "";
            Dictionary<(int, int, int), Func<int,string>> translator = new Dictionary<(int, int, int), Func<int,string>>
            {
                 {(0,3, 3), (x) => "Fizz" },
                 {(0,1, 6), (x) => "Fizz" },
                 {(0,4, 9), (x) => "Fizz" },
                 {(0,2, 12), (x) => "Fizz" },
                 {(2,0, 5), (x) => "Buzz" },
                 {(2,0, 10), (x) => "Buzz" },
                 {(1,0, 5), (x) => "Buzz" },
                 {(1,0, 10), (x) => "Buzz" },
                 {(0,0, 0), (x) => "FizzBuzz" },
                 {(1,1, 1), (x) => x.ToString() },
                 {(2,2, 2), (x) => x.ToString() },
                 {(1,4, 4), (x) => x.ToString() },
                 {(1,2, 7), (x) => x.ToString() },
                 {(2,3, 8), (x) => x.ToString() },
                 {(2,1, 11), (x) => x.ToString() },
                 {(1,3, 13), (x) => x.ToString() },
                 {(2, 4, 14), (x) => x.ToString() },
            };
            for (int i = 1; i <= number; i++)
            {
                result += translator[(i % 3, i % 5, i % 15)](i) + "\n";
            }


            return result;
        }

    }

           
}
