using GameField;
using Pathfinder;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Walkers
{
    public class WalkerController : MonoBehaviour
    {
        [SerializeField] float m_speed;
        private Node m_targetNode;
        private Cell m_location = null;
        private Rigidbody2D m_rigidbody;
        private bool m_emergencyStop = false;
        private Action m_endAction = null;
        private GameGrid m_myGrid;

        public event Action<Cell> onCellEnter;

        public bool Moving { get; private set; } = false;
        public Cell CurrentCell => m_location;

        [Inject]
        private void Construct(GameGrid myGrid)
        {
            m_myGrid = myGrid;
        }

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Stop(Action action)
        {
            m_endAction = action;
            m_emergencyStop = true;
            if (!Moving)
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
            if (m_targetNode != null && deltaTime > 0.0001f)
            {
                Vector2 direction = m_myGrid.GetPosition(m_targetNode.Cell) - m_rigidbody.position;
                Vector2 step = direction.normalized * m_speed * deltaTime;

                //step length exceeds length to point, point reached
                if (step.sqrMagnitude >= direction.sqrMagnitude)
                {
                    if (m_emergencyStop)
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

            if (m_emergencyStop) return;


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
}