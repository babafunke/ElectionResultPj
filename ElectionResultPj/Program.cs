using ElectionResultPj.Models;

namespace ElectionResultPj
{
    public class Program
    {
        private static List<ElectionDataModel> _data = [];

        static void Main(string[] args)
        {
            _data = FileProcessor.ReadDataFromFile();

            Console.WriteLine("Select from the menu.");
            Console.WriteLine("1. List all MPs\n2. List all Parties.\n3. Get MP details.\n4. Get Constituency details.");

            var input = Console.ReadLine();

            var response = QueryFileData(input);

            DisplayResponse(response);
        }

        static object QueryFileData(string optionIndex)
        {
            var electionModel = new ElectionDataModel();

            switch (optionIndex)
            {
                case "1":
                    var mpModels = _data.Select(d => new MpModel 
                    { 
                        MpFirstName = d.Memberfirstname, 
                        MpLastName = d.Membersurname, 
                        MpGender = d.Membergender,
                        Constituency = new ConstituencyModel { ConstituencyName = d.Constituencyname },
                    });
                    return mpModels.ToList();
                case "2":
                    var partyModels = _data.Select(d => new PartyModel { PartyName = d.Firstparty }).DistinctBy(p => p.PartyName);
                    return partyModels.ToList();
                case "3":
                    Console.WriteLine("Enter MP Firstname.");
                    var mpFirstName = Console.ReadLine();

                    Console.WriteLine("Enter MP Surname.");
                    var mpLastName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(mpFirstName) || string.IsNullOrWhiteSpace(mpLastName))
                    {
                        Console.WriteLine("Error --> Firstname or Lastname missing!");
                        break;
                    }

                    electionModel = _data.FirstOrDefault(d => d.Memberfirstname.ToLower() == mpFirstName.ToLower() && d.Membersurname.ToLower() == mpLastName.ToLower());

                    if (electionModel is not null)
                    {
                        return new MpModel
                        {
                            MpFirstName = electionModel.Memberfirstname,
                            MpLastName = electionModel.Membersurname,
                            MpGender = electionModel.Membergender,
                            Constituency = new ConstituencyModel { ConstituencyName = electionModel.Constituencyname },
                        };
                    }
                    break;
                case "4":
                    Console.WriteLine("Enter Contituency name.");
                    var contituencyName = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(contituencyName))
                    {
                        Console.WriteLine("Error --> Contituency name is missing!");
                        break;
                    }

                    electionModel = _data.FirstOrDefault(d => d.Constituencyname.ToLower() == contituencyName.ToLower());

                    if (electionModel is not null)
                    {
                        return new ConstituencyModel
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
                    }
                    break;
                default:
                    Console.WriteLine("No matching option!");
                    break;
            }

            return null;
        }

        static void DisplayResponse(object model)
        {
            if (model is List<MpModel> mpModels)
            {
                Console.WriteLine("\tFirstName\tLastName\tGender");
                var counter = 1;
                foreach (var mp in mpModels)
                {
                    Console.WriteLine($"{counter}\t{mp.MpFirstName}\t{mp.MpLastName}\t{mp.MpGender}");
                    counter++;
                }

                Console.WriteLine($"Saving {mpModels.Count} records to {FileProcessor.OutputFileDirectory}");

                FileProcessor.WriteDataToFile(mpModels);
            }

            if (model is List<PartyModel> partyModels)
            {
                Console.WriteLine("\tName");
                var counter = 1;
                foreach (var party in partyModels)
                {
                    Console.WriteLine($"{counter}\t{party.PartyName}");
                    counter++;
                }

                Console.WriteLine($"Saving {partyModels.Count} records to {FileProcessor.OutputFileDirectory}");

                FileProcessor.WriteDataToFile(partyModels);
            }

            if (model is MpModel mpModel)
            {
                Console.WriteLine("FirstName\tLastName\tGender");
                Console.WriteLine($"{mpModel.MpFirstName}\t{mpModel.MpLastName}\t{mpModel.MpGender}");

                Console.WriteLine($"Saving 1 record to {FileProcessor.OutputFileDirectory}");

                FileProcessor.WriteDataToFile(new List<MpModel> { mpModel });
            }

            if (model is ConstituencyModel constituencyModel)
            {
                Console.WriteLine($"{constituencyModel.ConstituencyName}\n{constituencyModel.CountryName}\n{constituencyModel.RegionName}\n{constituencyModel.ConstituencyType}" +
                    $"\n{constituencyModel.Mp.MpFirstName}\n{constituencyModel.Mp.MpLastName}");

                Console.WriteLine($"Saving 1 record to {FileProcessor.OutputFileDirectory}");

                FileProcessor.WriteDataToFile(new List<ConstituencyModel> { constituencyModel });
            }
        }
    }
}
