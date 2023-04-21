# Steps taken

1. Install [Node JS](https://nodejs.org/en/) on your dev machine
2. Using Visual Studio, add a new project using the "JavaScript Console Application" template
3. Open a console inside the new test project and install [Jest](https://jestjs.io/docs/getting-started):
```
npm install --save-dev jest
npm install --save-dev jest-editor-support
npm install --save-dev babel-jest
npm install --save-dev @babel/preset-env
```
4. Add these two lines to the .esproj file (Visual Studio 2022 added support for testing with Jest):
```
<JavaScriptTestRoot>tests\</JavaScriptTestRoot>
<JavaScriptTestFramework>Jest</JavaScriptTestFramework>
```
5. Add local code and test files to prove it works locally within this project (`sum.js` and `tests/sum_local.test.js`)
6. Run tests from command line
	npm test
7. Run tests from the Test Explorer in VS
8. I haven't been able to run JS tests from VS Code's test explorer yet.  There is an extension for it (Jest, by Orta) but I haven't gotten it working
9. Add test code in the main .NET project as well as new test file (`wwwroot/js/sum.js` and `tests/sum_wwroot.test.js`).  Add the given `babel.config.json` file.  Now run tests again and we can test JS in our main project.
10. Adjust how we use these ES6 modules from within our normal JS code.  Use the syntax
```
import { sum } from './sum.js'
```
Add `type="module"` to your script elements: 
```
<script src="~/js/site.js" type="module" asp-append-version="true"></script>
```