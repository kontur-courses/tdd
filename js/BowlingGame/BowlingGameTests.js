import Game from "./BowlingGame"

export default function BowlingGameTests() {
    it("Have zero score before any rolls", function () {
        let game = new Game();
        game.GetScore();
    });
}

export const Names = "ENTER YOUR NAME HERE";