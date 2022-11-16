export default class StringCalculator {
    add(expr) {
        const {delimiters, numbers} = this.parseDelimiters(expr);
        const parsedNumbers = this.parseNumbers(numbers, delimiters);
        this.failOnNegatives(parsedNumbers);
        return parsedNumbers.reduce((a, b) => a + b, 0);
    }

    parseDelimiters(expr) {
        if (!expr.startsWith("//") || expr.length <= 2) {
            return {
                delimiters: /[\n,]+/,
                numbers: expr
            };
        }
        return {
            delimiters: expr[2],
            numbers: expr.split("\n")[1]};
    }

    parseNumbers(numbers, delimiters) {
        return numbers !== ""
            ? numbers.split(delimiters).map(n => parseInt(n))
            : [];
    }

    failOnNegatives(numbers) {
        const negatives = numbers.filter(n => n < 0);
        if (negatives.length > 0){
            throw new Error("negatives not allowed: "
                + negatives.join(", "));
        }
    }
}
