using System;
using System.Linq;
using System.Threading.Tasks;

namespace CramerRule.Solvers.Cholesky
{
    public class ParallelCholeskySolver : CholeskySolver
    {
        protected override double[][] Decompose(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            double[][] decomposedL = new double[rows][];

            for (int i = 0; i < rows; i++)
            {
                decomposedL[i] = new double[i + 1];

                for (int j = 0; j < i; j++)
                {
                    double[] tempProducts = new double[Environment.ProcessorCount];
                    int iterationsPerThread = (int)Math.Ceiling((double)j / Environment.ProcessorCount);

                    Parallel.For(0, Environment.ProcessorCount, k =>
                    {
                        int start = k * iterationsPerThread;
                        int end = Math.Min(j, start + iterationsPerThread);

                        for (int l = start; l < end; l++)
                        {
                            tempProducts[k] += decomposedL[i][k] * decomposedL[j][k];
                        }
                    });

                    decomposedL[i][j] = (matrix[i, j] - tempProducts.Sum()) / decomposedL[j][j];
                }

                double temp = matrix[i, i];

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