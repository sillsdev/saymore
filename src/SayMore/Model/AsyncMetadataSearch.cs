using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SIL.Reporting;

namespace SayMore.Model
{
	/// ----------------------------------------------------------------------------------------
	/// <summary>
	/// Runs a (potentially expensive) metadata search off the calling thread, so it doesn't
	/// block the UI while searching. If a newer search is started before an older one
	/// finishes, the older one's results are discarded instead of being applied out of order.
	/// </summary>
	/// ----------------------------------------------------------------------------------------
	public class AsyncMetadataSearch
	{
		// A transient fault (e.g. "Collection was modified") is expected to be rare and
		// short-lived - it means the search raced a concurrent field edit, not that the
		// search itself is broken - so a handful of quick retries is enough to ride it out
		// without risking a long-hung search if something is persistently wrong.
		private const int MaxAttempts = 3;
		private static readonly TimeSpan RetryDelay = TimeSpan.FromMilliseconds(50);

		private readonly Func<string, IEnumerable<string>> _search;
		private readonly Action<Action> _runOnUiThread;
		private int _generation;

		/// ------------------------------------------------------------------------------------
		/// <param name="search">The (synchronous, potentially slow) search function, e.g.
		/// MetadataSearch.SearchSessions. Called on a background thread.</param>
		/// <param name="runOnUiThread">Marshals an action back to the UI thread (e.g. via
		/// Control.BeginInvoke). In production, this must actually dispatch back to the UI
		/// thread, since onResults callbacks passed to Search are expected to touch UI state.</param>
		/// ------------------------------------------------------------------------------------
		public AsyncMetadataSearch(Func<string, IEnumerable<string>> search,
			Action<Action> runOnUiThread)
		{
			_search = search;
			_runOnUiThread = runOnUiThread;
		}

		/// ------------------------------------------------------------------------------------
		/// <summary>
		/// Starts a new search for the given query. Supersedes (and discards the results of)
		/// any previous search still in flight. onResults is invoked, via the UI-thread
		/// dispatcher given to the constructor, with the matching ids - but only if no newer
		/// search has been started in the meantime. Returns the background task, mainly so
		/// tests can wait for it; production callers can ignore the return value.
		/// </summary>
		/// ------------------------------------------------------------------------------------
		public Task Search(string query, Action<HashSet<string>> onResults)
		{
			var generation = ++_generation;
			return SearchAttempt(query, onResults, generation, 1);
		}

		/// ------------------------------------------------------------------------------------
		private Task SearchAttempt(string query, Action<HashSet<string>> onResults,
			int generation, int attempt)
		{
			// A search superseded by a newer one has nothing left to retry for - the newer
			// search (or none at all) already owns the user's attention. Re-checked here (not
			// just after a fault below) because a retry can sit behind RetryDelay long enough
			// for a newer search to have started in the meantime.
			if (generation != _generation)
				return Task.CompletedTask;

			return Task.Run(() => new HashSet<string>(_search(query))).ContinueWith(task =>
			{
				if (task.IsFaulted)
				{
					if (generation != _generation)
						return Task.CompletedTask;

					// Running this scan on a background thread means it can race with the UI
					// thread adding/removing a field while the user edits it (e.g. typing into
					// a previously-empty field), which can throw (e.g. "Collection was
					// modified"). That's rare and expected to be transient, so retry a few
					// times before giving up - if the user already finished typing what they
					// wanted to search for, there might be no further keystroke to retry on.
					if (attempt < MaxAttempts)
					{
						return Task.Delay(RetryDelay)
							.ContinueWith(_ => SearchAttempt(query, onResults, generation, attempt + 1))
							.Unwrap();
					}

					// Retries exhausted; log it, and give an attached debugger a chance to break
					// right here - but stay quiet (no dialog) for everyone else, since this
					// isn't worth alarming an end user, or an automated test run, over.
					Logger.WriteEvent("Handled exception during metadata search: {0}",
						task.Exception?.ToString());
					if (System.Diagnostics.Debugger.IsAttached)
						System.Diagnostics.Debugger.Break();
					return Task.CompletedTask;
				}

				_runOnUiThread(() =>
				{
					if (generation == _generation)
						onResults(task.Result);
				});

				return Task.CompletedTask;
			}).Unwrap();
		}
	}
}
