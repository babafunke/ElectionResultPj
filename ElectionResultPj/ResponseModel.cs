namespace ElectionResultPj
{
    public class ResponseModel
    {
        public MpModel Mp { get; set; }
        public List<MpModel> Mps { get; set; }
        public ConstituencyModel Constituency { get; set; }
        public List<ConstituencyModel> Constituencies { get; set; }
        public PartyModel Party { get; set; }
        public List<PartyModel> Parties { get; set; }
    }
}
