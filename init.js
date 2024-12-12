var orders = [
    {
        "_id": "eb40a270-1f44-48bc-861e-a3529c09ae6e",
        "OrderID": "eb40a270-1f44-48bc-861e-a3529c09ae6e",
        "UserID": "1f366ec1-339e-482b-a3be-77de20a92c4a", // John Smith
        "OrderDate": "2024-12-06T14:30:00Z",
        "OrderItems": [
            {
                "_id": "a1b2c3d4-e5f6-47ab-89cd-000000000001",
                "ProductID": "a1b2c3d4-e5f6-47ab-89cd-000000000001", // iPhone 14 Pro
                "UnitPrice": 999.99,
                "Quantity": 1,
                "TotalPrice": 999.99
            },
            {
                "_id": "b2c3d4e5-f6a7-48bc-9de0-000000000002",
                "ProductID": "b2c3d4e5-f6a7-48bc-9de0-000000000002", // Smart Coffee Maker
                "UnitPrice": 899.99,
                "Quantity": 1,
                "TotalPrice": 899.99
            }
        ],
        "TotalBill": 1899.98
    },
    {
        "_id": "eb40a277-1f44-48bc-861e-a3529c09ae75",
        "OrderID": "eb40a277-1f44-48bc-861e-a3529c09ae75",
        "UserID": "2a45bf32-448f-493c-9cb3-88de31bc35bb", // Emma Wilson
        "OrderDate": "2024-12-05T16:00:00Z",
        "OrderItems": [
            {
                "_id": "c3d4e5f6-a7b8-49cd-ef01-000000000003",
                "ProductID": "c3d4e5f6-a7b8-49cd-ef01-000000000003", // MacBook Air M2
                "UnitPrice": 1299.99,
                "Quantity": 1,
                "TotalPrice": 1299.99
            },
            {
                "_id": "d4e5f6a7-b8c9-4de0-1234-000000000004",
                "ProductID": "d4e5f6a7-b8c9-4de0-1234-000000000004", // Yoga Mat Premium
                "UnitPrice": 129.99,
                "Quantity": 2,
                "TotalPrice": 259.98
            }
        ],
        "TotalBill": 1559.97
    },
    {
        "_id": "eb40a27e-1f44-48bc-861e-a3529c09ae7c",
        "OrderID": "eb40a27e-1f44-48bc-861e-a3529c09ae7c",
        "UserID": "1f366ec1-339e-482b-a3be-77de20a92c4a", // John Smith
        "OrderDate": "2024-12-06T08:00:00Z",
        "OrderItems": [
            {
                "_id": "e5f6a7b8-c9d0-4ef1-2345-000000000005",
                "ProductID": "e5f6a7b8-c9d0-4ef1-2345-000000000005", // Designer Dress
                "UnitPrice": 149.99,
                "Quantity": 1,
                "TotalPrice": 149.99
            },
            {
                "_id": "f6a7b8c9-d0e1-4123-3456-000000000006",
                "ProductID": "f6a7b8c9-d0e1-4123-3456-000000000006", // Smart TV 65"
                "UnitPrice": 799.99,
                "Quantity": 1,
                "TotalPrice": 799.99
            }
        ],
        "TotalBill": 949.98
    }
];

console.log(orders);

// Switch to your database
var db = db.getSiblingDB("OrdersDatabase");

db.orders.insertMany(orders);
