CREATE TABLE [dbo].[LineItems] (
    [Id]        INT             NOT NULL IDENTITY,
    [ProductId] INT             NOT NULL,
    [CartId]    INT             NULL,
    [OrderId]   INT             NULL,
    [Price]     DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [CreatedAt] DATE            DEFAULT (getdate()) NOT NULL,
    [Qty]       INT             DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LineItems_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]),
    CONSTRAINT [FK_LineItems_Cart] FOREIGN KEY ([CartId]) REFERENCES [dbo].[Carts] ([Id]),
    CONSTRAINT [FK_LineItems_Orders] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([Id])
);

