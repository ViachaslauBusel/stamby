using GameField;
using Pathfinder;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Walkers;
using Zenject;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera m_camera;
    private WalkerController m_controller;
    private GameOver m_gameOver;
    private GameGrid m_grid;

    public WalkerController Controller => m_controller;

    [Inject]
    private void Construct(GameOver gameOver, GameGrid grid)
    {
        m_gameOver = gameOver;
        m_grid = grid;
    }

    private void Awake()
    {
        m_controller = GetComponent<WalkerController>();
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
            GameObject enemy = neigbour.FindTag("Enemy");
            if(enemy != null)
            {
                Debug.Log($"Player has detected an enemy");
                enemy.GetComponent<WalkerController>().Stop(() =>
                {
                    enemy.GetComponent<WalkerAnimator>().PlayAtack(transform);
                });
                m_controller.Stop(() =>
                {
                    m_gameOver.Open(2.0f);
                });
                return;
            }
        }
        if (cell.Finish)
        {
            m_gameOver.Open(1.0f, "YOU WIN");
        }
    }



    private void Start()
    {
        m_controller.SetTargetNode(m_grid.Find(transform.position, transform.position));
       
    }
    public void PlayerInput()
    {
            m_controller.SetTargetNode(m_grid.Find(transform.position, m_camera.ScreenToWorldPoint(Input.mousePosition)));
    }

    private void OnDestroy()
    {
        m_controller.onCellEnter -= OnCellEnter;
    }
}
