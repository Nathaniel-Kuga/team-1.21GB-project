class GameDto {
    constructor(listKind, gameTitle, imageSrc, igdbID) {
        this.listKind = listKind;
        this.gameTitle = gameTitle;
        this.imageSrc = imageSrc;
        this.igdbID = igdbID; // === undefined ? -1: igdbID; //ternary operater in js
    }
}
console.log("Calling Constructor for GameDto inside GameDtoClass");
export { GameDto }