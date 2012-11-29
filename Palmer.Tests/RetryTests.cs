using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Diagnostics;

namespace Palmer.Tests
{
    [TestClass]
    public class RetryTests
    {
        private static Random m_Generator = new Random();

        [TestMethod]
        [ExpectedException(typeof(RetryException))]
        public void GivenInvalidUrlWebExceptionRaisedThreeTimesThenRetryExceptionThrown()
        {
            var invalidUrl = "http://invalid";
            Retry.On<WebException>().For(3).With((context) =>
            {
                var client = new WebClient();
                client.DownloadData(invalidUrl);
            });
        }

        [TestMethod]
        [ExpectedException(typeof(RetryException))]
        public void GivenInvalidUrlWebExceptionRaisedUntilRandomConditionMet()
        {
            var invalidUrl = "http://invalid";
            Retry.On<WebException>().Until(handle => m_Generator.Next(5) == 3).With((context) =>
            {
                var client = new WebClient();
                client.DownloadData(invalidUrl);
            });
        }

        [TestMethod]
        public void GivenSimpleOperationResultCanBeReturned()
        {
            var result = Retry.On<Exception>().Indefinately().With((context) =>
                {
                    return 2 + 2;
                });

            Assert.AreEqual(4, result.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(RetryException))]
        public void GivenInvalidUrlWebExceptionRaisedAndSelectedByPredicate()
        {
            var invalidUrl = "http://invalid";
            Retry.On<WebException>(handle => handle.Context.LastException.Message == "The remote name could not be resolved: 'invalid'").For(2).With((context) =>
                {
                    var client = new WebClient();
                    client.DownloadData(invalidUrl);
                });
        }

        [TestMethod]
        [Ignore]
        public void GivenInvalidUrlWebExceptionGivesUpAfter15Seconds()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var invalidUrl = "http://invalid";
            try
            {
                Retry.On<WebException>().For(TimeSpan.FromSeconds(15)).With((context) =>
                {
                    var client = new WebClient();
                    client.DownloadData(invalidUrl);
                });
            }
            catch
            {
            }

            stopwatch.Stop();

            // Hard to truly test this, but you would think with execution overheads
            // that if you tell it to wait for ten seconds before giving up that
            // the total execution time would be slightly more than ten seconds.
            Assert.IsTrue(stopwatch.Elapsed > TimeSpan.FromSeconds(15), "Stop watch elapsed time exceeded allotted time.", stopwatch.Elapsed);
        }
    }
}
