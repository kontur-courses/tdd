import Game from "./bowlingGame"

export default function BowlingGameTests() {
    it("Have zero score before any rolls", function () {
        let game = new Game();
        game.getScore();
    });
}
