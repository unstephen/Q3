using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GamePlay
{
    public abstract class GameBase
    {
        public abstract GameMode GameMode
        {
            get;
        }


        public bool GameOver
        {
            get;
            protected set;
        }

        private Dictionary<Type, string> abilityNameDic = new Dictionary<Type, string>();

        public virtual void Initialize(GameMode fromMode)
        {
            RegAbilityName();
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);


            GameOver = false;

        }

        public virtual void Shutdown()
        {
            GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        }

        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {
           /* if (m_MyAircraft != null && m_MyAircraft.IsDead)
            {
                GameOver = true;
                return;
            }*/
        }

        protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
           /* if (ne.EntityLogicType == typeof(MyAircraft))
            {
                m_MyAircraft = (MyAircraft)ne.Entity.Logic;
            }*/
        }

        protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }
        public string GetAbilityName(Type type)
        {
            string ret ="";
            if(abilityNameDic.TryGetValue(type, out ret))
            {
                return ret;
            }
            return ret;
        }
        private void RegAbilityName()
        {
            abilityNameDic.Clear();
            abilityNameDic.Add(typeof(BuffControll), "BuffControll");
        }
    }
}
