using Pathfinder;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIComponent : MonoBehaviour
{
    [SerializeField] float m_speed = 6.0f;
    private Dictionary<State, BaseState> m_states = new Dictionary<State, BaseState>();
    private BaseState m_activeState;
    private Rigidbody2D m_rigidbody;
    private WalkerController m_controller;


    public Rigidbody2D Rigidbody => m_rigidbody;
    public float Speed => m_speed;
    public WalkerController Controller => m_controller;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_controller = GetComponent<WalkerController>();
        m_states.Add(State.Waiting, new IdleState(this));
        m_states.Add(State.Walk, new WalkState(this));

        m_controller.onCellEnter += OnCellEnter;

    }

    /// <summary>
    /// Вызывется при смене ячейки
    /// </summary>
    /// <param name="cell"></param>
    private void OnCellEnter(Cell cell)
    {
        foreach (Cell neigbour in MyGrid.Instance.GetAround(cell))
        {
            //Если в соседних ячейках находиться враг, остановить игрока и врага и запустить анимацию атаки
            GameObject player = neigbour.FindTag("Player");
            if (player != null)
            {
                Debug.Log($"Враг обноружил игрока");
                player.GetComponent<WalkerController>().Stop(() =>
                {
                    GameOver.Instance.Open(2.0f);
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
        m_controller.SetTargetNode(Path.Find(transform.position, transform.position));
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
        //Если true смена состояния автомата
        if (m_activeState?.Update(Time.deltaTime, out state) ?? true)
        {
            if (m_states.ContainsKey(state))
            {
                m_activeState = m_states[state];
                //Debug.Log($"Состояние сменилось на {state}");
                m_activeState.Reset();
            }
        }
    }

    private void OnDestroy()
    {
        m_controller.onCellEnter -= OnCellEnter;
    }
}

