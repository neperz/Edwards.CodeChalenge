# .NET Core 6 Web API with Docker, SQLite, and Unit Tests

This is a sample .NET Core 6 Web API project that demonstrates how to create a user management system using in-memory storage. It includes Docker support for containerization and uses SQLite for storing data persistently. Additionally, it incorporates unit tests to ensure code quality.

## User Story

As a user, I want an application created in .NET 6 that allows me to manage user objects via a Web API. The application should store users in memory and provide the following functionality through the API:

- Adding a new user to the storage.
- Deleting a user from the storage.
- Updating a user in the storage.
- Getting a list of users with filters from the storage.

The application should follow Microsoft's code conventions and adhere to SOLID principles. It also integrates Swashbuckle Swagger UI for API documentation.

## Prerequisites

Before running the application, ensure you have the following prerequisites installed:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Docker](https://www.docker.com/get-started)
- [SQLite](https://www.sqlite.org/index.html)

## Getting Started

1. Clone the repository:

   ```bash
   git clone https://github.com/neperz/Edwards.CodeChalenge.git
   ```

2. Navigate to the project directory:

   ```bash
   cd Edwards.CodeChalenge/setup
   ```


3. Run the Docker container on nginx:

   ```bash
   .\run.sh
   ```

   This will start two instances of the Web API application in a Docker container hosted on nginx load balace, exposing it on port 9999.

4. Access the Swagger UI documentation by opening a web browser and navigating to:

   ```
   http://localhost:9999/swagger
   ```

   Use Swagger UI to interact with the API and test its endpoints.

## Running Unit Tests

To run the unit tests, use the following command:

```bash
dotnet test
```

This will execute the unit tests and display the results in the console.


## License

This project is licensed under the MIT License 

## Acknowledgments

- This project follows best practices for .NET Core Web API development.
- The use of Docker and SQLite ensures portability and ease of deployment.

Feel free to modify and extend this project as needed for your requirements.
