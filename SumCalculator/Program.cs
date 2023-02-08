// See https://aka.ms/new-console-template for more information
using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Program Start ....\n");

            TestCase();

            Console.WriteLine("\nProgram End ....");
        }

        private static void TestCase()
        {
            List<string> list = new()
            {
                "1 + 1", //2
                "2 * 2", //4
                "1 + 2 + 3", //6
                "6 / 2", // 3
                "11 + 23", //34
                "11.1 + 23", //34.1
                "1 + 1 * 3", //4
                "(11.5 + 15.4) + 10.1", //37
                "23 - (29.3 -12.5)", //6.2
                "(1/2) - 1 + 1",//0.5
                "10 - (2 + 3 * (7-5))",//2
                "(11.5+15.4*7+91/5*9.2)+10.1-2.1*9" //277.94
            };

            foreach (string xL in list)
            {
                double resultL = SumCalculator(Regex.Replace(xL, @"\s+", ""));

                Console.WriteLine(xL + " = " + resultL);
            };
        }

        static double SumCalculator(String calc)
        {
            double result = 0.00;

            //string priority_1 = @"\((\d|\d\.\d)*[\/\+\-\\*](\d|\d\.\d)*\)";
            string priority_2 = @"\((\d*\.?\d*[\/\+\-\\*]\d*\.?\d*)*\)";

            string number_double = @"\d*\.?\d{1,}";
            string ops_type = @"[\/\+\-\\*]";

            string ops_div = @"(\d*\.?\d*[\/](\d{1,}\.?)*\d{1,})*";
            string ops_multi = @"(\d*\.?\d*[\*](\d{1,}\.?)*\d{1,})*";

            string priority_Ops= @"(\d*\.?\d*[\/\*](\d{1,}\.?)*\d{1,}){1,}";

            //prioritise "*" and "/"
            string[] prio = Regex.Matches(calc, priority_Ops).Cast<Match>().Select(m => m.Value).ToArray();

            foreach (string match in prio)
            {
                //Console.WriteLine("Original Calculation : " + calc);

                string[] operations = Regex.Matches(match, ops_type).Cast<Match>().Select(m => m.Value).ToArray();
                List<string> numberData = Regex.Matches(match, number_double).Cast<Match>().Select(m => m.Value).ToList();
                List<double> tmpList = new();

                for (int x = 0; x < operations.Count() && x != -1; x++)
                {
                    if (x != operations.Length)
                    {
                        double x1 = Double.Parse(numberData[x]);
                        double y2 = Double.Parse(numberData[x+1]);
                        string ops = operations[x];

                        numberData[x + 1] = calculate(x1, y2, ops).ToString();

                        if(x == (operations.Count() - 1))
                        {
                            tmpList.Add(Double.Parse(numberData[x + 1]));
                        }
                    }
                }

                foreach (double xy in tmpList)
                {
                    calc = calc.Replace(match, xy.ToString());
                }

                //Console.WriteLine("New Calculation : " + calc + "\n");
            }


            // proceed with "()"
            string[] ints = Regex.Matches(calc, priority_2).Cast<Match>().Select(m => m.Value).ToArray();

            foreach (var match in ints)
            {
                string[] operations = Regex.Matches(match, ops_type).Cast<Match>().Select(m => m.Value).ToArray();
                List<string> numberData = Regex.Matches(match, number_double).Cast<Match>().Select(m => m.Value).ToList();
                List<double> tmpList = new();

                for (int x = 0; x < operations.Count() && x != -1; x++)
                {
                    if (x != operations.Length)
                    {
                        double x1 = Double.Parse(numberData[x]);
                        double y2 = Double.Parse(numberData[x + 1]);
                        string ops = operations[x];

                        numberData[x + 1] = calculate(x1, y2, ops).ToString();

                        if (x == (operations.Count() - 1))
                        {
                            tmpList.Add(Double.Parse(numberData[x + 1]));
                        }
                    }
                }

                foreach (double xy in tmpList)
                {
                    calc = calc.Replace(match, xy.ToString());
                }

                // if there's still "*" and "/" will reloop
                if(Regex.IsMatch(calc, priority_Ops))
                    return SumCalculator(calc);
            }

            // proceed with "+" and "-"
            string[] operationsX = Regex.Matches(calc, ops_type).Cast<Match>().Select(m => m.Value).ToArray();
            List<string> numberDataX = Regex.Matches(calc, number_double).Cast<Match>().Select(m => m.Value).ToList();
            List<double> tmpListX = new();

            for (int x = 0; x < operationsX.Count() && x != -1; x++)
            {
                if (x != operationsX.Length)
                {
                    double x1 = Double.Parse(numberDataX[x]);
                    double y2 = Double.Parse(numberDataX[x + 1]);
                    string ops = operationsX[x];

                    numberDataX[x + 1] = calculate(x1, y2, ops).ToString();

                    if (x == (operationsX.Count() - 1))
                    {
                        tmpListX.Add(Double.Parse(numberDataX[x + 1]));
                    }
                }
            }

            foreach (double xy in tmpListX)
            {
                calc = calc.Replace(calc, xy.ToString());
            }

            return Double.Parse(calc);
        }

        static double calculate(double x, double y, string ops)
        {
            double result = 0;

            switch (ops)
            {
                case "*":
                    result = x * y;
                    break;
                case "/":
                    result = x / y;
                    break;
                case "+":
                    result = x + y;
                    break;
                case "-":
                    result = x - y;
                    break;
            }

            return result;
        }
    }
}

