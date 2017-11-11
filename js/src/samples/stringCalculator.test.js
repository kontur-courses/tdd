import StringCalculator from "./stringCalculator";
import "chai/register-assert";

describe("StringCalculator should", () => {
    const calc = new StringCalculator();
    
    it("return zero on empty input", function(){
        const result = calc.add("");
        assert.equal(result, 0);
    });

    describe("sum coma separated numbers", () => {
        [
            {values: "", expected: 0},
            {values: "42", expected: 42},
            {values: "42,13", expected: 55},
            {values: "1,2,3,4,5", expected: 15},
        ].forEach(({values, expected}) => {
            it(`from "${values}"`, () => {
                assert.equal(calc.add(values), expected);
            });
        });
    });

    describe("sum newline separated numbers", () => {
        [
            {values: "1\n2\n3", expected: 6}
        ].forEach(({values, expected}) => {
            it(`from "${values}"`, () => {
                assert.equal(calc.add(values), expected);
            });
        });
    });

    describe("sum numbers separated by both comma and newline", () => {
        [
            {values: "1\n2,3", expected: 6}
        ].forEach(({values, expected}) => {
            it(`from "${values}"`, () => {
                assert.equal(calc.add(values), expected);
            });
        });
    });

    describe("sum delimiter from first special line", () => {
        [
            {values: "//;\n1;2;3", expected: 6},
            {values: "//|\n4|5|61", expected: 70}
        ].forEach(({values, expected}) => {
            it(`from "${values}"`, () => {
                assert.equal(calc.add(values), expected);
            });
        });
    });

    it("throw exception on negative numbers", function(){
        assert.throws(()=>calc.add("-1"));
    });
    
    it("list all negatives in exception message when fail", function(){
        assert.throws(()=>calc.add("-1,2,-5,-100,8"), "-1, -5, -100");
    });
});
