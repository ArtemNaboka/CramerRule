using System;
using System.Linq;
using System.Threading.Tasks;

namespace CramerRule.Solvers.KDiagonal
{
    public class ParallelDiagonalSolver : IEquationSolver
    {
        public CalculationResult Solve(Matrix matrix)
        {
            const int kSize = 5;

            int h = kSize / 2;
            int m = matrix.Size / 2;

            double[,] dMatrix = CopyMatrixAsDouble(matrix.Variables);
            double[] dResults = matrix.Results.Select(Convert.ToDouble).ToArray();

            ParallelTriangulation(dMatrix, dResults, matrix.Size, kSize, m, h);

            for (int i = 0; i < m; i++)
            {
                for (int j = m + kSize - 1; j > i + 1 + h; j--)
                {
                    double multiplier = -dMatrix[j, i] / dMatrix[i, i];
                    int columnBound = Math.Min(matrix.Size, i + kSize);

                    for (int k = i; k < columnBound; k++)
                    {
                        dMatrix[j, k] += dMatrix[i, k] * multiplier;
                    }

                    dResults[j] += dResults[i] * multiplier;
                }
            }

            double[] answers = GetAnswersInParallel(dMatrix, dResults, matrix.Size, kSize, m);

            CalculationResult result = new CalculationResult(matrix);
            result.Answers = answers;

            return result;
        }

        private static void ParallelTriangulation(double[,] dMatrix, double[] dResults, int size, int kSize, int m, int h)
        {
            Task task1 = Task.Run(() =>
            {
                for (int i = 0; i < m; i++)
                {
                    for (int j = i + 1; j < i + 1 + h; j++)
                    {
                        double multiplier = -dMatrix[j, i] / dMatrix[i, i];
                        int columnBound = Math.Min(size, i + kSize);

                        for (int k = i; k < columnBound; k++)
                        {
                            dMatrix[j, k] += dMatrix[i, k] * multiplier;
                        }

                        dResults[j] += dResults[i] * multiplier;
                    }
                }
            });

            Task task2 = Task.Run(() =>
            {
                for (int i = size - 1; i > m + kSize; i--)
                {
                    for (int j = i + 1; j < i + 1 - h; j++)
                    {
                        double multiplier = -dMatrix[j, i] / dMatrix[i, i];
                        int columnBound = Math.Max(0, i - kSize);

                        for (int k = i; k >= columnBound; k--)
                        {
                            dMatrix[j, k] += dMatrix[i, k] * multiplier;
                        }

                        dResults[j] += dResults[i] * multiplier;
                    }
                }
            });

            Task.WaitAll(task1, task2);
        }

        private static double[] GetAnswersInParallel(double[,] dMatrix, double[] dResults, int size, int kSize, int m)
        {
            double[] answers = new double[size];

            Task task1 = Task.Run(() =>
            {
                for (int i = m - 1; i >= 0; i--)
                {
                    double currentResult = dResults[i];
                    int columnBound = Math.Min(size, i + 1 + kSize / 2);

                    for (int j = i + 1; j < columnBound; j++)
                    {
                        currentResult -= answers[j] * dMatrix[i, j];
                    }

                    answers[i] = currentResult / dMatrix[i, i];
                }
            });

            Task task2 = Task.Run(() =>
            {
                for (int i = m; i < size; i++)
                {
                    double currentResult = dResults[i];
                    int columnBound = Math.Min(size, i + 1 + kSize / 2);

                    for (int j = i + 1; j < columnBound; j++)
                    {
                        currentResult -= answers[j] * dMatrix[i, j];
                    }

                    answers[i] = currentResult / dMatrix[i, i];
                }
            });

            Task.WaitAll(task1, task2);

            return answers;
        }

        private static double[,] CopyMatrixAsDouble(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);

            double[,] copy = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    copy[i, j] = matrix[i, j];
                }
            }

            return copy;
        }
    }
}