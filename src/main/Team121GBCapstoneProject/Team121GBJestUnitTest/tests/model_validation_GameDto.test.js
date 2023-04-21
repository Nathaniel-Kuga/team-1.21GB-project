import { GameDto } from "../../Team121GBCapstoneProject/wwwroot/js/GameDtoClass";
import { validateGameDtoData } from "../../Team121GBCapstoneProject/wwwroot/js/validation";

describe("GameDto data validation tests", () => {
    test("undefined GameDto fails validation", () => {
        console.log("undefined GameDto fails validation");
        expect(validateGameDtoData(undefined)).toBe(false);
    });
    // * check against objects or attributes being empty.
    test("empty GameDto fails validation", () => {
        console.log("empty GameDto fails validation");
        const gameDto = {};
        expect(validateGameDtoData(gameDto)).toBe(false);
    });
    test("empty list GameDto fails validation", () => {
        console.log("empty list GameDto fails validation");
        const gameDto = new GameDto("", "title", "source", 1232)
        expect(validateGameDtoData(gameDto)).toBe(false);
    });
    test("empty title GameDto fails validation", () => {
        console.log("empty title GameDto fails validation");
        const gameDto = new GameDto("list", "", "source", 1232);
        expect(validateGameDtoData(gameDto)).toBe(false);
    });
    test("empty imageSrc GameDto fails validation", () => {
        console.log("empty imageSrc GameDto fails validation");
        const gameDto = new GameDto("list", "title", "", 1232);
        expect(validateGameDtoData(gameDto)).toBe(false);
    });
    test("string igdbId GameDto fails validation", () => {
        console.log("string igdbId GameDto fails validation");
        const gameDto = new GameDto("list", "title", "source", "1232");
        expect(validateGameDtoData(gameDto)).toBe(false);
    });
    test("constructed GameDto passes validation", () => {
        console.log("constructed GameDto passes validation");
        const gameDto = new GameDto("list", "title", "source", 1232);
        expect(validateGameDtoData(gameDto)).toBe(true);
    });
});