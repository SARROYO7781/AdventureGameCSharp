using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Security.Cryptography;

namespace AdventureGame;

public class AdventureGame
{
    private readonly string GO_NORTH = "W";
    private readonly string GO_SOUTH = "S";
    private readonly string GO_EAST = "D";
    private readonly string GO_WEST = "A";
    private readonly string GET_LAMP = "L";
    private readonly string GET_KEY = "K";
    private readonly string OPEN_CHEST = "O";
    private readonly string QUIT = "Q";
    
    private Adventurer adventurer;
    private Room[,] dungeon;
    private int aRow;
    private int aCol;
    private bool isChestOpen;
    private bool hasPlayerQuit;
    private bool isAdventurerAlive;
    private string lastDirection;

    public AdventureGame()
    {
        
    }

    public void start()
    {
        Init();

        ShowGameStartScreen();

        string input;

        do
        {
            ShowScene();

            do
            {
                ShowInputOptions();

                input = GetInput();
            }
            while(!IsValidInput(input));

            ProccessInput(input);

            UpdateGameState();
        }
        while(!IsGameOver());

        ShowGameOverScreen();
    }

    private void Init()
    {
        adventurer = new Adventurer();

        Room r1 = new Room();
        r1.SetLit(true);
        r1.SetDescription("Room 1");
        r1.SetSouth(true);
        r1.SetEast(true);
        r1.SetLamp(true);

        Room r2 = new Room();
        r2.SetKey(true);
        r2.SetDescription("Room 2");
        r2.SetWest(true);
        r2.SetSouth(true);

        Room r3 = new Room();
        r3.SetLit(true);
        r3.SetDescription("Room 3");
        r3.SetNorth(true);
        r3.SetEast(true);
        r3.SetChest(true);


        Room r4 = new Room();
        r4.SetDescription("Room 4");
        r4.SetNorth(true);
        r4.SetWest(true);


        dungeon = new Room[,]
        {
            { r1, r2 },
            { r3, r4 }
        };

        aRow = 1;
        aCol = 0;

        isChestOpen = false;
        hasPlayerQuit = false;
        isAdventurerAlive = true;
        
        lastDirection = string.Empty;
    }

    private void ShowGameStartScreen()
    {
        Console.WriteLine("Welcome to Adventure Game!");
    }

    private void ShowScene()
    {
      var r = dungeon[aRow, aCol];

        if (adventurer.HasLamp() || r.IsLit())
        {
            Console.WriteLine(r.GetDescription());
        }
        
        else
        {
            Console.WriteLine("This room is pitch black!");
        }
    }

    private void ShowInputOptions()
    {
        string options = ""
        +$"GO NORTH[{GO_NORTH}] | GO EAST[{GO_EAST}] | GET LAMP[{GET_LAMP}] | OPEN CHEST[{OPEN_CHEST}] \n"
        +$"GO SOUTH[{GO_SOUTH}] | GO WEST[{GO_WEST}] | GET KEY [{GET_KEY}] | QUIT      [{QUIT}] \n"
        +$"> ";

        Console.Write(options);
    }

    private string GetInput()
    {
        return Console.ReadLine()!.ToUpper();
    }

    private bool IsValidInput(string input)
    {
        if(input != GO_NORTH && 
           input != GO_SOUTH &&
           input != GO_EAST &&
           input != GO_WEST &&
           input != GET_LAMP &&
           input != GET_KEY &&
           input != OPEN_CHEST &&
           input != QUIT)
        {
            Console.WriteLine("Error: Inavlid Input. Pleae try again");
            return false;
        }

        return true;
    }

    private void ProccessInput(string input)
    {
        Room r = dungeon[aRow, aCol];

        if(!adventurer.HasLamp() && !r.IsLit() && input != lastDirection)
        {
            Console.WriteLine("You got eaten alive by the grue!");
            isAdventurerAlive = false;
        }

        else if(input == GO_NORTH)
        {
            GoNorth(r);
        }

         else if(input == GO_SOUTH)
        {
            GoSouth(r);
        }

        else if(input == GO_EAST)
        {
            GoEast(r);
        }

        else if(input == GO_WEST)
        {
            GoWest(r);
        }

         else if(input == GET_LAMP)
        {
            GetLamp(r);
        }

         else if(input == GET_KEY)
        {
            GetKey(r);
        }

         else if(input == OPEN_CHEST)
        {
            OpenChest(r);
        }

         else
        {
            Quit();
        }
    }

    private void UpdateGameState()
    {
        
    }

    private bool IsGameOver()
    {
        return isChestOpen || hasPlayerQuit || !isAdventurerAlive;
    }

    private void ShowGameOverScreen()
    {
        Console.WriteLine("Game Over!");
    }

    private void GoNorth(Room r)
    {
        {
            if (r.HasNorth())
            {
                aRow -= 1;
                lastDirection = GO_SOUTH;
            }

            else
            {
                Console.WriteLine("You cannot go north!\a");
            }
        }
    }
    private void GoSouth(Room r)
    {
         if (r.HasSouth())
            {
                aRow += 1;
                lastDirection = GO_NORTH;
            }

            else
            {
                Console.WriteLine("You cannot go south!\a");
            }
    }

     private void GoEast(Room r)
    {
         if (r.HasEast())
            {
                aCol += 1;
                lastDirection = GO_WEST;
            }

            else
            {
                Console.WriteLine("You cannot go East!\a");
            }
    }

     private void GoWest(Room r)
    {
         if (r.HasWest())
            {
                aCol -= 1;
                lastDirection = GO_EAST;
            }

            else
            {
                Console.WriteLine("You cannot go West!\a");
            }
    }

    private void GetLamp(Room r)
    {
        if (r.HasLamp())
        {
            Console.WriteLine("You got the lamp!");
            adventurer.SetLamp(true);
            r.SetLamp(false);
        }

    

        else
        {
            Console.WriteLine("There is no lamp in this room.");
        }
    }

         private void GetKey(Room r)
    {
    
        if (r.HasKey())
        {
            Console.WriteLine("You got the key!");
            adventurer.SetKey(true);
            r.SetKey(false);
        }

        else
        {
            Console.WriteLine("There is no key in this room.");
        }
        
    }

     private void OpenChest(Room r)
    {
    
        if (r.HasChest())
        {
            if (adventurer.HasKey())
            {
                 Console.WriteLine("You got the treasure!");
                 isChestOpen = true;
            }
            else
            {
                Console.WriteLine("You do not have the key.");
            }
        }
        else
        {
            Console.WriteLine("There is no chest in this room.");
        }
    }

    private void Quit()
    {
        Console.WriteLine("You quit the game!");
        hasPlayerQuit = true;
    }

}
