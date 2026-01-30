using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISendEvent { }

public interface IRegisterEvent
{
}

public interface IEvent 
{

}



public class EventManager : Singleton<EventManager>
{
    // ȫ���¼��������ֵ�
    private Dictionary<Type, Action<IEvent>> globalEventDictionary;

    // �ض������ߵ��¼��������ֵ�
    private Dictionary<ISendEvent, Dictionary<Type, Action<IEvent>>> specificEventDictionary;

    // ί��ӳ�仺�棨������ȷע����
    private Dictionary<Delegate, Action<IEvent>> globalListenerMap = new Dictionary<Delegate, Action<IEvent>>();
    private Dictionary<ISendEvent, Dictionary<Delegate, Action<IEvent>>> specificListenerMap = new Dictionary<ISendEvent, Dictionary<Delegate, Action<IEvent>>>();

    protected EventManager() : base()
    {
        globalEventDictionary = new Dictionary<Type, Action<IEvent>>();
        specificEventDictionary = new Dictionary<ISendEvent, Dictionary<Type, Action<IEvent>>>();
    }

    // ע��ȫ���¼�������
    public void Register<T>(Action<T> listener) where T : struct, IEvent
    {
        var eventType = typeof(T);
        Action<IEvent> wrapper = (e) => listener((T)e);
        globalListenerMap[listener] = wrapper;

        if (globalEventDictionary.TryGetValue(eventType, out var existing))
        {
            globalEventDictionary[eventType] = existing + wrapper;
        }
        else
        {
            globalEventDictionary[eventType] = wrapper;
        }
    }

    // ȡ��ȫ���¼�������
    public void Unregister<T>(Action<T> listener) where T : struct, IEvent
    {
        var eventType = typeof(T);
        if (globalListenerMap.TryGetValue(listener, out var wrapper))
        {
            if (globalEventDictionary.TryGetValue(eventType, out var existing))
            {
                existing -= wrapper;
                if (existing == null)
                    globalEventDictionary.Remove(eventType);
                else
                    globalEventDictionary[eventType] = existing;
            }

            globalListenerMap.Remove(listener);
        }
    }

    // ע���ض������ߵ��¼�������
    public void Register<T>(ISendEvent sender, Action<T> listener) where T : struct, IEvent
    {
        var eventType = typeof(T);
        Action<IEvent> wrapper = (e) => listener((T)e);

        if (!specificEventDictionary.ContainsKey(sender))
            specificEventDictionary[sender] = new Dictionary<Type, Action<IEvent>>();
        if (!specificListenerMap.ContainsKey(sender))
            specificListenerMap[sender] = new Dictionary<Delegate, Action<IEvent>>();

        var senderDict = specificEventDictionary[sender];
        var listenerMap = specificListenerMap[sender];

        if (senderDict.TryGetValue(eventType, out var existing))
        {
            senderDict[eventType] = existing + wrapper;
        }
        else
        {
            senderDict[eventType] = wrapper;
        }

        listenerMap[listener] = wrapper;
    }

    // ȡ���ض������ߵ��¼�������
    public void Unregister<T>(ISendEvent sender, Action<T> listener) where T : struct, IEvent
    {
        var eventType = typeof(T);

        if (specificEventDictionary.ContainsKey(sender) && specificListenerMap.ContainsKey(sender))
        {
            var senderDict = specificEventDictionary[sender];
            var listenerMap = specificListenerMap[sender];

            if (listenerMap.TryGetValue(listener, out var wrapper))
            {
                if (senderDict.TryGetValue(eventType, out var existing))
                {
                    existing -= wrapper;
                    if (existing == null)
                        senderDict.Remove(eventType);
                    else
                        senderDict[eventType] = existing;
                }

                listenerMap.Remove(listener);
            }

            // �������ֵ�
            if (listenerMap.Count == 0)
                specificListenerMap.Remove(sender);
            if (senderDict.Count == 0)
                specificEventDictionary.Remove(sender);
        }
    }

    // �����¼�������ȫ���뷢���ߣ�
    public void TriggerEvent<T>(ISendEvent sender, T eventToSend) where T : struct, IEvent
    {
        var eventType = typeof(T);

        // ����ȫ���¼�
        if (globalEventDictionary.TryGetValue(eventType, out var globalHandler))
        {
            globalHandler?.Invoke(eventToSend);
        }

        // �����ض��������¼�
        if (specificEventDictionary.TryGetValue(sender, out var senderDict))
        {
            if (senderDict.TryGetValue(eventType, out var senderHandler))
            {
                senderHandler?.Invoke(eventToSend);
            }
        }
    }
}

public static class EventSenderExtensions
{
    public static void SendEvent<T>(this ISendEvent sender, T eventToSend) where T : struct, IEvent
    {
        EventManager.Instance.TriggerEvent(sender, eventToSend);
    }
    public static void Register<T>(this IRegisterEvent registerer, Action<T> listener) where T : struct, IEvent
    {
        EventManager.Instance.Register(listener);
    }

    public static void Register<T>(this IRegisterEvent registerer, ISendEvent sender, Action<T> listener) where T : struct, IEvent
    {
        EventManager.Instance.Register(sender, listener);
    }

    public static void Unregister<T>(this IRegisterEvent registerer, Action<T> listener) where T : struct, IEvent
    {
        EventManager.Instance.Unregister(listener);
    }

    public static void Unregister<T>(this IRegisterEvent registerer, ISendEvent sender, Action<T> listener) where T : struct, IEvent
    {
        EventManager.Instance.Unregister(sender, listener);
    }
}