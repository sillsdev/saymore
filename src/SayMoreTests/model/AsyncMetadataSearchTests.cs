using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using SayMore.Model;

namespace SayMoreTests.Model
{
	[TestFixture]
	public class AsyncMetadataSearchTests
	{
		[Test]
		public void Search_InvokesSearchFunctionOffCallingThread()
		{
			var callingThreadId = Thread.CurrentThread.ManagedThreadId;
			int? searchThreadId = null;
			var done = new ManualResetEventSlim();

			var search = new AsyncMetadataSearch(
				query =>
				{
					searchThreadId = Thread.CurrentThread.ManagedThreadId;
					return new[] { "x" };
				},
				// Run the "UI thread" dispatch synchronously, right here, for this test.
				action => action());

			search.Search("q", ids => done.Set());

			Assert.IsTrue(done.Wait(TimeSpan.FromSeconds(5)), "Search did not complete in time.");
			Assert.AreNotEqual(callingThreadId, searchThreadId,
				"The search function should run off the thread that called Search, not block it.");
		}

		[Test]
		public void Search_DeliversResultsThroughRunOnUiThreadDispatcher_NotDirectly()
		{
			bool dispatcherInvoked = false;
			var done = new ManualResetEventSlim();

			var search = new AsyncMetadataSearch(
				query => new[] { "x" },
				action =>
				{
					dispatcherInvoked = true;
					action();
				});

			search.Search("q", ids => done.Set());

			Assert.IsTrue(done.Wait(TimeSpan.FromSeconds(5)), "Search did not complete in time.");
			Assert.IsTrue(dispatcherInvoked,
				"Results should always be delivered through the injected UI-thread dispatcher, " +
				"never called directly from the background thread.");
		}

		[Test]
		public void Search_SupersededByNewerSearch_DiscardsStaleResults()
		{
			var firstSearchStarted = new ManualResetEventSlim();
			var releaseFirstSearch = new ManualResetEventSlim();
			var receivedResults = new List<HashSet<string>>();
			var done = new ManualResetEventSlim();

			var search = new AsyncMetadataSearch(
				query =>
				{
					if (query == "first")
					{
						firstSearchStarted.Set();
						// Block until the test lets this (now-superseded) search finish,
						// simulating a slow metadata scan that outlives a newer keystroke.
						Assert.IsTrue(releaseFirstSearch.Wait(TimeSpan.FromSeconds(5)));
						return new[] { "stale-result" };
					}
					return new[] { "fresh-result" };
				},
				action => action());

			search.Search("first", ids => receivedResults.Add(ids));
			Assert.IsTrue(firstSearchStarted.Wait(TimeSpan.FromSeconds(5)));

			search.Search("second", ids =>
			{
				receivedResults.Add(ids);
				done.Set();
			});
			Assert.IsTrue(done.Wait(TimeSpan.FromSeconds(5)), "Second search did not complete in time.");

			releaseFirstSearch.Set();
			Thread.Sleep(200); // give the stale search's continuation a chance to (not) call back

			Assert.AreEqual(1, receivedResults.Count,
				"Results from the superseded ('first') search should be discarded.");
			Assert.That(receivedResults[0], Is.EquivalentTo(new[] { "fresh-result" }));
		}

		[Test]
		public void Search_SearchFunctionAlwaysThrows_GivesUpWithoutPropagatingOrInvokingOnResults()
		{
			// A metadata scan running concurrently with the user editing a field (e.g. adding
			// text to a previously-empty field) can race with the underlying
			// List<FieldInstance> mutation and throw (e.g. InvalidOperationException:
			// "Collection was modified"). Search retries a few times (see the test below) in
			// case that was just transient, but if it never succeeds, this must still not
			// crash or hang - it should just give up quietly.
			bool onResultsCalled = false;

			var search = new AsyncMetadataSearch(
				query => throw new InvalidOperationException(
					"Collection was modified; enumeration operation may not execute."),
				action => action());

			Task backgroundTask = null;
			Assert.DoesNotThrow(() => backgroundTask = search.Search("q", ids => onResultsCalled = true));

			Assert.IsTrue(backgroundTask.Wait(TimeSpan.FromSeconds(5)),
				"Search's returned task did not complete in time.");
			Assert.IsFalse(onResultsCalled,
				"onResults should never be invoked when the search function never succeeds.");
		}

		[Test]
		public void Search_FailsOnFirstAttemptThenSucceeds_RetriesAndDeliversResults()
		{
			// If the user finished typing before the search's one and only attempt happened
			// to lose a rare race (see above), there might be no further keystroke to trigger
			// another search - so a transient failure must retry on its own rather than
			// silently leaving the list showing stale/incomplete results forever.
			int attempts = 0;
			HashSet<string> delivered = null;
			var done = new ManualResetEventSlim();

			var search = new AsyncMetadataSearch(
				query =>
				{
					attempts++;
					if (attempts == 1)
					{
						throw new InvalidOperationException(
							"Collection was modified; enumeration operation may not execute.");
					}
					return new[] { "result-after-retry" };
				},
				action => action());

			search.Search("q", ids =>
			{
				delivered = ids;
				done.Set();
			});

			Assert.IsTrue(done.Wait(TimeSpan.FromSeconds(5)), "Search did not complete in time.");
			Assert.That(delivered, Is.EquivalentTo(new[] { "result-after-retry" }));
			Assert.GreaterOrEqual(attempts, 2, "The search function should have been retried after failing once.");
		}

		[Test]
		public void Search_SupersededWhileRetryingAfterFailure_StopsRetryingTheStaleQuery()
		{
			var firstAttemptFailed = new ManualResetEventSlim();
			var receivedResults = new List<HashSet<string>>();
			var done = new ManualResetEventSlim();
			int firstQueryAttempts = 0;

			var search = new AsyncMetadataSearch(
				query =>
				{
					if (query == "first")
					{
						Interlocked.Increment(ref firstQueryAttempts);
						firstAttemptFailed.Set();
						throw new InvalidOperationException(
							"Collection was modified; enumeration operation may not execute.");
					}
					return new[] { "fresh-result" };
				},
				action => action());

			search.Search("first", ids => receivedResults.Add(ids));
			Assert.IsTrue(firstAttemptFailed.Wait(TimeSpan.FromSeconds(5)));

			// Supersede it before it has a chance to retry.
			search.Search("second", ids =>
			{
				receivedResults.Add(ids);
				done.Set();
			});
			Assert.IsTrue(done.Wait(TimeSpan.FromSeconds(5)), "Second search did not complete in time.");

			var attemptsAtSupersession = firstQueryAttempts;
			Thread.Sleep(300); // give any further (unwanted) retries of "first" a chance to happen

			Assert.AreEqual(attemptsAtSupersession, firstQueryAttempts,
				"A superseded search should stop retrying, not keep retrying a query nothing cares about anymore.");
			Assert.AreEqual(1, receivedResults.Count, "Only the superseding search's results should be delivered.");
			Assert.That(receivedResults[0], Is.EquivalentTo(new[] { "fresh-result" }));
		}
	}
}
