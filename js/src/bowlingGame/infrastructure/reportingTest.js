import ResultPoster from "./resultPoster";
import jsonfile from "jsonfile";
import fs from "fs";
import { AUTHORS } from "../yourName";


export function beginAndEndWithReporting() {
    let currentRunTests = [];
    let now = new Date().toISOString();
    const resultsFileName = "temp/results.json";

    before(function () {
        if (fs.existsSync(resultsFileName))
            currentRunTests = jsonfile.readFileSync(resultsFileName);
    });

    after(function () {
        if (!AUTHORS) {
            throw new Error("Enter your surnames at yourName.js in AUTHORS constant");
        }
        jsonfile.writeFileSync(resultsFileName, currentRunTests);
        const reporter = new ResultPoster();
        currentRunTests = currentRunTests.filter(x => getTimeDiffInSeconds(now, x.LastRunTime) < 5*60);
        reporter.writeAsync(AUTHORS, currentRunTests);
    });

    afterEach(function () {
        const testFullName = this.currentTest.fullTitle();
        const foundTest = currentRunTests.find(x => x.TestName === testFullName);
        if (foundTest) {
            foundTest.LastRunTime = now;
            foundTest.Succeeded = this.currentTest.state === "passed";
            return;
        }
        currentRunTests.push(
            {
                FirstRunTime: now,
                LastRunTime: now,
                TestName: testFullName,
                TestMethod: this.currentTest.title,
                Succeeded: this.currentTest.state === "passed"
            });
    });

    function getTimeDiffInSeconds(dateString1, dateString2) {
        const t2 = new Date(dateString2).getTime();
        const t1 = new Date(dateString1).getTime();
        return Math.ceil(Math.abs(t2 - t1)/1000);
    }
}
