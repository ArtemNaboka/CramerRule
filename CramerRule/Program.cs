﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using CramerRule.Extensions;
using CramerRule.Solvers;
using CramerRule.Solvers.Cholesky;
using CramerRule.Solvers.Jacobi;
using CramerRule.Solvers.KDiagonal;

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

            IEquationSolver choleskySolver = new CholeskySolver();
            IEquationSolver parallelCholeskySolver = new ParallelCholeskySolver();

            IEquationSolver diagonalSolver = new DiagonalSolver();
            IEquationSolver parallelDiagonalSolver = new ParallelDiagonalSolver();

            IEquationSolver jacobiSolver = new JacobiSolver();
            IEquationSolver parallelJacobiSolver = new ParallelJacobiSolver();

            int[] sizes = { 10, 100, 500 };

            sequentialSolver.Solve(GetTestMatrix());

            // Console.WriteLine("Sequential Cramer:");
            // await PerformTesting(sizes, provider, saver, sequentialSolver);
            //
            // Console.WriteLine();
            // Console.WriteLine("Parallel Cramer:");
            // await PerformTesting(sizes, provider, saver, parallelSolver);

            // Console.WriteLine();
            // Console.WriteLine("Sequential Cholesky:");
            // choleskySolver.Solve(GetCholeskyTestMatrix());
            // await PerformTesting(sizes, provider, saver, choleskySolver);
            //
            // Console.WriteLine();
            // Console.WriteLine("Parallel Cholesky:");
            // parallelSolver.Solve(GetCholeskyTestMatrix());
            // await PerformTesting(sizes, provider, saver, parallelCholeskySolver);
            //
            // Console.WriteLine();
            // Console.WriteLine("Sequential K-Diagonal:");
            // diagonalSolver.Solve(GetDiagonalTestMatrix());
            // await PerformTesting(sizes, provider, saver, diagonalSolver);
            //
            // Console.WriteLine();
            // Console.WriteLine("Parallel K-Diagonal:");
            // parallelDiagonalSolver.Solve(GetDiagonalTestMatrix2());
            // await PerformTesting(sizes, provider, saver, parallelDiagonalSolver);

            Console.WriteLine();
            Console.WriteLine("Sequential Jacobi on common matrix:");
            jacobiSolver.Solve(GetDiagonalTestMatrix2());
            await PerformTesting(sizes, provider, saver, jacobiSolver);

            Console.WriteLine();
            Console.WriteLine("Sequential Jacobi on chess matrix:");
            jacobiSolver.Solve(GetDiagonalTestMatrix2());
            await PerformTesting(sizes, provider, saver, jacobiSolver, matrix => matrix.ToChessZero());

            Console.WriteLine();
            Console.WriteLine("Parallel Jacobi on common matrix:");
            parallelJacobiSolver.Solve(GetDiagonalTestMatrix2());
            await PerformTesting(sizes, provider, saver, parallelJacobiSolver);

            Console.WriteLine();
            Console.WriteLine("Parallel Jacobi on chess matrix:");
            parallelJacobiSolver.Solve(GetDiagonalTestMatrix2());
            await PerformTesting(sizes, provider, saver, parallelJacobiSolver, matrix => matrix.ToChessZero());
        }

        private static async Task PerformTesting(
            int[] sizes,
            MatrixProvider provider,
            ResultsSaver saver,
            IEquationSolver solver,
            Action<Matrix> matrixAction = null)
        {
            Stopwatch stopwatch = new Stopwatch();

            foreach (int size in sizes)
            {
                Matrix matrix = provider.GetMatrix(size);

                if (matrixAction != null)
                {
                    matrixAction(matrix);
                }

                stopwatch.Start();
                CalculationResult result = solver.Solve(matrix);
                stopwatch.Stop();

                Console.WriteLine($"{size}x{size} ({result.Answers.Length}): {stopwatch.ElapsedMilliseconds}ms, {stopwatch.ElapsedTicks} ticks");
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

        private static Matrix GetDiagonalTestMatrix()
        {
            Matrix m = new Matrix(5);
            m.Variables = new[,]
            {
                { 11, 12, 54, 32, 47 },
                { 64, 12, 43, 94, 42 },
                { 82, 74, 28, 45, 18 },
                { 37, 28, 19, 36, 25 },
                { 64, 32, 12, 38, 10 }
            };

            m.Results = new[]
            {
                23, 119, 147, 80, 48
            };

            return m;
        }

        private static Matrix GetCholeskyTestMatrix()
        {
            Matrix m = new Matrix(3);
            m.Variables = new[,]
            {
                { 81, -45, 45 },
                { -45, 50, -15 },
                { 45, -15, 38 },
            };

            m.Results = new[]
            {
                531, -460, 193
            };

            return m;
        }

        private static Matrix GetDiagonalTestMatrix2()
        {
            Matrix m = new Matrix(5);
            m.Variables = new[,]
            {
                { 11, 12, 54, 32, 47, 11, 12, 54, 32, 47 },
                { 64, 12, 43, 94, 42, 82, 74, 28, 45, 18 },
                { 82, 74, 28, 45, 18, 64, 12, 43, 94, 42 },
                { 37, 28, 19, 36, 25, 64, 32, 12, 38, 10 },
                { 64, 32, 12, 38, 10, 37, 28, 19, 36, 25 },
                { 11, 12, 54, 32, 47, 11, 12, 54, 32, 47 },
                { 64, 12, 43, 94, 42, 82, 74, 28, 45, 18 },
                { 82, 74, 28, 45, 18, 64, 12, 43, 94, 42 },
                { 37, 28, 19, 36, 25, 64, 32, 12, 38, 10 },
                { 64, 32, 12, 38, 10, 37, 28, 19, 36, 25 }
            };

            m.Results = new[]
            {
                23, 119, 147, 80, 48, 45, 18, 64, 12, 43
            };

            return m;
        }
    }
}