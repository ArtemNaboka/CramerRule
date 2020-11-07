using System.Drawing;

namespace CramerRule
{
    public class MatrixProvider
    {
        private readonly string _imagePath;

        public MatrixProvider(string imagePath)
        {
            _imagePath = imagePath;
        }

        public Matrix GetMatrix(int size)
        {
            Matrix matrix = new Matrix(size);

            using (Bitmap bitmap = new Bitmap(Image.FromFile(_imagePath)))
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        byte green = bitmap.GetPixel(i, j).G;
                        matrix.Variables[i, j] = green;
                    }
                }

                for (int i = 0; i < size; i++)
                {
                    byte green = bitmap.GetPixel(i, size).G;
                    matrix.Results[i] = green;
                }

                return matrix;
            }
        }
    }
}