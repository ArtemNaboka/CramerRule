using System;

namespace CramerRule
{
    public class CalculationResult
    {
        public CalculationResult(Matrix matrix)
        {
            Matrix = matrix;
            Answers = new double[matrix.Size];
        }

        public Matrix Matrix { get; }
        public double[] Answers { get; set; }
    }
}