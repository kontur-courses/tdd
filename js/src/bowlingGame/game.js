import "chai/register-should";
import {beginAndEndWithReporting} from "./infrastructure/reportingTest";

class Game {
    constructor() {
        this.frameIndex = 0;
        this.history = [
            this.getEmptyFrame()
        ];
    }

    getEmptyFrame() {
        return {
            rolls: [],
            score: 0,
            bonus: 0,
            spare: false,
            strike: false,
        }
    }

    getRollsSum(rolls) {
        return rolls.reduce((prev, current) => prev + current, 0);
    }


    roll(pins) {
        const frame = this.history[this.frameIndex];
        frame.rolls.push(pins);
        frame.score = this.getRollsSum(frame.rolls);
        if (frame.score >= 10 || frame.rolls.length > 1) {
            frame.spare = frame.score === 10 && frame.rolls.length === 2;
            frame.strike = frame.score === 10 && frame.rolls.length === 1;
            this.history[++this.frameIndex] = this.getEmptyFrame();
        }
    }

    getScore() {
        this.history.forEach((frame, index) => {
            const nextFrame = this.history[index + 1];
            if (!nextFrame) {
                return true;
            }
            if (frame.spare) {
                frame.bonus = nextFrame.rolls[0] || 0;
            }
            if (frame.strike) {
                frame.bonus = this.getRollsSum(nextFrame.rolls);
            }
        })
        return this.history.reduce((prev, current) => {
            return prev + current.score + current.bonus;
        }, 0)
    }

    debug() {
        console.table(this.history);
    }
}

describe("Боулинг", () => {
    it("В начале игры должно быть 0 очков", () => {
        const game = new Game();
        game.getScore().should.be.eq(0);
    });

    it("После броска в 10 очков должно быть 10 очков", () => {
        const game = new Game();
        game.roll(10);
        game.getScore().should.be.eq(10);
    });

    it("После двух простых бросков на 2 и 5 сумма очков 7", () => {
        const game = new Game();
        game.roll(2);
        game.roll(5);
        game.getScore().should.be.eq(7);
    });

    it("После броска 2+8 (spare) и 3 во в следующем фрейме должно быть 16 очков", () => {
        const game = new Game();
        game.roll(2);
        game.roll(8);
        game.roll(3);
        game.getScore().should.be.eq(16);
    });

    it("Два spare подряд", () => {
        const game = new Game();

        game.roll(3);
        game.roll(7); // 10 + 5

        game.roll(5);
        game.roll(5); // + 10 + 4

        game.roll(4);
        game.roll(6); // + 10

        game.getScore().should.be.eq(39);
    });

    it("strike", () => {
        const game = new Game();
        game.roll(1);
        game.roll(4); // + 5

        game.roll(10); // + 10 + 4 + 2

        game.roll(4);
        game.roll(2); // + 6

        game.getScore().should.be.eq(27);
    });

    it("double strike");
    it("all strikes");
    it("Третий бросок в последнем фрейме");

    beginAndEndWithReporting();
});
