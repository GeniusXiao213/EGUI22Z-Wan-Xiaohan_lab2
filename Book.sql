CREATE TABLE [dbo].[Book]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [author] NVARCHAR(50) NOT NULL, 
    [title] NVARCHAR(100) NOT NULL, 
    [publisher] NVARCHAR(50) NOT NULL, 
    [date] INT NOT NULL, 
    [user] NVARCHAR(50) NULL, 
    [reserved] NVARCHAR(50) NULL, 
    [leased] NVARCHAR(50) NULL
)
