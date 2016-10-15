using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FireSharp;
using FireSharp.Config;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace BowlingGame
{
    public enum SimpleTestStatus
    {
        Unknown = 0,
        Passed = TestStatus.Passed,
        Failed = TestStatus.Failed
    }

    public class ReportingTest<TTestClass>
    {
        private static readonly object locker = new object();
        private static readonly string resultsFile = typeof(TTestClass).Name + ".txt";
        private static Dictionary<string, SimpleTestStatus> results;

        [OneTimeSetUp]
        public void ClearLocalResults()
        {
            results = new Dictionary<string, SimpleTestStatus>();
        }


        [TearDown]
        public static void WriteLastRunResult()
        {

            var test = TestContext.CurrentContext.Test;
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var  simpleStatus = status == TestStatus.Passed ? SimpleTestStatus.Passed
                    : status == TestStatus.Failed ? SimpleTestStatus.Failed
                    : SimpleTestStatus.Unknown;
            results[test.MethodName] = 
                results.ContainsKey(test.MethodName) && results[test.MethodName] == SimpleTestStatus.Failed
                ? SimpleTestStatus.Failed
                : simpleStatus;
        }

        private static void SaveResults(Dictionary<string, SimpleTestStatus> testsSuccess)
        {
            File.WriteAllLines(resultsFile, testsSuccess.Select(kv => $"{kv.Key} {kv.Value}"));
        }

        private static Dictionary<string, SimpleTestStatus> LoadResults()
        {
            try
            {
                return File.ReadAllLines(resultsFile)
                    .Select(line => line.Split(' '))
                    .ToDictionary(
                        line => line[0],
                        line => (SimpleTestStatus)Enum.Parse(typeof(SimpleTestStatus), line[1]));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new Dictionary<string, SimpleTestStatus>();
            }
        }

        [OneTimeTearDown]
        public static void ReportResults()
        {
            var names = typeof(TTestClass).GetField("Names").GetValue(null);
            var updatedResults = UpdateResults();
            Console.WriteLine(names);
            if (updatedResults.Count == 0) return;
            foreach (var tuple in updatedResults)
            {
                Console.WriteLine(tuple);
            }
            var config = new FirebaseConfig
            {
                BasePath = "https://testing-challenge.firebaseio.com/bowling/"
            };
            using (var client = new FirebaseClient(config))
            {
                client.Set(names + "/tests", updatedResults.ToDictionary(kv => kv.Key, kv => kv.Value.ToString().ToLower()));
            }
            Console.WriteLine("reported");
        }

        private static Dictionary<string, SimpleTestStatus> UpdateResults()
        {
            var testMethods = typeof(TTestClass).GetMethods()
                .Where(
                    m =>
                        m.GetCustomAttributes<TestAttribute>().Any() ||
                        m.GetCustomAttributes<TestCaseAttribute>().Any())
                .ToList();
            Dictionary<string, SimpleTestStatus> newResults;
            lock (locker)
            {
                var prevResults = LoadResults();
                newResults = testMethods.ToDictionary(
                    m => m.Name,
                    m => results.ContainsKey(m.Name) ? results[m.Name] 
                    : prevResults.ContainsKey(m.Name) ? prevResults[m.Name]
                    : SimpleTestStatus.Unknown);
                SaveResults(newResults);
            }
            return newResults;
        }
    }
}