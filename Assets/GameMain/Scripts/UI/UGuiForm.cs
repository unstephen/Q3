﻿using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace GamePlay
{
    public abstract class UGuiForm : UIFormLogic
    {
        public const int DepthFactor = 100;
        private const float FadeTime = 0.3f;

        private static Font s_MainFont = null;
        private Canvas m_CachedCanvas = null;
        private CanvasGroup m_CanvasGroup = null;
        public bool initialized
        {
            get;
            private set;
        }

        public int OriginalDepth
        {
            get;
            private set;
        }

        public int Depth
        {
            get
            {
                return m_CachedCanvas.sortingOrder;
            }
        }

        public void Close()
        {
            Close(false);
        }

        public void Close(bool ignoreFade)
        {
            StopAllCoroutines();

            if (ignoreFade)
            {
                GameEntry.UI.CloseUIForm(this);
            }
            else
            {
                StartCoroutine(CloseCo(FadeTime));
            }
        }

        public void PlayUISound(int uiSoundId)
        {
            GameEntry.Sound.PlayUISound(uiSoundId);
        }

        public static void SetMainFont(Font mainFont)
        {
            if (mainFont == null)
            {
                Log.Error("Main font is invalid.");
                return;
            }

            s_MainFont = mainFont;

            GameObject go = new GameObject();
            go.AddComponent<Text>().font = mainFont;
            Destroy(go);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);

            m_CachedCanvas = gameObject.GetOrAddComponent<Canvas>();
            m_CachedCanvas.overrideSorting = true;
            OriginalDepth = m_CachedCanvas.sortingOrder;

            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();

            RectTransform transform = GetComponent<RectTransform>();
            transform.anchorMin = Vector2.zero;
            transform.anchorMax = Vector2.one;
            transform.anchoredPosition = Vector2.zero;
            transform.sizeDelta = Vector2.zero;

            gameObject.GetOrAddComponent<GraphicRaycaster>();

            Text[] texts = GetComponentsInChildren<Text>(true);
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].font = s_MainFont;
                if (!string.IsNullOrEmpty(texts[i].text))
                {
                    texts[i].text = GameEntry.Localization.GetString(texts[i].text);
                }
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_CanvasGroup.alpha = 0f;
            StopAllCoroutines();
            StartCoroutine(m_CanvasGroup.FadeToAlpha(1f, FadeTime));
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
            int oldDepth = Depth;
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);
            int deltaDepth = UGuiGroupHelper.DepthFactor * uiGroupDepth + DepthFactor * depthInUIGroup - oldDepth + OriginalDepth;
            Canvas[] canvases = GetComponentsInChildren<Canvas>(true);
            for (int i = 0; i < canvases.Length; i++)
            {
                canvases[i].sortingOrder += deltaDepth;
            }
        }

        private IEnumerator CloseCo(float duration)
        {
            yield return m_CanvasGroup.FadeToAlpha(0f, duration);
            GameEntry.UI.CloseUIForm(this);
        }

        public virtual void SetEvent()
        {
        }

        internal void DoInitIfDont()
        {
            if (!this.initialized)
            {
                this.OnInit(null);
                this.SetEvent();
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

        public bool SetSelfActive(bool active)
        {
            bool result;
            if (base.gameObject.activeSelf != active)
            {
                base.gameObject.SetActive(active);
                this.DoActiveChange();
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public virtual void DoActiveChange()
        {
        }

        public static T AddComponent<T>(GameObject go, object userData) where T : UGuiForm
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
    }
}