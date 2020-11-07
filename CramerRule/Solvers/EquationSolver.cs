using CramerRule.Determinants;

namespace CramerRule.Solvers
{
    public class EquationSolver : IEquationSolver
    {
        private readonly GaussDeterminantCalculator _determinantCalculator = new GaussDeterminantCalculator();

        public CalculationResult Solve(Matrix matrix)
        {
            CalculationResult result = new CalculationResult(matrix);
            int[,] workVariables = CopyMatrix(matrix.Variables);
            double baseDeterminant = CalculateDeterminant(matrix.Variables);

            SetAnswers(matrix, workVariables, baseDeterminant, result);

            return result;
        }

        protected virtual void SetAnswers(
            Matrix matrix,
            int[,] workVariables,
            double baseDeterminant,
            CalculationResult result)
        {
            for (int i = 0; i < matrix.Size; i++)
            {
                CopyColumn(workVariables, i, matrix.Results);

                result.Answers[i] = CalculateDeterminant(workVariables) / baseDeterminant;

                CopyColumn(workVariables, matrix.Variables, i);
            }
        }

        protected double CalculateDeterminant(int[,] matrix)
        {
            return _determinantCalculator.Calculate(matrix);
        }

        protected void CopyColumn(int[,] matrix, int[,] newMatrix, int columnIndex)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                matrix[i, columnIndex] = newMatrix[i, columnIndex];
            }
        }

        protected void CopyColumn(int[,] matrix, int columnIndex, int[] newColumn)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                matrix[i, columnIndex] = newColumn[i];
            }
        }

        private int[,] CopyMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[,] copy = new int[rows, cols];

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