export default class StringCalculator {
    add(expr) {
        const {delimiters, processedText} = parseDelimiters(expr);
        const parsedNumbers = parseNumbers(processedText, delimiters);
        failOnNegatives(parsedNumbers);
        return parsedNumbers.reduce((a, b) => a + b, 0);
    };
};

function parseNumbers(expr, delimiters) {
    return expr.split(delimiters).map(Number);
}

function parseDelimiters(text) {
    if (!text.startsWith("//") || text.length <= 2) {
        return {delimiters: /[\n,]+/, processedText: text};
    }
    return {delimiters: text[2], processedText: text.split("\n")[1]};
}

function failOnNegatives(numbers) {
    const negatives = numbers.filter(n => n < 0);
    if (negatives.length > 0){
        throw new Error("negatives not allowed: " + negatives.join(", "));
    }
}
