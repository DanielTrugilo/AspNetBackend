
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddAWSTalker();
//builder.Services.AddAzureTalker();
builder.Services.AddSingleton<PersonRepo>(); //Let's read about it :)

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
    private PersonRepo repo;

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
    private PersonRepo repo;
    public void Say(string msg) {
        Console.BackgroundColor = repo.GetColor(0);
        Console.WriteLine("{0} {1}", msg, count++);
    }
}

public record Person(string FirstName, string LastName);

public class PersonRepo
{
    Dictionary<int, Person> dict = new() { //super database lol
        {0,new ("Ryan", "Anderson")},
        {1,new ("Dani", "Trugilo")},
    };
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

    public ConsoleColor GetColor(int id)
    {
        var dict = new Dictionary<int, ConsoleColor>() {
            {0,ConsoleColor.DarkBlue},
            {1,ConsoleColor.DarkYellow}
        };
        return dict[id];
    }
    public List<string> ListPersons(int[] id)
    {
        List<string> nameList = new List<string>();
        foreach (KeyValuePair<int, Person> persons in dict){
            nameList.Add("Test");
        }
        return nameList;
    }

}

public interface ISystemTalker  // Interface
{
    void Say(string msg);
}

[ApiController] // Controller Attribute 
public class TalkerController { 
    ISystemTalker talker;
    private PersonRepo personRepo;

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


    //Get a list of persons that we have with ID and Name
    //Remove Person
}