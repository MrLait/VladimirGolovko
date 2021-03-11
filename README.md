# Task 1
To run tests, you need to follow these steps:

1) Open the TicketManagement.sln using Visual Studio 2019 Community.
2) If you need to run integration tests, you need to change change the _serverName constant in the  file:
\test\TicketManagement.IntegrationTests\DatabaseConnectionBase.cs

If you don't know your server name to find out this you need to open: 
SQL Server Object Explorer => Add sql server => local;
In the locale section will present a list of all available server names. Сopy the server name and paste it into _serverName constant.

3) Run tests.
