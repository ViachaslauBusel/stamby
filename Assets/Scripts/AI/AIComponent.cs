using AI;
using GameField;
using Pathfinder;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Walkers;
using Zenject;


namespace AI
{
    public class AIComponent : MonoBehaviour
    {
        [SerializeField] float m_speed = 6.0f;
        private Dictionary<State, BaseState> m_states = new Dictionary<State, BaseState>();
        private BaseState m_activeState;
        private Rigidbody2D m_rigidbody;
        private WalkerController m_controller;
        private AiStateFactory m_stateFactory;
        private GameOver m_gameOver;
        private GameGrid m_grid;

        public Rigidbody2D Rigidbody => m_rigidbody;
        public float Speed => m_speed;
        public WalkerController Controller => m_controller;


        [Inject]
        private void Constrcut(AiStateFactory stateFactory, GameOver gameOver, GameGrid myGrid)
        {
            m_stateFactory = stateFactory;
            m_gameOver = gameOver;
            m_grid = myGrid;
        }

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
            m_controller = GetComponent<WalkerController>();
            m_states.Add(State.Waiting, m_stateFactory.CreateState(this, State.Waiting));
            m_states.Add(State.Walk, m_stateFactory.CreateState(this, State.Walk));

            m_controller.onCellEnter += OnCellEnter;

        }

        /// <summary>
        /// Called when a cell changes
        /// </summary>
        /// <param name="cell"></param>
        private void OnCellEnter(Cell cell)
        {
            foreach (Cell neigbour in m_grid.GetAround(cell))
            {
                //If there is an enemy in adjacent cells, stop the player and enemy and start the attack animation
                GameObject player = neigbour.FindTag("Player");
                if (player != null)
                {
                    Debug.Log($"The enemy has detected the player");
                    player.GetComponent<WalkerController>().Stop(() =>
                    {
                        m_gameOver.Open(2.0f);
                    });
                    m_controller.Stop(() =>
                    {
                        gameObject.GetComponent<WalkerAnimator>().PlayAtack(player.transform);
                    });
                }
            }

        }

        private void Start()
        {
            m_controller.SetTargetNode(m_grid.Find(transform.position, transform.position));
        }
        private void OnEnable()
        {
            m_activeState = m_states[State.Waiting];
            m_activeState.Reset();
        }

        private void OnDisable()
        {
            m_activeState = null;
        }

        private void FixedUpdate()
        {
            State state = State.None;
            //If true, change the state of the machine
            if (m_activeState?.Update(Time.deltaTime, out state) ?? true)
            {
                if (m_states.ContainsKey(state))
                {
                    m_activeState = m_states[state];
                    //Debug.Log($"The status changed to {state}");
                    m_activeState.Reset();
                }
            }
        }

        private void OnDestroy()
        {
            m_controller.onCellEnter -= OnCellEnter;
        }
    }
}
