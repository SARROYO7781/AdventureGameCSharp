using Xunit;
using AdventureGame;

namespace AdventureGame.Tests;

public class RoomTests
{
    [Fact]
    public void Constructor_ShouldInitializeDefaultValues()
    {
        Room room = new Room();

        Assert.False(room.IsLit());
        Assert.False(room.HasLamp());
        Assert.False(room.HasKey());
        Assert.False(room.HasChest());
        Assert.False(room.HasNorth());
        Assert.False(room.HasSouth());
        Assert.False(room.HasEast());
        Assert.False(room.HasWest());
        Assert.Equal(string.Empty, room.GetDescription());
    }

    [Fact]
    public void SetLit_ShouldUpdateIsLit()
    {
        Room room = new Room();

        room.SetLit(true);

        Assert.True(room.IsLit());
    }

    [Fact]
    public void SetLamp_ShouldUpdateHasLamp()
    {
        Room room = new Room();

        room.SetLamp(true);

        Assert.True(room.HasLamp());
    }

    [Fact]
    public void SetKey_ShouldUpdateHasKey()
    {
        Room room = new Room();

        room.SetKey(true);

        Assert.True(room.HasKey());
    }

    [Fact]
    public void SetChest_ShouldUpdateHasChest()
    {
        Room room = new Room();

        room.SetChest(true);

        Assert.True(room.HasChest());
    }

    [Fact]
    public void SetNorth_ShouldUpdateHasNorth()
    {
        Room room = new Room();

        room.SetNorth(true);

        Assert.True(room.HasNorth());
    }

    [Fact]
    public void SetSouth_ShouldUpdateHasSouth()
    {
        Room room = new Room();

        room.SetSouth(true);

        Assert.True(room.HasSouth());
    }

    [Fact]
    public void SetEast_ShouldUpdateHasEast()
    {
        Room room = new Room();

        room.SetEast(true);

        Assert.True(room.HasEast());
    }

    [Fact]
    public void SetWest_ShouldUpdateHasWest()
    {
        Room room = new Room();

        room.SetWest(true);

        Assert.True(room.HasWest());
    }

    [Fact]
    public void SetDescription_ShouldUpdateDescription()
    {
        Room room = new Room();

        room.SetDescription("A dark cave");

        Assert.Equal("A dark cave", room.GetDescription());
    }

    [Fact]
    public void ToString_ShouldReturnCorrectFormat()
    {
        Room room = new Room();

        room.SetLit(true);
        room.SetLamp(true);
        room.SetKey(true);
        room.SetChest(true);
        room.SetNorth(true);
        room.SetSouth(true);
        room.SetEast(true);
        room.SetWest(true);
        room.SetDescription("Treasure room");

        string result = room.ToString();

        Assert.Contains("Room[", result);
        Assert.Contains("isLit=True", result);
        Assert.Contains("hasLamp=True", result);
        Assert.Contains("hasKey=True", result);
        Assert.Contains("hasChest=True", result);
        Assert.Contains("description=Treasure room", result);
    }
}