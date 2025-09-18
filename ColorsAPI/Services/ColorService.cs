using ColorsAPI.Models;
using System.Text.RegularExpressions;

namespace ColorsAPI.Services;

public interface IColorService
{
    IEnumerable<ColorItem> GetAllColors();
    bool AddColor(ColorItem color);
    ColorItem? GetRandomColor();
    bool IsValidHexCode(string hexCode);
}

public class ColorService : IColorService
{
    private readonly List<ColorItem> _colors;
    private static readonly Regex HexCodeRegex = new(@"^#[0-9A-Fa-f]{6}$", RegexOptions.Compiled);

    public ColorService()
    {
        // Initialize with Red, Yellow, and Black
        _colors = new List<ColorItem>
        {
            new() { Name = "Red", HexCode = "#FF0000" },
            new() { Name = "Yellow", HexCode = "#FFFF00" },
            new() { Name = "Black", HexCode = "#000000" }
        };
    }

    public IEnumerable<ColorItem> GetAllColors()
    {
        return _colors.AsReadOnly();
    }

    public bool AddColor(ColorItem color)
    {
        if (string.IsNullOrWhiteSpace(color.Name) || !IsValidHexCode(color.HexCode))
        {
            return false;
        }

        _colors.Add(color);
        return true;
    }

    public ColorItem? GetRandomColor()
    {
        if (_colors.Count == 0)
        {
            return null;
        }

        var random = new Random();
        return _colors[random.Next(_colors.Count)];
    }

    public bool IsValidHexCode(string hexCode)
    {
        return !string.IsNullOrWhiteSpace(hexCode) && HexCodeRegex.IsMatch(hexCode);
    }
}