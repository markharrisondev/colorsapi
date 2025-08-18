using Microsoft.AspNetCore.Mvc;
using ColorsAPI.Models;
using ColorsAPI.Services;

namespace ColorsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColorsController : ControllerBase
{
    private readonly IColorService _colorService;

    public ColorsController(IColorService colorService)
    {
        _colorService = colorService;
    }

    /// <summary>
    /// Get all colors
    /// </summary>
    /// <returns>List of all colors</returns>
    [HttpGet]
    public ActionResult<IEnumerable<ColorItem>> GetAllColors()
    {
        var colors = _colorService.GetAllColors();
        return Ok(colors);
    }

    /// <summary>
    /// Add a new color
    /// </summary>
    /// <param name="color">Color to add</param>
    /// <returns>Created color or validation error</returns>
    [HttpPost]
    public ActionResult<ColorItem> AddColor([FromBody] ColorItem color)
    {
        if (color == null)
        {
            return BadRequest("Color data is required");
        }

        if (string.IsNullOrWhiteSpace(color.Name))
        {
            return BadRequest("Color name is required");
        }

        if (!_colorService.IsValidHexCode(color.HexCode))
        {
            return BadRequest("Invalid hex code format. Use format #RRGGBB (e.g., #FF0000)");
        }

        bool added = _colorService.AddColor(color);
        if (!added)
        {
            return BadRequest("Failed to add color");
        }

        return CreatedAtAction(nameof(GetAllColors), color);
    }

    /// <summary>
    /// Get a random color
    /// </summary>
    /// <returns>A random color from the collection</returns>
    [HttpGet("random")]
    public ActionResult<ColorItem> GetRandomColor()
    {
        var randomColor = _colorService.GetRandomColor();
        if (randomColor == null)
        {
            return NotFound("No colors available");
        }

        return Ok(randomColor);
    }
}