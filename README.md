This is a .NET project that use both MongoDB and SQL Server. Program gets "ClassNames" and "Namespaces" from MongoDB Database and by using 'System.Reflection' creates a new table in MongoDB and inserts properties of each class.
After that, it connects to MS SQL Server, creates multiple tables that use 'One-To-Many' relationships and database normalization when storing methods of each class, return type and parameters of each method and their names.
When MongoDB is locally installed, SQL Server is running as Docker Container.
This project is developed on internship in Digiturk for the purpose of learning basics of MongoDB Database, MSSQL Database, .NET framework, EntityFramework and Docker. 
