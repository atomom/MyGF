/////////////////////////////////////////////////////////////////////////////////
//
//	GlobalEvent.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	This class allows the sending of generic events to and from
//					any class with generic listeners which register and unregister
//					from the events. Events can have 0-3 arguments.
//
/////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GlobalCallback(); // 0 Arguments
public delegate void GlobalCallback<T>(T arg1); // 1 Argument
public delegate void GlobalCallback<T, U>(T arg1, U arg2); // 2 Arguments
public delegate void GlobalCallback<T, U, V>(T arg1, U arg2, V arg3); // 3 Arguments

public enum GlobalEventMode
{
    DONT_REQUIRE_LISTENER,
    REQUIRE_LISTENER
}

static internal class GlobalEventInternal
{

    public static Hashtable Callbacks = new Hashtable();

    public static UnregisterException ShowUnregisterException(string name)
    {

        return new UnregisterException(string.Format("Attempting to Unregister the event {0} but GlobalEvent has not registered this event.", name));

    }

    public static SendException ShowSendException(string name)
    {

        return new SendException(string.Format("Attempting to Send the event {0} but GlobalEvent has not registered this event.", name));

    }

    public class UnregisterException : Exception { public UnregisterException(string msg) : base(msg) { } }
    public class SendException : Exception { public SendException(string msg) : base(msg) { } }

}

// Event with no arguments
public static class GlobalEvent
{

    private static Hashtable m_Callbacks = GlobalEventInternal.Callbacks;

    /// <summary>
    /// Registers the event specified by name
    /// </summary>
    public static void Register(string name, GlobalCallback callback)
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(@"name");

        if (callback == null)
            throw new ArgumentNullException("callback");

        List<GlobalCallback> callbacks = (List<GlobalCallback>) m_Callbacks[name];
        if (callbacks == null)
        {
            callbacks = new List<GlobalCallback>();
            m_Callbacks.Add(name, callbacks);
        }
        callbacks.Add(callback);

    }

    /// <summary>
    /// Unregisters the event specified by name
    /// </summary>
    public static void UnRegister(string name, GlobalCallback callback)
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(@"name");

        if (callback == null)
            throw new ArgumentNullException("callback");

        List<GlobalCallback> callbacks = (List<GlobalCallback>) m_Callbacks[name];
        if (callbacks != null)
            callbacks.Remove(callback);
        else
            throw GlobalEventInternal.ShowUnregisterException(name);

    }

    /// <summary>
    /// sends an event
    /// </summary>
    public static void Send(string name)
    {

        Send(name, GlobalEventMode.DONT_REQUIRE_LISTENER);

    }

    /// <summary>
    /// sends an event
    /// </summary>
    public static void Send(string name, GlobalEventMode mode)
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(@"name");

        List<GlobalCallback> callbacks = (List<GlobalCallback>) m_Callbacks[name];
        if (callbacks != null)
        {
            Call(callbacks);
        }
        else if (mode == GlobalEventMode.REQUIRE_LISTENER)
            throw GlobalEventInternal.ShowSendException(name);

    }

    private static void Call(List<GlobalCallback> calls)
    {
        if (calls == null) return;
        for (int i = calls.Count - 1; i > -1; --i)
        {
            calls[i]();
        }
    }

}

// Accepts 1 Argument
public static class GlobalEvent<T>
{

    private static Hashtable m_Callbacks = GlobalEventInternal.Callbacks;

    /// <summary>
    /// Registers the event specified by name
    /// </summary>
    public static void Register(string name, GlobalCallback<T> callback)
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(@"name");

        if (callback == null)
            throw new ArgumentNullException("callback");

        List<GlobalCallback<T>> callbacks = (List<GlobalCallback<T>>) m_Callbacks[name];
        if (callbacks == null)
        {
            callbacks = new List<GlobalCallback<T>>();
            m_Callbacks.Add(name, callbacks);
        }
        callbacks.Add(callback);

    }

    /// <summary>
    /// Unregisters the event specified by name
    /// </summary>
    public static void UnRegister(string name, GlobalCallback<T> callback)
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(@"name");

        if (callback == null)
            throw new ArgumentNullException("callback");

        List<GlobalCallback<T>> callbacks = (List<GlobalCallback<T>>) m_Callbacks[name];
        if (callbacks != null)
            callbacks.Remove(callback);
        else
            throw GlobalEventInternal.ShowUnregisterException(name);

    }

    /// <summary>
    /// sends an event with 1 argument
    /// </summary>
    public static void Send(string name, T arg1)
    {

        Send(name, arg1, GlobalEventMode.DONT_REQUIRE_LISTENER);

    }

    /// <summary>
    /// sends an event with 1 argument
    /// </summary>
    public static void Send(string name, T arg1, GlobalEventMode mode)
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(@"name");

        List<GlobalCallback<T>> callbacks = (List<GlobalCallback<T>>) m_Callbacks[name];
        if (callbacks != null)
            Call(callbacks, arg1);
        else if (mode == GlobalEventMode.REQUIRE_LISTENER)
            throw GlobalEventInternal.ShowSendException(name);

    }

    private static void Call(List<GlobalCallback<T>> calls, T arg1)
    {
        if (calls == null) return;
        for (int i = calls.Count - 1; i > -1; --i)
        {
            calls[i](arg1);
        }
    }

}

