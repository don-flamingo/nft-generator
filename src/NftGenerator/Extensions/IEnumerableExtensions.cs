using System;
using System.Collections.Generic;
using System.Linq;

namespace NftGenerator.Extensions
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences, int size)
        {
            var tab = new List<IEnumerable<T>>(size);
            var i = 0;

            while (i < size)
            {
                var set = sequences
                    .Select(trait => trait.OrderByDescending(x => Guid.NewGuid())
                        .First());

                if (!tab.Any(s => s.SequenceEqual(set)))
                {
                    tab.Add(set);
                    i++;
                }
            }

            return tab;

            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
                emptyProduct,
                (accumulator, sequence) =>
                    from accseq in accumulator
                    from item in sequence
                    select accseq.Concat(new[] { item })
            );
        }
    }
}
