# Modular-Commerce 🚀

A comprehensive and scalable e-commerce backend built with **.NET** and **C#**, utilizing a **Modular Monolith** architecture. This project is heavily focused on structural integrity, applying **Clean Architecture** principles and **CQRS** to ensure high maintainability and loose coupling between distinct business capabilities.

## 🛠️ Tech Stack & Technologies
* **Language & Framework:** C#, ASP.NET Core
* **Architecture:** Clean Architecture, Modular Monolith
* **Patterns:** CQRS (Command Query Responsibility Segregation)
* **Data Access & ORM:** Entity Framework Core (EF Core) with PostgreSQL
* **Security:** Role-Based Authorization, Cookie Authentication

## 🏗️ System Architecture

The solution is divided into autonomous business modules. Each module encapsulates its own Domain, Application, and Infrastructure layers, adhering to the dependency inversion principle.

### Core Modules

| Module | Core Responsibility |
| :--- | :--- |
| **AccountManagement** | User identity, registration, authentication, and role management. |
| **ShopManagement** | Core catalog handling (products, categories, brands, pictures). |
| **InventoryManagement** | Stock control, inventory tracking, and warehouse operations. |
| **DiscountManagement** | Pricing policies, discount definitions, and application. |
| **BlogManagement** | Content management for the marketing and blogging layer. |
| **CommentManagement** | Handling and moderation of user reviews and comments. |

### Cross-Cutting & Infrastructure

* **`0_Framework`**: Contains shared building blocks, utilities, and base classes used across all modules.
* **`01_ShopQuery`**: Implements the Query side (read model) handling read operations efficiently as dictated by the CQRS pattern.
* **`ServiceHost`**: The Composition Root and entry point of the application, responsible for Dependency Injection (DI) wiring and API hosting.

## ⚙️ Module Inter-Communication

To prevent tight coupling between distinct domains, the modules do not reference each other's persistence layers directly. Instead, they communicate using an **Access Control Layer (ACL)** approach (e.g., `AccountAcl`, `InventoryAcl`) and rely on shared contracts (`Application.Contracts`), ensuring that changes in one module's internal logic do not break the rest of the system.
