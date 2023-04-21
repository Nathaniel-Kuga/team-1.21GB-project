//// Moshe Test

//// Import the function to be tested
//import { displaySearchResults } from "../../Team121GBCapstoneProject/wwwroot/js/search";

//// Define a mock response data
//const mockResponse = [
//    {
//        gameTitle: 'Game 1',
//        gameWebsite: 'http://example.com/game1',
//        gameCoverArt: 'http://example.com/game1/thumb.jpg',
//    },
//    {
//        gameTitle: 'Game 2',
//        gameWebsite: 'http://example.com/game2',
//        gameCoverArt: 'http://example.com/game2/thumb.jpg',
//    },
//];

//// Define a mock jQuery AJAX function
//jest.mock('jquery', () => ({
//    ajax: jest.fn(() => Promise.resolve(mockResponse)),
//}));

//describe('displaySearchResults', () => {
//    test('displays search results', async () => {
//        // Arrange
//        document.body.innerHTML = '<table><tbody id="gameTableBody"></tbody></table>';
//        const query = 'test';

//        // Act
//        await displaySearchResults(query);

//        // Assert
//        const rows = document.querySelectorAll('#gameTableBody tr');
//        expect(rows.length).toBe(mockResponse.length);

//        mockResponse.forEach((game, i) => {
//            const row = rows[i];
//            expect(row.querySelector('img').src).toBe(game.gameCoverArt.replace('thumb', 'logo_med'));
//            expect(row.querySelector('b').textContent).toBe(game.gameTitle);
//            expect(row.querySelector('a').href).toBe(game.gameWebsite);
//        });
//    });
//});