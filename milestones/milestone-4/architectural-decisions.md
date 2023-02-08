## Architectural Decisions

Updated 2/6/23

### **What folder structure and naming convention will you use for your project source code?**

> src →
>> - main
>> - test → 
>>>> - NUnit_Tests
>>>> - BDD_Tests
>>>> - Jest_Tests

### **What .NET Core version to use?**

ASP.NET Core 7

### **What front-end CSS library and version are you going to use?**

- Bootstrap v5

### **How will you use JavaScript**

- jQuery (DOM traversal)
- [Anime.js](https://animejs.com) (for animations)
- [Granim.js](https://sarcadass.github.io/granim.js/) (background gradient, particularly gradient animation with image)

### **How will you name your Git branches?**

S-[*Sprint #*]-GP-[*GP # on Jira*]-[Description]

### **How will you write your database scripts, table names, PK and FK names?**

-   PKs will be labeled `ID` while FKs will be labeled the class name with `ID` at the end
-   When creating tables, the PK will be set with `PRIMARY KEY IDENTITY(1,1)` and PKs will be integers
-   FK declarations will go in an `ALTER TABLE` line with the addition of a named `CONSTRAINT`
-   All declarations will end with a semicolon

### **Will you use eager loading or enable lazy loading of related entities in Entity Framework Core?**

Lazy loading

### **Will you enable or disable nullable?**

Disable nullable