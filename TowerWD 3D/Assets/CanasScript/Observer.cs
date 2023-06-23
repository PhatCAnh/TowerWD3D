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

    public void UnListen(string id, UnityAction<DataEventChange> action)
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