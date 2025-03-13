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
        public void ItemData_Constructor_InvalidId_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ItemMasterTable.ItemData(0, "Item1"));
            Assert.Throws<ArgumentException>(() => new ItemMasterTable.ItemData(-1, "Item1"));
        }

        [Test]
        public void ItemData_Constructor_InvalidName_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ItemMasterTable.ItemData(1, null));
            Assert.Throws<ArgumentException>(() => new ItemMasterTable.ItemData(1, string.Empty));
            Assert.Throws<ArgumentException>(() => new ItemMasterTable.ItemData(1, "   "));
        }

        [Test]
        public void Add_ValidItem_AddsToList()
        {
            var table = new ItemMasterTable();
            var item = new ItemMasterTable.ItemData(1, "Sword");
            table.Add(item);
            Assert.That(table.Items.Count, Is.EqualTo(1));
            Assert.That(table.Items[0], Is.EqualTo(item));
        }

        [Test]
        public void Add_NullItem_ThrowsArgumentNullException()
        {
            var table = new ItemMasterTable();
            Assert.Throws<ArgumentNullException>(() => table.Add(null));
        }

        [Test]
        public void Add_DuplicateItemId_ThrowsArgumentException()
        {
            var table = new ItemMasterTable();
            table.Add(new ItemMasterTable.ItemData(1, "Sword"));
            Assert.Throws<ArgumentException>(() => table.Add(new ItemMasterTable.ItemData(1, "Shield")));
        }


        [Test]
        public void Append_ValidItemMasterTable_AppendsItems()
        {
            var table1 = new ItemMasterTable();
            table1.Add(new ItemMasterTable.ItemData(1, "Item1"));
            var table2 = new ItemMasterTable();
            table2.Add(new ItemMasterTable.ItemData(2, "Item2"));

            table1.Append(table2);

            Assert.That(table1.Items.Count, Is.EqualTo(2));
            Assert.That(table1.Find(1), Is.Not.Null);
            Assert.That(table1.Find(2), Is.Not.Null);
        }

        [Test]
        public void Append_NullMasterTable_ThrowsArgumentNullException()
        {
            var table = new ItemMasterTable();
            Assert.Throws<ArgumentNullException>(() => table.Append(null));
        }

        [Test]
        public void Append_InvalidMasterTableType_ThrowsArgumentException()
        {
            var table = new ItemMasterTable();
            var invalidTable = new InvalidMasterTable(); // Assuming InvalidMasterTable exists and inherits from MasterTable_Base
            Assert.Throws<ArgumentException>(() => table.Append(invalidTable));
        }

        [Test]
        public void Append_DuplicateItemId_ThrowsArgumentException()
        {
            var table1 = new ItemMasterTable();
            table1.Add(new ItemMasterTable.ItemData(1, "Sword"));
            var table2 = new ItemMasterTable();
            table2.Add(new ItemMasterTable.ItemData(1, "Shield"));

            Assert.Throws<ArgumentException>(() => table1.Append(table2));
        }


        [Test]
        public void Append_EmptyTable_AppendsSuccessfully()
        {
            var table1 = new ItemMasterTable();
            var table2 = new ItemMasterTable();
            table1.Append(table2);
            Assert.That(table1.Items.Count, Is.EqualTo(0));
        }


        [Test]
        public void Find_ExistingId_ReturnsItemData()
        {
            var table = new ItemMasterTable();
            var item = new ItemMasterTable.ItemData(1, "Sword");
            table.Add(item);

            var foundItem = table.Find(1);

            Assert.That(foundItem, Is.Not.Null);
            Assert.That(foundItem.GetId(), Is.EqualTo(1));
        }

        [Test]
        public void Find_NonExistingId_ReturnsNull()
        {
            var table = new ItemMasterTable();
            var item = new ItemMasterTable.ItemData(1, "Sword");
            table.Add(item);

            var foundItem = table.Find(2);

            Assert.That(foundItem, Is.Null);
        }

        [Test]
        public void Find_InvalidId_ThrowsArgumentException()
        {
            var table = new ItemMasterTable();
            Assert.Throws<ArgumentException>(() => table.Find(0));
            Assert.Throws<ArgumentException>(() => table.Find(-1));
        }

        [Test]
        public void Find_EmptyTable_ReturnsNull()
        {
            var table = new ItemMasterTable();
            var foundItem = table.Find(1);
            Assert.That(foundItem, Is.Null);
        }

        [Test]
        public void GetId_ReturnsCorrectId()
        {
            var itemData = new ItemData(123, "Test Item");
            Assert.That(itemData.GetId(), Is.EqualTo(123));
        }

        [Test]
        public void ItemData_LongName_DoesNotThrowException()
        {
            string longName = new string('a', 1000); // Create a string with 1000 characters
            var itemData = new ItemMasterTable.ItemData(1, longName);
            Assert.That(itemData.Name, Is.EqualTo(longName));
        }

        // Dummy class for testing Append with invalid type
        private class InvalidMasterTable : MasterTable_Base
        {
            public override void Append(MasterTable_Base data) { }
            public override MasterData_Base Find(int id) => null;
        }
    }
}