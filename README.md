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
2. Open the application from task two and deploy the database see;
3. Run the application and login under eventManager;
4. Go to tab Event manager area;
5. Press the "import Json" button;
6. Upload the received JSON file and create an event/events. 


# Description of the template:
Requirements:
1. Create a new ASP.NET Core WebAPI project that will expose service contacts for the following operations: - Done
• Event Management - Done
• Add documentation for API (swagger) Done
2. Create a new ASP.Net Core WebAPI project. This service should be close to RESTful API. - Done
3. Almost done
4. As a requirement you should create or amend existing README.md with details on: -  Not yet completed.
5. All unit, integration should be able to run and should be all passed. - Done
6. All components should have logging in place. - Not yet completed.
7. Not yet completed.