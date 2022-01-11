using Pathfinder;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerController : MonoBehaviour
{
    [SerializeField] float m_speed;
    private Node m_targetNode;
    private Cell m_location = null;
    private Rigidbody2D m_rigidbody;
    private bool m_work = true;
    private Action m_endAction = null;

    public event Action<Cell> onCellEnter;

    public bool Moving { get; private set; } = false;
    public Cell CurrentCell => m_location;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Stop(Action action)
    {
        m_endAction = action;
        m_work = false;
        if(!Moving)
        {
            m_endAction?.Invoke();
        }
    }
    private void FixedUpdate()
    {
        if (m_targetNode != null)
        {
            m_rigidbody.MovePosition(UpdatePosition(Time.deltaTime));
        }
    }

    private Vector2 UpdatePosition(float deltaTime)
    {
        if (m_targetNode != null && deltaTime > 0.001f)
        {
            Vector2 direction = MyGrid.Instance.GetPosition(m_targetNode.Cell) - m_rigidbody.position;
            Vector2 step = direction.normalized * m_speed * deltaTime;

            //длина шага превышает длину до точки, точка достигнута
            if (step.sqrMagnitude >= direction.sqrMagnitude)
            {
                if (!m_work)
                {
                    m_targetNode = null;
                    m_endAction?.Invoke();
                }
                else { SetTargetNode(m_targetNode.NextNode); }

                float usedTime = deltaTime * (direction.sqrMagnitude / step.sqrMagnitude);
                return direction + UpdatePosition(deltaTime - usedTime);
            }

            return m_rigidbody.position + step;
        }
        return m_rigidbody.position;
    }

    internal void SetTargetNode(Node node)
    {
      
        if (!m_work) return;


        if (node != null) { m_location?.Exit(gameObject); }

        m_targetNode = node;
        Moving = m_targetNode != null;
        if (m_targetNode != null) 
        {
            m_location = m_targetNode.Cell;
            m_location.Enter(gameObject);
            onCellEnter?.Invoke(m_targetNode.Cell);
        }
    }

    private void OnDestroy()
    {
        m_location?.Exit(gameObject);
    }

}
