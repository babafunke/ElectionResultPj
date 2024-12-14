namespace ElectionResultPj
{
    public class ConstituencyModel
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Type { get; set; }
        public MpModel Mp { get; set; }
    }
}
