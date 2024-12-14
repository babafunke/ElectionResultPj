namespace ElectionResultPj.Models
{
    public class MpModel
    {
        public string MpFirstName { get; set; }
        public string MpLastName { get; set; }
        public string MpGender { get; set; }
        public ConstituencyModel Constituency { get; set; }
    }
}
