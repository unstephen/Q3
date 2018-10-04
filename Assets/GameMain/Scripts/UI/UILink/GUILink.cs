using System;
using System.Collections.Generic;
using UnityEngine;

using GamePlay;

public class GUILink : MonoBehaviour
{
    [Serializable]
    public class UILink
    {
        public string Name;

        public string Tips;

        public GameObject LinkObj;

        public MonoBehaviour component;

        public T Get<T>() where T : Component
        {
            T result;
            if (!this.LinkObj)
            {
                result = default(T);
            }
            else if (this.component && this.component is T)
            {
                result = (this.component as T);
            }
            else
            {
                result = (this.LinkObj.GetComponent(typeof(T)) as T);
            }
            return result;
        }

        public T Add<T>() where T : Component
        {
            T result;
            if (!this.LinkObj)
            {
                result = default(T);
            }
            else if (this.component && this.component is T)
            {
                result = (this.component as T);
            }
            else
            {
                result = (this.LinkObj.AddComponent(typeof(T)) as T);
            }
            return result;
        }

        public T AddComponent<T>() where T : UGuiForm
        {
            T result;
            if (!this.LinkObj)
            {
                result = default(T);
            }
            else
            {
                T t = default(T);
                if (this.component && this.component is T)
                {
                    t = (this.component as T);
                }
                else
                {
                    t = (this.LinkObj.AddComponent(typeof(T)) as T);
                }
                if (t)
                {
                    t.DoInitIfDont();
                }
                result = t;
            }
            return result;
        }

        public T GetComponent<T>() where T : UGuiForm
        {
            T result;
            if (!this.LinkObj)
            {
                result = default(T);
            }
            else if (this.component && this.component is T)
            {
                result = (this.component as T);
            }
            else
            {
                result = (this.LinkObj.GetComponent(typeof(T)) as T);
            }
            return result;
        }

        public void RemoveComponent<T>() where T : UGuiForm
        {
            if (this.LinkObj)
            {
                if (this.component && this.component is T)
                {
                    UnityEngine.Object.Destroy(this.component);
                }
                else
                {
                    Component component = this.LinkObj.GetComponent(typeof(T)) as T;
                    if (component)
                    {
                        UnityEngine.Object.Destroy(component);
                    }
                }
            }
        }
    }

    public List<GUILink.UILink> Links = new List<GUILink.UILink>();

    private Dictionary<string, GUILink.UILink> all_objs = new Dictionary<string, GUILink.UILink>();

    private bool inited = false;

    private void Awake()
    {
        this.DoInitIfDont();
    }

    private void DoInitIfDont()
    {
        if (!this.inited)
        {
            this.inited = true;
            if (this.Links != null)
            {
                for (int i = 0; i < this.Links.Count; i++)
                {
                    GUILink.UILink uILink = this.Links[i];
                    if (uILink != null && uILink.LinkObj)
                    {
                        this.all_objs[uILink.Name] = uILink;
                    }
                }
            }
        }
    }

    public void ReBuildLinkMap()
    {
        if (this.Links != null)
        {
            this.all_objs.Clear();
            for (int i = 0; i < this.Links.Count; i++)
            {
                GUILink.UILink uILink = this.Links[i];
                if (uILink != null && uILink.LinkObj)
                {
                    this.all_objs[uILink.Name] = uILink;
                }
            }
        }
    }

    public T Get<T>(string name) where T : Component
    {
        this.DoInitIfDont();
        GUILink.UILink uILink = null;
        this.all_objs.TryGetValue(name, out uILink);
        T result;
        if (uILink == null)
        {
            Debug.Log(string.Format("[GUILink] Get<T> object {0} is not exist", name));
            result = default(T);
        }
        else
        {
            result = uILink.Get<T>();
        }
        return result;
    }

    public GameObject Get(string name)
    {
        this.DoInitIfDont();
        GUILink.UILink uILink = null;
        this.all_objs.TryGetValue(name, out uILink);
        GameObject result;
        if (uILink == null)
        {
            Debug.Log(string.Format("[GUILink] Get object {0} is not exist", name));
            result = null;
        }
        else
        {
            result = uILink.LinkObj;
        }
        return result;
    }

    public void ShowKeys()
    {
        this.DoInitIfDont();
        foreach (string current in this.all_objs.Keys)
        {
            Debug.Log("[GUILink] key : " + current);
        }
    }

    public T AddComponent<T>(string obj_name) where T : UGuiForm
    {
        GUILink.UILink uILink = this.GetUILink(obj_name);
        T result;
        if (uILink == null)
        {
            result = default(T);
        }
        else
        {
            result = uILink.AddComponent<T>();
        }
        return result;
    }

