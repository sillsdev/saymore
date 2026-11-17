using NUnit.Framework;
using System;
using System.Runtime.Serialization;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using L10NSharp;

namespace SayMoreTests.UI.ElementListScreen
{
    [TestFixture]
    public class ElementListScreenTests
    {
        [SetUp]
        public void Setup()
        {
            LocalizationManager.StrictInitializationMode = false;
        }

        class TestRepo : SayMore.Model.ElementRepository<SayMore.Model.Session>
        {
            private readonly IEnumerable<SayMore.Model.Session> _items;
            [System.Obsolete("For Mocking Only")]
            public TestRepo(IEnumerable<SayMore.Model.Session> items) : base() { _items = items; }
            public override IEnumerable<SayMore.Model.Session> AllItems => _items;

            // ElementGrid.SetFileType needs a non-null FileType to build its columns from.
            public override SayMore.Model.Files.FileType ElementFileType =>
                new SayMore.Model.Files.FileType("Test", path => true);
        }

        // Wraps a sequence and counts how many times it is actually enumerated, so a test
        // can detect code that re-enumerates (and so re-sorts/re-filters) a deferred LINQ
        // query instead of reusing an already-materialized result.
        private class CountingEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> _inner;
            public int EnumerationCount;
            public CountingEnumerable(IEnumerable<T> inner) { _inner = inner; }
            public IEnumerator<T> GetEnumerator()
            {
                EnumerationCount++;
                return _inner.GetEnumerator();
            }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }

        // Builds an ElementListScreen<Session> (by default a ConcreteSessionScreen) wired up
        // with a real ListPanel and ElementGrid, backed by the given sessions, ready for
        // LoadElementList/search tests. A caller that needs a different subclass (e.g. to
        // observe DoAsyncMetadataSearch calls) can supply screenFactory.
        private static (SayMore.UI.ElementListScreen.ElementListScreen<SayMore.Model.Session> screen,
            SayMore.UI.LowLevelControls.ListPanel listPanel,
            SayMore.UI.ElementListScreen.ElementGrid grid) CreateScreen(
                params SayMore.Model.Session[] sessions)
        {
            return CreateScreen((IEnumerable<SayMore.Model.Session>)sessions);
        }

