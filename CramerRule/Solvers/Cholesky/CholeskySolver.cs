using System;
using System.Collections.Generic;
using System.Linq;

namespace CramerRule.Solvers.Cholesky
{
    public class CholeskySolver : IEquationSolver
    {
        public CalculationResult Solve(Matrix matrix)
        {
            CalculationResult result = new CalculationResult(matrix);

            double[][] decomposed = Decompose(matrix.Variables);

            double[] tempAnswers = FindAnswersFromTop(decomposed, matrix.Results.Select(Convert.ToDouble).ToArray());

            double[][] swapped = SwapRowsAndColumns(decomposed);

            double[] answers = FindAnswersFromBottom(swapped, tempAnswers);
            result.Answers = answers;

            return result;
        }

        private static double[] FindAnswersFromTop(double[][] decomposed, double[] results)
        {
            double[] answers = new double[decomposed.Length];

            for (int i = 0; i < decomposed.Length; i++)
            {
                double currentResult = results[i];

                for (int j = 0; j < i; j++)
                {
                    currentResult -= answers[j] * decomposed[i][j];
                }

                answers[i] = currentResult / decomposed[i][i];
            }

            return answers;
        }

        private static double[] FindAnswersFromBottom(double[][] decomposed, double[] results)
        {
            double[] answers = new double[decomposed.Length];

            for (int i = decomposed.Length - 1; i >= 0; i--)
            {
                double currentResult = results[i];

                for (int j = i + 1; j < decomposed.Length; j++)
                {
                    currentResult -= answers[j] * decomposed[i][j - i];
                }

                answers[i] = currentResult / decomposed[i][0];
            }

            return answers;
        }

        private double[][] SwapRowsAndColumns(double[][] decomposed)
        {
            double[][] result = new double[decomposed.Length][];

            for (int i = 0; i < decomposed.Length; i++)
            {
                result[i] = new double[decomposed.Length - i];
                int j = i;

                for (int k = 0; k < result[i].Length; k++)
                {
                    result[i][k] = decomposed[j++][i];
                }
            }

            return result;
        }

        protected virtual double[][] Decompose(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            double[][] decomposedL = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                decomposedL[i] = new double[i + 1]; //L - треугольная матрица, поэтому в i-ой строке i+1 элементов

                double temp;
                //Сначала вычисляем значения элементов слева от диагонального элемента,
                //так как эти значения используются при вычислении диагонального элемента.
                for (int j = 0; j < i; j++)
                {
                    temp = 0;
                    for (int k = 0; k < j; k++)
                    {
                        temp += decomposedL[i][k] * decomposedL[j][k];
                    }

                    decomposedL[i][j] = (matrix[i, j] - temp) / decomposedL[j][j];
                }

                //Находим значение диагонального элемента
                temp = matrix[i, i];
                for (int k = 0; k < i; k++)
                {
                    temp -= decomposedL[i][k] * decomposedL[i][k];
                }

                decomposedL[i][i] = Math.Sqrt(temp);
            }

            return decomposedL;
        }
    }
}