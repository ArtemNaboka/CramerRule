namespace CramerRule.Solvers
{
    public interface IEquationSolver
    {
        CalculationResult Solve(Matrix matrix);
    }
}