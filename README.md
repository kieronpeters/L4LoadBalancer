# L4LoadBalancer

Overview of Architecture

This project works with a LoadBalancer project that is an API project, that will be called by the user and any calls to the underlying servers is denied as they will not know
the header value required to connect to any instance directly, the load balancer will know this secret only.

Then the server instances are created from the SimpleBackendServer example that is provided here as well, first you spin these up, which are also API projects but have an auth check that will return forbidden without the expected request header being present. Then this will store the amount of connections to itself and a status to report its current count of connections and the port number that it is running on.

Basically this architecture, is 3 or more servers that are fronted by an API load balancer operating at Level 4 of the OSI model. So the routing is using connection termination and not passthrough, as we modify and provide the necessary auth header to ensure that the requests to the servers is accepted without ever exposing this to the clients. The aim of this is to evenly rotate in simple round-robin fashion through all available and healthy servers. This also will remove and add servers with 5 second periodic checks in a separate thread process to ensure that the servers are added or removed as they report healthy or not.


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

