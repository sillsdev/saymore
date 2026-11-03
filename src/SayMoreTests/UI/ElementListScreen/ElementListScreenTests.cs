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
        }

        [Test]
        public void ElementListScreen_Reloads_FilteredList_When_SearchRequested_Fires()
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
            var screen = new SayMore.UI.ElementListScreen.ConcreteSessionScreen(vm);
            var grid = new SayMore.UI.ElementListScreen.ElementGrid();
            var gridField = screen.GetType().GetField("_elementsGrid", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            gridField.SetValue(screen, grid);

            // Prepare a ListPanel and initialize the screen so it subscribes to SearchRequested
            var listPanel = new SayMore.UI.LowLevelControls.ListPanel();
            screen.GetType().GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Invoke(screen, new object[] { new Panel(), new SayMore.UI.ElementListScreen.ComponentFileGrid(), listPanel });

            // Set the search text to a value that should match only s1
            listPanel.SearchText = "match";

            // Invoke the private keyup handler to simulate typing (this will raise SearchRequested)
            var mi = typeof(SayMore.UI.LowLevelControls.ListPanel)
                .GetMethod("_searchTextBox_KeyUp", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(mi);
            mi.Invoke(listPanel, new object[] { listPanel, new KeyEventArgs(Keys.A) });

            // Get the items currently in the grid and assert filtering applied
            var items = grid.Items.Cast<SayMore.Model.ProjectElement>().ToList();
            Assert.AreEqual(1, items.Count, "Filtered list should contain exactly one matching element");
            Assert.AreEqual("match123", items[0].Id);
        }
    }
}
