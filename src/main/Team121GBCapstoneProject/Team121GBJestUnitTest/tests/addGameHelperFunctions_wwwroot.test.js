import { getUserListsSuccess, setUpURL } from "../../Team121GBCapstoneProject/wwwroot/js/addGameHelperFunctions"
const $ = require('jquery');
describe("Test Methods for adding game helper functions", () => {
    test("setUpUrl Success", () => {
        console.log("setUpUrl success");
        const origin = "https://Home";
        const url = `${origin}/api/Game/addGame`;
        const urlFrom_setUpURL = setUpURL(origin);
        expect(urlFrom_setUpURL).toEqual(url);
    });
    test("setUpUrl failure null origin", () => {
        console.log("setUpUrl failure null origin");
        const origin = null;
        const urlFrom_setUpURL = setUpURL(origin);
        expect(urlFrom_setUpURL).toBe(false);
    });
    test("setUpUrl failure undefined origin", () => {
        console.log("setUpUrl failure undefined origin");
        const origin = undefined;
        const urlFrom_setUpURL = setUpURL(origin);
        expect(urlFrom_setUpURL).toBe(false);
    });
    test("setUpUrl failure empty origin", () => {
        console.log("setUpUrl failure empty origin");
        const origin = "";
        const urlFrom_setUpURL = setUpURL(origin);
        expect(urlFrom_setUpURL).toBe(false);
    });
    //!---------------------------------------------------
    test("getUserListSuccess success", () => {
        console.log("getUserListSuccess success");
        const data = [{
            "listKind": "Currently Playing"
        },
        {
            "listKind": "Completed"
        },
        {
            "listKind": "Want To Play"
        }];
        expect(getUserListsSuccess(data)).toEqual(data);
    });
    test("getUserListSuccess failure null data", () => {
        console.log("getUserListSuccess failure null data");
        const data = null;
        expect(getUserListsSuccess(data)).toBe(false);
    });
    test("getUserListSuccess failure undefined data", () => {
        console.log("getUserListSuccess failure undefined data");
        const data = undefined;
        expect(getUserListsSuccess(data)).toBe(false);
    });
    test("getUserListSuccess failure empty data", () => {
        console.log("getUserListSuccess failure empty data");
        const data = [];
        expect(getUserListsSuccess(data)).toBe(false);
    });
});