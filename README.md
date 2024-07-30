# KalanalyzeCode.ConfigurationManager
This solution is provide a much safe way to manage your dotnet configuration than inbuilt appsetting.json on IIS

[![Build and execute tests](https://github.com/ubhaya/KalanalyzeCode.ConfigurationManager/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/ubhaya/KalanalyzeCode.ConfigurationManager/actions/workflows/dotnet.yml)

# Project Roadmap: Configuration System for .NET Applications

## Step 1: Define Project Requirements
1. - [x] **Configuration Source**: Decide where the configurations will be stored (e.g., database, cloud storage, environment variables).
   > Configuration Manager will using NoSql database as a persistent storage. which database to use will decide later. 
2. - [x]  **Security**: Ensure that sensitive data is securely managed.
   > Data that stored in persistent layer should encrypt before saving. This will add a more overhead on the operations, but this will not be a big problem.
3. - [x]  **Flexibility**: Allow for different environments (development, staging, production).
   > Multiple environment support is a necessary to succeed the project.
4. - [ ]  **Scalability**: Ensure the system can handle large configurations efficiently.
5. - [ ]  **Integration**: Seamlessly integrate with existing .NET Core/Framework applications.

## Step 2: Choose Technology Stack
1. - [x]  **Backend**: .NET Core/Framework.
   > This project will use dotnet LTS versions. but further down the road it may possible to maintained a LTS version and STS version.
2. - [x]  **Storage**: SQL/NoSQL database, cloud storage (Azure Blob, AWS S3).
   > MongoDb will be use as primary database provider. As the project moved on user may able to choose custom database providers.
3. - [x]  **Authentication & Authorization**: OAuth, JWT.
   > Currently any authentication is not a consideration. But as a security reason project should use some authentication system.
4. - [x]  **Deployment**: Docker, Kubernetes, Azure/AWS/GCP.
   > This project will deploy mainly as a docker image. Still the options are open to provide the standalone application.

## Step 3: Design the System
1. - [ ]  **Configuration Service**:
    - - [x]  **API**: Create RESTful APIs for CRUD operations on configurations.
      > [Limitation][api-limitation]
      > [Functions][api-functions]
    - - [x]  **Client Library**: Develop a client library to fetch configurations.
    - - [ ] **Manager UI**: Create a UI to managed the configuration
      > [Functions][ui-functions]
2. - [ ]  **Configuration Storage**:
    - Design database schema or storage structure for configurations.
3. - [ ]  **Security**:
    - Implement encryption for sensitive data.
    - Secure API endpoints with proper authentication and authorization mechanisms.

## Step 4: Setup Development Environment
1. - [ ]  **Tools**: Visual Studio, .NET SDK, Docker.
   >  - **Docker file is not completed.**
   >  - Project is currently based on dotnet 8.0.300.
2. - [x]  **Repositories**: GitHub/GitLab for version control
   >  - Using Github
   >  - Continues development pipeline is configured.

## Step 5: Develop Configuration Service
1. - [x]  **API Development**:
    - Set up a new .NET project for the API.
    - Implement endpoints for managing configurations (GET, POST, PUT, DELETE).
    - Implement authentication and authorization.
    - > [Limitation][api-limitation]
2. - [x]  **Database Integration**:
    - Set up the database.
    - Implement data access layer using Entity Framework or Dapper.
3. - [x]  **Client Library**:
    - Create a .NET Standard library.
    - Implement methods to fetch configurations from the API.

## Step 6: Develop Configuration Fetching Logic
1. - [X]  **Replace `appsettings.json`**:
    - Create a custom configuration provider by implementing `IConfigurationProvider` and `IConfigurationSource`.
    - Use the client library to fetch configurations from your service.
    - Register your custom provider in the `Startup.cs`.

## Step 7: Implement Security Features
1. - [ ]  **Encryption**:
    - Encrypt sensitive data before storing it.
    - Decrypt data when fetching configurations.
2. - [x]  **Authentication**:
    - Use OAuth or JWT to secure the API.
    - Ensure the client library handles authentication.

## Step 8: Testing
1. - [ ]  **Unit Testing**:
    - Write unit tests for the API endpoints.
    - Test the client library.
2. - [ ]  **Integration Testing**:
    - Ensure the configuration provider works seamlessly with .NET applications.
3. - [ ]  **Security Testing**:
    - Perform security audits to ensure data protection.

## Step 9: Documentation
1. - [ ]  **API Documentation**:
    - Document all API endpoints using Swagger/OpenAPI.
2. - [ ]  **Usage Guide**:
    - Provide a guide on how to integrate the custom configuration provider into .NET applications.
3. - [ ]  **Security Guidelines**:
    - Document how to securely manage configurations.

## Step 10: Deployment
1. - [ ]  **Containerization**:
    - Containerize your API using Docker.
2. - [ ]  **Continuous Integration/Continuous Deployment (CI/CD)**:
    - Set up CI/CD pipelines for automated testing and deployment.
3. - [ ]  **Cloud Deployment**:
    - Deploy your API and storage solution to a cloud platform (Azure, AWS, GCP).

## Step 11: Monitoring and Maintenance
1. - [ ]  **Monitoring**:
    - Set up monitoring for your API using tools like Prometheus, Grafana, or Azure Monitor.
2. - [ ]  **Logging**:
    - Implement logging to track errors and usage patterns.
3. - [ ]  **Regular Updates**:
    - Keep dependencies updated.
    - Regularly review and update security practices.

## Step 12: Community Engagement and Feedback
1. - [ ]  **Open Source**:
    - Consider open-sourcing the project to get feedback and contributions.
2. - [ ]  **Feedback Loop**:
    - Collect feedback from users and continuously improve the system.

[api-limitation]: ./docs/Limitation.md/#api-limitation
[api-functions]: ./docs/Limitation.md/#api-functions
[ui-functions]: ./docs/Functions.md/#ui-functions
