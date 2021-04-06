import "chai/register-should";
import {beginAndEndWithReporting} from "./infrastructure/reportingTest";

class Game {
    constructor() {
        this.frameIndex = 0;
        this.history = [
            {rolls: []}
        ];
    }

    roll(pins) {
        const rolls = this.history[this.frameIndex].rolls;
        rolls.push(pins);
        if (rolls.length > 1) {
            this.frameUp();
        }
    }

    frameUp() {
        this.frameIndex++;
        this.history[this.frameIndex] = {rolls: []};
    }

    getScore() {
        return this.history.reduce((accumulator, currentValue)=>{
            let current = currentValue.rolls.reduce((a, b) => a + b, 0);

            if (currentValue.rolls.length === 2 && current === 10) { // spare
                current ++;
            }

            return accumulator + current;
        }, 0);
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

    it("Два spare подряд");
    it("strike");
    it("double strike");
    it("all strikes");
    it("Третий бросок в последнем фрейме");

    beginAndEndWithReporting();
});
