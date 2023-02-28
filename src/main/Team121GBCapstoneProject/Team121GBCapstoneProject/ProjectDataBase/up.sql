--Note: Only Run The create database once and then comment it again. 
--Uncomment, highight create database, then comment over and run query

--CREATE DATABASE [GamingPlatform]

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