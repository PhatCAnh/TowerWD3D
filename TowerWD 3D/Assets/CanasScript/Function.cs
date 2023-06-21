using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public static class Function
{
    public static void CalculationTimeFuncRun([NotNull] Action action)
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        action();
        stopwatch.Stop();
        UnityEngine.Debug.Log("Time run of" + action.GetType().Name + ": " + stopwatch.ElapsedMilliseconds + "ms");
        stopwatch.Reset();
    }
}
