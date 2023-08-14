namespace DarkDarkerArmorCalc;

public static  class LinqExtensions
{
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T> action) => items.Select(item => { action(item); return item; });
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> items, Action<T, int> action) => items.Select((item, index) => { action(item, index); return item; });
}
