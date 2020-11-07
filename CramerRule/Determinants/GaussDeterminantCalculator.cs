using System;

namespace CramerRule.Determinants
{
    public class GaussDeterminantCalculator
    {
        public double Calculate(int[,] matrix)
        {
            double[,] triangulated = Triangulate(matrix);

            double result = 1;

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                result *= triangulated[i, i];
            }

            return Math.Round(result, 5);
        }

        private double[,] Triangulate(int[,] matrix)
        {
            int size = matrix.GetLength(0);
            double[,] triangulatedMatrix = CopyMatrixAsDouble(matrix);

            for (int i = 0; i < size - 1; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    double multiplier = -triangulatedMatrix[j, i] / triangulatedMatrix[i, i];

                    for (int k = i; k < size; k++)
                    {
                        triangulatedMatrix[j, k] += triangulatedMatrix[i, k] * multiplier;
                    }
                }
            }

            return triangulatedMatrix;
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