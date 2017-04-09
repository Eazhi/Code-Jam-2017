using System;
using System.Collections.Generic;
using System.Linq;

namespace Code_Jam
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Fashion();
        }

        public static void Pancakes()
        {
            int n = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                string line = Console.ReadLine();
                string[] parsed = line.Split(' ');
                int size = Convert.ToInt32(parsed[1]);
                int left = 0;
                int flip = 0;
                int row = parsed[0].Length;
                string[] pancakes = parsed[0].Select(c => c.ToString()).ToArray();
                while (row - left >= size)
                {
                    while (left < row && pancakes[left] == "+")
                        left++;
                    if (left >= row - 1)
                        break;
                    if (pancakes[left] == "-")
                    {
                        flip++;
                        for (int j = left; j < ((left + size > row)? row : left + size); j++)
                        {
                            switch(pancakes[j])
                            {
                                case "-":
                                    pancakes[j] = "+";
                                    break;
                                case "+":
                                    pancakes[j] = "-";
                                    break;
                            }
                        }
                    }
                }
                string plus = new string('+', size);
                string minus = new string('-', size);
                if (left >= row)
                {
                    Console.WriteLine("Case #{0}: {1}", i + 1, flip);
                    continue;
                }
                if (size > row - left)
                {
                    Console.WriteLine("Case #{0}: IMPOSSIBLE", i + 1);
                    continue;
                }
                if (parsed[0].Substring(left, row - 1) == plus
                    || parsed[0].Substring(left, row - 1) == minus)
                {
                    Console.WriteLine("Case #{0}: {1}", i + 1, flip);
                    continue;
                }
                Console.WriteLine("Case #{0}: IMPOSSIBLE", i + 1);
            }
        }

        public static void TidyNumbers()
        {
            int n = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                string num = Console.ReadLine();
                int[] digits = num.Select(c => Convert.ToInt32(c.ToString())).ToArray();
                for (int j = 0; j < digits.Length; j++)
                {
                    if (j >= digits.Length - 1)
                        break;
                    if (digits[j] > digits[j + 1])
                    {
                        digits[j] -= 1;
                        for (int x = j + 1; x < digits.Length; x++)
                        {
                            digits[x] = 9;
                        }
                        j = (j - 2 >= -1) ? j - 2 : -1;
                    }
                }

                // Output
                Console.Write("Case #{0}: ", i + 1);
                foreach (int nu in digits)
                {
                    bool start = false;
                    if (nu == 0 && !start)
                        continue;
                    Console.Write(nu);
                    start = true;
                }
                Console.WriteLine();
            }
        }

        public static double pos_max(double n, long k)
        {
            return (Math.Pow(2, Math.Floor(Math.Log(k, 2))) + n - k) / (Math.Pow(2, Math.Floor(Math.Log(k, 2)) + 1));
        }

        public static double pos_min(double n, long k)
        {
            return (pos_max(n - Math.Pow(2, Math.Floor(Math.Log(k, 2))), k));
        }

        public static void BathroomStalls()
        {
            int n = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                string[] parse = Console.ReadLine().Split(' ');
                double output1 = pos_max(double.Parse(parse[0]), long.Parse(parse[1]));
                double output2 = pos_min(double.Parse(parse[0]), long.Parse(parse[1]));
                Console.WriteLine("Case #{0}: {1} {2}", i + 1, Math.Floor(output1), Math.Floor(output2));
            }
        }

        public static void Fashion()
        {
            int n = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < n; i++)
            {
                string[] parse = Console.ReadLine().Split(' ');
                int dim = Convert.ToInt32(parse[0]);
                int lines = Convert.ToInt32(parse[1]);

                string[,] matrix = new string[dim, dim];
                int[,] score = new int[dim, dim];
                List<Tuple<int, int, int>> Gain = new List<Tuple<int, int, int>>();

                // 0 -> lines
                // 1 -> columns
                int[,] Count_plus = new int[dim, 2];
                int[,] Count_other = new int[dim, 2];

                int[] diag_times = new int[2 * dim];
                int[] diag_other = new int[2 * dim];

                // Init matrix to full dots
                for (int j = 0; j < dim; j++) for (int k = 0; k < dim; k++) matrix[j, k] = ".";

                for (int j = 0; j < lines; j++)
                {
                    parse = Console.ReadLine().Split(' ');
                    string car = parse[0];
                    int x = Convert.ToInt32(parse[1]);
                    int y = Convert.ToInt32(parse[2]);

                    matrix[x, y] = car;
                    switch(car)
                    {
                        case "o":
                            Count_other[x, 0]++;
                            Count_other[y, 1]++;
                            diag_other[x + y]++;
                            break;
                        case "+":
                            Count_plus[x, 0]++;
                            Count_plus[y, 1]++;
                            diag_other[x + y]++;
                            break;
                        case "x":
                            Count_other[x, 0]++;
                            Count_other[y, 1]++;
                            diag_times[x + y]++;
                            break;
                    }
                }

                for (int j = 0; j < dim; j++)
                    for (int k = 0; k < dim; k++)
                    {
                        if (matrix[j, k] == ".")
                        {
                            score[j, k] = (dim - Count_plus[j, 0])*(dim - Count_plus[k, 1])*(dim - diag_times[j + k]);
                        }
                        else
                        {
                            score[j, k] = int.MaxValue;
                        }
                        Gain.Add(new Tuple<int, int, int>(j, k, score[j, k]));
                    }
                Gain = Gain.OrderBy(x => x.Item3).ToList();

                int changed = 0;
                List<String> output = new List<string>();

                foreach (var item in Gain)
                {
                    if (matrix[item.Item1, item.Item2] == ".")
                    {
                        if (Count_other[item.Item1, 0] == 0 && Count_other[item.Item2, 1] == 0)
                        {
                            matrix[item.Item1, item.Item2] = "o";
                            string blabla = "o " + item.Item1 + item.Item2;
                            output.Add(blabla);
                            changed++;
                        }
                        if (Count_other[item.Item1, 0] <= 1 && Count_other[item.Item2, 1] <= 1)
                        {
                            matrix[item.Item1, item.Item2] = "+";
                            string blabla = "+ " + item.Item1 + item.Item2;
                            output.Add(blabla);
                            changed++;
                        }
                    }
                }

                int points = 0;

                foreach (var item in matrix)
                {
                    switch(item)
                    {
                        case "+":
                            points++;
                            break;
                        case "x":
                            points++;
                            break;
                        case "o":
                            points += 2;
                            break;
                    }
                }

                Console.WriteLine("Case #{0}: {1} {2}", i + 1, points, changed);
                foreach (var item in output)
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}
