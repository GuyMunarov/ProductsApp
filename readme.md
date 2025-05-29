# ProductsApp API

An application that demonstrated:

- User login with JWT token issuance
- product querying via dynamic filters
- Health check endpoint
- EF Core + SQLite + JWT authentication
- XUnit tests
---

## How To Test

## Seeded Users

To authenticate, you must use one of the pre-seeded usernames:

- `alice`
- `bob`
- `charlie`

Use any of these to obtain a valid JWT.

![Login](./login.png )

After obtaining a JWT please click on the authorize button
and paste the token inside the dialog (Do not prefix with Bearer!)


![btn](./btn.png )
![Dialog](./open%20dialog.png.png )


## Products

After the tokens are set you will have access to the products endpoints.

1. The api/products endpoint returns all products existing in the database.
2. The api/products/query returns by the criteria specified in the query params
3. the api/products/{id} gets a product by id or returns a not found response
