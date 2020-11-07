namespace CramerRule
{
    public class Matrix
    {
        public Matrix(int size)
        {
            Size = size;

            Variables = new int[size, size];
            Results = new int[size];
        }

        public int Size { get; }

        public int[,] Variables { get; set; }
        public int[] Results { get; set; }
    }
}