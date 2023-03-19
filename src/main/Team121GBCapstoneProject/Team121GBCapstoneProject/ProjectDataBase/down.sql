--Down Script
ALTER TABLE GameGenre DROP CONSTRAINT [FK_GameGenreID];
ALTER TABLE GameGenre DROP CONSTRAINT [FK_GenreID];
ALTER TABLE GamePlatform DROP CONSTRAINT [FK_PlatformID];
ALTER TABLE GamePlatform DROP CONSTRAINT [FK_GamePlatformID];

ALTER TABLE PersonList DROP CONSTRAINT FK_PersonID;
ALTER TABLE PersonList DROP CONSTRAINT FK_ListKindID;
ALTER TABLE PersonGame DROP CONSTRAINT FK_PersonListID;
ALTER TABLE PersonGame DROP CONSTRAINT FK_GameID;

DROP TABLE [PersonList];
DROP TABLE [PersonGame];
DROP TABLE [ListKind];
DROP TABLE [Person];
DROP TABLE [Game];
DROP TABLE [Genre];
DROP TABLE [GameGenre];
DROP TABLE [Platform];
DROP TABLE [GamePlatform];
