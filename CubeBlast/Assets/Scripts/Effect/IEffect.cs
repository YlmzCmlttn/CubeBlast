using System;
using System.Collections;

public interface IEffect
{
    IEnumerator Execute();
    event Action<IEffect> OnComplete;
}