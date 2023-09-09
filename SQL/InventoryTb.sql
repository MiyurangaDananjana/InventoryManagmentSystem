use InventorySystem

SELECT * FROM UserRegister

CREATE Table Sales(
	SaleId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	QuantityOfSales INT NOT NULL,
	PricePerUnit DECIMAL(10,2) NOT NULL,
	TotalPrice DECIMAL(10,2) NOT NULL,
	SalesByUserId INT FOREIGN KEY REFERENCES dbo.UserRegister(Id) NOT NULL,
	SalesDateTime datetime 
);

CREATE TABLE Brands(
	BrandId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	BrandName NVARCHAR(100) NOT NULL UNIQUE,
	BrandUpdateBy INT FOREIGN KEY REFERENCES dbo.UserRegister(Id) NOT NULL
);

CREATE TABLE Products(
	ProductId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	BrandId INT FOREIGN KEY REFERENCES Brands(BrandId) NOT NULL,
	ProductName NVARCHAR(100) NOT NULL,
	ProductDescription NVARCHAR(200),
	Price DECIMAL(10,2),
);

CREATE TABLE ProductVariante(
	ProductVariantId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	ProductsId INT FOREIGN KEY REFERENCES Products(ProductId) NOT NULL,
	VariantDescription NVARCHAR(200),
	Description NVARCHAR(200),
	CreateBy INT FOREIGN KEY REFERENCES dbo.UserRegister(Id) NOT NULL,
	PriceModifier DECIMAL(10,2),
	StockQuantity INT
);

CREATE TABLE Supplier(
	SupplierID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	SupplierName VARCHAR(100) NOT NULL,
	PhoneNumber NVARCHAR(100) UNIQUE NOT NULL,
	Address NVARCHAR(200) NOT NULL,
	SupplierDescription NVARCHAR(200),
	SupplierStatus INT NOT NULL
)

CREATE TABLE OrderStatus(
	OrderStatusId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	OrderStatus VARCHAR(50)
)

CREATE TABLE PayMentMethods(
	PaymentMethodsId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	PaymentMethod VARCHAR(100)
);

INSERT INTO dbo.OrderStatus (OrderStatus) VALUES ('Pending'),('Shipped'),('Delivered')
INSERT INTO dbo.PayMentMethods (PaymentMethod) VALUES ('Card'),('Cash'),('Online'),('Cheque')

CREATE TABLE Orders(
	OrderId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	OrderDate DateTime NOT NULL,
	CustomerId INT FOREIGN KEY REFERENCES dbo.Customer(Id),
	OrderStatus INT FOREIGN KEY REFERENCES dbo.OrderStatus(OrderStatusId) NOT NULL,
	TotalAmount DECIMAL(10,2) NOT NULL, 
	PaymentMethod INT FOREIGN KEY REFERENCES dbo.PayMentMethods(PaymentMethodsId) NOT NULL,
	OrderCreateBy INT FOREIGN KEY REFERENCES dbo.UserRegister(Id) NOT NULL
);

CREATE TABLE OrderItems(
	OrderItemsId INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	OrderId INT FOREIGN KEY REFERENCES Orders(OrderId),
	ProductId INT FOREIGN KEY REFERENCES ProductVariante(ProductVarianteId),
	Quantity INT NOT NULL,
	UnitPrice DECIMAL(10,2)
	
);




