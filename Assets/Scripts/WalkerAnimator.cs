using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerAnimator : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_render;
    [SerializeField] Sprite m_leftWalk, m_rightWalk, m_upWalk, m_downWalk;
    [SerializeField] Sprite m_leftAttack, m_righAttack, m_upAttack, m_downAttack;

    private Vector3 m_lasPosition;


    private void Awake()
    {
        m_lasPosition = transform.position;
    }

    private void Update()
    {
        Vector3 direction = transform.position - m_lasPosition;
        m_lasPosition = transform.position;

        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x < 0) { m_render.sprite = m_leftWalk; }
            else if (direction.x > 0) { m_render.sprite = m_rightWalk; }
        }
        else
        {
            if (direction.y < 0) { m_render.sprite = m_downWalk; }
            else if (direction.y > 0) { m_render.sprite = m_upWalk; }
        }
    }

    internal void PlayAtack(Transform target)
    {
        enabled = false;
        Vector3 direction = target.position - transform.position;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x < 0) { m_render.sprite = m_leftAttack; }
            else if (direction.x > 0) { m_render.sprite = m_righAttack; }
        }
        else
        {
            if (direction.y < 0) { m_render.sprite = m_downAttack; }
            else if (direction.y > 0) { m_render.sprite = m_upAttack; }
        }
    }
}
