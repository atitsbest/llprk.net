ALTER TABLE [dbo].[Carts]
    ADD [OrderId] INT NULL;
ALTER TABLE [dbo].[Carts] WITH NOCHECK
    ADD CONSTRAINT [FK_Carts_Orders] FOREIGN KEY ([OrderId]) REFERENCES [dbo].[Orders] ([Id]);
ALTER TABLE [dbo].[Carts] WITH CHECK CHECK CONSTRAINT [FK_Carts_Orders];
