using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Diagnostics;
using System.Threading;

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
            Retry.On<WebException>().For(3).With((context) =>
            {
                throw new WebException();
            });
        }

        [TestMethod]
        [ExpectedException(typeof(RetryException))]
        public void GivenInvalidUrlWebExceptionRaisedUntilRandomConditionMet()
        {
            var counter = 0;

            Retry.On<WebException>().Until(handle => counter == 3).With((context) =>
            {
                counter++;
                throw new WebException();
            });
        }

        [TestMethod]
        public void GivenSimpleOperationResultCanBeReturned()
        {
            var result = Retry.On<Exception>().Indefinitely().With((context) =>
                {
                    return 2 + 2;
                });

            Assert.AreEqual(4, result.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(RetryException))]
        public void GivenInvalidUrlWebExceptionRaisedAndSelectedByPredicate()
        {
            Retry.On<WebException>(handle => handle.Context.LastException.Message == "The remote name could not be resolved: 'invalid'").For(2).With((context) =>
                {
                    throw new WebException("The remote name could not be resolved: 'invalid'");
                });
        }

        [TestMethod]
        public void GivenInvalidUrlWebExceptionGivesUpAfter1Seconds()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                Retry.On<WebException>().For(TimeSpan.FromSeconds(1)).With((context) =>
                {
                    throw new WebException();
                });
            }
            catch(RetryException)
            {
                stopwatch.Stop();

                // Hard to truly test this, but you would think with execution overheads
                // that if you tell it to wait for ten seconds before giving up that
                // the total execution time would be slightly more than ten seconds.
                Assert.IsTrue(
                    IsWithinAcceptableTolerances(stopwatch, 1000, 10),
                    "Stop watch elapsed time was below minimum time, the time was '{0}'.",
                    stopwatch.Elapsed
                    );
            }
        }

        private bool IsWithinAcceptableTolerances(Stopwatch stopwatch, int duration, int tolerance)
        {
            return stopwatch.Elapsed > TimeSpan.FromMilliseconds(duration - tolerance) && stopwatch.Elapsed < TimeSpan.FromMilliseconds(duration + tolerance);
        }

        [TestMethod]
        public void GivenInitiallyFailingCodeWillExecuteUntilSuccessful()
        {
            var timesToFail = 100;

            Retry.On<Exception>().Indefinitely().With((context) =>
                {
                    if (timesToFail == 0)
                    {
                        return;
                    }
                    else
                    {
                        timesToFail--;
                        throw new Exception();
                    }
                });
        }

        [TestMethod]
        public void GivenPredicateFailureWillExecuteUntilSuccessful()
        {
            var counter = 0;

            Retry.On(handle => handle.Occurences < 100).Indefinitely().With(context =>
                {
                    counter++;
                }
                );

            Assert.AreEqual(101, counter);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GivenCodeThatThrowsUnexpectedExceptionNoRetryExceptionIsThrown()
        {
            Retry.On<WebException>().Indefinitely().With(context =>
                {
                    throw new InvalidOperationException();
                });
        }

        [TestMethod]
        public void GivenRetryPolicyWithMultipleExceptionsSuccessfullyRetriesMultipleTimes()
        {
            var retryCount = 0;

            var result = Retry
                .On<InvalidOperationException>().For(5)
                .AndOn<ArgumentOutOfRangeException>().For(5)
                .With(context =>
                    {
                        retryCount++;
                        
                        if (retryCount < 10)
                        {
                            if (retryCount % 2 == 0)
                            {
                                throw new InvalidOperationException();
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException();
                            }
                        }
                    });
        }
    }
}
