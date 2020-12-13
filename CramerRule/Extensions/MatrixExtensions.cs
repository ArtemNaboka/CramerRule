using System;

namespace CramerRule.Extensions
{
    public static class MatrixExtensions
    {
        public static void ConvertToDiagonal(this Matrix matrix, int k)
        {
            if (k % 2 == 0)
            {
                throw new InvalidOperationException();
            }

            for (int i = 0; i < matrix.Size / 2 + 1; i++)
            {
                for (int j = i + 1 + k / 2; j < matrix.Size; j++)
                {
                    matrix.Variables[i, j] = 0;
                }
            }

            for (int i = matrix.Size / 2; i < matrix.Size; i++)
            {
                for (int j = 0; j < i - k / 2; j++)
                {
                    matrix.Variables[i, j] = 0;
                }
            }
        }

        public static void ToChessZero(this Matrix matrix)
        {
            for (int i = 1; i < matrix.Size; i++)
            {
                int rest = i % 2;

                for (int j = 0; j < i; j++)
                {
                    if (j % 2 != rest)
                    {
                        matrix.Variables[i, j] = 0;
                    }
                }
            }
        }
    }
}