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

    private int grueRow;
    private int grueCol;

    private bool isChestOpen;
    private bool hasPlayerQuit;
    private bool isAdventurerAlive;
    private bool isGrueActive;
    private bool hasWon;

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
            while (!IsValidInput(input));

            ProccessInput(input);

            UpdateGameState();
        }
        while (!IsGameOver());

        ShowGameOverScreen();
    }

    private void Init()
    {
        adventurer = new Adventurer();

        dungeon = DungeonLoader.LoadDungeon(out aRow, out aCol);

        grueRow = 0;
        grueCol = 0;

        isChestOpen = false;
        hasPlayerQuit = false;
        isAdventurerAlive = true;
        isGrueActive = false;
        hasWon = false;

        lastDirection = string.Empty;
    }

    private void ShowGameStartScreen()
    {
        Console.WriteLine("Welcome to Adventure Game!");
    }

    private void ShowScene()
    {
        Room r = dungeon[aRow, aCol];

        Console.WriteLine($"Player Position: [{aRow},{aCol}]");

        if (adventurer.HasLamp() || r.IsLit())
        {
            Console.WriteLine(GetDynamicRoomDescription(r));
        }
        else
        {
            Console.WriteLine("This room is pitch black!");
        }
    }

    private string GetDynamicRoomDescription(Room r)
    {
        string description = r.GetDescription();

        if (r.HasLamp())
        {
            description += "\nA faint metallic glow flickers softly from the corner of the room.";
        }

        if (r.HasKey())
        {
            description += "\nA small metallic object reflects a faint glimmer near the ground.";
        }

        if (r.HasChest())
        {
            description += "\nAn old wooden chest rests against the far wall, covered in dust and scratches.";
        }

        if (r.HasExit())
        {
            description += "\nA cold breeze flows through the corridor ahead. It feels like there may be a way out nearby.";
        }

        return description;
    }

    private void ShowInputOptions()
    {
        string options = ""
            + $"GO NORTH[{GO_NORTH}] | GO EAST[{GO_EAST}] | GET LAMP[{GET_LAMP}] | OPEN CHEST[{OPEN_CHEST}]\n"
            + $"GO SOUTH[{GO_SOUTH}] | GO WEST[{GO_WEST}] | GET KEY [{GET_KEY}] | QUIT      [{QUIT}]\n"
            + "> ";

        Console.Write(options);
    }

    private string GetInput()
    {
        return Console.ReadLine()!.ToUpper();
    }

    private bool IsValidInput(string input)
    {
        if (input != GO_NORTH &&
            input != GO_SOUTH &&
            input != GO_EAST &&
            input != GO_WEST &&
            input != GET_LAMP &&
            input != GET_KEY &&
            input != OPEN_CHEST &&
            input != QUIT)
        {
            Console.WriteLine("Error: Invalid input. Please try again.");
            return false;
        }

        return true;
    }

    private void ProccessInput(string input)
    {
        Room r = dungeon[aRow, aCol];

        if (!adventurer.HasLamp()
            && !r.IsLit()
            && input != lastDirection
            && !(input == GET_LAMP && r.HasLamp()))
        {
            Console.WriteLine("You got eaten alive by the grue!");
            isAdventurerAlive = false;
        }
        else if (input == GO_NORTH)
        {
            GoNorth(r);
        }
        else if (input == GO_SOUTH)
        {
            GoSouth(r);
        }
        else if (input == GO_EAST)
        {
            GoEast(r);
        }
        else if (input == GO_WEST)
        {
            GoWest(r);
        }
        else if (input == GET_LAMP)
        {
            GetLamp(r);
        }
        else if (input == GET_KEY)
        {
            GetKey(r);
        }
        else if (input == OPEN_CHEST)
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
        Room currentRoom = dungeon[aRow, aCol];

        if (isChestOpen && currentRoom.HasExit())
        {
            Console.WriteLine("You escaped the dungeon with the treasure!");
            hasWon = true;
            return;
        }

        if (isGrueActive)
        {
            MoveGrue();

            if (grueRow == aRow && grueCol == aCol)
            {
                Console.WriteLine("The Grue caught you!");
                isAdventurerAlive = false;
            }
            else
            {
                ShowGrueWarning();
            }
        }
    }

    private bool IsGameOver()
    {
        return hasWon || hasPlayerQuit || !isAdventurerAlive;
    }

    private void ShowGameOverScreen()
    {
        if (hasWon)
        {
            Console.WriteLine("You win!");
        }
        else
        {
            Console.WriteLine("Game Over!");
        }
    }

    private void GoNorth(Room r)
    {
        if (r.HasNorth())
        {
            aRow -= 1;
            Console.Clear();
            lastDirection = GO_SOUTH;
        }
        else
        {
            Console.WriteLine("You cannot go north!");
        }
    }

    private void GoSouth(Room r)
    {
        if (r.HasSouth())
        {
            aRow += 1;
            Console.Clear();
            lastDirection = GO_NORTH;
        }
        else
        {
            Console.WriteLine("You cannot go south!");
        }
    }

    private void GoEast(Room r)
    {
        if (r.HasEast())
        {
            aCol += 1;
            Console.Clear();
            lastDirection = GO_WEST;
        }
        else
        {
            Console.WriteLine("You cannot go east!");
        }
    }

    private void GoWest(Room r)
    {
        if (r.HasWest())
        {
            aCol -= 1;
            Console.Clear();
            lastDirection = GO_EAST;
        }
        else
        {
            Console.WriteLine("You cannot go west!");
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
                Console.WriteLine("You opened the treasure chest!");
                Console.WriteLine("The Grue has awakened and is now chasing you!");

                isChestOpen = true;
                isGrueActive = true;
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

    private void MoveGrue()
    {
        List<(int row, int col)> path = FindShortestPath(
            grueRow,
            grueCol,
            aRow,
            aCol
        );

        if (path.Count > 1)
        {
            grueRow = path[1].row;
            grueCol = path[1].col;
        }

        Console.WriteLine("You hear footsteps getting closer...");
    }

    private List<(int row, int col)> FindShortestPath(
        int startRow,
        int startCol,
        int targetRow,
        int targetCol)
    {
        int rows = dungeon.GetLength(0);
        int cols = dungeon.GetLength(1);

        bool[,] visited = new bool[rows, cols];
        (int row, int col)[,] previous = new (int row, int col)[rows, cols];

        Queue<(int row, int col)> queue = new Queue<(int row, int col)>();

        queue.Enqueue((startRow, startCol));
        visited[startRow, startCol] = true;
        previous[startRow, startCol] = (-1, -1);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current.row == targetRow && current.col == targetCol)
            {
                break;
            }

            foreach (var neighbor in GetNeighbors(current.row, current.col))
            {
                if (!visited[neighbor.row, neighbor.col])
                {
                    visited[neighbor.row, neighbor.col] = true;
                    previous[neighbor.row, neighbor.col] = current;
                    queue.Enqueue(neighbor);
                }
            }
        }

        List<(int row, int col)> path = new List<(int row, int col)>();

        if (!visited[targetRow, targetCol])
        {
            return path;
        }

        var step = (targetRow, targetCol);

        while (step != (-1, -1))
        {
            path.Add(step);
            step = previous[step.targetRow, step.targetCol];
        }

        path.Reverse();

        return path;
    }

    private List<(int row, int col)> GetNeighbors(int row, int col)
    {
        List<(int row, int col)> neighbors = new List<(int row, int col)>();

        Room room = dungeon[row, col];

        if (room.HasNorth())
        {
            neighbors.Add((row - 1, col));
        }

        if (room.HasSouth())
        {
            neighbors.Add((row + 1, col));
        }

        if (room.HasEast())
        {
            neighbors.Add((row, col + 1));
        }

        if (room.HasWest())
        {
            neighbors.Add((row, col - 1));
        }

        return neighbors;
    }

    private void ShowGrueWarning()
    {
        if (!isGrueActive)
        {
            return;
        }

        int distance =
            Math.Abs(grueRow - aRow)
            + Math.Abs(grueCol - aCol);

        if (distance >= 4)
        {
            Console.WriteLine("You hear distant scratching somewhere in the dungeon...");
        }
        else if (distance == 3)
        {
            Console.WriteLine("Heavy footsteps echo through the stone halls...");
        }
        else if (distance == 2)
        {
            Console.WriteLine("Something is moving nearby...");
        }
        else if (distance == 1)
        {
            Console.WriteLine("Something is breathing just beyond the darkness...");
        }
    }

    private void Quit()
    {
        Console.WriteLine("You quit the game!");
        hasPlayerQuit = true;
    }
}