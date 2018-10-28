using GameFramework.DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GamePlay
{
    [Serializable]
    public abstract class ActorData : TargetableObjectData
    {
        [SerializeField]
        private float m_Speed = 3f;
        public ActorData(int entityId, int typeId, CampType camp)
            : base(entityId, typeId,camp)
        {

        }
        /// <summary>
        /// 速度。
        /// </summary>
        public float Speed
        {
            get
            {
                return m_Speed;
            }
        }
    }
}
