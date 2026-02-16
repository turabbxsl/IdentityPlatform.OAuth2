IdentityPlatform.OAuth2

Identity Platform (OAuth 2.0 & OIDC)
This project is a centralized Identity Provider (IdP) built on .NET 10. It implements industry-standard protocols, specifically OAuth 2.0 and OpenID Connect (OIDC), to ensure secure authentication and authorization across distributed applications.

Technology Stack & Architecture

Backend & Core Logic
 - Framework: .NET 10 (C# 14/15) utilizing the latest features for robust and high-performance server-side logic.
 - Architecture: Built using a Clean Architecture approach with a clear separation between Domain, Application, and Infrastructure layers.
 - Design Patterns: Implementation of Repository and Unit of Work patterns to abstract the data layer and ensure transaction integrity.

Data Management
 - Database: PostgreSQL used as the primary relational data store for high performance and reliability.
 - ORM: Entity Framework Core 10 with a Code-First approach, including automated Migrations and Fluent API configurations.
 - Object Mapping: Strategic use of DTOs (Data Transfer Objects) to decouple internal domain models from external API contracts.

Security & Identity
 - Tokenization: Implementation of JWT (JSON Web Tokens) for stateless authentication.
 - Protocols: Strictly adheres to the OpenID Connect (OIDC) 1.0 and OAuth 2.0 specifications, specifically the Authorization Code Grant Type.
 - Cryptographic Standards: Secure handling of client secrets and user credentials with industry-standard hashing algorithms.

Authentication Flow
The system utilizes the Authorization Code Flow, which is the most secure method for web applications. The process follows these stages:
 - Client Validation: The system validates the requesting application using its ClientId and RedirectUri.
 - Authentication: Users are directed to the official login page to authenticate using their credentials (e.g., Registration Number and Password).
 - Consent: Upon successful login, the user is presented with a consent screen showing which scopes (Profile, Email, etc.) the client application is requesting.
 - Authorization Code: Once consent is granted, the system generates a short-lived, one-time-use Authorization Code.
 - Token Exchange (Back-channel): This is the core security feature. The exchange of the Authorization Code for tokens happens via a secure server-to-server request. This ensures that sensitive tokens are never exposed to the browser or URL history.

Token Management
The platform issues three distinct types of tokens upon a successful exchange:
 - ID Token (OIDC): A JWT containing user profile information (claims) such as name, email, and registration number. Used by the client for user identification and UI personalization.
 - Access Token: A short-lived credential used to access protected API resources via the Bearer scheme.
 - Refresh Token: A long-lived, high-security token used to obtain new access tokens without requiring the user to re-authenticate, ensuring a seamless experience.

Key Considerations
 - Simulation UI: The Frontend (MVC) part of this project serves as a Test Client to demonstrate the authentication lifecycle. While functional, its primary purpose is to simulate how a real-world application interacts with the Identity API.
 - Architectural Flexibility: The project is designed with a modular approach. It is fully extensible, allowing for the addition of new grant types, multi-tenant support, or advanced claim management.
 - Security First: By separating identity concerns into a dedicated provider, applications can offload sensitive credential handling and focus on their core business logic.

Setup and Configuration
 - Clone the repository.
 - Update the appsettings.json file with your PostgreSQL connection string.
 - Configure a secure JWT SecretKey (minimum 32 characters) in the configuration.
 - Run dotnet ef database update to apply migrations.
 - Launch the API and the UI Simulation projects.
