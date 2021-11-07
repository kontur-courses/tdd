import StringCalculator from "./stringCalculator";
import "chai/register-assert";

describe("StringCalculator", () => {
    const calc = new StringCalculator();

    it("returns zero on empty input", () => {
        const result = calc.add("");
        assert.equal(result, 0);
    });

    context("when coma separated numbers", () => {
        [
            {numbers: "", expected: 0},
            {numbers: "42", expected: 42},
            {numbers: "42,13", expected: 55},
            {numbers: "1,2,3,4,5", expected: 15},
        ].forEach(({numbers, expected}) => {
            it(`sums from "${numbers}"`, () => {
                assert.equal(calc.add(numbers), expected);
            });
        });
    });

    context("when newline separated numbers", () => {
        [
            {numbers: "1\n2\n3", expected: 6}
        ].forEach(({numbers, expected}) => {
            it(`sums from "${numbers}"`, () => {
                assert.equal(calc.add(numbers), expected);
            });
        });
    });

    context("when delimiter from first special line", () => {
        [
            {numbers: "//;\n1;2;3", expected: 6},
            {numbers: "//|\n4|5|61", expected: 70}
        ].forEach(({numbers, expected}) => {
            it(`sums from "${numbers}"`, () => {
                assert.equal(calc.add(numbers), expected);
            });
        });
    });

    it("throws exception on negative numbers", () => {
        assert.throws(() => calc.add("-1"));
    });

    it("lists all negatives in exception message when fail", () => {
        assert.throws(() => {
            calc.add("-1,2,-5,-100,8");
        }, "-1, -5, -100");
    });
});
