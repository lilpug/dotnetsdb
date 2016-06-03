## dotnetsdb
Dotnetsdb is a C#.NET database library which sits on top of the default functionality of both the .NET SQL Server and MySql adapters. Its intension is to bring a more modular and dynamic approach to making database calls in .NET. 


## Documentation

There is currently no documentation for this library, but a website will be setup shortly to explain how to intereact with the library.


## Extra Credits
This library uses the MySql.data.dll connector in order to create the flexibility with MySql database calls. The MySql connector is available from http://dev.mysql.com/downloads/connector/net/6.8.6.html, it is used under the FOSS exception Licenses, please see the MySql notice for further information.


This library also uses the Microsoft.SqlServer.Types.dll to help make queries much quicker on Sql Server by defining the correct parameter types on binding and running a query.

Please Note: The project uses a small application i built on top of Microsoft's ILMerge in ordered to merge all the required DLL files into a single file on compiling.


## License
This project is licensed under the MIT License. see license file for more information.

