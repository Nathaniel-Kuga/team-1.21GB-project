# OPENXBL
Microsoft's public API is for video game development and is features are beyond the scope our project. OPENXBL is a more lightweight, RESTFUL API, with methods geared toward user profile data retrieval. OPENXBL will give us the tools we need for our project and not overload us with features.

# Implementation
The API is written in PHP and there isn't a NUGET package for it, but the documentation does provide the [base URL for non PHP projects](https://xbl.io/getting-started). The users of our website will have to give Microsoft permission to share data with us by logging into the microsoft account, and grating us access.

# Value
 I think this will be a good fit for our app because it adds value to the end user by allowing a given user to include their Xbox data in their account, but it will not force them to if they do not want to share that data with us.

# Documentation
The API has very simple to read instructions on how to use it, and also include a [Swagger powered API console](https://xbl.io/console) to test out its features yourself. I have done this, and found it accurately returned my Xbox Live account information.

[Github Repository](https://github.com/OpenXBL)

# Cost and Terms of Use
The cost is free, with a paid tier that include extra features. The free version limits requests to 150 an hour. As far the extra features go, they don't seem to have any added benefit for our project, and also they lack sufficient documentation to justify the cost.

[OPENXBL Website](https://xbl.io)