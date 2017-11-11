 /**
  * 
  * @param {string} expr 
  * @param {string[]} delimiters 
  * @returns {number[]}
  */
 function parseNumbers(expr, delimiters) {
     return expr.split(delimiters).map(Number);
 }

 /**
 * @param {string} text The string
 */
 function parseDelimiters(text) {
     let result = {
         delimiters: "",
         processedText: text
     }
     if (text.startsWith("//") && text.length > 2) {
         result.delimiters = text[2];
         result.processedText = text.split("\n")[1];
     }
     else {
         result.delimiters = /[\n,]+/;
     }

     return result;
 }
 
 /**
  * 
  * @param {number[]} numbers 
  */
 function failOnNegatives(numbers) {
     var negatives = numbers.filter(n => n < 0);
     if (negatives.length > 0){
         throw new Error("negatives not allowed: " + negatives.join(", "));
     }
 }

export default class StringCalculator {
    /**
     * 
     * @param {string} expr 
     */
    add(expr) {
        let {delimiters, processedText} = parseDelimiters(expr);
        var parsedNumbers = parseNumbers(processedText, delimiters);

        failOnNegatives(parsedNumbers);
        return parsedNumbers.length > 0 
            ? parsedNumbers.reduce((a, b) => a + b)
            : 0;  
    };
};