using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

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
            var policy = Retry.On<WebException>().For(3).With((context) =>
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
            var policy = Retry.On<WebException>().Until(handle => m_Generator.Next(5) == 3).With((context) =>
            {
                var client = new WebClient();
                client.DownloadData(invalidUrl);
            });
        }
    }
}
