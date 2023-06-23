using System;
using System.Collections.Generic;
using CanasSource;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class EventBase
{
    public readonly string ModleID;
    public string NameOfType;
    public object Value;

    public EventBase(string modleID, string nameOfType, object value)
    {
        this.ModleID = modleID;
        this.NameOfType = nameOfType;
        this.Value = value;
    }
}

public class Observer : MonoBehaviour
{
    private Dictionary<string, UnityEvent<EventBase>> _dictionaryEventChange = new();
    private void Awake()
    {
        Singleton<Observer>.Instance = this;
    }

    public void InvokeDataChange(string id, EventBase value)
    {
        if (_dictionaryEventChange.TryGetValue(id, out var unityEvent))
        {
            unityEvent.Invoke(value);
        }
    }

    public void InvokeDataChange(EventBase value)
    {
        if (_dictionaryEventChange.TryGetValue(GetType().Name, out var unityEvent))
        {
            unityEvent.Invoke(value);
        }
    }

    public void AddListenerDataChange(string id, UnityAction<EventBase> action)
    {
        if (_dictionaryEventChange.TryGetValue(id, out var unityEvent))
        {
            unityEvent.AddListener(action);
        }
        else
        {
            unityEvent = new UnityEvent<EventBase>();
            unityEvent.AddListener(action);
            _dictionaryEventChange.Add(id, unityEvent);
        }
    }

    public void AddListenerDataChange(UnityAction<EventBase> action)
    {
        if (_dictionaryEventChange.TryGetValue(GetType().Name, out var unityEvent))
        {
            unityEvent.AddListener(action);
        }
        else
        {
            unityEvent = new UnityEvent<EventBase>();
            unityEvent.AddListener(action);
            _dictionaryEventChange.Add(GetType().Name, unityEvent);
        }
    }

    public void UnListen(string id, UnityAction<EventBase> action)
    {
        if (_dictionaryEventChange.TryGetValue(id, out var unityEvent))
        {
            unityEvent.RemoveListener(action);
        }
    }

    public void Clear()
    {
        _dictionaryEventChange.Clear();
    }
}