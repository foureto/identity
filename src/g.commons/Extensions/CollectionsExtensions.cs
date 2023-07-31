using System.Runtime.CompilerServices;

namespace g.commons.Extensions;

public static class CollectionsExtensions
{
    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
            action?.Invoke(item);
    }

    public static IEnumerable<T> ForEachRet<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        foreach (var item in enumerable)
        {
            action?.Invoke(item);
            yield return item;
        }
    }

    public static async IAsyncEnumerable<T> ForEachAsync<T>(
        this IEnumerable<T> enumerable,
        Func<T, CancellationToken, Task> action,
        [EnumeratorCancellation] CancellationToken token = default)
    {
        foreach (var item in enumerable)
        {
            if (token.IsCancellationRequested)
                yield break;

            await action?.Invoke(item, token)!;
            yield return item;
        }
    }

    public static async IAsyncEnumerable<TK> ForEachAsync<T, TK>(
        this IEnumerable<T> enumerable,
        Func<T, CancellationToken, Task<TK>> action,
        [EnumeratorCancellation] CancellationToken token = default)
    {
        foreach (var item in enumerable)
        {
            if (token.IsCancellationRequested)
                yield break;

            yield return await action?.Invoke(item, token)!;
        }
    }

    public static T ValueOrDefault<TK, T>(this Dictionary<TK, T> dictionary, TK key, T defaultT = default)
    {
        return dictionary?.ContainsKey(key) ?? false ? dictionary[key] : defaultT;
    }
}