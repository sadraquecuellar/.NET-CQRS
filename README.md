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
   git clone https://github.com/sadraquecuellar/.NET-CQRS.git
   ```

2. **Navigate to the project folder:**
   ```bash
   cd "[YourPath]\.NET-CQRS\template\backend"
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

# About Project
The main objective of this project is to implement sales-oriented features, here is all the documentation of the developed routes.

See [Docs - Sales-API](/.doc/sales-api.md)

## Relevant points

Since the focus of the development was on sales functionalities, it was decided to implement some less critical entities as enums, aiming to make the development process more agile and focused on the priorities. Entities such as Products, Branchs, and Customers (which could be treated in the application as a User with a Customer role) were implemented this way. One identified area for improvement in the project would be to implement these entities properly, with all their fields and business rules properly structured.

## Message Broker

As a key differentiator of the project, event dispatching was implemented using the RabbitMQ message broker. During the service implementation, configurations were created to set up exchanges and queues, including predefined dead letter queues (DLQs). This setup facilitates the future implementation of consumers, allowing them to send messages that cannot be processed to the DLQ, which will simplify troubleshooting in the future.