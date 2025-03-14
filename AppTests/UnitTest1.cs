using InMemoryTable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using static InMemoryTable.ItemMasterTable;

namespace InMemoryTableTests
{
    public class ItemMasterTableTests
    {
        [Test]
        public void ItemData_Constructor_ValidInput_CreatesInstance()
        {
            var itemData = new ItemMasterTable.ItemData(1, "Item1");
            Assert.That(itemData.Id, Is.EqualTo(1));
            Assert.That(itemData.Name, Is.EqualTo("Item1"));
        }

        [Test]
        public void ItemData_Constructor_InvalidId_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ItemMasterTable.ItemData(0, "Item1"));
            Assert.Throws<ArgumentOutOfRangeException>(() => new ItemMasterTable.ItemData(-1, "Item1"));
        }

        [Test]
        public void ItemData_Constructor_InvalidName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ItemMasterTable.ItemData(1, null));
            Assert.Throws<ArgumentException>(() => new ItemMasterTable.ItemData(1, string.Empty));
            Assert.Throws<ArgumentException>(() => new ItemMasterTable.ItemData(1, "   "));
        }

        [Test]
        public void ItemMasterTable_Constructor_ValidInput_CreatesInstance()
        {
            var table = new ItemMasterTable();
            Assert.That(table, Is.Not.Null);
        }

        [Test]
        public void ItemMasterTable_ConstructorWithItems_ValidInput_CreatesInstanceWithItems()
        {
            var items = new List<ItemData>
            {
                new ItemData(1, "Item1"),
                new ItemData(2, "Item2")
            };
            var table = new ItemMasterTable(items);

            Assert.That(table.Find(1), Is.Not.Null);
            Assert.That(table.Find(2), Is.Not.Null);
        }

        [Test]
        public void ItemMasterTable_ConstructorWithItems_NullItems_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ItemMasterTable(null));
        }


        [Test]
        public void ItemMasterTable_Constructor_DuplicateId_ThrowsArgumentException()
        {
            var items = new List<ItemData>
            {
                new ItemData(1, "Item1"),
                new ItemData(1, "Item2") // Duplicate ID
            };
            Assert.Throws<ArgumentException>(() => new ItemMasterTable(items));
        }

        [Test]
        public void Append_ValidItemMasterTable_AppendsItems()
        {
            var table1 = new ItemMasterTable(new List<ItemData>() { new ItemData(1, "Item1") });
            var table2 = new ItemMasterTable(new List<ItemData>() { new ItemData(2, "Item2") });

            table1.Append(table2);

            Assert.That(table1.Find(1), Is.Not.Null);
            Assert.That(table1.Find(2), Is.Not.Null);
        }

        [Test]
        public void Append_NullData_ThrowsArgumentNullException()
        {
            var table = new ItemMasterTable();
            Assert.Throws<ArgumentNullException>(() => table.Append(null));
        }

        [Test]
        public void Append_InvalidDataType_ThrowsArgumentException()
        {
            var table = new ItemMasterTable();
            var invalidData = new InvalidMasterTable(); // Some other class inheriting MasterTable_Base
            Assert.Throws<ArgumentException>(() => table.Append(invalidData));
        }

        [Test]
        public void Append_DuplicateId_ThrowsArgumentException()
        {
            var table1 = new ItemMasterTable(new List<ItemData> { new ItemData(1, "Item1") });
            var table2 = new ItemMasterTable(new List<ItemData> { new ItemData(1, "Item2") }); // Duplicate ID

            Assert.Throws<ArgumentException>(() => table1.Append(table2));
        }

        [Test]
        public void Find_ExistingId_ReturnsItemData()
        {
            var table = new ItemMasterTable(new List<ItemData> { new ItemData(1, "Item1") });
            var foundItem = table.Find(1) as ItemMasterTable.ItemData;
            Assert.That(foundItem, Is.Not.Null);
            Assert.That(foundItem.Id, Is.EqualTo(1));
            Assert.That(foundItem.Name, Is.EqualTo("Item1"));
        }

        [Test]
        public void Find_NonExistingId_ReturnsNull()
        {
            var table = new ItemMasterTable();
            var foundItem = table.Find(1);
            Assert.That(foundItem, Is.Null);
        }

        [Test]
        public void Find_InvalidId_ThrowsArgumentOutOfRangeException()
        {
            var table = new ItemMasterTable();
            Assert.Throws<ArgumentOutOfRangeException>(() => table.Find(0));
        }

        [Test]
        public void Append_EmptyTable_SuccessfullyAppends()
        {
            var table1 = new ItemMasterTable();
            var table2 = new ItemMasterTable(new List<ItemData> { new ItemData(1, "Item1") });
            table1.Append(table2);
            Assert.That(table1.Find(1), Is.Not.Null);

        }

        [Test]
        public void Append_ToFilledTable_SuccessfullyAppends()
        {
            var table1 = new ItemMasterTable(new List<ItemData> {
                new ItemData(1,"Item1"),
                new ItemData(2, "Item2")
                }
            );
            var table2 = new ItemMasterTable(new List<ItemData> { new ItemData(3, "Item3") });

            table1.Append(table2);

            Assert.That(table1.Find(3), Is.Not.Null);

        }

        // Helper class for testing invalid Append scenario
        private class InvalidMasterTable : MasterTable_Base
        {
            public override void Append(MasterTable_Base data)
            {
                throw new NotImplementedException();
            }

            public override MasterData_Base Find(int id)
            {
                throw new NotImplementedException();
            }
        }
    }
}