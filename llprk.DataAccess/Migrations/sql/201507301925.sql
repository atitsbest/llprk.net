﻿ALTER TABLE [dbo].[Orders]
    ADD [Total] MONEY DEFAULT 0 NOT NULL,
        [Tax]   MONEY DEFAULT 0 NOT NULL;
