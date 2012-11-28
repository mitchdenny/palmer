using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Palmer.Tests
{
    [TestClass]
    public class RetryTests
    {
        [TestMethod]
        public void Test1()
        {
            Retry.On<WebException>().For(5).With<int>(
                context => 1
                );

            Retry.On<WebException>().For(TimeSpan.FromSeconds(15)).With<int>(
                context => 1
                );

            Retry.On<WebException>().Until(x => 1 == 1).With(
                delegate(RetryContext context)
                {
                    Console.WriteLine("Hello World!");
                    Console.WriteLine("Good-bye World!");
                }
                );
        }
    }
}
