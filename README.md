# DawgShop – ASP.NET Core E-Commerce Application

DawgShop is a modern, responsive e-commerce web application built with **ASP.NET Core MVC**, **Entity Framework Core**, and **Bootstrap 5**.  
It supports user authentication, product browsing, cart management, checkout, and order tracking with a polished UI/UX.

---

##  Features

###  User & Authentication
- User registration and login
- Session-based authentication
- Profile management (view & edit basic details)
- Secure logout

###  Products
- Dynamic product listing
- Search, filter, and sort products
- Featured products on home page
- Smooth animations and modern UI cards

### Cart
- Add products to cart (AJAX)
- Slide-in cart drawer
- Update quantities in real time
- Remove items from cart
- Cart persisted per logged-in user
- Login validation before checkout

###  Checkout & Orders
- Shipping information form
- Order summary before placing order
- Place order and clear cart
- Order success confirmation
- View all orders
- View order details (items, totals, shipping info)

### UI / UX
- Bootstrap 5 + custom styling
- Toast notifications (success, error, info)
- Animated transitions (Animate.css / AOS)
- Responsive layout (mobile & desktop)
- Sticky cart drawer & summary panels

---

## Tech Stack

- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server**
- **Bootstrap 5**
- **jQuery & Fetch API**
- **Animate.css / AOS**
- **Session-based authentication**

---

---

## ▶️ Demo Video

🎥 **Watch the full demo here:**  
 **[Google Drive Demo Video](https://drive.google.com/file/d/1bHdow2lqYDmFasLc6lrubzf3GHqZ4TmI/view?usp=sharing)**
  **[Google Drive Demo Video](https://drive.google.com/file/d/1e5z-LPuD-pOFCYbbzOn04IGcy9A9B4QV/view?usp=sharing)**

> The video demonstrates:
> - User  login  
> - Browsing products  
> - Adding items to cart  
> - Cart drawer interactions  
> - Checkout & placing orders  
> - Viewing orders & profile page  

---

## Setup Instructions

### Clone the repository
```bash
git clone https://github.com/DouglasHutchful1/EcommerceWebAPP.git
cd EcommerceWebAPP

2) Update database connection

Edit appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=DawgShopDb;Trusted_Connection=True;"
}

3) Run migrations
dotnet ef database update

4) Run the app
dotnet run


Visit:

http://localhost:5155

 Notes

Checkout and cart actions require login

Non-logged-in users are prompted with login modal + toast

Passwords are securely hashed

Session is used for authentication (can be upgraded to Identity/JWT later)

🧩 Future Improvements

Change password & email

Admin dashboard

Product categories table

Payment gateway integration

Wishlist & reviews

Email notifications

📄License

This project is for educational and portfolio purposes.

Author

Douglas Hutchful

⭐ If you like this project, feel free to star the repository!


