# Authentication & Authorization Implementation Plan

This document outlines the steps to implement Authentication, Authorization, and Role-Based Access Control in your `skillSewa` project.

## Milestone 1: Setup & Model Updates
**Goal:** Prepare the project dependencies and database schema.

- [x] **Install BCrypt Package**
  - Run the following command in your terminal to install the password hashing library:
    ```bash
    dotnet add package BCrypt.Net-Next
    ```

- [x] **Update `User` Model**
  - Open `Models/User.cs`.
  - Add a `Role` property to store the user's role (e.g., "Admin", "User").
    ```csharp
    public string Role { get; set; } = "User"; // Default role
    ```

- [x] **Create Database Migration**
  - Open your terminal and run the commands to update the database:
    ```bash
    dotnet tool install --global dotnet-ef  # If not installed
    dotnet ef migrations add AddUserRole
    dotnet ef database update
    ```

## Milestone 2: Configuration in Program.cs
**Goal:** Enable Authentication and Authorization services in the application pipeline.

- [x] **Add Authentication Service**
  - Open `Program.cs`.
  - Add the following code **before** `builder.Build()`:
    ```csharp
    using Microsoft.AspNetCore.Authentication.Cookies; // Add this namespace

    // ... existing code ...

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });
    ```

- [x] **Enable Middleware**
  - Ensure the middleware is called in the correct order **after** `app.Build()` and **before** `app.MapControllerRoute...`:
    ```csharp
    app.UseAuthentication(); // Add this line
    app.UseAuthorization();  // This should already be there
    ```

## Milestone 3: Create ViewModels
**Goal:** Create separate models for Login and Registration forms to keep the domain model clean.

- [x] **Create `ViewModels` Folder**
  - Create a new folder named `ViewModels` inside the project.

- [x] **Create `RegisterViewModel.cs`**
  ```csharp
  namespace skillSewa.ViewModels;
  
  public class RegisterViewModel
  {
      public string Name { get; set; }
      public string Email { get; set; }
      public string Password { get; set; }
      public string ConfirmPassword { get; set; }
  }
  ```

- [x] **Create `LoginViewModel.cs`**
  ```csharp
  namespace skillSewa.ViewModels;
  
  public class LoginViewModel
  {
      public string Email { get; set; }
      public string Password { get; set; }
  }
  ```

## Milestone 4: Implement Account Controller
**Goal:** Handle Login, Register, and Logout logic.

- [x] **Create `AccountController`**
  - Create a new controller `Controllers/AccountController.cs`.

- [x] **Implement Register Action**
  - **GET**: Return the Register view.
  - **POST**: 
    1. Check if email already exists.
    2. Hash the password using `BCrypt.Net.BCrypt.HashPassword(model.Password)`.
    3. Create a new `User` object with the hashed password and default role.
    4. Save to database.
    5. Redirect to Login.

- [x] **Implement Login Action**
  - **GET**: Return the Login view.
  - **POST**:
    1. Find user by email.
    2. Verify password using `BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash)`.
    3. Create `ClaimsPrincipal` with claims (Name, Email, Role).
    4. Call `HttpContext.SignInAsync(...)`.
    5. Redirect to Home.

- [x] **Implement Logout Action**
  - Call `HttpContext.SignOutAsync()`.
  - Redirect to Home or Login.

## Milestone 5: Create Views
**Goal:** Create the user interface for authentication.

- [ ] **Create Views Folder**
  - Create `Views/Account` folder.

- [ ] **Create `Register.cshtml`**
  - Create a form that posts to `Account/Register` with Name, Email, Password, ConfirmPassword fields.

- [ ] **Create `Login.cshtml`**
  - Create a form that posts to `Account/Login` with Email and Password fields.

- [ ] **Update `_Layout.cshtml`**
  - Add logic to show "Login/Register" links if user is not authenticated (`!User.Identity.IsAuthenticated`).
  - Show "Logout" and "Hello [Name]" if user is authenticated.

## Milestone 6: Authorization & Protection
**Goal:** Protect specific pages based on login status and roles.

- [ ] **Protect Controllers**
  - Add `[Authorize]` attribute to controllers/actions that require login (e.g., `SkillsController`).

- [ ] **Role-Based Protection**
  - Add `[Authorize(Roles = "Admin")]` to actions that only admins should access (e.g., `UserController` which manages users).

- [ ] **Handle Access Denied**
  - Create an `AccessDenied` view in `Views/Account` to show a friendly error message when a user tries to access an unauthorized page.
