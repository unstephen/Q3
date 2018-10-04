using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDelegate
{
    public delegate void Callback(params object[] args);

    public float shotInterval = 0f;

    private float nextShotTime = 0f;

    private EventDelegate.Callback mCachedCallback;

    public void Clear()
    {
        this.mCachedCallback = null;
    }

    public EventDelegate()
    {
    }

    public EventDelegate(EventDelegate.Callback call)
    {
        this.mCachedCallback = call;
    }

    public override bool Equals(object obj)
    {
        bool result;
        if (obj == null)
        {
            result = false;
        }
        else if (obj is EventDelegate.Callback)
        {
            EventDelegate.Callback callback = obj as EventDelegate.Callback;
            result = callback.Equals(this.mCachedCallback);
        }
        else
        {
            if (obj is EventDelegate)
            {
                EventDelegate eventDelegate = obj as EventDelegate;
                if (eventDelegate.mCachedCallback != null)
                {
                    result = eventDelegate.mCachedCallback.Equals(this.mCachedCallback);
                    return result;
                }
            }
            result = false;
        }
        return result;
    }

    public bool Execute(params object[] args)
    {
        bool result;
        if (this.shotInterval > 1E-05f)
        {
            if (this.nextShotTime >= Time.realtimeSinceStartup)
            {
                result = false;
                return result;
            }
            this.nextShotTime = Time.realtimeSinceStartup + this.shotInterval;
        }
        if (this.mCachedCallback != null)
        {
            this.mCachedCallback(args);
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }

    public static void Execute(List<EventDelegate> list, params object[] args)
    {
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                EventDelegate eventDelegate = list[i];
                if (eventDelegate != null)
                {
                    eventDelegate.Execute(args);
                }
            }
        }
    }

    public static EventDelegate Set(List<EventDelegate> list, EventDelegate.Callback callback, float shotInterval = 0f)
    {
        EventDelegate result;
        if (list != null)
        {
            EventDelegate eventDelegate = new EventDelegate(callback);
            if (shotInterval != 0f)
            {
                eventDelegate.shotInterval = shotInterval;
            }
            list.Clear();
            list.Add(eventDelegate);
            result = eventDelegate;
        }
        else
        {
            result = null;
        }
        return result;
    }

    public static EventDelegate Add(List<EventDelegate> list, EventDelegate.Callback callback, float shotInterval = 0f)
    {
        EventDelegate result;
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                EventDelegate eventDelegate = list[i];
                if (eventDelegate != null && eventDelegate.Equals(callback))
                {
                    result = eventDelegate;
                    return result;
                }
            }
            EventDelegate eventDelegate2 = new EventDelegate(callback);
            eventDelegate2.shotInterval = shotInterval;
            list.Add(eventDelegate2);
            result = eventDelegate2;
        }
        else
        {
            Debug.LogWarning("Attempting to add a callback to a list that's null");
            result = null;
        }
        return result;
    }

    public static EventDelegate Add(List<EventDelegate> list, EventDelegate ed, float shotInterval = 0f)
    {
        EventDelegate result;
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                EventDelegate eventDelegate = list[i];
                if (eventDelegate != null && eventDelegate.Equals(ed))
                {
                    result = eventDelegate;
                    return result;
                }
            }
            list.Add(ed);
            result = ed;
        }
        else
        {
            Debug.LogWarning("Attempting to add a callback to a list that's null");
            result = null;
        }
        return result;
    }

    public static bool Remove(List<EventDelegate> list, EventDelegate.Callback callback)
    {
        bool result;
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                EventDelegate eventDelegate = list[i];
                if (eventDelegate != null && eventDelegate.Equals(callback))
                {
                    list.RemoveAt(i);
                    result = true;
                    return result;
                }
            }
        }
        result = false;
        return result;
    }
}
