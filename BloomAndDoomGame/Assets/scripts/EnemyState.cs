
public  abstract class EnemyState
{
    protected EnemyMovement m_EnemyMovement;
    protected EnemyPerception m_EnemyPerception;

    public EnemyState(EnemyMovement enemyMovement, EnemyPerception enemyPerception)
    {
        m_EnemyMovement = enemyMovement;
        m_EnemyPerception = enemyPerception;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
