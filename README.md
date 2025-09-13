# L4LoadBalancer

Instructions how to run programme:

Open the directory to where this file is and run the servers.bat file which runs these commands:

cd SimpleBackendServer/
start cmd /k "dotnet run --urls https://localhost:9001"
start cmd /k "dotnet run --urls https://localhost:9002"
start cmd /k "dotnet run --urls https://localhost:9003"

This will spin up 3 instances on the given ports, you may spin up more if you wish by modifying the file here

We then want to spin up the instace of the load balancer, to do this navigate from here to the directory /LoadBalancer

then run:
dotnet run 9001 9002 9003 --urls=https://localhost:1001

This will allow you to then run the postman URLs

/admin/status -> will return the state of the servers connected and will list a status overview of the server name, total calls made to it as a list of servers to give overview

/admin/connect -> will return the name of the server instance running and the total count of calls made to this server since it was created
This is pdated and incremented in each call made to it, including this current call

If you do not want to create the postman url's for the given example above, we can call the following endpoints in a new GET request in postman:
(will not work in browser, must use postman GET request with header that is needed:
X-LB-SECRET : my-secret)

load balancer:
(works with simple get request from browser as loadbalancer should know a header secret to allow access to the underlying servers only)

https://localhost:1001/admin/connect
https://localhost:1001/admin/status

server instances:
(will not work in browser, must use postman GET request with header that is needed:
X-LB-SECRET : my-secret)

https://localhost:9001/connect
https://localhost:9001/status
https://localhost:9002/connect
https://localhost:9002/status
https://localhost:9003/connect
https://localhost:9003/status

