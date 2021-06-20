# SolutionTemplate

To deploy the database, you should follow these steps:
1. Publish TicketManagement.Database;
2. Change the connectionstrings in:
	VladimirGolovko\src\EventFlow.API\appsettings.json; 
	VladimirGolovko\src\Identity.API\appsettings.json.

To run tests, you should follow these steps:
1. Open VladimirGolovko\TicketManagement.sln;
2. You don't need to change connection string in VladimirGolovko\test\TicketManagement.IntegrationTests\appsettings.json..
2. Run tests.

To import the "JSON" file in task two, you need to perform the following steps:
1. Get a JSON file from task three by clicking on the download button or detail, save the received data to a json file;
2. Open the application from task two and deploy the database see # Task 2;
3. Run the application and login under eventManager;
4. Go to tab Event manager area;
5. Press the "import Json" button;
6. Upload the received JSON file and create an event/events. 