        private static (SayMore.UI.ElementListScreen.ElementListScreen<SayMore.Model.Session> screen,
            SayMore.UI.LowLevelControls.ListPanel listPanel,
            SayMore.UI.ElementListScreen.ElementGrid grid) CreateScreen(
                IEnumerable<SayMore.Model.Session> sessions,
                Func<SayMore.UI.ElementListScreen.ElementListViewModel<SayMore.Model.Session>,
                    SayMore.UI.ElementListScreen.ElementListScreen<SayMore.Model.Session>> screenFactory = null)
        {
            var repo = new TestRepo(sessions);

            // Create an ElementListViewModel<T> instance without invoking its ctor, and inject our test repo
            var vm = (SayMore.UI.ElementListScreen.ElementListViewModel<SayMore.Model.Session>)
                FormatterServices.GetUninitializedObject(typeof(SayMore.UI.ElementListScreen.ElementListViewModel<SayMore.Model.Session>));
            var repoField = typeof(SayMore.UI.ElementListScreen.ElementListViewModel<SayMore.Model.Session>)
                .GetField("_repository", BindingFlags.Instance | BindingFlags.NonPublic);
            repoField.SetValue(vm, repo);

            // Create the screen and ensure it has an ElementGrid instance (localization
            // manager is irrelevant to the filtering behavior under test, and is never
            // dereferenced since the control's handle is never created here)
            var screen = screenFactory != null
                ? screenFactory(vm)
                : new SayMore.UI.ElementListScreen.ConcreteSessionScreen(vm, null);
            var grid = new SayMore.UI.ElementListScreen.ElementGrid();
            var gridField = screen.GetType().GetField("_elementsGrid", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            gridField.SetValue(screen, grid);

            // Prepare a ListPanel and initialize the screen so it subscribes to SearchRequested
            var listPanel = new SayMore.UI.LowLevelControls.ListPanel();
            screen.GetType().GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Invoke(screen, new object[] { new Panel(), new SayMore.UI.ElementListScreen.ComponentFileGrid(), listPanel });

            return (screen, listPanel, grid);
        }

        // Records DoAsyncMetadataSearch calls, so tests can verify the base class invokes
        // this hook (rather than touching metadata-search state directly) exactly when a
        // non-blank search is active.
        private class RecordingDoAsyncMetadataSearchScreen :
            SayMore.UI.ElementListScreen.ElementListScreen<SayMore.Model.Session>
        {
            public int CallCount;
            public string LastSearchParam;
            public object LastItemToSelect;

            public RecordingDoAsyncMetadataSearchScreen(
                SayMore.UI.ElementListScreen.ElementListViewModel<SayMore.Model.Session> vm)
                : base(vm, null) { }

            protected override void DoAsyncMetadataSearch(string searchParam, object itemToSelectAfterLoad)
            {
                CallCount++;
                LastSearchParam = searchParam;
                LastItemToSelect = itemToSelectAfterLoad;
            }
        }

        private static SayMore.Model.Session CreateSessionWithId(string id)
        {
            var session = new SayMore.Model.Session();
            var idField = typeof(SayMore.Model.ProjectElement).GetField("_id", BindingFlags.Instance | BindingFlags.NonPublic);
            idField.SetValue(session, id);
            return session;
        }

        [Test]
        public void LoadElementList_FiltersGridToMatchingIds_WhenSearchRequestedFires()
        {
            var s1 = CreateSessionWithId("match123");
            var s2 = CreateSessionWithId("other");
            var (screen, listPanel, grid) = CreateScreen(s1, s2);

            // Set the search text to a value that should match only s1 (this starts the
            // debounce timer, same as if the user had typed, pasted, or dropped it in).
            listPanel.SearchText = "match";

            // Simulate the timer elapsing, which is what actually raises SearchRequested.
            listPanel.HandleSearchTimerTick(listPanel, System.EventArgs.Empty);

            // Get the items currently in the grid and assert filtering applied
            var items = grid.Items.Cast<SayMore.Model.ProjectElement>().ToList();
            Assert.AreEqual(1, items.Count, "Filtered list should contain exactly one matching element");
            Assert.AreEqual("match123", items[0].Id);
        }

        [Test]
        public void LoadElementList_CalledWithoutExplicitSearchParamWhileSearchActive_FilterStaysApplied()
        {
            var s1 = CreateSessionWithId("match123");
            var s2 = CreateSessionWithId("other");
            var (screen, listPanel, grid) = CreateScreen(s1, s2);

            // Activate a search, as the user would by typing.
            listPanel.SearchText = "match";
            listPanel.HandleSearchTimerTick(listPanel, System.EventArgs.Empty);

            // Simulate a call site (e.g. AfterNewItemAdded, DeleteSelectedElements,
            // HandleParentFormActivated) that reloads the list without passing the
            // current search text along - this should NOT clear the active filter.
            var loadMethod = screen.GetType().GetMethod("LoadElementList",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy,
                null, new[] { typeof(object) }, null);
            loadMethod.Invoke(screen, new object[] { s1 });

            var items = grid.Items.Cast<SayMore.Model.ProjectElement>().ToList();
            Assert.AreEqual(1, items.Count,
                "Reloading without an explicit search param should preserve the active search filter, " +
                "not silently revert to the full list while the search box still shows the query.");
            Assert.AreEqual("match123", items[0].Id);
        }

        [Test]
        public void LoadElementList_EnumeratesModelElementsOnlyOnce()
        {
            var s1 = CreateSessionWithId("a");
            var s2 = CreateSessionWithId("b");
            var counting = new CountingEnumerable<SayMore.Model.Session>(new[] { s1, s2 });
            var (screen, listPanel, grid) = CreateScreen(counting);

            Assert.AreEqual(1, counting.EnumerationCount,
                "_elementsGrid.Items = items already materializes the OrderBy/Where query via " +
                "ToList(); re-checking items.Any() afterwards re-enumerates (and re-sorts/re-filters) " +
                "the same deferred query from scratch instead of reusing the materialized grid items.");
        }

        [Test]
        public void ShouldAutoResizeGridColumns_SearchActiveAndNoSavedGridSettings_ReturnsFalse()
        {
            // Without this exclusion, a user who hasn't persisted column widths gets a full
            // column-width remeasurement pass over the filtered rows on every debounced
            // search keystroke (AutoResizeColumns previously ran whenever GridSettings was
            // null, with no regard for whether the reload was search-triggered).
            var (screen, _, _) = CreateScreen(CreateSessionWithId("a"));

            var method = screen.GetType().GetMethod("ShouldAutoResizeGridColumns",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            Assert.IsFalse((bool)method.Invoke(screen, new object[] { "some search text" }));
        }

        [Test]
        public void ShouldAutoResizeGridColumns_NoActiveSearchAndNoSavedGridSettings_ReturnsTrue()
        {
            var (screen, _, _) = CreateScreen(CreateSessionWithId("a"));

            var method = screen.GetType().GetMethod("ShouldAutoResizeGridColumns",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            Assert.IsTrue((bool)method.Invoke(screen, new object[] { null }));
        }

        [Test]
        public void LoadElementList_MetadataMatchWithoutIdSubstringMatch_StillIncludesElement()
        {
            // Characterizes the OR-of-two-ways-to-match behavior (by metadata, or by ID
            // substring) before collapsing the duplicated if/else that implements it.
            var s1 = CreateSessionWithId("alpha");
            var s2 = CreateSessionWithId("beta");
            var (screen, listPanel, grid) = CreateScreen(s1, s2);

            // "xyz" (3+ chars) only starts the debounce timer rather than firing
            // SearchRequested immediately, so it won't trigger a reload (and overwrite our
            // injected _metadataSearchMatchingIds below) until we invoke LoadElementList
            // ourselves.
            listPanel.SearchText = "xyz";

            var matchingIdsField = screen.GetType().GetField("_metadataSearchMatchingIds",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            matchingIdsField.SetValue(screen, new HashSet<string> { "alpha" });

            var loadMethod = screen.GetType().GetMethod("LoadElementList",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy,
                null, new[] { typeof(object) }, null);
            loadMethod.Invoke(screen, new object[] { null });

            var items = grid.Items.Cast<SayMore.Model.ProjectElement>().ToList();
            Assert.AreEqual(1, items.Count,
                "An element found only by metadata match (not by ID substring) should still be included.");
            Assert.AreEqual("alpha", items[0].Id);
        }

        [Test]
        public void ViewActivated_SearchBoxNotFocused_ShowsPlaceholder()
        {
            // A pre-existing WinForms quirk (disabling a container that holds focus bypasses
            // the normal Leave event) can leave the search box's placeholder stuck hidden
            // after a view is deactivated and reactivated, even though nothing has focus.
            // ViewActivated should re-check and correct that.
            var (screen, listPanel, grid) = CreateScreen(CreateSessionWithId("a"));
            listPanel.ShowSearchBar = true;

            var searchTextBoxField = typeof(SayMore.UI.LowLevelControls.ListPanel)
                .GetField("_searchTextBox", BindingFlags.Instance | BindingFlags.NonPublic);
            var searchTextBox = (SayMore.UI.LowLevelControls.PromptTextBox)searchTextBoxField.GetValue(listPanel);
            var labelField = typeof(SayMore.UI.LowLevelControls.PromptTextBox)
                .GetField("_labelPrompt", BindingFlags.Instance | BindingFlags.NonPublic);
            var label = (System.Windows.Forms.Label)labelField.GetValue(searchTextBox);

            label.Visible = false;

            screen.ViewActivated(false);

            Assert.IsTrue(label.Visible);
        }

        [Test]
        public void LoadElementList_ItemToSelectDoesNotMatchActiveFilter_ClearsFilterAndSelectsItem()
        {
            var existing = CreateSessionWithId("match123");
            var sessions = new List<SayMore.Model.Session> { existing };
            var (screen, listPanel, grid) = CreateScreen(sessions);

            listPanel.SearchText = "match";
            listPanel.HandleSearchTimerTick(listPanel, System.EventArgs.Empty);

            var newItem = CreateSessionWithId("brandNew");
            // Simulate the repository now containing the item the caller wants selected.
            sessions.Add(newItem);

            // This is the same call pattern used by AfterNewItemAdded and by
            // SessionsListScreen's "new from files"/"new from recording" handlers: passing a
            // ProjectElement that an active, unrelated filter would otherwise hide (and that
            // ElementGrid.SelectElement would otherwise throw an ArgumentException trying to
            // select, since it isn't among the filtered rows).
            var loadMethod = screen.GetType().GetMethod("LoadElementList",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy,
                null, new[] { typeof(object) }, null);
            loadMethod.Invoke(screen, new object[] { newItem });

            Assert.AreEqual(string.Empty, listPanel.SearchText,
                "A newly added item whose auto-generated ID has nothing to do with an active " +
                "search filter should not be silently hidden (or cause SelectElement to throw) " +
                "- clear the filter instead.");

            var items = grid.Items.Cast<SayMore.Model.ProjectElement>().ToList();
            Assert.IsTrue(items.Any(x => x.Id == "brandNew"),
                "The item should be visible in the grid once the filter that would have hidden " +
                "it is cleared.");
        }

        [Test]
        public void LoadElementList_ItemToSelectMatchesActiveFilter_LeavesFilterInPlace()
        {
            var existing = CreateSessionWithId("match123");
            var sessions = new List<SayMore.Model.Session> { existing };
            var (screen, listPanel, grid) = CreateScreen(sessions);

            listPanel.SearchText = "match";
            listPanel.HandleSearchTimerTick(listPanel, System.EventArgs.Empty);

            var newItem = CreateSessionWithId("match456");
            // Simulate the repository now containing the item the caller wants selected.
            sessions.Add(newItem);

            var loadMethod = screen.GetType().GetMethod("LoadElementList",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy,
                null, new[] { typeof(object) }, null);
            loadMethod.Invoke(screen, new object[] { newItem });

            Assert.AreEqual("match", listPanel.SearchText,
                "A newly added item that already matches the active filter should not have " +
                "the filter cleared out from under the user.");
        }

        [Test]
        public void LoadElementList_IndexBeyondFilteredRowCount_SelectsLastRowInsteadOfThrowing()
        {
            // DeleteSelectedElements reselects a row by its pre-deletion index. If a search
            // filter is active, the post-reload filtered row count can end up smaller than
            // that index (e.g. the deleted row was the last matching one), and
            // ElementGrid.SelectElement(int) throws IndexOutOfRangeException unless the index
            // is clamped to a valid row first.
            var s1 = CreateSessionWithId("match1");
            var s2 = CreateSessionWithId("other");
            var (screen, listPanel, grid) = CreateScreen(s1, s2);

            listPanel.SearchText = "match";
            listPanel.HandleSearchTimerTick(listPanel, System.EventArgs.Empty);
            // The filtered grid now has exactly 1 row (index 0); index 1 is out of range for it.

            var loadMethod = screen.GetType().GetMethod("LoadElementList",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy,
                null, new[] { typeof(object) }, null);
            loadMethod.Invoke(screen, new object[] { 1 });

            Assert.AreEqual("match1", grid.GetCurrentElement().Id,
                "An out-of-range index should be clamped to the last available row rather than throwing.");
        }

        [Test]
        public void LoadElementList_SearchActive_CallsDoAsyncMetadataSearchHookWithPinnedSearchParam()
        {
            var s1 = CreateSessionWithId("match123");
            var (screen, listPanel, grid) = CreateScreen(new[] { s1 },
                vm => new RecordingDoAsyncMetadataSearchScreen(vm));
            var recordingScreen = (RecordingDoAsyncMetadataSearchScreen)screen;

            listPanel.SearchText = "match";
            listPanel.HandleSearchTimerTick(listPanel, System.EventArgs.Empty);

            Assert.AreEqual(1, recordingScreen.CallCount,
                "A non-blank search should invoke the DoAsyncMetadataSearch hook once per reload, " +
                "so a derived class can run its own (e.g. off-UI-thread) search without the base " +
                "class needing to know anything about it.");
            Assert.AreEqual("match", recordingScreen.LastSearchParam);
            Assert.IsNull(recordingScreen.LastItemToSelect);
        }

        [Test]
        public void LoadElementList_NoActiveSearch_DoesNotCallDoAsyncMetadataSearchHook()
        {
            var s1 = CreateSessionWithId("match123");
            var (screen, listPanel, grid) = CreateScreen(new[] { s1 },
                vm => new RecordingDoAsyncMetadataSearchScreen(vm));
            var recordingScreen = (RecordingDoAsyncMetadataSearchScreen)screen;

            Assert.AreEqual(0, recordingScreen.CallCount,
                "The hook should not be invoked (e.g. during Initialize's initial load) when " +
                "no search is active.");
        }
    }
}
