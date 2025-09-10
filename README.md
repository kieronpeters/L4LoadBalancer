# L4LoadBalancer

Instructions how to run programme:

Open up three console application in this directory and navigate to /instances directory
Run the command in each terminal of
"dotnet run --urls=http://localhost:9001"
"dotnet run --urls=http://localhost:9002"
"dotnet run --urls=http://localhost:9003"

This will spin up 3 instances on the given ports, you may spin up more if you wish here

Then in the config file, speicify the URL connections by specifying a comma-separated list of port numbers, to specify the URLs to be the server instances in the application

This will be in the main repo entry point inside the .env file located here:
SERVER_INSTANCES=9001,9002,9003

Then to spin up this main instance, open and console app in this directory at the base of the repo and then use the console command:
"dotnet run --urls=http://localhost:1000"

This will allow you to then run the postman URLs

/admin/status -> will return the state of the servers connected and will list a status overview of the server name, total calls made to it as a list of servers to give overview

/admin/connect -> will return the name of the server instance running and the total count of calls made to this server since it was created
This is pdated and incremented in each call made to it, including this current call



