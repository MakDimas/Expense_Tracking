Веб-додаток "Expense Tracking" має на меті зменшити та впорядкувати витрати користувача. 
Програма використовує шаблон "Identity" для того, щоб кожен користувач мав свій акаунт.
Витрати, додані користувачем, додаються у базу даних MS SQL Server з допомогою EF Core. Кожна витрата зберігається за певним користувачем.
Для того, щоб створити акаунт, потрібно підтвердити пошту. Ця можливість є завдяки реалізованому інтерфейсу "IEmailSender" у класі "EmailSender".
Для підтвердження пошти я використовую API "Send Grid". Відповідні пакети підключені.
Також, змінені деякі класи шаблону "Identity", що і дає змогу мати кожному користувачу ім'я, прізвище та добовий ліміт витрат.

_________________________________________________________________________________________________________________________________________________

The "Expense Tracking" web application is designed to reduce and organise user expenses. 
The application uses the "Identity" pattern to ensure that each user has their own account.
The expenses added by the user are added to the MS SQL Server database using EF Core. Each expense is stored for a specific user.
To create an account, you need to confirm your email. This is possible thanks to the implemented interface "IEmailSender" in the class "EmailSender".
I use the "Send Grid" API to confirm the email. The corresponding packages are connected.
Also, some classes of the "Identity" template have been changed, which allows each user to have a first name, last name and daily spending limit.
