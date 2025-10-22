# Linh Long Coding Challenge: Simple ERP System

## 🌟 Overview
Welcome to the Linh Long coding challenge! First of all, thank you for taking the time to participate. We see this challenge as a mutual opportunity — for you to showcase your skills, and for us to explore how well we can work together.

This assignment focuses on building a simplified ERP system using modern technologies. Please treat it as a real-world task, and feel free to demonstrate your best practices in architecture, coding, and documentation.

---

## 🧩 Requirements

### 1. User Authentication & Authorization
- Users must be able to log in using a secure password.
- Password policy:
  - Minimum 12 characters
  - At least 1 uppercase letter
  - At least 1 special character
- Lockout after 5 consecutive failed login attempts.
- Implement persistent login (e.g., via refresh tokens or cookies).
- After login, display the user's assigned **Role** (e.g., Viewer, Editor).

### 2. Inventory Management
- Implement full **CRUD** operations for inventory items.
- Inventory attributes:
  - Name
  - Description
  - Color
  - Type
  - Suppliers
  - Manufacturers
- Role-based access:
  - **Viewer**: Can view inventory data only.
  - **Editor**: Can create, update, and delete inventory items.

---

## 🎯 Implementation Scope
You are required to implement **one** of the above two modules (Authentication or Inventory Management). Choose the one that best showcases your strengths.

---

## ✅ Acceptance Criteria

### 📄 Documentation (Word or PDF)
- **Architecture Diagram**: Visualize the system components (e.g., client, server, database).
- **Component Descriptions**: Briefly explain the role of each major component.
- **Data Flow Explanation**: Describe how data moves through the system for the module you implemented.

### 💻 Working System
- Tech stack:
  - **Backend**: .NET Core
  - **Frontend**: ReactJS + TypeScript
  - **Database**: MSSQL
- Code should be:
  - Well-structured and modular
  - Clearly documented
  - Easy to set up and run

---

## 📅 Submission Deadline
- **Timeframe**: 1 week from the date you receive this challenge.

Good luck, and we look forward to seeing your solution!
