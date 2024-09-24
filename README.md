
# eShop

Eshop is an e-commerce web application built using ASP.NET Core MVC and .NET 8. The project provides users with an intuitive shopping experience, including product browsing, cart management, and an integrated payment system using the Stripe API. It also incorporates a robust admin interface for managing products, users, and orders.


## Tech Stack

**Client:** ASP.NET Core MVC with Razor Pages, Bootstrap

**Server:** ASP.NET Core MVC, .NET 8, EF Core, SQLite, Stripe API, ASP.NET Core Identity


## Features

### General Features

- User Authentication & Authorization: Users can register, log in, and manage their profiles using **ASP.NET Core Identity**.
- Social Login Integration: Users can register and log in using their Facebook accounts.
- Shopping Cart: Session-based cart management allowing users to add, remove, and update items.
- Orders: Complete order lifecycle management with CRUD operations, summary, and checkout via **Stripe**.
- Multi-Image Product Support: Products can have multiple images with a carousel display.
- Admin Interface: Admins can manage products, users, roles, and categories through a secure interface.

### Admin Features

- Manage Products: Add, update (upsert), delete, and display products with multi-image support.
- User Management: Admins can manage users, assign roles, and lock/unlock accounts.
- Category Management: CRUD operations for categories.
- Company Management: CRUD operations for companies.
## Project Structure

The project is divided into different areas to separate customer and admin functionalities:

- Customer Area: The main shopping interface for regular users.
- Admin Area: An admin dashboard to manage products, categories, users, and more.
