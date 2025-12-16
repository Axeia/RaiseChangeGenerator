using RaiseChangeGenerator;
using ReactiveUI;
using System;
using System.Reactive.Linq;

/// <summary>
/// Third party framework class that can't be modified.
/// </summary>
/// <param name="houseNumber"></param>
/// <param name="streetName"></param>
public sealed class Address(int houseNumber, string streetName) : ReactiveObject
{
    public int HouseNumber = houseNumber;
    public string StreetName = streetName;
}

public partial class Person(
    string firstName, string lastName,
    int houseNumber, string streetName
) : ReactiveObject
{
    [RaiseChange]
    [AlsoNotify(nameof(FullName))]
    private string _firstName = firstName;

    [RaiseChange]
    [AlsoNotify(nameof(FullName))]
    private string _lastName = lastName;

    public string FullName => $"{FirstName} {LastName}";

    [RaiseChangeProxy(nameof(Address.HouseNumber))]
    [RaiseChangeProxy(nameof(Address.StreetName))]
    private readonly Address _address = new(houseNumber, streetName);
}

class Program
{
    static void Main()
    {
        var person = new Person("John", "Doe", 60, "Hde Road");

        person.WhenAnyValue(p => p.StreetName)
            .Skip(1)
            .Subscribe(streetName => Console.WriteLine($"StreetName updated to '{streetName}'") );
        person.WhenAnyValue(p => p.FirstName)
            .Skip(1)
            .Subscribe(firstName => Console.WriteLine($"First name updated to '{firstName}'"));
        person.WhenAnyValue(p => p.FullName)
            .Skip(1)
            .Subscribe(fullName => Console.WriteLine($"Full name updated to '{fullName}'"));

        // Note how StreetName now exists directly on Person, proxied to its _address.StreetName
        // Will trigger update which will update UI (or in this case will write a console line
        // due to WhenAnyValue Observer defined earlier.
        Console.WriteLine("Correcting street name (proxied property)...");
        person.StreetName = "Hyde Road";

        Console.WriteLine("Setting FirstName to 'Jane' (will update both FirstName and FullName)...");
        // Will trigger observer (and UI update) due to [AutoPropertyChanged] 
        // Will trigger observer (and UI update) due to [AlsoNotify]
        person.FirstName = "Jane";

        Console.WriteLine("\nPress any key to exit...");
        Console.WriteLine("(but which one is the any key?)");
        Console.ReadKey();
    }
}