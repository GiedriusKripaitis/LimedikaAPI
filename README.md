# QuickStart guide

1. Open CMD in the solution folder
2. Write command `docker compose build`
3. Write command `docker compose up`

The MSSQL database and API are up.

# Users guide
There are three endpoints in this API
1. [POST] http://localhost:5000/v1/api/clients/ to import clients from the `data.json` file to the database
2. [GET] http://localhost:5000/v1/api/clients/ to get existing clients from the database
3. [POST] http://localhost:5000/v1/api/clients/post-codes-update to update post codes for all the clients in the database

You can user Swagger (accessible in http://localhost:5000/v1/api/swagger/index.html) or Postman to access those endpoints.