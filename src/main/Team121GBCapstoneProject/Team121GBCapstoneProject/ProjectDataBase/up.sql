--Note: Only Run The create database once and then comment it again. 
--Uncomment, highight create database, then comment over and run query

-- CREATE DATABASE [GamingPlatform]

--Creating Person Table
CREATE TABLE [Person] (
    [ID]              INT           PRIMARY KEY IDENTITY(1, 1),
    [DALL-E_Credits] INT,
    [AuthorizationID] nvarchar(450)
);

CREATE TABLE [ESRBRating] (
    [ID] INT PRIMARY KEY IDENTITY (1,1),
    [ESRBRatingName] NVARCHAR(4),
    [IGDBRatingValue] INT
);

CREATE TABLE [Game] (
    [ID]              INT           PRIMARY KEY IDENTITY(1, 1),
    [Title]           NVARCHAR(64),
    [Description]     NVARCHAR(MAX),
    [YearPublished]   INT,
    [ESRBRatingID]    INT,
    [AverageRating]   FLOAT,
    [CoverPicture]    NVARCHAR(MAX),
    [IGDBUrl]         NVARCHAR(MAX),
    [IGDBGameID]      INT,
    CONSTRAINT [FK_ESRBRatingID] FOREIGN KEY ([ESRBRatingID]) REFERENCES [ESRBRating]([ID]),
);

CREATE TABLE [ListKind] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [Kind] NVARCHAR(50)
);

CREATE TABLE [PersonList] (
    [ID] INT PRIMARY KEY IDENTITY (1,1),
    [PersonID] INT NOT NULL,
    [ListKindID] INT, 
    [ListKind] NVARCHAR(50),
    CONSTRAINT [FK_PersonID] FOREIGN KEY ([PersonID]) REFERENCES [Person]([ID]),
    CONSTRAINT [FK_ListKindID] FOREIGN KEY ([ListKindID]) REFERENCES [ListKind]([ID])
);

CREATE TABLE [PersonGame] (
    [ID] INT PRIMARY KEY IDENTITY (1,1),
    [PersonListID] INT NOT NULL,
    [GameID] INT NOT NULL,
    CONSTRAINT [FK_PersonListID] FOREIGN KEY ([PersonListID]) REFERENCES [PersonList] ([ID]),
    CONSTRAINT [FK_GameID] FOREIGN KEY ([GameID]) REFERENCES [Game] ([ID])
);

CREATE TABLE [Genre] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(64) NOT NULL
);

CREATE TABLE [GameGenre] (
    [ID] INT PRIMARY KEY IDENTITY (1,1),
    [GameID] INT,
    [GenreID] INT,
    CONSTRAINT [FK_GameGenreID] FOREIGN KEY ([GameID]) REFERENCES [Game]([ID]),
    CONSTRAINT [FK_GenreID] FOREIGN KEY ([GenreID]) REFERENCES [Genre]([ID])
);

CREATE TABLE [Platform] (
    [ID] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(64) NOT NULL
);

CREATE TABLE [GamePlatform] (
    [ID] INT PRIMARY KEY IDENTITY (1,1),
    [GameID] INT,
    [PlatformID] INT,
    CONSTRAINT [FK_GamePlatformID] FOREIGN KEY ([GameID]) REFERENCES [Game]([ID]),
    CONSTRAINT [FK_PlatformID] FOREIGN KEY ([PlatformID]) REFERENCES [Platform]([ID])
);

