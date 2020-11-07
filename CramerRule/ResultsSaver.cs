using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CramerRule
{
    public class ResultsSaver
    {
        public async Task SaveAsync(string outputFile, CalculationResult result)
        {
            RemoveFileIfExists(outputFile);

            StringBuilder tempSb = new StringBuilder();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Results:");

            for (int i = 0; i < result.Answers.Length; i++)
            {
                sb.AppendLine($"x{i + 1}: {result.Answers[i]}");
            }

            sb.AppendLine("Matrix:");

            for (int i = 0; i < result.Matrix.Size; i++)
            {
                for (int j = 0; j < result.Matrix.Size; j++)
                {
                    tempSb.Append($"{result.Matrix.Variables[i, j]} ");
                }

                sb.AppendLine($"{tempSb} = {result.Matrix.Results[i]}");
                tempSb.Clear();
            }

            using (FileStream fileStream = File.Create(outputFile))
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                await writer.WriteAsync(sb.ToString());
            }
        }

        private void RemoveFileIfExists(string outputFile)
        {
            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }
        }
    }
}