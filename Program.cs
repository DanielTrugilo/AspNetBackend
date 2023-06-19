
<<<<<<< HEAD
=======

using System.Text.Json;
>>>>>>> upstream/master
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAWSTalker();
//builder.Services.AddAzureTalker();
<<<<<<< HEAD
builder.Services.AddSingleton<PersonRepo>(); //Let's read about it :)
=======
builder.Services.AddSingleton<PersonRepo>();
>>>>>>> upstream/master

var app = builder.Build();

app.MapControllers();

// app.Map("Greet/{person}", (string person) => {
//     Talker talker = new(new TalkBlue());
//     talker.Greet(person);
//     return $"Hello {person}";
// });

// app.Map("GoodBye/{person}", (string person) => {
//     Talker talker = new(new TalkBlue());
//     talker.Greet(person);
//     return $"Hello {person}";
// });

app.Run();



public static class TalkerCloudExtentions // Extensions
{
    public static IServiceCollection AddAWSTalker(this IServiceCollection services) 
    {
        return services.AddSingleton<ISystemTalker,TalkRed>();
    }

    public static IServiceCollection AddAzureTalker(this IServiceCollection services) 
    {
        return services.AddSingleton<ISystemTalker,TalkBlue>();
    }
}

public class TalkRed : ISystemTalker  // Class implementing a interface
{
    public TalkRed(PersonRepo repo)
    {
        this.repo = repo;
    }
        int count = 0;
    private readonly PersonRepo repo;

    public void Say(string msg) {
        Console.BackgroundColor = repo.GetColor(1);
        Console.WriteLine("{0} {1}", msg, count++);
    }
}

public class TalkBlue : ISystemTalker // Class implementing a interface
{
    public TalkBlue(PersonRepo repo)
    {
        this.repo = repo;
    }
        int count = 0;
    private readonly PersonRepo repo;
    public void Say(string msg) {
        Console.BackgroundColor = repo.GetColor(0);
        Console.WriteLine("{0} {1}", msg, count++);
    }
}

public record Person(string FirstName, string LastName) {
    public List<Vaccine>? Vaccinations {get;set;}
}

public record Vaccine(string Name);

public class PersonRepo
{
<<<<<<< HEAD
    Dictionary<int, Person> dict = new() { //super database lol
=======
    Dictionary<int, Person> dict = new() {
>>>>>>> upstream/master
        {0,new ("Ryan", "Anderson")},
        {1,new ("Dani", "Trugilo")},
    };

    public PersonRepo()
    {
        LoadData();
    }

    public Person GetPerson(int id)
    {
        return dict[id];
    }

    public int AddPerson(Person person) {
        dict.Add(dict.Keys.Max() + 1, person);
        return dict.Keys.Max();
    }

    public Person DeletePerson(int id) {
        Person person = dict[id];
        dict.Remove(id);
        return person;
    }

    public Person AddVaccine(int id, string vaccineName) {
        if(dict[id].Vaccinations == null) dict[id].Vaccinations = new List<Vaccine>();
        dict[id].Vaccinations?.Add(new Vaccine(vaccineName));
        return dict[id];
    }

    public ConsoleColor GetColor(int id)
    {
        var dict = new Dictionary<int, ConsoleColor>() {
            {0,ConsoleColor.DarkBlue},
            {1,ConsoleColor.DarkYellow}
        };
        return dict[id];
    }
<<<<<<< HEAD
    public List<string> ListPersons(int[] id)
    {
        List<string> nameList = new List<string>();
        foreach (KeyValuePair<int, Person> persons in dict){
            nameList.Add("Test");
        }
        return nameList;
    }

=======

    public List<Person> GetPeople() 
    {
        return dict.Values.ToList();
    }

    public void PersistData() {
        File.WriteAllText("data.db", JsonSerializer.Serialize(dict));
    }

    public void InitializeData() {
        File.WriteAllText("data.db", JsonSerializer.Serialize(dict));
    }

    public void LoadData() {
        dict = JsonSerializer.Deserialize<Dictionary<int, Person>>(File.ReadAllText("data.db")) ?? dict; //if null reassign to itself;
    }
>>>>>>> upstream/master
}

public interface ISystemTalker  // Interface
{
    void Say(string msg);
}

<<<<<<< HEAD
[ApiController] // Controller Attribute 
public class TalkerController { 
    ISystemTalker talker;
    private PersonRepo personRepo;
=======
[ApiController]
public class TalkerController {
    readonly ISystemTalker talker;
    private readonly PersonRepo personRepo;
>>>>>>> upstream/master

    public TalkerController(ISystemTalker talker,PersonRepo personRepo) // Constructor? Initializing the variables ?
    {
        this.talker = talker;
        this.personRepo = personRepo;
    }

    [HttpGet("Greet/{personId}")] // api address
    public string Greet(int personId) {
        talker.Say($"Hi {personRepo.GetPerson(personId).FirstName}");
        return $"Hi {personRepo.GetPerson(personId).FirstName}";
    }

    [HttpGet("GoodBye/{personId}")]
    public string GoodBye(int personId) {
        talker.Say($"GoodBye {personRepo.GetPerson(personId).FirstName}");
        return $"GoodBye {personRepo.GetPerson(personId).FirstName}";
    }

<<<<<<< HEAD
    [HttpPost("AddPerson")]
    public string AddPerson([FromBody]Person person) {
        var personId = personRepo.AddPerson(person);
        return $"{person.FirstName} is cool at {personId}!!";
    }

    [HttpDelete("DeletePerson/{personId}")]
    public string DeletePerson(int personId) {
        var personIdDelete = personRepo.DeletePerson(personId);
        return $"Id removed at position {personId}.";
    }
=======
    [HttpGet("People")]
    public List<Person> People() {
        return personRepo.GetPeople();
    }

    [HttpPost("AddPerson")]
    public string AddPerson(Person person) {
        personRepo.AddPerson(person);
        personRepo.PersistData();
        return $"{person.FirstName} Added!!";
    }

    [HttpGet("AddVaccine/{personId}/{vaccineName}")]
    public Person AddPerson(int personId, string vaccineName) {
        var person = personRepo.AddVaccine(personId, vaccineName);
        personRepo.PersistData();
        return person;
    }

}
>>>>>>> upstream/master


    //Get a list of persons that we have with ID and Name
    //Remove Person
}