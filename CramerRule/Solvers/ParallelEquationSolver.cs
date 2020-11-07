using System.Threading.Tasks;

namespace CramerRule.Solvers
{
    public class ParallelEquationSolver : EquationSolver
    {
        protected override void SetAnswers(
            Matrix matrix,
            int[,] workVariables,
            double baseDeterminant,
            CalculationResult result)
        {
            Parallel.For(0, matrix.Size, i =>
            {
                CopyColumn(workVariables, i, matrix.Results);

                result.Answers[i] = CalculateDeterminant(workVariables) / baseDeterminant;

                CopyColumn(workVariables, matrix.Variables, i);
            });
        }
    }
}