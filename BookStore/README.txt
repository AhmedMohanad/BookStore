 Online Book Store

Book Store is a web application that allows users to browse, search, and purchase books online
. It includes features such as user authentication,
shopping cart management, and order history tracking. 
The system is designed to simulate a basic e-commerce platform
focused on selling books.

# Features:
- JWT Authentication
- Request security
- Sysloger
- Validators
- Relational Database

#  Tech Stack
- Backend: ASP.NET Core
- Authentication: JWT
- Database: SQL Server 

=========================================================
How to use :

 0- **** you need to change the connection string first ****

 1- register (to be autherized):

  a- user this url : POST https://localhost:7082/api/user/register 

  b- in body write this JSON :

 {
    "username": "test",
    "email": "test@gmail.com",
    "password": "12345678"
 }

  c- you are now ready to use any action 

  d- Note: your port maybe different 

=================================================================

Installation:

bash
git clone https://github.com/AhmedMohanad/BookStore.git
cd BookStore

=================================================================




