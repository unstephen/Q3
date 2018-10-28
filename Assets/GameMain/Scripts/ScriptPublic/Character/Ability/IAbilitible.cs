namespace GamePlay
{
    public interface IAbilitible
    {

        void Init(Actor actor);

        void Reset();

        //void Execute(InputData data);

        void SelfUpdate(float elapseSeconds, float realElapseSeconds);

        void EarlySimulate();

        void Simulate();

        void Visualize();


        void LateSimulate();


        void SetPause(bool pause);

    }
}