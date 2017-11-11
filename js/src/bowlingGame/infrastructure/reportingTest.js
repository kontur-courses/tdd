import * as Tests from "../bowlingGame.test";
import ResultReporter from "./resultReporter";
import jsonfile from "jsonfile";
import fs from "fs";

let currentRunTests = [];
let now = new Date().toISOString();
const resultsFileName = "results.json";

describe("Bowling game", () => {
    Tests.default();

    before(function () {
        if (Tests.Names === "ENTER YOUR NAME HERE") {
            throw new Error("Please enter your name in bowlingGame.test.js");
        }
        if (fs.existsSync(resultsFileName))
            currentRunTests = jsonfile.readFileSync(resultsFileName);;
    });
    after(function () {
        var reporter = new ResultReporter();
        currentRunTests = currentRunTests.filter(x => x.LastRunTime === now);
        reporter.reportResults(currentRunTests, Tests.Names);
    });
    beforeEach(function () {

    });
    afterEach(function () {
        let testName = this.currentTest.fullTitle();
        let foundTest = currentRunTests.find(x => x.TestName === testName);
        if (foundTest) {
            foundTest.LastRunTime = now;
            foundTest.Succeeded = this.currentTest.state === "passed";
            return;
        }
        currentRunTests.push(
            {
                FirstRunTime: now,
                LastRunTime: now,
                TestName: testName,
                TestMethod: this.currentTest.title,
                Succeeded: this.currentTest.state === "passed"
            });
        jsonfile.writeFileSync(resultsFileName, currentRunTests);
    });
});
