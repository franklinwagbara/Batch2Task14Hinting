using System;
using System.Collections.Generic;
using System.Linq;
using static InMemoryTable.ItemMasterTable;

namespace InMemoryTable
{
    [Serializable]
    public abstract class MasterTable_Base
    {
        public abstract class MasterData_Base
        {
            public abstract int GetId();
        }

        public abstract void Append(MasterTable_Base data);

        public abstract MasterData_Base Find(int id);
    }

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
                    throw new ArgumentException("Id must be a positive integer.", nameof(id));
                }
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
                }

                Id = id;
                Name = name;
            }

            public override int GetId()
            {
                return Id;
            }
        }


        private readonly List<ItemData> _items = new List<ItemData>();

        public IReadOnlyList<ItemData> Items => _items.AsReadOnly();

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

            foreach (var item in itemTable.Items)
            {
                if (_items.Any(existingItem => existingItem.Id == item.Id))
                {
                    throw new ArgumentException($"An item with Id {item.Id} already exists in the table.", nameof(data));
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
                throw new ArgumentException("Id must be a positive integer.", nameof(id));
            }

            return _items.FirstOrDefault(item => item.Id == id);
        }

        public void Add(ItemData item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            }
            if (_items.Any(existingItem => existingItem.Id == item.Id))
            {
                throw new ArgumentException($"An item with Id {item.Id} already exists in the table.", nameof(item));
            }

            _items.Add(item);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var itemMasterTable1 = new ItemMasterTable();
            itemMasterTable1.Add(new ItemData(1, "Item1"));
            itemMasterTable1.Add(new ItemData(2, "Item2"));
            itemMasterTable1.Add(new ItemData(3, "Item3"));

            var itemMasterTable2 = new ItemMasterTable();
            itemMasterTable2.Add(new ItemData(4, "Item4"));
            itemMasterTable2.Add(new ItemData(5, "Item5"));
            itemMasterTable2.Add(new ItemData(6, "Item6"));

            itemMasterTable1.Append(itemMasterTable2);

            Console.WriteLine((itemMasterTable1.Find(1) as ItemData).Name);
            Console.WriteLine((itemMasterTable1.Find(2) as ItemData).Name);
            Console.WriteLine((itemMasterTable1.Find(3) as ItemData).Name);
            Console.WriteLine((itemMasterTable1.Find(4) as ItemData).Name);
            Console.WriteLine((itemMasterTable1.Find(5) as ItemData).Name);
            Console.WriteLine((itemMasterTable1.Find(6) as ItemData).Name);
        }
    }
}