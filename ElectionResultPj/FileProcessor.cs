using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace ElectionResultPj
{
    public class FileProcessor
    {
        public List<ElectionDataModel> ReadFileData()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.Replace(" ",string.Empty)
            };

            using (var reader = new StreamReader(@"C:\Users\Adegbembo\Documents\WafunkLtdWebProjects\ElectionResultPj\Projectdata.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                var data = csv.GetRecords<ElectionDataModel>().ToList();
                return data;
            }
        }
    }
}
