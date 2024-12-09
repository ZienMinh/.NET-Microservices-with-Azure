var orders = [
    {
        "_id": "eb40a270-1f44-48bc-861e-a3529c09ae6e",
        "OrderID": "eb40a270-1f44-48bc-861e-a3529c09ae6e",
        "UserID": "eb40a272-1f44-48bc-861e-a3529c09ae70",
        "OrderDate": "2024-12-06T14:30:00Z",
        "TotalBill": 2000.00,
        "OrderItems": [
            {
                "_id": "eb40a273-1f44-48bc-861e-a3529c09ae71",
                "ProductID": "eb40a273-1f44-48bc-861e-a3529c09ae71",
                "UnitPrice": 500.00,
                "Quantity": 3,
                "TotalPrice": 1500.00
            },
            {
                "_id": "eb40a275-1f44-48bc-861e-a3529c09ae73",
                "ProductID": "eb40a275-1f44-48bc-861e-a3529c09ae73",
                "UnitPrice": 250.00,
                "Quantity": 2,
                "TotalPrice": 500.00
            }
        ]
    },
    {
        "_id": "eb40a277-1f44-48bc-861e-a3529c09ae75",
        "OrderID": "eb40a277-1f44-48bc-861e-a3529c09ae75",
        "UserID": "eb40a279-1f44-48bc-861e-a3529c09ae77",
        "OrderDate": "2024-12-05T16:00:00Z",
        "TotalBill": 1200.00,
        "OrderItems": [
            {
                "_id": "eb40a27b-1f44-48bc-861e-a3529c09ae79",
                "ProductID": "eb40a27b-1f44-48bc-861e-a3529c09ae79",
                "UnitPrice": 600.00,
                "Quantity": 1,
                "TotalPrice": 600.00
            },
            {
                "_id": "eb40a27c-1f44-48bc-861e-a3529c09ae7a",
                "ProductID": "eb40a27c-1f44-48bc-861e-a3529c09ae7a",
                "UnitPrice": 300.00,
                "Quantity": 2,
                "TotalPrice": 600.00
            }
        ]
    },
    {
        "_id": "eb40a27e-1f44-48bc-861e-a3529c09ae7c",
        "OrderID": "eb40a27e-1f44-48bc-861e-a3529c09ae7c",
        "UserID": "eb40a280-1f44-48bc-861e-a3529c09ae7e",
        "OrderDate": "2024-12-06T08:00:00Z",
        "TotalBill": 800.00,
        "OrderItems": [
            {
                "_id": "eb40a281-1f44-48bc-861e-a3529c09ae7f",
                "ProductID": "eb40a281-1f44-48bc-861e-a3529c09ae7f",
                "UnitPrice": 200.00,
                "Quantity": 4,
                "TotalPrice": 800.00
            }
        ]
    }
];

console.log(orders);

// Switch to your database
var db = db.getSiblingDB("OrdersDatabase");

db.orders.insertMany(orders);
