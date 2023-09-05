This is a demo project using .NET 7, CQRS, Dapper, SQLite db and FluentValidation.
The essence of the demo is to upload and parse SWIFT MT799 message and record the data into a SQLite db without using a pre existing librarty.

Original task (in bulgarian):
Трябва да се изгради web api на .net 7, приемащо файл със съобщение Swift MT799.
WebApi услугата трябва да може да прочете изпратеното му съобщение и да запише полетата от съобщенията в база данни.
 Допълнителни параметри:
•	Не използвайте готова библиотека за swift съобщения.
•	Трябва да има документация на webapi-то, което да позволява тестване.
•	Без Authorization&Authentication.
•	База данни SQLite.
•	Комуникацията с SQLite да се изгради без EntityFramework
•	Логване чрез библиотека

The example upload file is example_mt799.txt
