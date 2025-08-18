using Microsoft.VisualStudio.TestTools.UnitTesting;
using ColorsAPI.Services;
using ColorsAPI.Models;

namespace ColorsAPI.Tests;

[TestClass]
public class ColorServiceTests
{
    private IColorService _colorService = null!;

    [TestInitialize]
    public void Setup()
    {
        _colorService = new ColorService();
    }

    [TestMethod]
    public void GetAllColors_ShouldReturnInitialColors()
    {
        // Act
        var colors = _colorService.GetAllColors().ToList();

        // Assert
        Assert.AreEqual(3, colors.Count);
        Assert.IsTrue(colors.Any(c => c.Name == "Red" && c.HexCode == "#FF0000"));
        Assert.IsTrue(colors.Any(c => c.Name == "Yellow" && c.HexCode == "#FFFF00"));
        Assert.IsTrue(colors.Any(c => c.Name == "Black" && c.HexCode == "#000000"));
    }

    [TestMethod]
    public void AddColor_WithValidColor_ShouldReturnTrue()
    {
        // Arrange
        var newColor = new ColorItem { Name = "Blue", HexCode = "#0000FF" };

        // Act
        var result = _colorService.AddColor(newColor);

        // Assert
        Assert.IsTrue(result);
        var colors = _colorService.GetAllColors().ToList();
        Assert.AreEqual(4, colors.Count);
        Assert.IsTrue(colors.Any(c => c.Name == "Blue" && c.HexCode == "#0000FF"));
    }

    [TestMethod]
    public void AddColor_WithInvalidHexCode_ShouldReturnFalse()
    {
        // Arrange
        var invalidColor = new ColorItem { Name = "InvalidColor", HexCode = "invalidhex" };

        // Act
        var result = _colorService.AddColor(invalidColor);

        // Assert
        Assert.IsFalse(result);
        var colors = _colorService.GetAllColors().ToList();
        Assert.AreEqual(3, colors.Count); // Should still be initial 3
    }

    [TestMethod]
    public void AddColor_WithEmptyName_ShouldReturnFalse()
    {
        // Arrange
        var invalidColor = new ColorItem { Name = "", HexCode = "#0000FF" };

        // Act
        var result = _colorService.AddColor(invalidColor);

        // Assert
        Assert.IsFalse(result);
        var colors = _colorService.GetAllColors().ToList();
        Assert.AreEqual(3, colors.Count); // Should still be initial 3
    }

    [TestMethod]
    public void GetRandomColor_ShouldReturnOneOfTheColors()
    {
        // Act
        var randomColor = _colorService.GetRandomColor();

        // Assert
        Assert.IsNotNull(randomColor);
        var allColors = _colorService.GetAllColors().ToList();
        Assert.IsTrue(allColors.Any(c => c.Name == randomColor.Name && c.HexCode == randomColor.HexCode));
    }

    [TestMethod]
    [DataRow("#FF0000", true)]
    [DataRow("#00FF00", true)]
    [DataRow("#0000FF", true)]
    [DataRow("#FFFFFF", true)]
    [DataRow("#000000", true)]
    [DataRow("#123ABC", true)]
    [DataRow("#abcdef", true)]
    [DataRow("FF0000", false)] // Missing #
    [DataRow("#FF00", false)] // Too short
    [DataRow("#FF00000", false)] // Too long
    [DataRow("#GGGGGG", false)] // Invalid characters
    [DataRow("", false)] // Empty
    [DataRow(null, false)] // Null
    public void IsValidHexCode_ShouldValidateCorrectly(string hexCode, bool expected)
    {
        // Act
        var result = _colorService.IsValidHexCode(hexCode);

        // Assert
        Assert.AreEqual(expected, result);
    }
}