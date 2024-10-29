using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///     Based on PracticAPI: https://www.youtube.com/watch?v=g89aJ4GlCWM
/// </summary>
public class UpdatePublisher : MonoBehaviour
{
    private static readonly List<IUpdateObserver> _updateObservers = new();
    private static readonly List<IUpdateObserver> _pendingUpdateObservers = new();

    private static readonly List<ILateUpdateObserver> _lateUpdateObservers = new();
    private static readonly List<ILateUpdateObserver> _pendingLateUpdateObservers = new();

    private static readonly List<IFixedUpdateObserver> _fixedUpdateObservers = new();
    private static readonly List<IFixedUpdateObserver> _pendingFixedUpdateObservers = new();

    private static UpdatePublisher _instance;


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public static void RegisterUpdateObserver(IUpdateObserver observer)
    {
        if (_pendingUpdateObservers.Contains(observer) || _updateObservers.Contains(observer))
        {
            return;
        }

        _pendingUpdateObservers.Add(observer);
    }


    public static void RegisterLateUpdateObserver(ILateUpdateObserver observer)
    {
        if (_pendingLateUpdateObservers.Contains(observer) || _lateUpdateObservers.Contains(observer))
        {
            return;
        }

        _pendingLateUpdateObservers.Add(observer);
    }


    public static void RegisterFixedUpdateObserver(IFixedUpdateObserver observer)
    {
        if (_pendingFixedUpdateObservers.Contains(observer) || _fixedUpdateObservers.Contains(observer))
        {
            return;
        }

        _pendingFixedUpdateObservers.Add(observer);
    }


    private void Update()
    {
        ProcessObservers(_updateObservers, _pendingUpdateObservers, observer => observer.ObservedUpdate());
    }


    private void LateUpdate()
    {
        ProcessObservers(_lateUpdateObservers, _pendingLateUpdateObservers, observer => observer.ObservedLateUpdate());
    }


    private void FixedUpdate()
    {
        ProcessObservers(_fixedUpdateObservers, _pendingFixedUpdateObservers, observer => observer.ObservedFixedUpdate());
    }


    private void ProcessObservers<T>(List<T> observers, List<T> pendingObservers, Action<T> action)
    {
        for (var i = observers.Count - 1; i >= 0; i--)
        {
            if (observers[i] == null)
            {
                Debug.LogError($"{typeof(T).Name}: Found null observer in list, will skip.");

                continue;
            }

            action(observers[i]);
        }

        observers.AddRange(pendingObservers);
        pendingObservers.Clear();
    }


    public static void UnregisterUpdateObserver(IUpdateObserver observer)
    {
        _pendingUpdateObservers.Remove(observer);
        _updateObservers.Remove(observer);
    }


    public static void UnregisterLateUpdateObserver(ILateUpdateObserver observer)
    {
        _pendingLateUpdateObservers.Remove(observer);
        _lateUpdateObservers.Remove(observer);
    }


    public static void UnregisterFixedUpdateObserver(IFixedUpdateObserver observer)
    {
        _pendingFixedUpdateObservers.Remove(observer);
        _fixedUpdateObservers.Remove(observer);
    }


    private void OnDisable()
    {
        _updateObservers.Clear();
        _pendingUpdateObservers.Clear();
        _lateUpdateObservers.Clear();
        _pendingLateUpdateObservers.Clear();
        _fixedUpdateObservers.Clear();
        _pendingFixedUpdateObservers.Clear();
    }
}