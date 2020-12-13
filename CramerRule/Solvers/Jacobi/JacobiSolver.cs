namespace CramerRule.Solvers.Jacobi
{
    public class JacobiSolver : IEquationSolver
    {
        public CalculationResult Solve(Matrix matrix)
        {
            const int iterations = 20;

            CalculationResult result = new CalculationResult(matrix);
            double[] answers = new double[matrix.Size];

            for (int iter = 0; iter < iterations; iter++)
            {
                for (int i = 0; i < matrix.Size; i++)
                {
                    double sigma = 0;

                    for (int j = 0; j < matrix.Size; j++)
                    {
                        if (j != i)
                        {
                            sigma += matrix.Variables[i, j] * answers[j];
                        }
                    }

                    answers[i] = (matrix.Results[i] - sigma) / matrix.Variables[i, i];
                }
            }

            result.Answers = answers;
            return result;
        }
    }
}