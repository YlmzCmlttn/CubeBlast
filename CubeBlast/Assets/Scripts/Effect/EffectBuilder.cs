using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectBuilder
{
    private MonoBehaviour Owner { get; }
    private readonly List<IEffect> _effects = new List<IEffect>();

    private int _completedEffects = 0;

    public event Action OnAllEffectsComplete;

    public EffectBuilder(MonoBehaviour owner)
    {
        Owner = owner;
    }

    public EffectBuilder AddEffect(IEffect effect)
    {
        _effects.Add(effect);
        effect.OnComplete += OnEffectComplete;
        return this;
    }

    public void ExecuteEffects()
    {
        Owner.StopAllCoroutines();
        foreach (var effect in _effects)
        {
            Owner.StartCoroutine(effect.Execute());
        }
    }

    private void OnEffectComplete(IEffect effect)
    {
        _completedEffects += 1;
        if (_completedEffects < _effects.Count)
            return;
        AllEffectsComplete();
    }

    private void AllEffectsComplete()
    {
        _completedEffects = 0;
        OnAllEffectsComplete?.Invoke();
    }
}