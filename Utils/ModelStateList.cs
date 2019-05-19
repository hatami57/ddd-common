using System;
using System.Collections.Generic;

namespace DDDCommon.Utils
{
    public class ModelStateList<T> : IReadOnlyModelStateList<T>
    {
        public IReadOnlyList<T> Items => _items;

        public IReadOnlyList<T> AddedItems => _addedItems;

        public IReadOnlyList<(T Old, T New)> UpdatedItems => _updatedItems;

        public IReadOnlyList<T> RemovedItems => _removedItems;

        private readonly List<T> _items;
        private readonly List<T> _addedItems;
        private readonly List<(T Old, T New)> _updatedItems;
        private readonly List<T> _removedItems;

        public ModelStateList(List<T> items = null)
        {
            _items = items ?? new List<T>();
            _addedItems = new List<T>();
            _updatedItems = new List<(T, T)>();
            _removedItems = new List<T>();
        }

        public void SetItems(List<T> items)
        {
            ClearAll();
            if (items != null) _items.AddRange(items);
        }

        private void ClearAll()
        {
            _items.Clear();
            _addedItems.Clear();
            _updatedItems.Clear();
            _removedItems.Clear();
        }

        public void AddItem(T item)
        {
            _items.Add(item);
            _addedItems.Add(item);
        }

        public void UpdateItem(T oldItem, T newItem)
        {
            var oldItemIndex = _items.IndexOf(oldItem);
            if (oldItemIndex < 0) throw Errors.NotFound();

            _items[oldItemIndex] = newItem;
            _updatedItems.Add((oldItem, newItem));
        }

        public void RemoveItem(T item)
        {
            if (_items.Remove(item)) _removedItems.Add(item);
        }
    }
}