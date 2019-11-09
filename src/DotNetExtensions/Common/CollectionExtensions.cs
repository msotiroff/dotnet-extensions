using DotNetExtensions.Validation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetExtensions.Common
{
    public static class CollectionExtensions
    {
        private const int Zero = 0;
        private const int One = 1;

        public static IEnumerable<T> ForEach<T>(
            this IEnumerable<T> collection, Action<T> action)
        {
            collection.ThrowIfNull("The given collection cannot be null.");

            action.ThrowIfNull("The given action cannot be null.");

            foreach (var item in collection)
            {
                action(item);
            }

            return collection;
        }

        public static IEnumerable<T> ForEachInParallel<T>(
            this IEnumerable<T> collection, Action<T> action)
        {
            collection.ThrowIfNull("The given collection cannot be null.");

            action.ThrowIfNull("The given action cannot be null.");

            Parallel.ForEach(collection, item =>
            {
                action(item);
            });

            return collection;
        }

        public static IEnumerable<TDestination> SelectInParallel<TSource, TDestination>(
            this IEnumerable<TSource> source, Func<TSource, TDestination> action)
        {
            source.ThrowIfNull("The given collection cannot be null.");

            action.ThrowIfNull("The given action cannot be null.");

            var destinationCollection = new ConcurrentBag<TDestination>();

            Parallel.ForEach(source, item =>
            {
                destinationCollection.Add(action(item));
            });

            return destinationCollection;
        }

        public static bool ContainsAll<T>(this IEnumerable<T> collection, params T[] args)
            where T : IComparable<T>
        {
            collection.ThrowIfNull("The passed collection cannot be null.");

            args.ThrowIfNull("The passed arguments cannot be null.");

            var collectionHashSet = new HashSet<T>(collection);
            var argsHashSet = new HashSet<T>(args);

            if (collectionHashSet.Count() < argsHashSet.Count())
            {
                foreach (var item in argsHashSet)
                {
                    collectionHashSet.Remove(item);
                }

                return collection.IsEmpty(); 
            }

            foreach (var item in collectionHashSet)
            {
                argsHashSet.Remove(item);
            }

            return argsHashSet.IsEmpty();
        }

        public static IEnumerable<TDest> TransformUsing<TSource, TDest>(
            this IEnumerable<TSource> input,
            Func<TSource, TSource, bool> comparer,
            Func<TSource, TSource, TDest> equalsAssigner,
            Func<TSource, TDest> notEqualsAssigner,
            bool isSliding = false)
            where TSource : class
            where TDest : class
        {
            input.ThrowIfNull(
                "The passed collection cannot be null.");

            comparer.ThrowIfNull("The comparing function cannot be null.");

            equalsAssigner.ThrowIfNull("The equals assigner cannot be null.");

            notEqualsAssigner.ThrowIfNull("The not equals assigner cannot be null.");

            IList<TSource> groupables = input.ToList();

            if (groupables.Count == Zero)
            {
                return new List<TDest>();
            }

            if (isSliding && (typeof(TSource) != typeof(TDest)))
            {
                throw new InvalidOperationException($"{nameof(isSliding)} is true, " +
                    $"but cannot cast {typeof(TSource).Name} to {typeof(TDest).Name}.");
            }

            IList<TDest> groupings = new List<TDest>();
            TDest currentGrouping = notEqualsAssigner(groupables.First());
            TSource sameGroupingObject = default(TSource);
            var addedOnLastIteration = false;

            for (int g = Zero; g < groupables.Count - One; g++)
            {
                TSource currentObject = groupables[g];
                TSource nextObject = groupables[g + One];

                if (comparer(currentObject, nextObject))
                {
                    sameGroupingObject = (sameGroupingObject.IsNull()) ?
                        currentObject :
                        isSliding ? currentGrouping as TSource : sameGroupingObject;
                    currentGrouping = equalsAssigner(sameGroupingObject, nextObject);
                }
                else
                {
                    groupings.Add(currentGrouping);

                    currentGrouping = notEqualsAssigner(nextObject);
                    addedOnLastIteration = (groupables.Count - One == g);
                    sameGroupingObject = default(TSource);
                }
            }

            if (!addedOnLastIteration)
            {
                groupings.Add(currentGrouping);
            }

            return groupings;
        }

        public static T ToFirstOrDefaultOfType<T>(this IEnumerable<object> items)
            where T : class
        {
            return items.FirstOrDefault(i => 
                typeof(T).IsAssignableFrom(i.GetType())) as T;
        }

        public static bool IsEmpty<T>(this IEnumerable<T> collection)
        {
            collection.ThrowIfNull("The passed collection cannot be null.");

            return collection.Count() == Zero;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection.IsNull() || collection.IsEmpty();
        }

        public static string JoinBy(this IEnumerable<string> source, string separator)
        {
            return string.Join(separator, source);
        }

        public static string JoinByNewLine(this IEnumerable<string> source)
        {
            return source.JoinBy(Environment.NewLine);
        }

        public static string JoinByDash(this IEnumerable<string> source)
        {
            return source.JoinBy("-");
        }

        public static string JoinByUnderscore(this IEnumerable<string> source)
        {
            return source.JoinBy("_");
        }

        public static string JoinByComma(this IEnumerable<string> source)
        {
            return source.JoinBy(",");
        }

        public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        {
            return new Queue<T>(source);
        }

        public static Stack<T> ToStack<T>(this IEnumerable<T> source)
        {
            return new Stack<T>(source);
        }
    }
}
