using Pathfinder;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   
    public static PlayerController Instance;

    [SerializeField] Camera m_camera;
    private WalkerController m_controller;


    public WalkerController Controller => m_controller;

    private void Awake()
    {
        Instance = this;
        m_controller = GetComponent<WalkerController>();
        m_controller.onCellEnter += OnCellEnter;
    }

    /// <summary>
    /// ��������� ��� ����� ������
    /// </summary>
    /// <param name="cell"></param>
    private void OnCellEnter(Cell cell)
    {
        foreach (Cell neigbour in MyGrid.Instance.GetAround(cell))
        {
            //���� � �������� ������� ���������� ����, ���������� ������ � ����� � ��������� �������� �����
            GameObject enemy = neigbour.FindTag("Enemy");
            if(enemy != null)
            {
                Debug.Log($"Player ��������� �����");
                enemy.GetComponent<WalkerController>().Stop(() =>
                {
                    enemy.GetComponent<WalkerAnimator>().PlayAtack(transform);
                });
                m_controller.Stop(() =>
                {
                    GameOver.Instance.Open(2.0f);
                });
                return;
            }
        }
        if (cell.Finish)
        {
            GameOver.Instance.Open(1.0f, "YOU WIN");
        }
    }



    private void Start()
    {
        m_controller.SetTargetNode(Path.Find(transform.position, transform.position));
       
    }
    public void PlayerInput()
    {
            m_controller.SetTargetNode(Path.Find(transform.position, m_camera.ScreenToWorldPoint(Input.mousePosition)));
    }

    private void OnDestroy()
    {
        m_controller.onCellEnter -= OnCellEnter;
    }
}
