public class EnemyState : IEnemyState
{
    protected EnemyBrain Brain;

    protected EnemyState(EnemyBrain brain)
    {
        Brain = brain;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
