using System.Diagnostics;
using ScottPlot;

namespace task14
{
    public class OptimalSettings
    {
        private static readonly Func<double, double> sin = (double x) => Math.Sin(x);
        private const double a = -100;
        private const double b = 100;
        private const double exactResult = 0.0;
        private const double accuracy = 1e-4;
        private const int operationCount = 30;

        public static void Main()
        {
            Console.WriteLine("Вычисление оптимального шага:");
            double[] steps = { 1e-3, 1e-4, 1e-5, 1e-6 }; //шаги 1e-1, 1e-2 не обеспечивают необходимую точность
            double optStep = 0;
            double optStepTime = double.MaxValue;

            foreach (var step in steps)
            {
                List<double> times = new();
                bool accurate = true;
                for (int i = 0; i < operationCount; i++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var result = DefiniteIntegral.OneThread(a, b, sin, step);
                    stopwatch.Stop();

                    accurate = Math.Abs(result - exactResult) <= accuracy;
                    times.Add(stopwatch.Elapsed.Microseconds);
                }
                var avg = times.Average();
                var min = times.Min();
                var max = times.Max();

                if (accurate && avg < optStepTime)
                {
                    optStep = step;
                    optStepTime = avg;
                }
                Console.WriteLine($"\tSTEP: {step} | AVG: {avg:F3} | MIN: {min:F3} | MAX: {max:F3} | ACCURATE: {accurate}");
            }

            Console.WriteLine("Вычисление оптимального количества потоков:");
            int optThreads = 0;
            double optThreadsTime = double.MaxValue;
            List<double> totalTimes = new();
            List<int> threads = new();

            for (int i = 2; i <= 24; i++)
            {
                List<double> times = new();
                bool accurate = true;
                for (int j = 0; j < operationCount; j++)
                {
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    var result = DefiniteIntegral.Solve(a, b, sin, optStep, i);
                    stopwatch.Stop();

                    accurate = Math.Abs(result - exactResult) <= accuracy;
                    times.Add(stopwatch.Elapsed.Microseconds);
                }
                var avg = times.Average();
                var min = times.Min();
                var max = times.Max();
                totalTimes.Add(avg);
                threads.Add(i);

                if (accurate && avg < optThreadsTime)
                {
                    optThreads = i;
                    optThreadsTime = avg;
                }
                Console.WriteLine($"\tTHREADS: {i} | AVG: {avg:F3} | MIN: {min:F3} | MAX: {max:F3} | ACCURATE: {accurate}");
            }

            Console.WriteLine("ИТОГ:");
            Console.WriteLine($"Оптимальный размер шага: {optStep}");
            Console.WriteLine($"Оптимальныое количество потоков: {optThreads}");
            Console.WriteLine($"Время выполнения однопоточной версии: {optStepTime}");
            Console.WriteLine($"Время выполнения многопоточной версии: {optThreadsTime}");
            Console.WriteLine($"Выгода: {(optStepTime - optThreadsTime) / optStepTime * 100}%");

            var plt = new Plot();
            plt.Title("Зависимость числа потоков от времени");
            plt.XLabel("Время выполнения (мкс)");
            plt.YLabel("Количество потоков");
            plt.Add.Scatter(totalTimes.ToArray(), threads.ToArray());
            plt.SavePng("OptimalSettings_Graph.png", 800, 600);

            using (var wrt = new StreamWriter("OptimalSettings_Result.txt"))
            {
                wrt.WriteLine($"Оптимальный размер шага: {optStep}");
                wrt.WriteLine($"Оптимальныое количество потоков: {optThreads}");
                wrt.WriteLine($"Время выполнения однопоточной версии: {optStepTime:F3} мкс");
                wrt.WriteLine($"Время выполнения многопоточной версии: {optThreadsTime:F3} мкс");
                wrt.WriteLine($"Выгода: {((optStepTime - optThreadsTime) / optStepTime * 100):F3}%");
            }
        }
    }
}
