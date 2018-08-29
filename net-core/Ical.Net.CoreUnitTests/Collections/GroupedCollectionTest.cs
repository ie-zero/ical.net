using System.Collections.Generic;
using System.Linq;
using Ical.Net.Collections;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace Ical.Net.CoreUnitTests.Collections
{
    [TestFixture]
    public class GroupedCollectionTest
    {
        [Test]
        public void NewlyCreatedCollectionShouldBeEmpty()
        {
            var collection = new GroupedCollection<string, DummyGroupedItem>();
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void CountShouldCountElementsOnAllGroups()
        {
            // Arrange
            var collection = new GroupedCollection<string, DummyGroupedItem>();
            var items = new[] {
                new DummyGroupedItem() { Group = "GROUP_A" },
                new DummyGroupedItem() { Group = "GROUP_B" },
                new DummyGroupedItem() { Group = "GROUP_B" },
                new DummyGroupedItem() { Group = "GROUP_C" },
            };

            foreach (var item in items) { collection.Add(item); }

            // Act + Assert
            Assert.AreEqual(4, collection.Count);
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void GetItemByIndexShouldReturnNullWhenNotFound(int index)
        {
            var collection = new GroupedCollection<string, DummyGroupedItem>();
            Assert.Null(collection[index]);
        }

        [Test]
        public void GetItemByIndexShouldReturnAnObjectWhenFound()
        {
            // The output order is 'usually' the addition order but it is not guaranteed.

            // Arrange
            var collection = new GroupedCollection<string, DummyGroupedItem>();
            var items = new[] {
                new DummyGroupedItem() { Group = "GROUP_A", Id = 1 },
                new DummyGroupedItem() { Group = "GROUP_A", Id = 2 },
                new DummyGroupedItem() { Group = "GROUP_B", Id = 3 },
            };

            foreach (var item in items) { collection.Add(item); }

            // Act 
            DummyGroupedItem[] actual = {
                collection[2],
                collection[1],
                collection[0]
            };

            // Assert
            CollectionAssert.AreEquivalent(items, actual);
        }

        [Test]
        [TestCase(null)]
        [TestCase("GROUP_NOT_EXISTS")]
        public void GetItemsByGroupNameShouldReturnEmptyEnumerableWhenNotFound(string group)
        {
            // Arrange
            var collection = new GroupedCollection<string, DummyGroupedItem>();

            // Act + Assert
            Assert.IsEmpty(collection[group]);
            Assert.IsEmpty(collection.Values(group));
        }

        [Test]
        public void GetItemsByGroupNameShouldReturnGroupItems()
        {
            // Arrange
            var collection = new GroupedCollection<string, DummyGroupedItem>();
            var groupA = new[] {
                new DummyGroupedItem() { Group = "GROUP_A", Id = 1 },                
            };
            var groupB = new[] {
                new DummyGroupedItem() { Group = "GROUP_B", Id = 2 },
                new DummyGroupedItem() { Group = "GROUP_B", Id = 3 }
            };

            foreach (var item in groupA) { collection.Add(item); }
            foreach (var item in groupB) { collection.Add(item); }

            // Act 
            var actual = collection["GROUP_B"];

            // Assert
            CollectionAssert.AreEquivalent(groupB, actual);
        }

        [Test]
        public void AddItemShouldIncreaseGroupItemCount()
        {
            // Arrange
            var collection = new GroupedCollection<string, DummyGroupedItem>();
            var groupedItem = new DummyGroupedItem() { Group = "GROUP_NAME" };

            // Act 
            collection.Add(groupedItem);

            // Assert
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(1, collection.Values("GROUP_NAME").Count());
        }

        [Test]
        public void AddShouldPlaceItemOnRelevantGroup()
        {
            // Arrange
            var collection = new GroupedCollection<string, DummyGroupedItem>();
            var groupA = new[] {
                new DummyGroupedItem() { Group = "GROUP_A", Id = 1 },
                new DummyGroupedItem() { Group = "GROUP_A", Id = 2 }
            };
            var groupB = new[] {
                new DummyGroupedItem() { Group = "GROUP_B", Id = 3 }
            };

            // Act 
            foreach (var item in groupA) { collection.Add(item); }
            foreach (var item in groupB) { collection.Add(item); }

            // Assert
            var actualGroupA = collection.Values("GROUP_A");
            CollectionAssert.AreEquivalent(groupA, actualGroupA, "Failed on 'GROUP_A'");

            var actualGroupB = collection.Values("GROUP_B");
            CollectionAssert.AreEquivalent(groupB, actualGroupB, "Failed on 'GROUP_B'");
        }

        [Test]
        public void ClearShouldRemoveAllElements()
        {
            // Arrange
            var collection = new GroupedCollection<string, DummyGroupedItem>();
            var items = new[] {
                new DummyGroupedItem() { Group = "GROUP_A", Id = 1 },
                new DummyGroupedItem() { Group = "GROUP_B", Id = 2 }
            };

            foreach (var item in items) { collection.Add(item); }

            // Act
            collection.Clear();

            // Assert
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void ClearGroupShouldRemoveOnlyTheElementsOfSpecifiedGroup()
        {
            // Arrange
            var collection = new GroupedCollection<string, DummyGroupedItem>();
            var toRemain = new[] {
                new DummyGroupedItem() { Group = "GROUP_A", Id = 1 },
                new DummyGroupedItem() { Group = "GROUP_C", Id = 2 },
            };
            var toClear = new[] {
                new DummyGroupedItem() { Group = "GROUP_B", Id = 3 },
                new DummyGroupedItem() { Group = "GROUP_B", Id = 4 },
            };

            foreach (var item in toRemain) { collection.Add(item); }
            foreach (var item in toClear) { collection.Add(item); }

            // Act
            collection.Clear("GROUP_B");

            // Assert
            CollectionAssert.AreEquivalent(toRemain, collection);
        }

        [Test]
        [TestCase(null)]
        [TestCase("GROUP_NOT_EXISTS")]
        public void ContainsGroupShouldReturnFalseWhenNotFound(string group)
        {
            // Arrange
            var collection = new GroupedCollection<string, DummyGroupedItem>();

            // Act + Assert
            Assert.IsTrue(!collection.Contains(group));
        }

        [Test, TestCaseSource(nameof(ContainsItemNotFound_TestCases))]
        public void ContainsItemNotFound(DummyGroupedItem item)
        {
            // Arrange
            var collection = new GroupedCollection<string, DummyGroupedItem>();
            var elements = new[] {
                new DummyGroupedItem() { Group = "GROUP_A", Id = 1 },
                new DummyGroupedItem() { Group = "GROUP_B", Id = 2 },
                new DummyGroupedItem() { Group = "GROUP_B", Id = 3 },
            };

            foreach (var elem in elements) { collection.Add(elem); }

            // Act + Assert
            Assert.IsTrue(!collection.Contains(item));
        }

        public static IEnumerable<ITestCaseData> ContainsItemNotFound_TestCases()
        {
            yield return new TestCaseData(null)
                .SetName("ContainsItem_Null");

            yield return new TestCaseData(new DummyGroupedItem() { Group = "GROUP_NOT_EXISTS" })
                .SetName("ContainsItem_GroupNotFound");

            yield return new TestCaseData(new DummyGroupedItem() { Group = "GROUP_A", Id = 5 })
                .SetName("ContainsItem_ItemNotFound");
        }

        [Test]
        public void ContainsItemFound()
        {
            // Arrange
            var collection = new GroupedCollection<string, DummyGroupedItem>();
            var items = new[] {
                new DummyGroupedItem() { Group = "GROUP_A", Id = 1 },
                new DummyGroupedItem() { Group = "GROUP_B", Id = 2 },
                new DummyGroupedItem() { Group = "GROUP_C", Id = 3 },
            };
            var expected = new DummyGroupedItem() { Group = "GROUP_B", Id = 4 };

            foreach (var item in items) { collection.Add(item); }
            collection.Add(expected);

            // Act + Assert
            Assert.IsTrue(collection.Contains(expected));
        }

        [Test]
        public void RemoveItemShouldDecreaseGroupItemCount()
        {
            // Arrange
            var collection = new GroupedCollection<string, DummyGroupedItem>();
            var groupedItem = new DummyGroupedItem() { Group = "GROUP_NAME" };

            collection.Add(groupedItem);
            
            // Act 
            Assert.AreEqual(1, collection.Count);
            Assert.AreEqual(1, collection.Values("GROUP_NAME").Count());

            collection.Remove(groupedItem);

            // Assert
            Assert.AreEqual(0, collection.Count);
            Assert.AreEqual(0, collection.Values("GROUP_NAME").Count());
        }

        [Test]
        public void RemoveGroupShouldEliminateAllItemsInTheSpecifiedGroup()
        {
            Assert.Inconclusive("Consider what is the difference between Remove(TKey) and Clear(TKey)");
        }

        [Test]
        public void ImplementEnumeratorInterface()
        {
            // Arrange
            var collection = new GroupedCollection<string, DummyGroupedItem>();
            var items = new[] {
                new DummyGroupedItem() { Group = "GROUP_A", Id = 1 },
                new DummyGroupedItem() { Group = "GROUP_B", Id = 2 },
                new DummyGroupedItem() { Group = "GROUP_B", Id = 3 },
            };

            foreach (var item in items) { collection.Add(item); }

            // Act + Assert
            CollectionAssert.AreEquivalent(items, collection);
        }

        public class DummyGroupedItem : IGroupedObject<string>
        {
            public string Group { get; set; }
            public int Id { get; set; }
        }
    }
}
