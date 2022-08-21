using System;
using System.Collections;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void Wait(this MonoBehaviour monoBehaviour, float seconds, Action action)
    {
        monoBehaviour.StartCoroutine(_wait(seconds, action));
    }

    private static IEnumerator _wait(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
}

