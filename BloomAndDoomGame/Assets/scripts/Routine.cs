using System;
using UnityEngine;
using System.Collections;

public class Routine : MonoBehaviour
{
    [SerializeField]
    private float m_IntervalSeconds = 1f;

    private EnemyPerception m_EnemyPerception;

    private bool m_PlayerDetected = false;

    private WaitForSeconds m_Wait;
    private Coroutine m_Loop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_EnemyPerception = GetComponent<EnemyPerception>();
        m_Loop = StartCoroutine(Loop());
        m_Wait = new WaitForSeconds(m_IntervalSeconds);
    }

    private IEnumerator Loop()
    {
        while (true && !m_PlayerDetected)
        {
            if (m_EnemyPerception.DetectTarget() != null)
            {
                m_PlayerDetected = true;
                Debug.Log("Player detected!");
                yield break;
            }
            else
            {
                Debug.Log("Player not detected.");
            }
            yield return m_Wait;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
