using Xunit;
using AdventureGame;
using System;
using System.IO;

namespace AdventureGame.Tests;

public class ProgramTests
{
    [Fact]
    public void Main_ShouldPrintRoomToConsole()
    {
        // Arrange
        StringWriter output = new StringWriter();
        Console.SetOut(output);

        // Act
        Program.Main();

        // Assert
        string result = output.ToString();

        Assert.Contains("Room[", result);
        Assert.Contains("isLit=False", result);
        Assert.Contains("hasLamp=False", result);
        Assert.Contains("hasKey=False", result);
        Assert.Contains("hasChest=False", result);
        Assert.Contains("description=", result);
    }
}