// See https://aka.ms/new-console-template for more information
namespace AdventureGame;

public class Program
{
    public static void Main()
    {
        Adventurer a = new Adventurer();
        Adventurer b = new Adventurer();
        b.SetLamp(true);
        b.SetKey(true);
        Room r = new Room();
        Console.WriteLine(r);
    }
}
