using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CramerRule.Solvers;

namespace CramerRule
{
    internal class Program
    {
        private const string ImagePath = "slide-1.jpg";
        private const string OutputFileTemplate = "output-{0}-{1}.txt";

        public static async Task Main(string[] args)
        {
            MatrixProvider provider = new MatrixProvider(ImagePath);
            ResultsSaver saver = new ResultsSaver();

            IEquationSolver sequentialSolver = new EquationSolver();
            IEquationSolver parallelSolver = new ParallelEquationSolver();
            // Matrix matrix = GetTestMatrix();

            int[] sizes = { 10, 100, 500 };

            Console.WriteLine("Sequential:");
            await PerformTesting(sizes, provider, saver, sequentialSolver);

            Console.WriteLine();
            Console.WriteLine("Parallel:");
            await PerformTesting(sizes, provider, saver, parallelSolver);
        }

        private static async Task PerformTesting(
            int[] sizes,
            MatrixProvider provider,
            ResultsSaver saver,
            IEquationSolver solver)
        {
            Stopwatch stopwatch = new Stopwatch();

            foreach (int size in sizes)
            {
                Matrix matrix = provider.GetMatrix(size);

                stopwatch.Start();
                CalculationResult result = solver.Solve(matrix);
                stopwatch.Stop();

                Console.WriteLine($"{size}x{size} ({result.Answers.Length}): {stopwatch.ElapsedMilliseconds}ms");
                stopwatch.Reset();

                string outputFile = string.Format(OutputFileTemplate, $"{size}x{size}", solver.GetType());
                await saver.SaveAsync(outputFile, result);
            }
        }

        private static Matrix GetTestMatrix()
        {
            Matrix matrix = new Matrix(3);
            matrix.Variables = new int[3, 3]
            {
                { 1, 2, 1 },
                { 3, -1, -1 },
                { -2, 2, 3 }
            };

            matrix.Results = new[] { -1, -1, 5 };

            return matrix;
        }
    }
}