using UniRx;
namespace GamePlay
{
    public abstract class BaseAbility : IAbilitible
    {
        protected CompositeDisposable disposables = new CompositeDisposable();
        protected Actor ownerActor;
        protected int curSimulateFrame;
        protected bool pause = false;
        public void Init(Actor actor)
        {
            ownerActor = actor;
            OnInit();
        }
        public void Reset()
        {
            OnReset();
            disposables.Clear();
        }
        //     public void Execute(InputData data)
        //     {
        //         OnExecute(data);
        //     }
        public void SelfUpdate(float elapseSeconds, float realElapseSeconds)
        {
            OnUpdate(elapseSeconds, realElapseSeconds);
        }


        public void EarlySimulate()
        {
            if (ownerActor != null)
            {
                OnEarlySimulate();
            }
        }

        public void Simulate()
        {
            if (ownerActor != null)
            {
                OnSimulate();
                curSimulateFrame++;
            }
        }

        public void Visualize()
        {
            OnVisualize();
        }

        public void LateSimulate()
        {
            OnLateSimulate();
        }

        public void SetPause(bool pause)
        {
            this.pause = pause;
            OnSetPause(pause);
        }

        public virtual string GetKey()
        {
            return "baseability";
        }

        public virtual void OnInit()
        { }
        public virtual void OnReset()
        { }
        public virtual void OnBorn()
        { }
        public virtual void OnDeath()
        { }
        //     protected virtual void OnExecute(InputData data)
        //     { }

        public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        { }
        public virtual void OnEarlySimulate()
        { }
        public virtual void OnSimulate()
        { }
        public virtual void OnVisualize()
        { }
        public virtual void OnLateSimulate()
        { }
        public virtual void OnSetPause(bool pause)
        { }
    }
}