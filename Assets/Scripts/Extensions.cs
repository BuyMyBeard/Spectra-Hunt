using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public static class Extensions
{
    public static float ClampAngle(float angle, float min, float max)
    {
        float start = (min + max) * 0.5f - 180;
        float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
        return Mathf.Clamp(angle, min + floor, max + floor);
    }
    public static void ResetAllTriggers(this Animator animator)
    {
        foreach (var param in animator.parameters)
            if (param.type == AnimatorControllerParameterType.Trigger)
                animator.ResetTrigger(param.name);
    }
    public static void ResetAllBooleans(this Animator animator)
    {
        foreach (var param in animator.parameters)
            if (param.type == AnimatorControllerParameterType.Bool)
                animator.SetBool(param.name, false);
    }
    public static T PickRandom<T>(this IEnumerable<WeightedAction<T>> possibleActions)
    {
        float totalWeight = possibleActions.Sum(action => action.weight);
        List<(float, T)> chanceList = new();
        float current = 0;
        foreach (var action in possibleActions)
        {
            current += action.weight;
            chanceList.Add((current, action.actionName));
        }
        float pickedNumber = UnityEngine.Random.Range(0, totalWeight);
        return chanceList.Find(action => pickedNumber <= action.Item1).Item2;
    }
    public static bool IsBetween<T>(this T obj, T minBound, T maxBound) where T : IComparable
    {
        return obj.CompareTo(minBound) > -1 && obj.CompareTo(maxBound) < 1;
    }
    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            (ts[r], ts[i]) = (ts[i], ts[r]);
        }
    }
}
[Serializable]
public struct WeightedAction<T>
{
    public WeightedAction(T actionName, float weight = 1)
    {
        this.actionName = actionName;
        this.weight = weight;
    }
    public T actionName;
    public float weight;
}