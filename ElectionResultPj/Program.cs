using ElectionResultPj.Models;

namespace ElectionResultPj
{
    public class Program
    {
        private static List<ElectionDataModel> _data = [];

        static void Main(string[] args)
        {
            _data = FileProcessor<ElectionDataModel>.ReadDataFromFile();

            Console.WriteLine("Select from the menu.");
            Console.WriteLine("1. List all MPs\n2. List all Parties.\n3. Get MP details.\n4. Get Constituency details.");

            var input = Console.ReadLine();

            QueryFileData(input);
        }

        static void QueryFileData(string optionIndex)
        {
            switch (optionIndex)
            {
                case "1":
                    DisplayMpList();
                    break;
                case "2":
                    DisplayPartyList();
                    break;
                case "3":
                    DisplayMp();
                    break;
                case "4":
                    DisplayConstituency();
                    break;
                default:
                    Console.WriteLine("No matching option!");
                    break;
            }
        }

        static void DisplayMpList()
        {
            var mpModels = _data.Select(d => new MpModel
            {
                MpFirstName = d.Memberfirstname,
                MpLastName = d.Membersurname,
                MpGender = d.Membergender,
                Constituency = new ConstituencyModel { ConstituencyName = d.Constituencyname },
            });

            FileProcessor<MpModel>.WriteDataToFile(mpModels.ToList());
        }

        static void DisplayPartyList()
        {
            var partyModels = _data.Select(d => new PartyModel { PartyName = d.Firstparty }).DistinctBy(p => p.PartyName);

            FileProcessor<PartyModel>.WriteDataToFile(partyModels.ToList());
        }

        static void DisplayMp()
        {
            Console.WriteLine("Enter MP Firstname.");
            var mpFirstName = Console.ReadLine();

            Console.WriteLine("Enter MP Surname.");
            var mpLastName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(mpFirstName) || string.IsNullOrWhiteSpace(mpLastName))
            {
                Console.WriteLine("Error --> Firstname or Lastname missing!");
                return;
            }

            var electionModel = _data.FirstOrDefault(d => d.Memberfirstname.ToLower() == mpFirstName.ToLower() && d.Membersurname.ToLower() == mpLastName.ToLower());

            if (electionModel is null)
            {
                Console.WriteLine($"No matching MP for {mpFirstName} {mpLastName}");
                return;
            }

            var mpModel = new MpModel
            {
                MpFirstName = electionModel.Memberfirstname,
                MpLastName = electionModel.Membersurname,
                MpGender = electionModel.Membergender,
                Constituency = new ConstituencyModel { ConstituencyName = electionModel.Constituencyname },
            };

            FileProcessor<MpModel>.WriteDataToFile(new List<MpModel> { mpModel });
        }

        static void DisplayConstituency()
        {
            Console.WriteLine("Enter Contituency name.");
            var contituencyName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(contituencyName))
            {
                Console.WriteLine("Error --> Contituency name is missing!");
                return;
            }

            var electionModel = _data.FirstOrDefault(d => d.Constituencyname.ToLower() == contituencyName.ToLower());

            if (electionModel is null)
            {
                Console.WriteLine($"No matching Constituency for {contituencyName}");
                return;
            }

            var constituencyModel = new ConstituencyModel
            {
                ConstituencyName = electionModel.Constituencyname,
                RegionName = electionModel.Regionname,
                CountryName = electionModel.Countryname,
                ConstituencyType = electionModel.Constituencytype,
                Mp = new MpModel
                {
                    MpFirstName = electionModel.Memberfirstname,
                    MpLastName = electionModel.Membersurname,
                    MpGender = electionModel.Membergender,
                },
                Result = new ResultModel
                {
                    FirstParty = new PartyModel { PartyName = electionModel.Firstparty },
                    SecondParty = new PartyModel { PartyName = electionModel.Secondparty },
                    ValidVotes = electionModel.Validvotes,
                    InvalidVotes = electionModel.Invalidvotes,
                    Result = electionModel.Result,
                    Electorate = electionModel.Electorate
                }
            };

            FileProcessor<ConstituencyModel>.WriteDataToFile(new List<ConstituencyModel> { constituencyModel });
        }

    }
}
