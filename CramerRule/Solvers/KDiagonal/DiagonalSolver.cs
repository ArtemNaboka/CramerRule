using System;
using System.Linq;
using CramerRule.Extensions;

namespace CramerRule.Solvers.KDiagonal
{
    public class DiagonalSolver : IEquationSolver
    {
        public CalculationResult Solve(Matrix matrix)
        {
            const int kSize = 5;
            matrix.ConvertToDiagonal(kSize);
            double[,] dMatrix = CopyMatrixAsDouble(matrix.Variables);
            double[] dResults = matrix.Results.Select(Convert.ToDouble).ToArray();

            Triangulate(dMatrix, dResults, matrix.Size, kSize);
            double[] answers = FindAnswers(dMatrix, dResults, kSize);

            CalculationResult result = new CalculationResult(matrix);
            result.Answers = answers;

            return result;
        }

        private static void Triangulate(double[,] dMatrix, double[] dResults, int size, int kSize)
        {
            for (int i = 0; i < size - 1; i++)
            {
                int rowBound = Math.Min(size, i + 1 + kSize / 2);

                for (int j = i + 1; j < rowBound; j++)
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
        }

        private static double[] FindAnswers(double[,] triangulated, double[] results, int kSize)
        {
            int size = triangulated.GetLength(0);
            double[] answers = new double[size];

            for (int i = size - 1; i >= 0; i--)
            {
                double currentResult = results[i];
                int columnBound = Math.Min(size, i + 1 + kSize / 2);

                for (int j = i + 1; j < columnBound; j++)
                {
                    currentResult -= answers[j] * triangulated[i, j];
                }

                answers[i] = currentResult / triangulated[i, i];
            }

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