    public T GetComponent<T>(string obj_name) where T : UGuiForm
    {
        GUILink.UILink uILink = this.GetUILink(obj_name);
        T result;
        if (uILink == null)
        {
            result = default(T);
        }
        else
        {
            result = uILink.GetComponent<T>();
        }
        return result;
    }

    public void RemoveComponent<T>(string obj_name) where T : UGuiForm
    {
        GUILink.UILink uILink = this.GetUILink(obj_name);
        if (uILink != null)
        {
            uILink.RemoveComponent<T>();
        }
    }

    public EventDelegate AddEvent(string name, UIEventType ev, EventDelegate.Callback callback, float interval = 0f)
    {
        GUILink.UILink uILink = this.GetUILink(name);
        EventDelegate result;
        if (uILink == null)
        {
            Debug.Log(string.Format("[GUILink] AddEvent object {0} is not exist", name));
            result = null;
        }
        else
        {
            UIEventTrigger trigger = uILink.Add<UIEventTrigger>();
            List<EventDelegate> delegateList = GUILink.GetDelegateList(trigger, ev);
            if (delegateList != null)
            {
                result = EventDelegate.Add(delegateList, callback, interval);
            }
            else
            {
                result = null;
            }
        }
        return result;
    }

    public EventDelegate SetEvent(string name, UIEventType ev, EventDelegate.Callback callback, float interval = 0f)
    {
        GUILink.UILink uILink = this.GetUILink(name);
        EventDelegate result;
        if (uILink == null)
        {
            Debug.Log(string.Format("[GUILink] AddEvent object {0} is not exist", name));
            result = null;
        }
        else
        {
            UIEventTrigger uIEventTrigger = uILink.Get<UIEventTrigger>();
            if (null == uIEventTrigger)
            {
                uIEventTrigger = uILink.Add<UIEventTrigger>();
            }
            List<EventDelegate> delegateList = GUILink.GetDelegateList(uIEventTrigger, ev);
            if (delegateList != null)
            {
                result = EventDelegate.Set(delegateList, callback, interval);
            }
            else
            {
                result = null;
            }
        }
        return result;
    }

    public bool RemoveEvent(string name, UIEventType ev, EventDelegate.Callback callback)
    {
        GUILink.UILink uILink = this.GetUILink(name);
        bool result;
        if (uILink == null)
        {
            Debug.Log(string.Format("[GUILink] AddEvent object {0} is not exist", name));
            result = false;
        }
        else
        {
            UIEventTrigger uIEventTrigger = uILink.Get<UIEventTrigger>();
            if (uIEventTrigger == null)
            {
                result = false;
            }
            else
            {
                List<EventDelegate> delegateList = GUILink.GetDelegateList(uIEventTrigger, ev);
                result = (delegateList != null && EventDelegate.Remove(delegateList, callback));
            }
        }
        return result;
    }

    private GameObject GetGameObject(string name)
    {
        GUILink.UILink uILink = this.GetUILink(name);
        GameObject result;
        if (uILink == null)
        {
            result = null;
        }
        else
        {
            result = uILink.LinkObj;
        }
        return result;
    }

    private GUILink.UILink GetUILink(string name)
    {
        this.DoInitIfDont();
        GUILink.UILink uILink = null;
        GUILink.UILink result;
        if (this.all_objs.TryGetValue(name, out uILink))
        {
            result = uILink;
        }
        else
        {
            result = null;
        }
        return result;
    }

    internal static List<EventDelegate> GetDelegateList(UIEventTrigger trigger, UIEventType ev)
    {
        List<EventDelegate> result;
        switch (ev)
        {
            case UIEventType.Click:
                result = trigger.onClick;
                return result;
            case UIEventType.Deselect:
                result = trigger.onDeselect;
                return result;
            case UIEventType.DoubleClick:
                result = trigger.onDoubleClick;
                return result;
            case UIEventType.Drag:
                result = trigger.onDrag;
                return result;
            case UIEventType.DragEnd:
                result = trigger.onDragEnd;
                return result;
            case UIEventType.DragOut:
                result = trigger.onDragOut;
                return result;
            case UIEventType.DragOver:
                result = trigger.onDragOver;
                return result;
            case UIEventType.HoverOut:
                result = trigger.onHoverOut;
                return result;
            case UIEventType.HoverOver:
                result = trigger.onHoverOver;
                return result;
            case UIEventType.Press:
                result = trigger.onPress;
                return result;
            case UIEventType.Release:
                result = trigger.onRelease;
                return result;
            case UIEventType.Select:
                result = trigger.onSelect;
                return result;
        }
        result = null;
        return result;
    }
}
