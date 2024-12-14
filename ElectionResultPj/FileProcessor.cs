using CsvHelper;
using CsvHelper.Configuration;
using ElectionResultPj.Models;
using System.Globalization;

namespace ElectionResultPj
{
    public class FileProcessor
    {
        public static readonly string InputFileDirectory = @"C:\Projectdata.csv";
        public static readonly string OutputFileDirectory = @"C:\ElectionResultsPj\OutputFiles";

        public static List<ElectionDataModel> ReadDataFromFile()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Replace(" ",string.Empty)
            };

            using (var reader = new StreamReader(InputFileDirectory))
            using (var csv = new CsvReader(reader, config))
            {
                var data = csv.GetRecords<ElectionDataModel>().ToList();
                return data;
            }
        }

        public static void WriteDataToFile(IEnumerable<object> records)
        {
            var filename = Guid.NewGuid().ToString();

            if (!Directory.Exists(OutputFileDirectory))
            {
                Directory.CreateDirectory(OutputFileDirectory);
            }

            var filePath = Path.Combine(OutputFileDirectory, $"{filename}.csv");

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(records);
            }
        }
    }
}
