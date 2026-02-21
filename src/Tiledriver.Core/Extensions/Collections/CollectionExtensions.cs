namespace Tiledriver.Core.Extensions.Collections;

public static class CollectionExtensions
{
	public static void AddRange<T>(this Queue<T> collection, IEnumerable<T> sequence)
	{
		foreach (var item in sequence)
		{
			collection.Enqueue(item);
		}
	}

	public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> sequence)
	{
		foreach (var item in sequence)
		{
			collection.Add(item);
		}
	}

	public static IList<T> AddRangeAndContinue<T>(this IList<T> list, IEnumerable<T> items)
	{
		list.AddRange(items);
		return list;
	}

	// Cheat because it's absurd that interface doesn't have this
	public static int FindIndex<T>(this IReadOnlyList<T> list, Predicate<T> predicate) =>
		list switch
		{
			List<T> l => l.FindIndex(predicate),
			_ => list.Select((item, i) => (item, i)).First(pair => predicate(pair.item)).i,
		};

	public static void Shuffle<T>(this IList<T> list, Random? random = null)
	{
		var rng = random ?? Random.Shared;

		int n = list.Count;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			(list[k], list[n]) = (list[n], list[k]);
		}
	}
}