// Accepts 2 arguments
public static class GlobalEvent<T, U>
{

    private static Hashtable m_Callbacks = GlobalEventInternal.Callbacks;

    /// <summary>
    /// Registers the event specified by name
    /// </summary>
    public static void Register(string name, GlobalCallback<T, U> callback)
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(@"name");

        if (callback == null)
            throw new ArgumentNullException("callback");

        List<GlobalCallback<T, U>> callbacks = (List<GlobalCallback<T, U>>) m_Callbacks[name];
        if (callbacks == null)
        {
            callbacks = new List<GlobalCallback<T, U>>();
            m_Callbacks.Add(name, callbacks);
        }
        callbacks.Add(callback);

    }

    /// <summary>
    /// Unregisters the event specified by name
    /// </summary>
    public static void UnRegister(string name, GlobalCallback<T, U> callback)
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(@"name");

        if (callback == null)
            throw new ArgumentNullException("callback");

        List<GlobalCallback<T, U>> callbacks = (List<GlobalCallback<T, U>>) m_Callbacks[name];
        if (callbacks != null)
            callbacks.Remove(callback);
        else
            throw GlobalEventInternal.ShowUnregisterException(name);

    }

    /// <summary>
    /// sends an event with 2 arguments
    /// </summary>
    public static void Send(string name, T arg1, U arg2)
    {

        Send(name, arg1, arg2, GlobalEventMode.DONT_REQUIRE_LISTENER);

    }

    /// <summary>
    /// sends an event with 2 arguments
    /// </summary>
    public static void Send(string name, T arg1, U arg2, GlobalEventMode mode)
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(@"name");

        List<GlobalCallback<T, U>> callbacks = (List<GlobalCallback<T, U>>) m_Callbacks[name];
        if (callbacks != null)
            Call(callbacks, arg1, arg2);
        else if (mode == GlobalEventMode.REQUIRE_LISTENER)
            throw GlobalEventInternal.ShowSendException(name);

    }
    private static void Call(List<GlobalCallback<T, U>> calls, T arg1, U arg2)
    {
        if (calls == null) return;
        for (int i = calls.Count - 1; i > -1; --i)
        {
            calls[i](arg1, arg2);
        }
    }
}

// Accepts 3 Arguments
public static class GlobalEvent<T, U, V>
{

    private static Hashtable m_Callbacks = GlobalEventInternal.Callbacks;

    /// <summary>
    /// Registers the event specified by name
    /// </summary>
    public static void Register(string name, GlobalCallback<T, U, V> callback)
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(@"name");

        if (callback == null)
            throw new ArgumentNullException("callback");

        List<GlobalCallback<T, U, V>> callbacks = (List<GlobalCallback<T, U, V>>) m_Callbacks[name];
        if (callbacks == null)
        {
            callbacks = new List<GlobalCallback<T, U, V>>();
            m_Callbacks.Add(name, callbacks);
        }
        callbacks.Add(callback);

    }

    /// <summary>
    /// Unregisters the event specified by name
    /// </summary>
    public static void UnRegister(string name, GlobalCallback<T, U, V> callback)
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(@"name");

        if (callback == null)
            throw new ArgumentNullException("callback");

        List<GlobalCallback<T, U, V>> callbacks = (List<GlobalCallback<T, U, V>>) m_Callbacks[name];
        if (callbacks != null)
            callbacks.Remove(callback);
        else
            throw GlobalEventInternal.ShowUnregisterException(name);

    }

    /// <summary>
    /// sends an event with 3 arguments
    /// </summary>
    public static void Send(string name, T arg1, U arg2, V arg3)
    {

        Send(name, arg1, arg2, arg3, GlobalEventMode.DONT_REQUIRE_LISTENER);

    }

    /// <summary>
    /// sends an event with 3 arguments
    /// </summary>
    public static void Send(string name, T arg1, U arg2, V arg3, GlobalEventMode mode)
    {

        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(@"name");

        List<GlobalCallback<T, U, V>> callbacks = (List<GlobalCallback<T, U, V>>) m_Callbacks[name];
        if (callbacks != null)
            Call(callbacks, arg1, arg2, arg3);
        else if (mode == GlobalEventMode.REQUIRE_LISTENER)
            throw GlobalEventInternal.ShowSendException(name);

    }

    private static void Call(List<GlobalCallback<T, U, V>> calls, T arg1, U arg2, V arg3)
    {
        if (calls == null) return;
        for (int i = calls.Count - 1; i > -1; --i)
        {
            calls[i](arg1, arg2, arg3);
        }
    }
}