using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using UniRx;

namespace GamePlay
{
    /// <summary>
    /// 有能力的东西。
    /// </summary>
    public abstract class Actor : TargetableObject
    {
        private Rect m_PlayerMoveBoundary = default(Rect);
        
        public Rect playerMoveBoundary
        {
            get
            {
                return m_PlayerMoveBoundary;
            }
            set
            {
                // if(lsBody!=null){lsBody.Position = value;}
                m_PlayerMoveBoundary = value;
            }
        }
        [SerializeField]
        private ActorData m_ActorData = null;
        public ActorData actorData
        {
            get
            {
                return m_ActorData;
            }
            set
            {
                // if(lsBody!=null){lsBody.Position = value;}
                m_ActorData = value;
            }
        }

       

        public Dictionary<string, BaseAbility> abilities;
        public bool isBorn { get; set; }

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            base.OnInit(userData);
            abilities = new Dictionary<string, BaseAbility>();
        }


        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_ActorData = userData as ActorData;
            if (m_ActorData == null)
            {
                Log.Error("ActorData data is invalid.");
                return;
            }

     
          

            Name = string.Format("Actor ({0})", Id.ToString());

            if (!isBorn)
            {
                foreach (var a in abilities)
                {
                    a.Value.OnBorn();
                }
            }
//            ScrollableBackground sceneBackground = FindObjectOfType<ScrollableBackground>();
//            if (sceneBackground == null)
//            {
//                Log.Warning("Can not find scene background.");
//                return;
//            }
//
//            m_PlayerMoveBoundary = new Rect(sceneBackground.PlayerMoveBoundary.bounds.min.x, sceneBackground.PlayerMoveBoundary.bounds.min.z,
//                sceneBackground.PlayerMoveBoundary.bounds.size.x, sceneBackground.PlayerMoveBoundary.bounds.size.z);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnHide(object userData)
#else
        protected internal override void OnHide(object userData)
#endif
        {
            base.OnHide(userData);
            foreach (var a in abilities)
            {
                a.Value.Reset();
            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
#else
        protected internal override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
#endif
        {
            base.OnAttached(childEntity, parentTransform, userData);
//            if (childEntity is Weaponer)
//            {
//                GetAbility<Weapon>().Weapons.Add((Weaponer)childEntity);
//                return;
//            }
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnDetached(EntityLogic childEntity, object userData)
#else
        protected internal override void OnDetached(EntityLogic childEntity, object userData)
#endif
        {
            base.OnDetached(childEntity, userData);
//            if (childEntity is Weaponer)
//            {
//                GetAbility<Weapon>().Weapons.Remove((Weaponer)childEntity);
//                return;
//            }
        }

        protected override void OnDead(Entity attacker)
        {
            base.OnDead(attacker);
            foreach (var a in abilities)
            {
                a.Value.OnDeath();
            }
            //             GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GenerateSerialId(), m_ActorData.DeadEffectId)
            //             {
            //                 Position = CachedTransform.localPosition,
            //             });
            //             GameEntry.Sound.PlaySound(m_ActorData.DeadSoundId);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            foreach (var e in abilities)
            {
                e.Value.SelfUpdate(elapseSeconds, realElapseSeconds);
            }
        }

        public void AddAbility(string name,BaseAbility a)
        {
            abilities.Add(name, a);
        }

        public void AddAbility<T>(T a) where T : BaseAbility
        {
            ProcedureMain main = GameEntry.Procedure.GetProcedure<ProcedureMain>() as ProcedureMain;
            if (main == null)
                return;
            string aName = main.CurrentGame.GetAbilityName(typeof(T));
            if (aName == null)
            {
                Debug.LogError("找不到能力类"+ typeof(T).Name);
                return;
            }
            abilities.Add(aName, a);
        }


        public void RemoveAbility(BaseAbility a)
        {
            if (a != null && abilities.ContainsKey(a.GetKey()))
                abilities.Remove(a.GetKey());
        }

        public T GetAbility<T>() where T : BaseAbility
        {
            if (GameEntry.Procedure == null)
                return null;
            ProcedureMain main = GameEntry.Procedure.GetProcedure<ProcedureMain>() as ProcedureMain;
            if (main == null)
                return null;
            string aName = main.CurrentGame.GetAbilityName(typeof(T));
            if (aName == null)
                return null;
            if (abilities.ContainsKey(aName))
                return (T)abilities[aName];
            return null;
        }

//        public void MoveByDir(Vector3 dir)
//        {
//            float speed = GetAbility<Attr>().Speed;
//            CachedTransform.localPosition += dir.normalized * speed;
//        }
//        public void MoveToTarget(Entity target)
//        {
//            GetAbility<Move>().MoveToTarget(target);
//        }
//        public void MoveToTarget(Vector3 target)
//        {
//            GetAbility<Move>().MoveToTarget(target);
//        }
    }
}
