using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    protected Animator m_Animator;

    [SerializeField]
    protected float m_AttackSpeed = 1.0f;
    
    float attackSpeedTimer = 0f;
    bool m_IsAttacking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        if (m_Animator == null)
        {
            Debug.LogError("Animator component not found on " + gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
          if(m_IsAttacking)
          {
              attackSpeedTimer += Time.deltaTime;
              if(attackSpeedTimer > m_AttackSpeed)
              {
                  attackSpeedTimer = 0;
                  BasicAttack();
              }
          }
    }
    public void StartAttacking(bool attack){
        m_IsAttacking = attack;
    }

    protected virtual void BasicAttack()
    {
    }

}
