# poekdex
**Some of the changes i will bring for production would be adding authentication and authorisation to our api ,configure it to use https also will try to pool the restclient / httpclient so that we don't need to create the instances again**


In order to run the project first clone the repo then go to the pokedex folder 

run the following command
dotnet restore
dotnet run

this will show the localhost url to run the api

for eg http://localhost:5000

just go to browser paste the url and add/pokemon/{name of the pokemon who you want details for}

for eg
http://localhost:5000/pokemon/ditto

similarly for the translated version

http://localhost:5000/pokemon/translated/ditto


**running it using docker **
ensure you have docker installed on your system

go to pokedex folder

run command  docker build -t pokedex .

this will create the image with the name pokedex
which you check using 

docker images

then run the following command to create the dockerise app from image
docker run -d -p 8080:80 --name myapp pokedex

and go to 
http://localhost:8080/pokemon/ditto
or 
http://localhost:8080/pokemon/translated/ditto
