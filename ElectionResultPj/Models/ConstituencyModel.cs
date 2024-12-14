namespace ElectionResultPj.Models
{
    public class ConstituencyModel
    {
        public string ConstituencyName { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public string ConstituencyType { get; set; }
        public MpModel Mp { get; set; }
        public ResultModel Result { get; set; }
    }
}
