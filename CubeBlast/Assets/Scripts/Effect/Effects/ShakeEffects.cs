using System;
using System.Collections;
using UnityEngine;

public class ShakeEffect : IEffect
{
    public event Action<IEffect> OnComplete;
    private Transform Transform { get; }
    private float WiggleSpeed { get; }
    private float MaxRotation { get; }


    public ShakeEffect(Transform transform, float maxRotation, float speed, Action<IEffect> onComplete = null)
    {
        Transform = transform;
        WiggleSpeed = speed;
        MaxRotation = maxRotation;
        OnComplete += onComplete;
    }

    public IEnumerator Execute()
    {
        var rotateTo = new Quaternion
        {
            eulerAngles = new Vector3(0, 0, MaxRotation)
        };


        var currentRotation = Transform.rotation.z;
        var nextRotation = MaxRotation * -1f;

        var time = 0f;

        while (Mathf.Abs(nextRotation) > 0.15f)
        {
            time += Time.deltaTime * WiggleSpeed;
            var newRotation = Mathf.Lerp(currentRotation, nextRotation, time);
            rotateTo.eulerAngles = new Vector3(0, 0, newRotation);
            Transform.rotation = rotateTo;
            if (time >= 1)
            {
                currentRotation = nextRotation;
                nextRotation = (nextRotation * 0.9f) * -1;
                time = 0;
            }

            yield return null;
        }

        rotateTo.eulerAngles = new Vector3(0, 0, 0);
        Transform.rotation = rotateTo;

        OnComplete?.Invoke(this);
    }
}