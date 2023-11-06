using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Zenject;

public class Bomb : MonoBehaviour
{
    [SerializeField] float m_explosionDelay = 2.0f;
    [SerializeField] float m_radiusStaticExplosion = 1.0f;
    [SerializeField] float m_radiusWalkerExplosion = 2.0f;
    private GameOver m_gameOver;

    [Inject]
    private void Construct(GameOver gameOver)
    {
        m_gameOver = gameOver;
    }

    private void Start()
    {
        StartCoroutine(ExplodeDelay());
    }

    private IEnumerator ExplodeDelay()
    {
        yield return new WaitForSeconds(m_explosionDelay);
        Collider2D[] staticOBJ = Physics2D.OverlapCircleAll(transform.position, m_radiusStaticExplosion);
        foreach (Collider2D _static in staticOBJ)
        {
            if (_static.gameObject.CompareTag("Static")) { Destroy(_static.gameObject); }
        }

        Collider2D[] walkerOBJ = Physics2D.OverlapCircleAll(transform.position, m_radiusWalkerExplosion);
        foreach (Collider2D _walker in walkerOBJ)
        {
            if (_walker.gameObject.CompareTag("Enemy")) { Destroy(_walker.gameObject); }

            if (_walker.gameObject.CompareTag("Player"))
            {
                Destroy(_walker.gameObject); 
                m_gameOver.Open(1.0f);
            }
        }
        Destroy(gameObject);
    }
}
