AUTHOR - Gbubemi Smith Walter

Application was built using clean architecture, SOLID principles, repository pattern,
EF core, Automapper, Ef Migrations, MySql, unit of work pattern(UoW), google api,
JWT token for authorization, Identity Manager, docker and swagger for API documentation

DEPENDENCIES/PREREQUISITIES

* .net 5.0
* MySql(if you do not run with docker compose file)
* Google api key
* ensure port 3306 and 8000 are available

SETUP

* Go to appsettings.json and input Google API key in the Api section

* Project has been dockerised and image can be created with the command :
    - docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

* Once the docker image is up and running, browse the url http://localhost:8000/swagger 

* if you do not want to go through the docker route then, ensure you have MySql running locally 
and configure the connection string in the appsettings.json accordingly with the user and password
 - Run the command dotnet watch run and browse the url https://localhost:5001/swagger

HOW THE APPLICATION WORKS

* We have four major entities Users(Idntity tables are created for user and roles), Vehicles, Devices and Locations. 
* The VehicleTrackingApi sloutin has 3 projects which follow clean architecture
  - API : The layer contains the api implementations
  - Core : contains DTOs and entities
  - Infrastructure : contains all the business logic and EF core implementations

Assumptions:

* A user owns vehicles and each vehicle has a teacking device in it 
* An admin user manages the back office and has the rights to view current position of a vehicle and positions over a given period.
* For a vehicle to be registered the user has to pass the JWT token gotten from registration or logging in
(sample payload can be found in the RegisterVehicle action in the VehicleController)
* A device can register a vehicles position by making a post with the deviceId, longitude and latitude position to the endpoint api/v1/vehicle/{userid}/RecordPosition
* Only authorized admins can view the current position or positions of a vehicle 


Thank you.



