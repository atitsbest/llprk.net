CREATE TABLE [dbo].[Taxes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Percent] INT NOT NULL DEFAULT 0, 
    [CountryId] NVARCHAR(5) NOT NULL, 
    CONSTRAINT [FK_Tax_Country] FOREIGN KEY ([CountryId]) REFERENCES [Countries]([Id])
)
