using System.ComponentModel;

namespace task14;

public class DefiniteIntegral
{
    //
    // a, b - границы отрезка, на котором происходит вычисление опредленного интеграла
    // function - функция, для которой вычисляется определнный интеграл
    // step - размер одного шага разбиения
    // threadsNumber - число потоков, которые используются для вычислений
    //
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsnumber)
    {
        // отсюда надо начинать реализацию задачи
        long totalSteps = (int)((b - a) / step);
        double totalSum = 0;
        var threads = new Thread[threadsnumber];
        long stepsPerThread = totalSteps / threadsnumber ;

        Barrier barrier = new Barrier(threadsnumber + 1);

        for (int t = 0; t < threadsnumber; t++)
        {
            long startStep = t * stepsPerThread;
            long endStep = (t == threadsnumber - 1) ? totalSteps : startStep + stepsPerThread;

            threads[t] = new Thread( () =>
            {
                double localSum = LocalIntegral(startStep, endStep, a, step, function);

                double initial;
                double result;

                do
                {
                    initial = totalSum;
                    result = initial + localSum;
                }
                while(Interlocked.CompareExchange(ref totalSum, result, initial) != initial);

                barrier.SignalAndWait();
            });
            threads[t].Start();
        }

        barrier.SignalAndWait();

        return totalSum;
    }

    private static double LocalIntegral(long startStep, long endStep, double a, double step, Func<double, double> func)
    {
        double sum = 0;
        for(long i = startStep; i < endStep; i++)
        {
            double x1 = a + i * step;
            double x2 = x1 + step;
            sum += ((func(x1) + func(x2)) / 2 * step);
        }

        return sum;
    }
}
