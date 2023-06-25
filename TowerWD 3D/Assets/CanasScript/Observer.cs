using System;
using System.Collections.Generic;
using CanasSource;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class DataEventChange
{
    public readonly string ModleID;
    public string NameOfType;
    public object Value;

    public DataEventChange(string modleID, string nameOfType, object value)
    {
        this.ModleID = modleID;
        this.NameOfType = nameOfType;
        this.Value = value;
    }
}

public class Observer : MonoBehaviour
{
    private Dictionary<string, UnityEvent<DataEventChange>> _dictionaryEventChange = new();
    private Dictionary<string, UnityEvent<object>> _dictionaryEvent = new();

    private void Awake()
    {
        Singleton<Observer>.Instance = this;
    }

    public void InvokeDataChange(DataEventChange value)
    {
        if (_dictionaryEventChange.TryGetValue(value.ModleID, out var unityEvent))
        {
            unityEvent.Invoke(value);
        }
    }

    public void AddListenerDataChange(string id, UnityAction<DataEventChange> action)
    {
        if (_dictionaryEventChange.TryGetValue(id, out var unityEvent))
        {
            unityEvent.AddListener(action);
        }
        else
        {
            unityEvent = new UnityEvent<DataEventChange>();
            unityEvent.AddListener(action);
            _dictionaryEventChange.Add(id, unityEvent);
        }
    }

    public void UnListenEventChange(string id, UnityAction<DataEventChange> action)
    {
        if (_dictionaryEventChange.TryGetValue(id, out var unityEvent))
        {
            unityEvent.RemoveListener(action);
        }
    }

    public void ClearEventChange()
    {
        _dictionaryEventChange.Clear();
    }

    public void Invoke(string id, object value)
    {
        if (_dictionaryEvent.TryGetValue(id, out var unityEvent))
        {
            unityEvent.Invoke(value);
        }
    }

    public void AddListener(string id, UnityAction<object> action)
    {
        if (_dictionaryEvent.TryGetValue(id, out var unityEvent))
        {
            unityEvent.AddListener(action);
        }
        else
        {
            unityEvent = new UnityEvent<object>();
            unityEvent.AddListener(action);
            _dictionaryEvent.Add(id, unityEvent);
        }
    }

    public void UnListen(string id, UnityAction<object> action)
    {
        if (_dictionaryEvent.TryGetValue(id, out var unityEvent))
        {
            unityEvent.RemoveListener(action);
        }
    }

    public void ClearEvent()
    {
        _dictionaryEvent.Clear();
    }
}