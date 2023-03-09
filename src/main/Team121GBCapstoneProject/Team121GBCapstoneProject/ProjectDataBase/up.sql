--Note: Only Run The create database once and then comment it again. 
--Uncomment, highight create database, then comment over and run query

-- CREATE DATABASE [GamingPlatform]

--Creating Person Table
CREATE TABLE [Person] (
    [ID]              INT           PRIMARY KEY IDENTITY(1, 1),
    [AuthorizationID] INT,
    [FirstName]       NVARCHAR(64),
    [LastName]        NVARCHAR(64),
    [Username]        NVARCHAR(64),
    [Email]           NVARCHAR(64),
    [ProfilePicture]  NVARCHAR(256),
    [ProfileBio]      NVARCHAR(526),
    [RoleID]          INT     
);

CREATE TABLE [Game] (
    [ID]              INT           PRIMARY KEY IDENTITY(1, 1),
    [Title]           NVARCHAR(64),
    [Description]     NVARCHAR(526),
    [YearPublished]   INT,
    [ESRBRatingID]    INT,
    [AverageRating]   FLOAT,
    [CoverPicture]    NVARCHAR(526)  
);
CREATE TABLE [GamePlayListType] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [ListKind] NVARCHAR(64)
);

CREATE TABLE [ListName] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [NameOfList] NVARCHAR(64) 
)

CREATE TABLE [PersonGameList] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [PersonID] INT NOT NULL, --Had to make FK nullable
    [GameID] INT,
    [ListKindID] INT NOT NULL,
    [ListNameID] INT NOT NULL,
    CONSTRAINT [FK_PersonID] FOREIGN KEY ([PersonId]) REFERENCES [Person]([ID]),
    CONSTRAINT [FK_GameID] FOREIGN KEY ([GameID]) REFERENCES [Game]([ID]),
    CONSTRAINT [FK_ListKindID] FOREIGN KEY ([ListKindID]) REFERENCES [GamePlayListType]([ID]),
    CONSTRAINT [FK_ListNameID] FOREIGN KEY ([ListNameID]) REFERENCES [ListName]([ID])
);