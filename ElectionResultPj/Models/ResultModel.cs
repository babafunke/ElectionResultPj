namespace ElectionResultPj.Models
{
    public class ResultModel
    {
        public string Result { get; set; }
        public PartyModel FirstParty { get; set; }
        public PartyModel SecondParty { get; set; }
        public string ValidVotes { get; set; }
        public string InvalidVotes { get; set; }
        public string Electorate { get; set; }
    }
}
