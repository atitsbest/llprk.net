CREATE TABLE [dbo].[LineItems]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [ProductId] INT NOT NULL, 
    [CartId] INT NULL, 
    [OrderId] INT NOT NULL, 
    [Price] DECIMAL(18, 2) NOT NULL DEFAULT (0), 
    [CreatedAt] DATE NOT NULL DEFAULT (getdate()), 
    [Qty] INT NOT NULL DEFAULT (1), 
    CONSTRAINT [FK_LineItems_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]),
    CONSTRAINT [FK_LineItems_Cart] FOREIGN KEY ([CartId]) REFERENCES [Carts]([Id]),
    CONSTRAINT [FK_LineItems_Orders] FOREIGN KEY ([OrderId]) REFERENCES [Orders]([Id])
)