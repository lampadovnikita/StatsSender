# StatsSender
The project at this stage works with little information about the user and the abstract game character he/she created.  
The project is built according to the classical client-server model, where parts of the project exchange data in JSON format via the REST API:
* [The client part](https://github.com/lampadovnikita/StatsSender/tree/main/CharacterStatsSender) on Unity, which essentially interacts with the server part, that is, sends requests to the server and displays the received data.
* [The server part](https://github.com/lampadovnikita/StatsSender/tree/main/Server) that interacts with the database and receives requests from the client, sending back the result
