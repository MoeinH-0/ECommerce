<h1 align="center">🛒 Modular Commerce</h1>

<p align="center">
A modular e-commerce backend built with <b>ASP.NET Core</b>, following <b>Clean Architecture</b> and <b>CQRS</b> principles.
</p>

<p align="center">
<img src="https://img.shields.io/badge/.NET-ASP.NET%20Core-512BD4?logo=dotnet&logoColor=white"/>
<img src="https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white"/>
<img src="https://img.shields.io/badge/PostgreSQL-4169E1?logo=postgresql&logoColor=white"/>
<img src="https://img.shields.io/badge/EF%20Core-68217A"/>
<img src="https://img.shields.io/badge/CQRS-success"/>
<img src="https://img.shields.io/badge/Clean%20Architecture-blue"/>
</p>

---

## 📖 Overview

**Modular Commerce** is a modular e-commerce backend developed with a strong focus on software architecture, maintainability, and separation of concerns.

The project follows a **Modular Monolith** architecture where each business domain is implemented as an independent module while remaining part of a single solution. It demonstrates how concepts such as **Clean Architecture**, **CQRS**, and **Dependency Injection** can be combined to build a scalable and maintainable backend.

---

## ✨ Features

| Feature | Status |
|---------|:------:|
| Modular Monolith Architecture | ✅ |
| Clean Architecture | ✅ |
| CQRS | ✅ |
| Entity Framework Core | ✅ |
| PostgreSQL | ✅ |
| Cookie Authentication | ✅ |
| Role-Based Authorization | ✅ |
| Dependency Injection | ✅ |
| Access Control Layer (ACL) Between Modules | ✅ |
| RESTful Web API | ✅ |

---

## 🏗 Architecture

The project follows the **Clean Architecture** pattern.

```text
Presentation
      │
Application
      │
Domain
      │
Infrastructure
      │
PostgreSQL
```

Each business module is implemented independently and communicates with other modules through dedicated interfaces, resulting in a modular and maintainable architecture.

---

## 📦 Modules

The solution consists of several independent business modules:

- Account Management
- Shop Management
- Inventory Management
- Discount Management
- Blog Management
- Comment Management

Each module contains its own layers:

```text
Application
Application.Contracts
Domain
Infrastructure
Presentation
```

Shared components are placed inside:

- 0_Framework
- ServiceHost
- ShopQuery

---

## 🛠 Technologies

- ASP.NET Core
- C#
- Entity Framework Core
- PostgreSQL
- CQRS
- Clean Architecture
- Dependency Injection
- Cookie Authentication
- Git

---

## 🚀 Getting Started

Clone the repository

```bash
git clone https://github.com/MoeinH-0/Modular-Commerce.git
```

Restore packages

```bash
dotnet restore
```

Run the project

```bash
dotnet run
```

---

## 👨‍💻 Author

**Moein Hosseini**

Computer Engineering Student — University of Isfahan

Interested in Backend Development, Software Architecture, and Problem Solving.
