using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinder
{
    public class Node
    {
        public Cell Cell { get; set; }


        /// <summary>��� �� �������� �� ������ � ����� ����</summary>
        public Node CameFrom { get; set; }
        /// <summary>��������� ��� ���� � ������</summary>
        public Node NextNode { get; set; }
        /// <summary>��� ����� ���� �� ��������� �����</summary>
        public int WeightToStart { get; set; } = 0;
        /// <summary>��� ����� ����  �� �������� �����</summary>
        public int WeightToFinish { get; set; } = 0;
        public int Weight => (WeightToFinish + WeightToFinish);

        internal Node Last()
        {
            if (NextNode != null) return NextNode.Last();
            return this;
        }
        public int Count()
        {
            if (NextNode != null) return NextNode.Count() + 1;
            return 1;
        }

    }
}