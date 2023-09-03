#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
// Taken from https://gist.github.com/aarthificial/f2dbb58e4dbafd0a93713a380b9612af
/// Requires Unity 2020.1+
[Serializable]
public struct Optional<T>
{
    [SerializeField] private bool enabled;
    [SerializeField] private T value;

    public bool Enabled => enabled;
    public T Value => value;

    public Optional(T initialValue)
    {
        enabled = true;
        value = initialValue;
    }

    public bool IsSome(out T value)
    {
        value = this.value;
        return enabled;
    }

    public static Optional<T> None => default;
    public static Optional<T> Some(T val) => new Optional<T>(val);
}
