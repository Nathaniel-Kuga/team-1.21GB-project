--Seed Script

-- --Inserting Sample Data for Game

INSERT INTO [ListKind] ([Kind]) VALUES ('Currently Playing');
INSERT INTO [ListKind] ([Kind]) VALUES ('Completed');
INSERT INTO [ListKind] ([Kind]) VALUES ('Want to Play');


INSERT INTO [Genre] ([Name]) VALUES ('Point and Click');
INSERT INTO [Genre] ([Name]) VALUES ('Fighting');
INSERT INTO [Genre] ([Name]) VALUES ('Shooter');
INSERT INTO [Genre] ([Name]) VALUES ('Music');
INSERT INTO [Genre] ([Name]) VALUES ('Platform');
INSERT INTO [Genre] ([Name]) VALUES ('Puzzle');
INSERT INTO [Genre] ([Name]) VALUES ('Racing');
INSERT INTO [Genre] ([Name]) VALUES ('Real Time Strategy (RTS)');
INSERT INTO [Genre] ([Name]) VALUES ('Role-Playing (RPG)');
INSERT INTO [Genre] ([Name]) VALUES ('Simulator');
INSERT INTO [Genre] ([Name]) VALUES ('Sport');
INSERT INTO [Genre] ([Name]) VALUES ('Strategy');
INSERT INTO [Genre] ([Name]) VALUES ('Turn-Based Strategy');
INSERT INTO [Genre] ([Name]) VALUES ('Tactical');
INSERT INTO [Genre] ([Name]) VALUES ('Quiz/Trivia');
INSERT INTO [Genre] ([Name]) VALUES ('Hack and slash/Beat ''em up');
INSERT INTO [Genre] ([Name]) VALUES ('Pinball');
INSERT INTO [Genre] ([Name]) VALUES ('Adventure');
INSERT INTO [Genre] ([Name]) VALUES ('Arcade');
INSERT INTO [Genre] ([Name]) VALUES ('Visual Novel');
INSERT INTO [Genre] ([Name]) VALUES ('Indie');
INSERT INTO [Genre] ([Name]) VALUES ('Card & Board Game');
INSERT INTO [Genre] ([Name]) VALUES ('MOBA');

INSERT INTO [Platform] ([Name]) VALUES ('Mac'); --1
INSERT INTO [Platform] ([Name]) VALUES ('PC (Microsoft Windows)'); --2
INSERT INTO [Platform] ([Name]) VALUES ('Linux'); --3
INSERT INTO [Platform] ([Name]) VALUES ('Xbox Series X|S'); --4
INSERT INTO [Platform] ([Name]) VALUES ('PlayStation 5'); --5
INSERT INTO [Platform] ([Name]) VALUES ('Xbox One'); --6
INSERT INTO [Platform] ([Name]) VALUES ('Playstation 4'); --7
INSERT INTO [Platform] ([Name]) VALUES ('Nintendo Switch'); --8
INSERT INTO [Platform] ([Name]) VALUES ('Playstation 3'); --9
INSERT INTO [Platform] ([Name]) VALUES ('Xbox 360'); --10
INSERT INTO [Platform] ([Name]) VALUES ('SNES'); --11
INSERT INTO [Platform] ([Name]) VALUES ('Nintendo 3DS'); --12
INSERT INTO [Platform] ([Name]) VALUES ('Game boy'); --13
INSERT INTO [Platform] ([Name]) VALUES ('Nintendo 64'); --14
INSERT INTO [Platform] ([Name]) VALUES ('Nintendo GameCube'); --15
INSERT INTO [Platform] ([Name]) VALUES ('Wii'); --16



