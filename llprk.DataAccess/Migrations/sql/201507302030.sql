ALTER TABLE [dbo].[Orders]
    ADD DEFAULT getutcdate() FOR [CreatedAt];
