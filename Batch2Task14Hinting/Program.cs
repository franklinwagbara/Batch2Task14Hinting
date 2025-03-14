using System;
using System.Collections.Generic;
using System.Linq;

namespace InMemoryTable
{
    public class ItemMasterTable : MasterTable_Base
    {
        [Serializable]
        public class ItemData : MasterData_Base
        {
            public int Id { get; }
            public string Name { get; }

            public ItemData(int id, string name)
            {
                if (id <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero.");
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
                }

                Id = id;
                Name = name;
            }
        }

        private readonly List<ItemData> _items = new();

        public ItemMasterTable() { }

        public ItemMasterTable(IEnumerable<ItemData> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items), "Items cannot be null.");
            }

            foreach (var item in items)
            {
                if (_items.Any(existingItem => existingItem.Id == item.Id))
                {
                    throw new ArgumentException($"An item with Id {item.Id} already exists.", nameof(items));
                }
                _items.Add(item);
            }
        }

        /// <summary>
        /// Appends items from another MasterTable_Base instance if it is of type ItemMasterTable.
        /// </summary>
        /// <param name="data">The MasterTable_Base instance to append data from.</param>
        /// <exception cref="ArgumentException">Thrown if the data type is invalid.</exception>
        public override void Append(MasterTable_Base data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Data to append cannot be null.");
            }

            if (!(data is ItemMasterTable itemTable))
            {
                throw new ArgumentException("Data must be of type ItemMasterTable.", nameof(data));
            }

            foreach (ItemData item in itemTable._items)
            {
                if (_items.Any(existingItem => existingItem.Id == item.Id))
                {
                    throw new ArgumentException($"An item with Id {item.Id} already exists.", nameof(data));
                }
                _items.Add(item);
            }
        }

        /// <summary>
        /// Finds an ItemData object by its Id from the internal list of items.
        /// </summary>
        /// <param name="id">The Id of the ItemData to find.</param>
        /// <returns>The ItemData object if found, otherwise null.</returns>
        public override MasterData_Base Find(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than zero.");
            }

            return _items.Find(item => item.Id == id);
        }
    }

    [Serializable]
    public abstract class MasterTable_Base
    {
        public abstract class MasterData_Base
        {
        }

        public abstract void Append(MasterTable_Base data);

        public abstract MasterData_Base Find(int id);
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var items = new List<ItemMasterTable.ItemData>
            {
                new ItemMasterTable.ItemData(1, "Item1"),
                new ItemMasterTable.ItemData(2, "Item2")
            };
            var table = new ItemMasterTable(items);
            Console.WriteLine((table.Find(1) as ItemMasterTable.ItemData).Name);
            Console.WriteLine((table.Find(2) as ItemMasterTable.ItemData).Name);
        }
    }
}