using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Text.Json;
using ColorsAPI.Models;

namespace ColorsAPI.Tests;

[TestClass]
public class ColorsControllerIntegrationTests
{
    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    [TestInitialize]
    public void Setup()
    {
        _factory = new WebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [TestMethod]
    public async Task GetAllColors_ShouldReturnInitialColors()
    {
        // Act
        var response = await _client.GetAsync("/api/colors");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var colors = JsonSerializer.Deserialize<ColorItem[]>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.IsNotNull(colors);
        Assert.AreEqual(3, colors.Length);
        Assert.IsTrue(colors.Any(c => c.Name == "Red" && c.HexCode == "#FF0000"));
        Assert.IsTrue(colors.Any(c => c.Name == "Yellow" && c.HexCode == "#FFFF00"));
        Assert.IsTrue(colors.Any(c => c.Name == "Black" && c.HexCode == "#000000"));
    }

    [TestMethod]
    public async Task GetRandomColor_ShouldReturnAValidColor()
    {
        // Act
        var response = await _client.GetAsync("/api/colors/random");

        // Assert
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var color = JsonSerializer.Deserialize<ColorItem>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.IsNotNull(color);
        Assert.IsFalse(string.IsNullOrWhiteSpace(color.Name));
        Assert.IsFalse(string.IsNullOrWhiteSpace(color.HexCode));
    }

    [TestMethod]
    public async Task AddColor_WithValidColor_ShouldReturnCreated()
    {
        // Arrange
        var newColor = new ColorItem { Name = "Blue", HexCode = "#0000FF" };
        var json = JsonSerializer.Serialize(newColor);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/colors", content);

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);

        // Verify the color was added by getting all colors
        var getAllResponse = await _client.GetAsync("/api/colors");
        var allColorsContent = await getAllResponse.Content.ReadAsStringAsync();
        var colors = JsonSerializer.Deserialize<ColorItem[]>(allColorsContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Assert.IsNotNull(colors);
        Assert.AreEqual(4, colors.Length);
        Assert.IsTrue(colors.Any(c => c.Name == "Blue" && c.HexCode == "#0000FF"));
    }

    [TestMethod]
    public async Task AddColor_WithInvalidHexCode_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidColor = new ColorItem { Name = "InvalidColor", HexCode = "invalidhex" };
        var json = JsonSerializer.Serialize(invalidColor);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/colors", content);

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task AddColor_WithEmptyName_ShouldReturnBadRequest()
    {
        // Arrange
        var invalidColor = new ColorItem { Name = "", HexCode = "#0000FF" };
        var json = JsonSerializer.Serialize(invalidColor);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/colors", content);

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task AddColor_WithNullBody_ShouldReturnBadRequest()
    {
        // Arrange
        var content = new StringContent("", Encoding.UTF8, "application/json");

        // Act
        var response = await _client.PostAsync("/api/colors", content);

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
    }
}