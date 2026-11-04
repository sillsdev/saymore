using NUnit.Framework;
using System.Runtime.Serialization;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SayMoreTests.UI.ElementListScreen
{
    [TestFixture]
    public class ElementListScreenTests
    {
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

        [Test]
        public void LoadElementList_FiltersGridToMatchingIds_WhenSearchRequestedFires()
        {
            // Create two sessions (uses the obsolete ctor available for tests)
            var s1 = new SayMore.Model.Session();
            var s2 = new SayMore.Model.Session();

            // Set their private id field via reflection
            var idField = typeof(SayMore.Model.ProjectElement).GetField("_id", BindingFlags.Instance | BindingFlags.NonPublic);
            idField.SetValue(s1, "match123");
            idField.SetValue(s2, "other");

            var repo = new TestRepo(new[] { s1, s2 });

            // Create an ElementListViewModel<T> instance without invoking its ctor, and inject our test repo
            var vm = (SayMore.UI.ElementListScreen.ElementListViewModel<SayMore.Model.Session>)
                FormatterServices.GetUninitializedObject(typeof(SayMore.UI.ElementListScreen.ElementListViewModel<SayMore.Model.Session>));
            var repoField = typeof(SayMore.UI.ElementListScreen.ElementListViewModel<SayMore.Model.Session>)
                .GetField("_repository", BindingFlags.Instance | BindingFlags.NonPublic);
            repoField.SetValue(vm, repo);

            // Create the concrete screen and ensure it has an ElementGrid instance
            // (localization manager is irrelevant to the filtering behavior under test,
            // and is never dereferenced since the control's handle is never created here)
            var screen = new SayMore.UI.ElementListScreen.ConcreteSessionScreen(vm, null);
            var grid = new SayMore.UI.ElementListScreen.ElementGrid();

            // Our mock Sessions have no real MetaDataFile (they're built with the
            // "For Mocking Only" constructor), so ElementGrid.GetValueForField would
            // throw a NullReferenceException if the grid tried to read a cell value to
            // measure row height. Disabling row auto-sizing avoids that measurement pass;
            // it has no bearing on the filtering behavior this test actually verifies.
            grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

            var gridField = screen.GetType().GetField("_elementsGrid", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            gridField.SetValue(screen, grid);

            // Prepare a ListPanel and initialize the screen so it subscribes to SearchRequested
            var listPanel = new SayMore.UI.LowLevelControls.ListPanel();
            screen.GetType().GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Invoke(screen, new object[] { new Panel(), new SayMore.UI.ElementListScreen.ComponentFileGrid(), listPanel });

            // Set the search text to a value that should match only s1
            listPanel.SearchText = "match";

            // Simulate typing (starts the debounce timer) and then the timer elapsing
            // (which is what actually raises SearchRequested).
            listPanel.HandleSearchTextBoxKeyUp(listPanel, new KeyEventArgs(Keys.A));
            listPanel.HandleSearchTimerTick(listPanel, System.EventArgs.Empty);

            // Get the items currently in the grid and assert filtering applied
            var items = grid.Items.Cast<SayMore.Model.ProjectElement>().ToList();
            Assert.AreEqual(1, items.Count, "Filtered list should contain exactly one matching element");
            Assert.AreEqual("match123", items[0].Id);
        }
    }
}
