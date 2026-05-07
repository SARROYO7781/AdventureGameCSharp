namespace AdventureGame;

public class DungeonLoader
{
    public static Room[,] LoadDungeon(out int startRow, out int startCol)
    {
        string[] lines =
        {
             ". L K",
             ". D .",
             "C . E"
        };

        int rows = lines.Length;

        int cols = lines[0]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Length;

        Room[,] dungeon = new Room[rows, cols];

        startRow = 0;
        startCol = 0;

        for (int r = 0; r < rows; r++)
        {
            string[] symbols = lines[r]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int c = 0; c < cols; c++)
            {
                Room room = new Room();

                room.SetDescription($"Room [{r},{c}]");

                string symbol = symbols[c];

                if (symbol == "L")
                {
                    room.SetLamp(true);
                }

                else if (symbol == "K")
                {
                    room.SetKey(true);
                    room.SetLit(true);
                }

                else if (symbol == "C")
                {
                    room.SetChest(true);
                    room.SetLit(true);
                }

                else if (symbol == "E")
                {
                    room.SetExit(true);
                    room.SetLit(true);
                }

                else if (symbol == "D")
                {
                    room.SetStart(true);
                    room.SetLit(true);

                    startRow = r;
                    startCol = c;
                }

                dungeon[r, c] = room;
            }
        }

        SetRoomConnections(dungeon);

        return dungeon;
    }

    private static void SetRoomConnections(Room[,] dungeon)
    {
        int rows = dungeon.GetLength(0);
        int cols = dungeon.GetLength(1);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                dungeon[r, c].SetNorth(r > 0);

                dungeon[r, c].SetSouth(r < rows - 1);

                dungeon[r, c].SetWest(c > 0);

                dungeon[r, c].SetEast(c < cols - 1);
            }
        }
    }

    private static string GetRoomDescription(string symbol, int row, int col)
    {
    string roomId = $"Room [{row},{col}]";

    if (symbol == "D")
    {
        return $"{roomId}\nYou awaken in a damp stone chamber, the silence around you almost unsettling.";
    }

    return $"{roomId}\nThe room is quiet, with cold stone walls surrounding you.";
    }
}