CREATE TABLE [dbo].[Todo] (
    [Id]          BIGINT        IDENTITY(1,1)       NOT NULL,
    [Name]        NVARCHAR (50) NOT NULL,
    [Description] TEXT          NULL,
    [IsComplete]  BIT           NULL,
    [Creation]    DATETIME      DEFAULT (getdate()) NOT NULL,
    [Deadline]    DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

