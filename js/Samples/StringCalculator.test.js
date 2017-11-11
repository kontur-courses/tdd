import StringCalculator from "./StringCalculator";
import test from 'mocha-cases'
import "chai/register-should";
import "chai/register-expect";

describe("StringCalculator", () => {
    var calc = new StringCalculator();
    
    it("Return zero on empty input", function(){
        var res = calc.add("");
        res.should.be.equal(0);
    });

    test({
        name: "Sum coma separated numbers of {value} is {expected}", 
        values: ["", "42", "42,13", "1,2,3,4,5"],
        expected: [0, 42, 55, 15] 
    }, calc.add);

    test({
        name: "Sum newline separated numbers of {value} is {expected}", 
        value: "1\n2\n3",
        expected: 6 
    }, calc.add);

    test({
        name: "Sum numbers separated by both comma and newline of {value} is {expected}", 
        value: "1\n2,3",
        expected: 6 
    }, calc.add);

    test({
        name: "Sum delimiter from first special line of {value} is {expected}", 
        values: ["//;\n1;2;3", "//|\n4|5|61"],
        expected: [6, 70]
    }, calc.add);

    it("Throw exception on negative numbers", function(){
        expect(()=>calc.add("-1")).to.throw();
    });
    
    it("List all negatives in exception message when fail", function(){
        expect(()=>calc.add("-1,2,-5,-100,8")).to.throw("negatives not allowed: -1, -5, -100");
    });
});