--seed ESRBRatings 
--according to the devs on the IGDB discord server, you have to map
--the value returned by the API to a name which is what the IGDBRatingValue column does
INSERT INTO [ESRBRating] ([ESRBRatingName], [IGDBRatingValue]) VALUES ('RP', 6);
INSERT INTO [ESRBRating] ([ESRBRatingName], [IGDBRatingValue]) VALUES ('EC', 7);
INSERT INTO [ESRBRating] ([ESRBRatingName], [IGDBRatingValue]) VALUES ('E', 8);
INSERT INTO [ESRBRating] ([ESRBRatingName], [IGDBRatingValue]) VALUES ('E10', 9);
INSERT INTO [ESRBRating] ([ESRBRatingName], [IGDBRatingValue]) VALUES ('T', 10);
INSERT INTO [ESRBRating] ([ESRBRatingName], [IGDBRatingValue]) VALUES ('M', 11);
INSERT INTO [ESRBRating] ([ESRBRatingName], [IGDBRatingValue]) VALUES ('AO', 12);

--Inserting Sample Data for Game
-- need to add igdb ids
INSERT INTO [Game] ([Title],[Description],[YearPublished],[AverageRating],[CoverPicture],[IGDBUrl], [ESRBRatingID], [IGDBGameID]) VALUES ('Gears Of War','The planet lies in ruin – cities crumbling, Man’s greatest works fallen. The Locust Horde has risen, and they won’t stop coming. They won’t stop killing. An inmate named Marcus Fenix, once left to die, is now charged with keeping humanity alive. He can take comfort in but one fact: The human race isn’t extinct. Yet.', 2006, 9.4, 'https://images.igdb.com/igdb/image/upload/t_thumb/co28gi.png', 'https://www.igdb.com/games/gears-of-war', 6,  547);
INSERT INTO [Game] ([Title],[Description],[YearPublished],[AverageRating],[CoverPicture],[IGDBUrl], [ESRBRatingID], [IGDBGameID]) VALUES ('Deep Rock Galatic','Deep Rock Galactic is a cooperative first-person shooter video game developed by Danish studio Ghost Ship Games and published by Coffee Stain Publishing. ', 2018, 9.0, 'https://images.igdb.com/igdb/image/upload/t_thumb/co48gx.png', 'https://www.igdb.com/games/deep-rock-galactic', 5, 27134);
INSERT INTO [Game] ([Title],[Description],[YearPublished],[AverageRating],[CoverPicture],[IGDBUrl], [ESRBRatingID], [IGDBGameID]) VALUES ('Minecraft','If you can dream it, you can build it. Put your imagination and limitless resources to work with Creative Mode.', 2011, 8.6, 'https://images.igdb.com/igdb/image/upload/t_thumb/co49x5.png', 'https://www.igdb.com/games/minecraft', 4, 121);
INSERT INTO [Game] ([Title],[Description],[YearPublished],[AverageRating],[CoverPicture],[IGDBUrl], [ESRBRatingID], [IGDBGameID]) VALUES ('Dark Souls','Dark Souls focuses on dungeon exploring and the tension and fear that arise when players encounter enemies in this setting.', 2011, 9.0, 'https://images.igdb.com/igdb/image/upload/t_thumb/co1x78.png', 'https://www.igdb.com/games/dark-souls', 6, 2155);
INSERT INTO [Game] ([Title],[Description],[YearPublished],[AverageRating],[CoverPicture],[IGDBUrl], [ESRBRatingID], [IGDBGameID]) VALUES ('Dark Souls 2','Join the dark journey and experience overwhelming enemy encounters, diabolical hazards, and the unrelenting challenge that only FROM SOFTWARE can deliver.', 2014, 9.0, 'https://images.igdb.com/igdb/image/upload/t_thumb/co2eoo.png', 'https://www.igdb.com/games/dark-souls-ii', 5, 2368);
INSERT INTO [Game] ([Title],[Description],[YearPublished],[AverageRating],[CoverPicture],[IGDBUrl], [ESRBRatingID], [IGDBGameID]) VALUES ('Dark Souls 3','As fires fade and the world falls into ruin, journey into a universe filled with more colossal enemies and environments. Now only embers remain… Prepare yourself once more and Embrace The Darkness!', 2016, 9.5, 'https://images.igdb.com/igdb/image/upload/t_thumb/co1vcf.png', 'https://www.igdb.com/games/dark-souls-iii', 6, 11133);
INSERT INTO [Game] ([Title],[Description],[YearPublished],[AverageRating],[CoverPicture],[IGDBUrl], [ESRBRatingID], [IGDBGameID]) VALUES ('BloodBorne','Hunt your nightmares as you search for answers in the ancient city of Yharnam, now cursed with a strange endemic illness spreading through the streets like wildfire. Danger, death and madness lurk around every corner of this dark and horrific world, and you must discover its darkest secrets in order to survive.', 2015, 9.1, 'https://images.igdb.com/igdb/image/upload/t_thumb/co1rba.png', 'https://www.igdb.com/games/bloodborne', 6, 7334);
INSERT INTO [Game] ([Title],[Description],[YearPublished],[AverageRating],[CoverPicture],[IGDBUrl], [ESRBRatingID], [IGDBGameID]) VALUES ('Demon Souls','Embark on a dangerous journey to become the “Slayer of Demons”', 2020, 9.0, 'https://images.igdb.com/igdb/image/upload/t_thumb/co27sk.png', 'https://www.igdb.com/games/demon-s-souls', 6, 5619);
INSERT INTO [Game] ([Title],[Description],[YearPublished],[AverageRating],[CoverPicture],[IGDBUrl], [ESRBRatingID], [IGDBGameID]) VALUES ('Elden Ring','Rise, Tarnished, and be guided by grace to brandish the power of the Elden Ring and become an Elden Lord in the Lands Between.', 2021, 10, 'https://images.igdb.com/igdb/image/upload/t_thumb/co4jni.png', 'https://www.igdb.com/games/elden-ring', 6, 119133);
INSERT INTO [Game] ([Title],[Description],[YearPublished],[AverageRating],[CoverPicture],[IGDBUrl], [ESRBRatingID], [IGDBGameID]) VALUES ('Xenoblade Cronicles','During an attack from the mechanical invaders known as the Mechon, Shulk discovers that he can tap into the full power of a mysterious blade known as the Monado.', 2010, 8.0, 'https://images.igdb.com/igdb/image/upload/t_thumb/co2609.png', 'https://www.igdb.com/games/xenoblade-chronicles', 5, 241167);
INSERT INTO [Game] ([Title],[Description],[YearPublished],[AverageRating],[CoverPicture],[IGDBUrl], [ESRBRatingID], [IGDBGameID]) VALUES ('Getting Over it','Getting Over It with Bennett Foddy is a punishing climbing game, a homage to Jazzuos 2002 B-Game classic Sexy Hiking.', 2017, 8.0, 'https://images.igdb.com/igdb/image/upload/t_thumb/co3wl5.png', 'https://www.igdb.com/games/getting-over-it-with-bennett-foddy', 3, 72373);


INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (1, 10);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (1, 2);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (2, 4);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (2, 2);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (2, 6);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (2, 7);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (2, 5);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (3, 1);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (3, 2);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (3, 3);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (3, 8);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (3, 6);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (3, 7);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (4, 10);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (4, 2);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (4, 9);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (5, 10);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (5, 2);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (5, 9);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (6, 2);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (6, 6);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (6, 7);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (7, 7);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (8, 5);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (9, 4);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (9, 6);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (9, 7);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (9, 5);
INSERT INTO [GamePlatform] ([GameID], [PlatformID]) VALUES (10, 8);

INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (2, 18);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (2, 21);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (2, 3);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (1, 3);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (3, 18);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (3, 10);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (4, 18);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (4, 9);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (5, 18);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (5, 9);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (6, 18);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (6, 9);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (7, 18);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (7, 9);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (8, 18);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (8, 9);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (9, 18);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (9, 9);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (10, 18);
INSERT INTO [GameGenre] ([GameID], GenreID) VALUES (10, 9);