using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GamePlay
{
    public class UGuiComponent : UIFormLogic
    {

        public bool initialized
        {
            get;
            private set;
        }
 


#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(object userData)
#else
        protected internal override void OnClose(object userData)
#endif
        {
            base.OnClose(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnPause()
#else
        protected internal override void OnPause()
#endif
        {
            base.OnPause();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnResume()
#else
        protected internal override void OnResume()
#endif
        {
            base.OnResume();
            
         //   m_CanvasGroup.alpha = 0f;
         //   StopAllCoroutines();
         //   StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnCover()
#else
        protected internal override void OnCover()
#endif
        {
            base.OnCover();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnReveal()
#else
        protected internal override void OnReveal()
#endif
        {
            base.OnReveal();
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnRefocus(object userData)
#else
        protected internal override void OnRefocus(object userData)
#endif
        {
            base.OnRefocus(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#else
        protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
#endif
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
#else
        protected internal override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
#endif
        {
        }


        internal void DoInitIfDont()
        {
            if (!this.initialized)
            {
                this.OnInit(null);
                this.initialized = true;
            }
        }

        public void SetActive(bool active)
        {
            if (base.gameObject.activeInHierarchy != active)
            {
                base.gameObject.SetActive(active);
            }
        }

        public void HideUI()
        {
            OnClose(null);
        }

        public bool SetSelfActive(bool active)
        {
            bool result;
            if (base.gameObject.activeSelf != active)
            {
                base.gameObject.SetActive(active);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }


        public static T AddComponent<T>(GameObject go, object userData) where T : UGuiComponent
        {
            T t = go.AddComponent(typeof(T)) as T;
            if (t != null)
            {
                t.DoInitIfDont();
                t.OnOpen(userData);
            }
            return t;
        }

        public EventDelegate AddEvent(UIEventType ev, EventDelegate.Callback callback, float interval = 0f)
        {
            UIEventTrigger uIEventTrigger = base.gameObject.GetComponent(typeof(UIEventTrigger)) as UIEventTrigger;
            if (uIEventTrigger == null)
            {
                uIEventTrigger = (base.gameObject.AddComponent(typeof(UIEventTrigger)) as UIEventTrigger);
            }
            List<EventDelegate> delegateList = GUILink.GetDelegateList(uIEventTrigger, ev);
            EventDelegate result;
            if (delegateList != null)
            {
                result = EventDelegate.Add(delegateList, callback, interval);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public EventDelegate SetEvent(UIEventType ev, EventDelegate.Callback callback, float interval = 0f)
        {
            UIEventTrigger uIEventTrigger = base.gameObject.GetComponent(typeof(UIEventTrigger)) as UIEventTrigger;
            if (uIEventTrigger == null)
            {
                uIEventTrigger = (base.gameObject.AddComponent(typeof(UIEventTrigger)) as UIEventTrigger);
            }
            List<EventDelegate> delegateList = GUILink.GetDelegateList(uIEventTrigger, ev);
            EventDelegate result;
            if (delegateList != null)
            {
                result = EventDelegate.Set(delegateList, callback, interval);
            }
            else
            {
                result = null;
            }
            return result;
        }

        public bool RemoveEvent(UIEventType ev, EventDelegate.Callback callback)
        {
            UIEventTrigger uIEventTrigger = base.gameObject.GetComponent(typeof(UIEventTrigger)) as UIEventTrigger;
            bool result;
            if (uIEventTrigger == null)
            {
                result = false;
            }
            else
            {
                List<EventDelegate> delegateList = GUILink.GetDelegateList(uIEventTrigger, ev);
                result = (delegateList != null && EventDelegate.Remove(delegateList, callback));
            }
            return result;
        }

        public void OpenUI()
        {
            OnOpen(null);
        }
    }
}
