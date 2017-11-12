import StringCalculator from "./stringCalculator";
import "chai/register-assert";

describe("StringCalculator should", () => {
    const calc = new StringCalculator();

    it("return zero on empty input", () => {
        const result = calc.add("");
        assert.equal(result, 0);
    });

    describe("sum coma separated numbers", () => {
        [
            {numbers: "", expected: 0},
            {numbers: "42", expected: 42},
            {numbers: "42,13", expected: 55},
            {numbers: "1,2,3,4,5", expected: 15},
        ].forEach(({numbers, expected}) => {
            it(`from "${numbers}"`, () => {
                assert.equal(calc.add(numbers), expected);
            });
        });
    });

    describe("sum newline separated numbers", () => {
        [
            {numbers: "1\n2\n3", expected: 6}
        ].forEach(({numbers, expected}) => {
            it(`from "${numbers}"`, () => {
                assert.equal(calc.add(numbers), expected);
            });
        });
    });

    describe("sum delimiter from first special line", () => {
        [
            {numbers: "//;\n1;2;3", expected: 6},
            {numbers: "//|\n4|5|61", expected: 70}
        ].forEach(({numbers, expected}) => {
            it(`from "${numbers}"`, () => {
                assert.equal(calc.add(numbers), expected);
            });
        });
    });

    it("throw exception on negative numbers", () => {
        assert.throws(() => calc.add("-1"));
    });

    it("list all negatives in exception message when fail", () => {
        assert.throws(() => {
            calc.add("-1,2,-5,-100,8");
        }, "-1, -5, -100");
    });
});
