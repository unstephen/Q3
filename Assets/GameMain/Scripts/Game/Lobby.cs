using UnityGameFramework.Runtime;
using GameFramework.Event;
using UnityEngine;

namespace GamePlay
{
    public class Lobby : GameBase
    {
        private float m_ElapseSeconds = 0f;

        public override GameMode GameMode
        {
            get
            {
                return GameMode.Lobby;
            }
        }

        public override void Initialize(GameMode fromMode)
        {
            base.Initialize(fromMode);
            if (fromMode > GameMode.Lobby)
            {
                //游戏返回大厅
                RoomManager.Instance.rData.Clear();
            }
            GameEntry.UI.OpenUIForm(UIFormId.MainForm, this);
            GameEntry.UI.OpenUIForm(UIFormId.TopBarForm, this);
            if (GameEntry.UI.HasUIForm(UIFormId.CreateRoomForm))
            {
                GameEntry.UI.GetUIForm(UIFormId.CreateRoomForm).Close(true);
            }
        }

        public override void Shutdown()
        {
            base.Shutdown();
            GameEntry.UI.GetUIForm(UIFormId.MainForm).Close(true);
            GameEntry.UI.GetUIForm(UIFormId.TopBarForm).Close(true);
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);

            m_ElapseSeconds += elapseSeconds;
         /*   if (m_ElapseSeconds >= 1f)
            {
                m_ElapseSeconds = 0f;
                IDataTable<DRAsteroid> dtAsteroid = GameEntry.DataTable.GetDataTable<DRAsteroid>();
                float randomPositionX = SceneBackground.EnemySpawnBoundary.bounds.min.x + SceneBackground.EnemySpawnBoundary.bounds.size.x * (float)Utility.Random.GetRandomDouble();
                float randomPositionZ = SceneBackground.EnemySpawnBoundary.bounds.min.z + SceneBackground.EnemySpawnBoundary.bounds.size.z * (float)Utility.Random.GetRandomDouble();
                GameEntry.Entity.ShowAsteroid(new AsteroidData(GameEntry.Entity.GenerateSerialId(), 60000 + Utility.Random.GetRandom(dtAsteroid.Count))
                {
                    Position = new Vector3(randomPositionX, 0f, randomPositionZ),
                });
            }*/
        }

    }
}
