# Developer Evaluation Project

## Instructions
Test instructions and requirements can be found here

See [Instructions](/.doc/instructions.md)

# How to Run the Project and Tests

## Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/en-us/download) installed
- [Docker](https://www.docker.com/products/docker-desktop/) installed and running
- [Git](https://git-scm.com/) installed

## Steps

1. **Clone the project from the repository:**
   ```bash
   git clone https://github.com/sadraquecuellar/AMBEV-Test.git
   ```

2. **Navigate to the project folder:**
   ```bash
   cd "[YourPath]\AMBEV-Test\template\backend"
   ```

3. **Clean the solution:**
   ```bash
   dotnet clean .\Ambev.DeveloperEvaluation.sln
   ```

4. **Restore NuGet packages:**
   ```bash
   dotnet restore .\Ambev.DeveloperEvaluation.sln
   ```

5. **Build the solution:**
   ```bash
   dotnet build .\Ambev.DeveloperEvaluation.sln --no-restore
   ```

6. **Run the unit tests:**
   ```bash
   dotnet test "tests\Ambev.DeveloperEvaluation.Unit\Ambev.DeveloperEvaluation.Unit.csproj" --logger "console;verbosity=detailed"
   ```

7. **Start Docker containers:**
   ```bash
   docker-compose up --build
   ```

8. **Open Swagger UI:**

   Open your browser and go to https://localhost:8081/swagger/index.html to see the Swagger UI.

## Notes
- Make sure Docker is running before executing `docker-compose up`.
- Use `docker-compose down` if you need to stop the containers after testing.

