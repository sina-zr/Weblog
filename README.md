# Weblog And Chat

# 📰 Weblog Realtime Chat & News Reader (ASP.NET Core 8.0 Project)

This project is a **simple weblog system** featuring a **realtime group chat system**, a **news reader powered by NewsAPI**, and a basic **user management** flow with admin-only access. It is developed using **ASP.NET Core 8.0**, **Entity Framework Core**, and **SQL Server 2022**.

The project is designed as a single-user-admin system suitable for small-scale applications or as a starter template for learning **SignalR**, **authentication**, and **clean Razor-based MVC web apps**.

#### Video Demo: https://youtu.be/MdqlyTlDK_E

---

## 🚀 Features

- 🗞️ **Fetch latest news headlines** from [NewsAPI.org](https://newsapi.org)
- 💬 **Realtime group chat** between users and admin using **SignalR**
- 🔐 **Admin-only access control** with custom authorization attribute
- 🧑‍💻 User authentication and simple role management
- 🎨 Styled UI with Bootstrap and Razor Layouts
- ⚙️ Automatic EF Core migration on startup — no need for manual database setup!

---

## 🛠️ Technologies Used

| Layer          | Tools / Frameworks                                         |
| -------------- | ---------------------------------------------------------- |
| Backend        | ASP.NET Core 8.0, SignalR, Entity Framework Core           |
| Database       | SQL Server 2022                                            |
| Frontend       | Razor Pages, Bootstrap 5, Static HTML template integration |
| Realtime       | SignalR                                                    |
| News API       | [NewsAPI.org](https://newsapi.org)                         |
| Authentication | ASP.NET Identity (customized)                              |
| Others         | Middleware, Filters, DTOs, Razor Layout System             |

---

## 🧪 Requirements

Before running the project, ensure you have the following installed:

- [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download)
- [SQL Server 2022 or LocalDB](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or VS Code with C# extension
- Internet connection (for chat and news features)

---

## ▶️ How to Run the Project

1. **Clone the repository**
2. **Make sure .NET 8 SDK & SQL Server 2022 is installed**
3. **Set your desired connection string to appsettings.json**
4. **Run the project**

