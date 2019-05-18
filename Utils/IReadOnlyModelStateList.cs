using System.Collections.Generic;

namespace DDDCommon.Utils
{
    public interface IReadOnlyModelStateList<T>
    {
        IReadOnlyList<T> Items { get; }

        IReadOnlyList<T> AddedItems { get; }

        IReadOnlyList<(T Old, T New)> UpdatedItems { get; }

        IReadOnlyList<T> RemovedItems { get; }
    }
}