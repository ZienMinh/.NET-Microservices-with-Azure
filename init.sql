CREATE DATABASE IF NOT EXISTS ecommerceproducts;
CREATE DATABASE IF NOT EXISTS ecommerceusers;

USE ecommerceusers;

CREATE TABLE IF NOT EXISTS Users (
    UserID CHAR(36) PRIMARY KEY,
    PersonName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Gender VARCHAR(50) NOT NULL
);

INSERT INTO Users (UserID, PersonName, Email, Password, Gender) VALUES
('1f366ec1-339e-482b-a3be-77de20a92c4a', 'John Smith', 'john.smith@email.com', 'string', 'Male'),
('2a45bf32-448f-493c-9cb3-88de31bc35bb', 'Emma Wilson', 'emma.wilson@email.com', 'string', 'Female');

USE ecommerceproducts;

CREATE TABLE IF NOT EXISTS Products (
    ProductID CHAR(36) PRIMARY KEY,
    ProductName VARCHAR(50) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    UnitPrice DOUBLE NOT NULL,
    QuantityInStock INT NOT NULL
);

INSERT INTO Products (ProductID, ProductName, Category, UnitPrice, QuantityInStock) VALUES
('a1b2c3d4-e5f6-47ab-89cd-000000000001', 'iPhone 14 Pro', 'Electronics', 999.99, 50),
('b2c3d4e5-f6a7-48bc-9de0-000000000002', 'Smart Coffee Maker', 'HomeAndKitchen', 899.99, 45),
('c3d4e5f6-a7b8-49cd-ef01-000000000003', 'MacBook Air M2', 'Electronics', 1299.99, 30),
('d4e5f6a7-b8c9-4de0-1234-000000000004', 'Yoga Mat Premium', 'SportsAndOutdoors', 129.99, 100),
('e5f6a7b8-c9d0-4ef1-2345-000000000005', 'Designer Dress', 'Fashion', 149.99, 85),
('f6a7b8c9-d0e1-4123-3456-000000000006', 'Smart TV 65"', 'Electronics', 799.99, 150),
('a7b8c9d0-e1f2-4234-4567-000000000007', 'Air Purifier Pro', 'HomeAndKitchen', 349.99, 40),
('b8c9d0e1-f2a3-4345-5678-000000000008', 'Tennis Racket Set', 'SportsAndOutdoors', 399.99, 60),
('c9d0e1f2-a3b4-4456-6789-000000000009', 'Luxury Watch', 'Fashion', 199.99, 75),
('d0e1f2a3-b4c5-4567-89ab-000000000010', 'Gaming Console', 'Electronics', 499.99, 15);