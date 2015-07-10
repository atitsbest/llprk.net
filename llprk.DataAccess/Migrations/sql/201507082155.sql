CREATE TABLE [dbo].[Pages] (
    [Id]          INT         NOT NULL IDENTITY,
    [Title]       NCHAR (100) NOT NULL,
    [Content]     TEXT        NOT NULL,
    [UrlHandle]   NCHAR (100) NULL,
    [CreatedAt]   DATE        DEFAULT (getdate()) NOT NULL,
    [IsPublished] BIT         DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
