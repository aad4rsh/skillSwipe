# Dashboard Implementation Plan

This document outlines the steps to create a dynamic and role-based Dashboard for `skillSewa`.

## Milestone 1: Dashboard ViewModel
**Goal:** Create a model to hold all the data needed for the dashboard view.

- [x] **Create `DashboardViewModel.cs`**
  - Create a class in `ViewModels` folder.
  - Properties:
    - `TotalSkillsOffered`: Count of skills the user is teaching.
    - `TotalSkillsLearned`: Count of skills the user wants to learn.
    - `PotentialMatches`: Count of users who teach what this user wants to learn.
    - `RecentMatches`: List of recent users who match skills.
    - `IsAdmin`: Boolean to determine if admin view should be shown.
    - `TotalUsers` (Admin only): Total count of users in the system.
    - `TotalSkills` (Admin only): Total count of unique skills in the system.

## Milestone 2: Dashboard Controller Logic
**Goal:** Implement the logic to fetch dashboard data.

- [x] **Create/Update Controller**
  - Use `HomeController` or create a new `DashboardController`.
  - **Action**: `Dashboard()`
  - **Logic**:
    - Get current user's ID.
    - **For Admin**: Fetch total user count, total skill count.
    - **For User**:
      - Count `UserSkill` where `UserId == CurrentUser` and `CanTeach == true`.
      - Count `UserSkill` where `UserId == CurrentUser` and `WantToLearn == true`.
      - Find matches: Users who `CanTeach` a skill that `CurrentUser` `WantToLearn`.
    - Populate `DashboardViewModel`.
    - Return View.

## Milestone 3: Dashboard View (User & Admin)
**Goal:** Create the visual interface for the dashboard.

- [x] **Create `Dashboard.cshtml`**
  - **Layout**: Use a card-based layout (Bootstrap).
  - **User Section**:
    - Display "Skills Teaching" and "Skills Learning" counts in colorful cards.
    - Display a "Matches Found" card that links to a detailed search/match page.
    - Show a "Quick Actions" section (e.g., "Add a new skill").
  - **Admin Section** (Visible only if `Model.IsAdmin`):
    - Display "Total Users" and "Total Skills".
    - Link to "Manage Users" (`UserController`).

## Milestone 4: Navigation Update
**Goal:** Make the dashboard accessible.

- [x] **Update `_Layout.cshtml`**
  - Change the "Hello, [Name]!" text to be a link to the `Dashboard` action.
  - Ensure the "Home" link still goes to the landing page, but "Dashboard" is prominent for logged-in users.
