using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameField
{
    public class Cell : MonoBehaviour, IEquatable<Cell>
    {
        [SerializeField] bool m_finish = false;
        private int m_row, m_column;
        private bool m_availableMove;
        private List<GameObject> m_walkers = new List<GameObject>();

        public int Row => m_row;
        public int Column => m_column;
        public bool AvailableMove => m_availableMove;
        public bool Finish => m_finish;
        public int RenderOrderInLayer => -m_row;


        public void Init(int row, int column)
        {
            m_row = row;
            m_column = column;
            m_availableMove = GetComponentInChildren<Collider2D>() == null;
        }

        private void Start()
        {
            SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer sprite in sprites)
            { sprite.sortingOrder = RenderOrderInLayer; }
        }

        public void UpdateStatus()
        {
            m_availableMove = true;
        }

        public GameObject FindTag(string tag)
        {
            return m_walkers.Find((obj) => obj.CompareTag(tag));
        }

        public void Exit(GameObject gameObject)
        {
            m_walkers.Remove(gameObject);
        }

        public void Enter(GameObject gameObject)
        {
            m_walkers.Add(gameObject);
        }

        public bool Equals(Cell other)
        {
            if (other == null) return false;
            return Row == other.Row && Column == other.Column;
        }

        public override bool Equals(object obj)
        {
            return Equals((Cell)obj);
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 31 + m_row;
            hash = hash * 31 + m_column;
            return hash;
        }

        public void MarkAsFinish()
        {
            m_finish = true;
        }
    }
}