
using System;

//Pair class
public class Pair<T1, T2>
{
    public T1 First { get; private set; }
    public T2 Second { get; private set; }
    internal Pair(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }
}

public static class Pair
{
    public static Pair<T1, T2> New<T1, T2>(T1 first, T2 second)
    {
        var tuple = new Pair<T1, T2>(first, second);
        return tuple;
    }
}

//JSON helper for collection of objects
public static class JsonHelper
{

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return UnityEngine.JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}