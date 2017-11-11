import ResultPoster from "./resultPoster";
import jsonfile from "jsonfile";
import fs from "fs";
import { AUTHORS } from "../yourName";


export function beginAndEndWithReporting() {
    let currentRunTests = [];
    let now = new Date().toISOString();
    const resultsFileName = "results.json";

    before(() => {
        if (fs.existsSync(resultsFileName))
            currentRunTests = jsonfile.readFileSync(resultsFileName);
    });

    after(() => {
        if (!AUTHORS) {
            throw new Error("Enter your surnames at yourName.js in AUTHORS constant");
        }
        jsonfile.writeFileSync(resultsFileName, currentRunTests);
        const reporter = new ResultPoster();
        currentRunTests = currentRunTests.filter(x => x.LastRunTime === now);
        reporter.writeAsync(AUTHORS, currentRunTests);
    });

    afterEach(() => {
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
    });
}
