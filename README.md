
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
## Usage

### 1. Clone the Repository

First, clone the project from GitHub to your local machine:

```bash
git clone https://github.com/EnesDemirtas/eshop.git
cd eshop
```

### 2. Install Dependencies

Ensure that you have all necessary dependencies installed. If you're using Visual Studio or Visual Studio Code, these dependencies will be managed automatically.

```bash
dotnet restore
```

### 3. Database Setup

The project uses **SQLite** for the database, and migrations have been configured using **Entity Framework Core**.

You will need to configure the connection string in `appsettings.json` file:

```json
ConnectionStrings": {
    // "Default": "Data Source=/home/your-user/databases/eshop.db"
    "Default": "Data Source=C:\\sqlite\\eshop.db"
}
```

Run the following commands to apply migrations and create the database:

```bash
dotnet ef database update
```

### 4. Configure Stripe API

You will need to configure the Stripe API for payment functionality. Add your Stripe keys to the `appsettings.json` file:

```json
"Stripe": {
    "SecretKey": "Stripe Secret Key",
    "PublishableKey": "Stripe Publishable Key"
}
```

### 5. Configure Facebook App

You will need to configure the Facebook App for Social Login functionality. Add your Facebook App credentials to the `appsettings.json` file:

```json
"Facebook": {
    "AppId": "Facebook App Id",
    "AppSecret": "Facebook App Secret"
}
```


### 6. Run the Application

To run the application, use the following command in the project root directory:

```bash
dotnet run
```

### 7. Testing Stripe Payments

To test payments via Stripe, you can use the following test credit card details:

- Card Number: `4242 4242 4242 4242`
- Expiration Date: Any future date
- CVC: Any 3-digit number

Stripe will handle these details in test mode and simulate successful payments.
