import * as Tests from "../bowlingGame.test";
import ResultReporter from "./resultPoster";
import jsonfile from "jsonfile";
import fs from "fs";
import { AUTHORS } from "../yourName";

let currentRunTests = [];
let now = new Date().toISOString();
const resultsFileName = "results.json";

describe("Bowling game", () => {
    Tests.default();

    before(function () {
        if (fs.existsSync(resultsFileName))
            currentRunTests = jsonfile.readFileSync(resultsFileName);
    });
    after(function () {
        if (!AUTHORS) {
            throw new Error("Enter your surnames at yourName.js in AUTHORS constant");
        }
        const reporter = new ResultReporter();
        currentRunTests = currentRunTests.filter(x => x.LastRunTime === now);
        reporter.writeAsync(AUTHORS, currentRunTests);
    });
    beforeEach(function () {
    });
    afterEach(function () {
        const testName = this.currentTest.fullTitle();
        const foundTest = currentRunTests.find(x => x.TestName === testName);
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
