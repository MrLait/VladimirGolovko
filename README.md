# SolutionTemplate

## To deploy the database, you should follow these steps:
1. Open VladimirGolovko\TicketManagement.sln
1. Publish EventFlow.Database and Identity.Database;
2. Change the connectionstrings in:
	VladimirGolovko\src\EventFlow.API\appsettings.json; 
	VladimirGolovko\src\Identity.API\appsettings.json.

## To run the whole solution, with all services necessary to correct application work.
Run bat file \VladimirGolovko\scripts\RunTicketManagementUI.bat

Url addresses:
TicketManagement.WebMVC : "https://localhost:5001",
Identity.API : "https://localhost:44370",
EventFlow.API :"https://localhost:44300"

## To run the whole solution in debug mode, you need to configure the project to run in <Multiple startup Projects> mode.
You should follow these steps
1. Choose - Debug or right click on the solution template => Set sturtup projects => Common Proporties => Startup Project => Set the flag to Multiple startup Projects:
2. For projects: Identity.Api, EventFlow.Api and TicketManagement.WebMVC - set actions to Start
3. Click start.

Url addresses:
TicketManagement.WebMVC : "https://localhost:5001",
Identity.API : "https://localhost:5004",
EventFlow.API :"https://localhost:5003"

## To run tests, you should follow these steps:
1. Open VladimirGolovko\TicketManagement.sln;
2. You don't need to change connection string in VladimirGolovko\test\TicketManagement.IntegrationTests\appsettings.json..
2. Run tests.
