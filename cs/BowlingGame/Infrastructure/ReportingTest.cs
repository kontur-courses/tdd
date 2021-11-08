using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace BowlingGame.Infrastructure
{
    public class ReportingTest<TTestClass>
    {
        private static readonly string resultsFileName = typeof(TTestClass).Name + ".json";
        private static string resultsFile;
        private static List<TestCaseStatus> tests;

        [OneTimeSetUp]
        public void ClearLocalResults()
        {
            resultsFile = Path.Combine(TestContext.CurrentContext.TestDirectory, resultsFileName);
            tests = LoadResults();
        }

        [TearDown]
        public static void WriteLastRunResult()
        {
            var test = TestContext.CurrentContext.Test;
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var succeeded = status == TestStatus.Passed;

            var testName = test.Name;
            if (!test.Name.Contains(test.MethodName))
                testName = test.MethodName + " " + test.Name;

            var testStatus = tests.FirstOrDefault(t => t.TestName == testName);
            if (testStatus != null)
            {
                testStatus.LastRunTime = DateTime.Now;
                testStatus.Succeeded = succeeded;
            }
            else
                tests.Add(new TestCaseStatus
                {
                    FirstRunTime = DateTime.Now,
                    LastRunTime = DateTime.Now,
                    TestName = testName,
                    TestMethod = test.MethodName,
                    Succeeded = succeeded
                });
        }

        private static void SaveResults(List<TestCaseStatus> tests)
        {
            File.WriteAllText(resultsFile, JsonConvert.SerializeObject(tests, Formatting.Indented));
        }

        private static List<TestCaseStatus> LoadResults()
        {
            try
            {
                var json = File.ReadAllText(resultsFile);
                var statuses = JsonConvert.DeserializeObject<List<TestCaseStatus>>(json);
                return RemoveOldNames(statuses);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<TestCaseStatus>();
            }
        }

        private static List<TestCaseStatus> RemoveOldNames(List<TestCaseStatus> statuses)
        {
            var names = new HashSet<string>(typeof(TTestClass).GetMethods().Select(m => m.Name));
            return statuses.Where(s => names.Contains(s.TestMethod)).ToList();
        }

        [OneTimeTearDown]
        public static void ReportResults()
        {
            tests = tests.OrderByDescending(t => t.LastRunTime).ThenByDescending(t => t.FirstRunTime).ToList();
            SaveResults(tests);

            foreach (var kv in tests)
                Console.WriteLine(kv.TestName);

            if (string.IsNullOrWhiteSpace(YourName.Authors))
                throw new Exception("Enter your surnames at YourName.cs in AUTHORS constant");

            using (var client = Firebase.CreateClient())
            {
                string authorsKey = MakeFirebaseSafe(YourName.Authors);
                client.Set(authorsKey, new
                {
                    tests,
                    time = DateTime.Now.ToUniversalTime(),
                    lang = "cs"
                });
            }
            Console.WriteLine("reported");
        }

        private static string MakeFirebaseSafe(string s)
        {
            var badChars = Enumerable.Range(0, 32).Select(code => (char)code)
                .Concat(".$#[]/" + (char)127);
            foreach (var badChar in badChars)
                s = s.Replace(badChar, '_');
            return s;
        }
    }
}