import "chai/register-assert";

//Перевод на JS оригинального решения от дядюшки Боба
class Game {
    constructor() {
        this.rolls = [];
        this.currentRoll = 0;
    }

    roll(pins) {
        this.rolls[this.currentRoll++] = pins;
    }

    getScore() {
        let score = 0;
        let frameIndex = 0;
        for (let frame = 0; frame < 10; frame++) {
            if (this.isStrike(frameIndex)) {
                score += 10 + this.getStrikeBonus(frameIndex);
                frameIndex++;
            }
            else if (this.isSpare(frameIndex)) {
                score += 10 + this.getSpareBonus(frameIndex);
                frameIndex += 2;
            }
            else {
                score += this.getSumOfBallsInFrame(frameIndex);
                frameIndex += 2;
            }
        }
        return score;
    }

    isStrike(frameIndex) {
        return this.rolls[frameIndex] === 10;
    }

    getSumOfBallsInFrame(frameIndex) {
        return this.rolls[frameIndex] + this.rolls[frameIndex + 1];
    }

    getSpareBonus(frameIndex) {
        return this.rolls[frameIndex + 2];
    }

    getStrikeBonus(frameIndex) {
        return this.rolls[frameIndex + 1] + this.rolls[frameIndex + 2];
    }

    isSpare(frameIndex) {
        return this.rolls[frameIndex] + this.rolls[frameIndex + 1] === 10;
    }
}

describe("Game tests", () => {
    let game;

    beforeEach(() => {
        game = new Game();
    });

    it("test gutter game", () => {
        rollMany(20, 0);
        assert.equal(game.getScore(), 0);
    });

    it("test all ones", () => {
        rollMany(20,1);
        assert.equal(game.getScore(), 20);
    });

    it("test one spare", () => {
        rollSpare();
        game.roll(3);
        rollMany(17,0);
        assert.equal(game.getScore(), 16);
    });

    it("test one strike", () => {
        rollStrike();
        game.roll(3);
        game.roll(4);
        rollMany(16, 0);
        assert.equal(game.getScore(), 24);
    });

    it("test perfect game", () => {
        rollMany(12,10);
        assert.equal(game.getScore(), 300);
    });

    function rollMany(n, pins) {
        for (let i = 0; i < n; i++) {
            game.roll(pins);
        }
    }

    function rollStrike() {
        game.roll(10);
    }

    function rollSpare() {
        game.roll(5);
        game.roll(5);
    }
});


