[Back to README](../README.md)

### Sales

#### GET /sales/{id}
- **Description**: Retrieve a specific sale by ID.
- **Path Parameters**:
  - `id`: Sale ID.
- **Response**:
  ```json
  {
    "id": "string (guid)",
    "date": "string (date)",
    "totalAmount": "decimal",
    "items": [
      {
        "productId": "integer",
        "quantity": "integer",
        "price": "decimal"
      }
    ]
  }
  ```

#### GET /sales
- **Description**: Retrieve a list of sales with optional filters.
- **Query Parameters**:
  - `_page` (optional): Page number for pagination (default: 1).
  - `_size` (optional): Number of items per page (default: 10).
  - `_order` (optional): Ordering of results (e.g., "id desc, date asc").
- **Response**:
  ```json
  {
    "data": [
      {
        "id": "string (guid)",
        "date": "string (date)",
        "totalAmount": "decimal",
        "items": [
          {
            "productId": "integer",
            "quantity": "integer",
            "price": "decimal"
          }
        ]
      }
    ],
    "totalItems": "integer",
    "currentPage": "integer",
    "totalPages": "integer"
  }
  ```

#### POST /sales
- **Description**: Create a new sale.
- **Request Body**:
  ```json
  {
    "date": "string (date)",
    "totalAmount": "decimal",
    "items": [
      {
        "productId": "integer",
        "quantity": "integer",
        "price": "decimal"
      }
    ]
  }
  ```
- **Response**:
  ```json
  {
    "id": "string (guid)",
    "date": "string (date)",
    "totalAmount": "decimal",
    "items": [
      {
        "productId": "integer",
        "quantity": "integer",
        "price": "decimal"
      }
    ]
  }
  ```

#### PUT /sales/{id}
- **Description**: Update an existing sale.
- **Path Parameters**:
  - `id`: Sale ID.
- **Request Body**:
  ```json
  {
    "date": "string (date)",
    "totalAmount": "decimal",
    "items": [
      {
        "productId": "integer",
        "quantity": "integer",
        "price": "decimal"
      }
    ]
  }
  ```
- **Response**:
  ```json
  {
    "id": "string (guid)",
    "date": "string (date)",
    "totalAmount": "decimal",
    "items": [
      {
        "productId": "integer",
        "quantity": "integer",
        "price": "decimal"
      }
    ]
  }
  ```

#### DELETE /sales/{id}
- **Description**: Delete a specific sale.
- **Path Parameters**:
  - `id`: Sale ID.
- **Response**:
  ```json
  {
    "message": "string"
  }
  ```

#### POST /sales/{saleId}/item
- **Description**: Add a new item to an existing sale.
- **Path Parameters**:
  - `saleId`: The ID of the sale to which the item should be added.
- **Request Body**:
  ```json
  {
    "productId": "integer",
    "quantity": "integer",
    "price": "decimal"
  }
  ```
- **Response**:
  ```json
  {
    "id": "string (guid)",
    "saleId": "string (guid)",
    "productId": "integer",
    "quantity": "integer",
    "price": "decimal"
  }
  ```

#### PUT /sales/item/{id}
- **Description**: Update a sale item.
- **Path Parameters**:
  - `id`: Sale item ID.
- **Request Body**:
  ```json
  {
    "productId": "integer",
    "quantity": "integer",
    "price": "decimal"
  }
  ```
- **Response**:
  ```json
  {
    "id": "string (guid)",
    "saleId": "string (guid)",
    "productId": "integer",
    "quantity": "integer",
    "price": "decimal"
  }
  ```

#### DELETE /sales/item/{id}
- **Description**: Delete a sale item.
- **Path Parameters**:
  - `id`: Sale item ID.
- **Response**:
  ```json
  {
    "message": "string"
  }
  ```
