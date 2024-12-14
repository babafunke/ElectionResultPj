namespace ElectionResultPj
{
    public class Program
    {
        private static List<ElectionDataModel> _data = [];

        static void Main(string[] args)
        {
            var fileProcessor = new FileProcessor();
            _data = fileProcessor.ReadFileData();

            Console.WriteLine("Select from the menu.");
            Console.WriteLine("1. List all MPs\n2. List all Parties.\n3. Get MP details.\n4. Get Constituency details.");

            var input = Console.ReadLine();

            var response = QueryFileData(input);

            DisplayResponse(response);
        }

        static ResponseModel QueryFileData(string optionIndex)
        {
            ResponseModel response = new ResponseModel();
            ElectionDataModel electionModel = new ElectionDataModel();

            switch (optionIndex)
            {
                case "1":
                    var mpModels = _data.Select(d => new MpModel { FirstName = d.Memberfirstname, LastName = d.Membersurname, Gender = d.Membergender });
                    response.Mps = mpModels.ToList();
                    break;
                case "2":
                    var partyModels = _data.Select(d => new PartyModel { Name = d.Firstparty }).DistinctBy(p => p.Name);
                    response.Parties = partyModels.ToList();
                    break;
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
                        response.Mp = new MpModel
                        {
                            FirstName = electionModel.Memberfirstname,
                            LastName = electionModel.Membersurname,
                            Gender = electionModel.Membergender
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
                        response.Constituency = new ConstituencyModel
                        {
                            Name = electionModel.Constituencyname,
                            Region = electionModel.Regionname,
                            Country = electionModel.Countryname,
                            Type = electionModel.Constituencytype,
                            Mp = new MpModel 
                            { 
                                FirstName = electionModel.Memberfirstname,
                                LastName = electionModel.Membersurname,
                                Gender = electionModel.Membergender,
                            }
                        };
                    }
                    break;
                default:
                    Console.WriteLine("No matching option!");
                    break;
            }

            return response;
        }

        static void DisplayResponse(ResponseModel model)
        {
            if (model.Mps is not null && model.Mps.Any())
            {
                Console.WriteLine("\tFirstName\tLastName\tGender");
                var counter = 1;
                foreach (var mp in model.Mps)
                {
                    Console.WriteLine($"{counter}\t{mp.FirstName}\t{mp.LastName}\t{mp.Gender}");
                    counter++;
                }
            }

            if (model.Parties is not null && model.Parties.Any())
            {
                Console.WriteLine("\tName");
                var counter = 1;
                foreach (var party in model.Parties)
                {
                    Console.WriteLine($"{counter}\t{party.Name}");
                    counter++;
                }
            }

            if (model.Mp is not null)
            {
                Console.WriteLine("FirstName\tLastName\tGender");
                Console.WriteLine($"{model.Mp.FirstName}\t{model.Mp.LastName}\t{model.Mp.Gender}");
            }

            if (model.Constituency is not null)
            {
                Console.WriteLine($"{model.Constituency.Name}\n{model.Constituency.Country}\n{model.Constituency.Region}\n{model.Constituency.Type}" +
                    $"\n{model.Constituency.Mp.FirstName}");
            }
        }
    }
}
