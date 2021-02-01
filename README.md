## Overview

The following REST API is the server-side of an application which purpose is providing shopping assistance. There are two types of users: clients and store administrators. Besides account management, this app helps store administrators to manage their stores and provides store suggestions for clients, based on their shopping list content, geographical coordinates, straight line distance (km) and date-time. An MS SQL server database was employed for data storage.

### Http Requests

1. Create new account: Post
>Stores the user in the database (if valid) and sends an account confirmation email.
```
http://localhost:4000/users/register
Request body: 
{
    "email" : "...",
    "password" :   "...",
    "username" : "...",
    "type" : "..."
}
Response: Ok / Bad Request
```
2. User authentication: Post
```
http://localhost:4000/users/authenticate/client
Request body:
{
    "email" : "...",
    "password" :   "..."
}

Response:
{
   “id” : “…”,
   “username”: “…”,
   “token”: “…”
}

```
3. Forgot password: Post
>Sends an email with a new random password and updates the old password with the new one in the database.
```
http://localhost:4000/users/forgotpass
Request body:
{
“email”: ”...”
}
Response:
Ok / Bad Request
```
4. Add new store: Post
>A store manager can add a new store and its schedule
```
http://localhost:4000/users/shops
Request body:
{
“id_user”: “…”
“address”: “…”,
“geographic_coordinates”: “…”,
“name”: “…”,
“monday”: “…”,
“tuesday”: “…”,
“wednesday”: “…”,
“thursday”: “…”,
“friday”: “…”,
“saturday”: “…”,
“sunday”: “…”
}
Response: Ok/ Bad Request
```
5. Update store information: Put
```
http://localhost:4000/users/shops
Request body:
{
“id_store”: “…”,
“name”: “…”,
“monday”: “…”,
“tuesday”: “…”,
“wednesday”: “…”,
“thursday”: “…”,
“friday”: “…”,
“saturday”: “…”,
“sunday”: “…”,
}

Response: Ok / Bad Request
```
5. Add products for a given store: Post
```
http://localhost:4000/users/products
Request body:
{
 ”id_store”: ”...”,
 ”producer”: ”...”,
 ”weight”: ”...”,
 ”name”: ”...”,
 ”description”: ”...”,
 ”category”: ”...”,
 ”bulk_product”: ”...”,
 ”price”: ”...”,
 ”availability”: ”...”
}
Response: Ok / Bad Request
```
6. Update product price and availability: Put
```
Request body:
{
 ”id_store”: ”...”,
 ”id_product”: ”...”,
 ”price”: ”...”,
 ”availability”: ”...”
}

Response: Ok / Bad Request
```
7. Get all stores managed by an admin: Get
```
Request body:
Response:
  [
    {
       ”id”: ”...”
       “address”: “…”, “geographic_coordinates”: “…”,
“name”: “…”,
    },
    .....
  ]
```
8. Get all products from a store: Get
```
http://localhost:4000/users/shops/{id}
Request body: -
Response:
[...]

```
9. Filter product name by a given string: Post
```
http://localhost:4000/filters/products/name
Request body:
{
    "name" : "..."
}

Response:
  [
        {
            "name": “…”
        },
        ..........
  ]

```
10. Filter product description by a given name: Post
```
http://localhost:4000/filters/products/description
Request body:
{
    "name" : "..."
}
Response:
  [
        {
            "description": “…”
        },
        ..........
   ]

```
11. Recommend store for a given shopping list: Post
```
http://localhost:4000/filters/stores
Request body:
{
    "UserCoordinates": "Latitude,Longitude",
"Radius": "...",
"UserDateTime": "....",
    "ShoppingListContent": [
        {
            "Name" : "...",
            "Bulk_product" : "true/false",
            "Quantity" : "..."              
        },
        ................
    ]
}

Response:
[
        {
            "storename": "...", 
            "storeaddress": "...",
            "price": ...                                                                      
        },
        ............
 ]
```

### Remarks
> As a source of inspiration for the user management functionalities (registration, authentication, password update) I have used https://jasonwatmore.com/post/2019/10/14/aspnet-core-3-simple-api-for-authentication-registration-and-user-management.
