# SolutionTemplate

## To deploy the database, you should follow these steps:
1. Open VladimirGolovko\TicketManagement.sln
1. Publish TicketManagement.Database;
2. Change the connectionstrings in:
	VladimirGolovko\src\EventFlow.API\appsettings.json; 
	VladimirGolovko\src\Identity.API\appsettings.json.

## To run react app 
Run inthe VladimirGolovko\src\ReactJS\ticket-management\ directory, you can run:
### `npm start`

Runs the app in the development mode.
Open [http://localhost:3000](http://localhost:3000) to view it in the browser.

## To run the whole solution, with all services necessary to correct application work.
Run bat file \VladimirGolovko\scripts\RunTicketManagementUI.bat

## Configuring the redirect flag from TicketManagement.WebMVC to React application
1. Open \VladimirGolovko\src\TicketManagement.WebMVC\appsettings.json file 
2. Сhange the "RedirectToReactApp" attribute from false to true.
For example:
"RedirectToReactApp": false - redirection is disabled.
"RedirectToReactApp": true - redirection is enabled.

Url addresses:
TicketManagement.WebMVC : "https://localhost:5001",
Identity.API : "https://localhost:5004",
EventFlow.API :"https://localhost:5003"
ReactJS/ticket-management :http://localhost:3000

## To run the whole solution in debug mode, you need to configure the project to run in <Multiple startup Projects> mode.
You should follow these steps
1. Choose - Debug or right click on the solution template => Set sturtup projects => Common Proporties => Startup Project => Set the flag to Multiple startup Projects:
2. For projects: Identity.Api, EventFlow.Api and TicketManagement.WebMVC - set actions to Start
3. Click start.

Url addresses:
TicketManagement.WebMVC : "https://localhost:5001",
Identity.API : "https://localhost:44370",
EventFlow.API :"https://localhost:44300",
ReactJS/ticket-management :http://localhost:3000

## To run tests, you should follow these steps:
1. Open VladimirGolovko\TicketManagement.sln;
2. You don't need to change connection string in VladimirGolovko\test\TicketManagement.IntegrationTests\appsettings.json..
2. Run tests.
