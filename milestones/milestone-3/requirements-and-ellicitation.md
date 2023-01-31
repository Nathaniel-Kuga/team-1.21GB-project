# Requirements Workup

## Elicitation

1. **Is the goal or outcome well defined?  Does it make sense?**

The goal or outcome is well-defined given the current documentation. The goal makes sense and has a purpose.

2. **What is not clear from the given description?**

Given the description, it is unclear what our architecture is for this application. But in our opinion, this is covered in the architecture diagram and it is understood given the project specifications.

3. **How about scope?  Is it clear what is included and what isn't?**

After some analysis, the team has decided that the scope is clear about what will be included in the application and what is not. As we see it, if it is not included in the needs and features, then it is not a part of the application.

4. **What do you not understand?**
* Technical domain knowledge

We do not have concrete APIs picked out for PlayStation or Xbox API. This means there is a slight lack of understanding of these APIs. We do not have the Data model finalized with up, down, and seed scripts.

* Business domain knowledge

As of current, we have not finalized the cost of the application to run daily. However, we do know the pricing for the APIs we have decided to use.

5. **Is there something missing?**

The only things missing are we have not fully decided on APIs for Xbox and PlayStation, but we have some contenders for which one we want. The other missing is our algorithmic component, we currently have two different ideas in the running. 


## Analysis

Go through all the information gathered during the previous round of elicitation.  

1. **For each attribute, term, entity, relationship, activity ... precisely determine its bounds, limitations, types and constraints in both form and function.  Write them down.**

In terms of our goal, the upper boundary is creating a website where gamers can organize their library of games, rate, review, and add games to their library, also see the top one hundred games according to other users, and also be able to browse the internet without spoilers on the application. Our limitation is the fair use of the APIs that we are using, along with the API being development friendly. One of our constraints is time and scheduling conflicts for working together in one place. 
In terms of entities we are going to be using we need a person, gameLists, games, and roles for users, it would also improve functionality if we have different genres as well and the ESRB rating system implemented. The bounds of this are almost endless. We could go extremely deep into the game's information, the only limit being our API’s data we can use and
from the looks of it, we won't be using all of it. Another limitation would be how we update the database with information, the current API we plan on using has some restrictions on the number of calls we can do. Also, when are we updating the database, and the process of doing so. 
In terms of the product's success is based on our database design, if it is built in a way that is easy to use and straightforward. If built poorly this may cause a lot of slowdown in terms of
development. Our main limitation is getting information from game companies' API to find the user's game library on that platform and how much information we can get from the API. However, the bounds for user functionality can be as useful as a player being able to migrate all the games they own on different systems just by a few button clicks. 

2. **Do they work together or are there some conflicting requirements, specifications or behaviors?**

From our research, it looks like everything will work together without too many conflicts. The main problem is trying all the data as well as the API together into one helpful and easy-to-use interface. One conflict we have is the different APIs provide different information for a given user, our main hurdle is going to make that work with our database.

3. **Have you discovered if something is missing?**  

After some searching, we discovered that Nintendo does not have APIs that match our use case. The only one we found was only for game development not for getting player information. However, in this case, we can use the IGDB for getting that information, but this means we can not populate the player's game library.


## Design and Modeling
Our first goal is to create a **data model** that will support the initial requirements.

1. **Identify all entities;  for each entity, label its attributes; include concrete types**


### Person
* ID : int (PK)
* Name : nvarchar(64)
* UserName : nvarchar(64)
* Password : nvarchar(64)
* Email : nvarchar(32)
* RoleID : int (FK)

### Role
* ID : int (PK)
* Name : nvarchar(16)
* Priority : int 

### List
* ID : int (PK)
* Title : nvarchar(32)
* PersonID : int (FK)

### GameList
* ListID : int (FK)
* GameID : int (FK)

### Game
* ID : int (PK)
* Title : nvarchar(64)
* Description : nvarchar(526)
* YearPublished : int 
* ESRBRatingID : int (FK)
* AverageRating : double
* CoverHTML : nvarchar(128)

### GameGenre
* GameID : int (FK)
* GenreID : int (FK)

### Genre
* ID : int (PK)
* Name : nvarchar(32)
* GamePlatform 
* PlatformID : int (FK)
* GameID : int(FK)

### Platform
* ID : int (PK)
* Name : nvarchar(32)
* YearCreated : int

### GamePublisher
* GameID : int (FK)
* PublisherID : int (FK)

### Publisher
* ID : int (PK)
* Name : nvarchar(64)

### ESRBRating 
* ID : int (PK)
* AgeRating : nvarchar(32)

### Acheivements 
* ID int (PK)
* Title nvarchar(32)
* Description nvarchar(256)
* GameID int (FK)

### UserAcheivements
* PersonID int (FK)
* GameID int (FK)

2. **Identify relationships between entities.  Write them out in English descriptions.**

* A person can only have one role but a role can apply to many people.
* A person can have many lists but a list can only have one owner.
* A list can have many games and a game can apply to many lists.
* A game can have multiple genres and a genre can apply to many games.
* A game can be on different platforms and a platform has many games.
* A game can have more than one publisher and a publisher can have multiple games.
* A game can only have one ESRB rating but a rating can apply to many games. 
* A person can have many achievements and Achievements and a achievement can apply to many people.
* A game can have many achievements but each achievement can only have one game. 


3. **Draw these entities and relationships in an _informal_ Entity-Relation Diagram.**

![Gaming Platform DB V3](https://user-images.githubusercontent.com/63754407/215356523-399cae5e-206e-4612-8043-11fda0f896b8.png)
(Also provided on GitHub under Milestone 3 Gaming Platform DB V3)


## Analysis of the Design
The next step is to determine how well this design meets the requirements _and_ fits into the existing system.

1. **Does it support all requirements/features/behaviors?**
    * For each requirement, go through the steps to fulfill it.  Can it be done?  Correctly?  Easily?

Given our current database model and our chosen API, we can complete all requirements/features/behaviors correctly and the way they are intended with relative ease.

2. **Does it meet all non-functional requirements?**
    * May need to look up specifications of systems, components, etc. to evaluate this.

After some analysis, all non-functional requirements will be met, Relational database is a good solution for storing information and ASP.NET CORE 7.0 will handle the functionality of the application. Our main concern will be security for users and data. Our solution will be using reCAPTCHA, our main concern is whether will it accomplish what we want and we are currently looking at that. The other concern is maintainability and reliability, our application is running on multiple APIs and we are concerned that the API could be removed or start requiring payments to use it. This would slow down our development of the application significantly.