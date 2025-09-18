# Colors API

A .NET 8.0 Web API for managing colors with hex code validation.

## Features

- **Get All Colors**: Retrieve all available colors in the system
- **Add New Color**: Add a new color with name and hex code validation
- **Get Random Color**: Get a randomly selected color from the collection
- **Hex Code Validation**: Ensures hex codes follow the #RRGGBB format

## Initial Data

The API starts with three default colors:
- Red (#FF0000)
- Yellow (#FFFF00) 
- Black (#000000)

## API Endpoints

### GET /api/colors
Returns all colors in the system.

**Response:**
```json
[
  {
    "name": "Red",
    "hexCode": "#FF0000"
  },
  {
    "name": "Yellow", 
    "hexCode": "#FFFF00"
  },
  {
    "name": "Black",
    "hexCode": "#000000"
  }
]
```

### POST /api/colors
Adds a new color to the system.

**Request Body:**
```json
{
  "name": "Blue",
  "hexCode": "#0000FF"
}
```

**Response:** Returns the created color with 201 status code.

**Validation:**
- Name is required and cannot be empty
- HexCode must follow #RRGGBB format (e.g., #FF0000)

### GET /api/colors/random
Returns a randomly selected color from the collection.

**Response:**
```json
{
  "name": "Red",
  "hexCode": "#FF0000"
}
```

## Running the API

1. **Prerequisites**: .NET 8.0 SDK
2. **Build**: `dotnet build`
3. **Run**: `dotnet run --project ColorsAPI`
4. **Test**: `dotnet test`

The API will be available at `http://localhost:5058` in development mode.

## Testing

The project includes comprehensive tests:
- **Unit Tests**: Test the ColorService logic and validation
- **Integration Tests**: Test the API endpoints end-to-end
- **Total Coverage**: 24 tests covering all functionality

Run tests with: `dotnet test`

## Project Structure

```
ColorsAPI/
├── ColorsAPI/                  # Main API project
│   ├── Controllers/            # API controllers
│   ├── Models/                 # Data models
│   ├── Services/               # Business logic services
│   └── Program.cs              # Application entry point
├── ColorsAPI.Tests/            # Test project
│   ├── ColorsControllerIntegrationTests.cs
│   └── UnitTest1.cs (ColorServiceTests)
└── ColorsAPI.sln              # Solution file
```

## Technology Stack

- **.NET 8.0**: Latest LTS framework
- **ASP.NET Core**: Web API framework
- **MSTest**: Testing framework
- **Swagger/OpenAPI**: API documentation
- **Dependency Injection**: Built-in DI container
