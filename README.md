# 🛒 Modular-Commerce

## 📖 About This Project
A fully functional e-commerce web application designed to implement advanced architectural patterns in .NET. The backend is built upon a **Modular Monolith** architecture combined with **Clean Architecture** principles, ensuring scalability, maintainability, and separation of concerns.

## 🚀 Technologies & Tools
* **Language:** C#
* **Framework:** ASP.NET Core
* **ORM:** Entity Framework Core (EF Core)
* **Database:** PostgreSQL
* **UI & Auth:** Razor Pages, Cookie-Based Authentication, Role-Based Authorization

## 🏗 Architecture & Key Concepts

* **Modular Monolith & Clean Architecture:** The system is divided into distinct business modules (e.g., Shop, Inventory, Account). Each strictly follows Clean Architecture, keeping Domain and Application layers independent of infrastructure.
* **Database Isolation:** To ensure loose coupling, each module has its own independent `DbContext` and configuration.
* **Inter-Module Communication (ACL):** Modules communicate securely via an Anti-Corruption Layer (ACL) and Application Contracts, without directly referencing each other's databases.
* **CQS (Command Query Separation):** Read and write operations are logically separated at the Application layer using dedicated class libraries, streamlining database interactions.

## 📦 Modules Overview
* 👤 **AccountManagement:** User registration, authentication, and role management.
* 🛍️ **ShopManagement:** Products, categories, brands, and product images.
* 📦 **InventoryManagement:** Stock tracking and inventory operations.
* 💰 **DiscountManagement:** Pricing strategies and discount codes.
* 📝 **BlogManagement:** Articles and content management.
* 💬 **CommentManagement:** User comments and feedback moderation.
