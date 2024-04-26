using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleEffect : IEffect
{
    public event Action<IEffect> OnComplete;
    private Transform Transform { get; }
    private YieldInstruction Wait { get; }
    private float ScaleSpeed { get; }
    private Vector3 MaxSize { get; }


    public ScaleEffect(Transform transform, Vector3 maxSize, float scaleSpeed, YieldInstruction wait, Action<IEffect> onComplete = null)
    {
        Transform = transform;
        MaxSize = maxSize;
        ScaleSpeed = scaleSpeed;
        Wait = wait;
        OnComplete += onComplete;
    }

    public IEnumerator Execute()
    {
        var time = 0f;
        var currentScale = Transform.localScale;
        while (Transform.localScale != MaxSize)
        {
            time += Time.deltaTime * ScaleSpeed;
            var scale = Vector3.Lerp(currentScale, MaxSize, time);
            Transform.localScale = scale;
            yield return null;
        }

        yield return Wait;

        currentScale = Transform.localScale;
        time = 0f;
        while (Transform.localScale != Vector3.one)
        {
            time += Time.deltaTime * ScaleSpeed;
            var scale = Vector3.Lerp(currentScale, Vector3.one, time);
            Transform.localScale = scale;
            yield return null;
        }

        OnComplete?.Invoke(this);
    }

}
