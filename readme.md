# ProductsApp API

An application that demonstrated:

- User login with JWT token issuance
- product querying via dynamic filters
- Health check endpoint
- EF Core + SQLite + JWT authentication
- XUnit tests
---

## How To Test

## Authentication approaches

1. Use one of the pre-seeded usernames to obtain a valid JWT:

- `alice`
- `bob`
- `charlie`

![Login](./login.png )

2. Register a new user using the api/Users/register endpoint


After obtaining a JWT please click on the authorize button
and paste the token inside the dialog (Do not prefix with Bearer!)


![btn](./btn.png )
![Dialog](./open%20dialog.png )


## Products

After the tokens are set you will have access to the products endpoints.

1. The api/products GET endpoint returns all products existing in the database.
2. The api/products/query GET endpoint returns by the criteria specified in the query params
3. The api/products/{id} GET endpoint returns a product by id or returns a not found response
4. The api/products POST endpoint creates a product (created by will be automatically assigned to the user the JWT is issued for)
5. The api/products/{id} PUT endpoint updates a product by id (only if the user is the one who created the product)
6. The api/products/{id} DELETE endpoint deletes a product by id (only if the user is the one who created the product)

## Ecommerce system diagram

![ecommerce](./ecommerce%20system%20diagram.png)