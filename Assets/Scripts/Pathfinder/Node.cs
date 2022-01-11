using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinder
{
    public class Node
    {
        public Cell Cell { get; set; }


        /// <summary>Нод из которого мы пришли к этому ноду</summary>
        public Node CameFrom { get; set; }
        /// <summary>Следующий нод пути к финишу</summary>
        public Node NextNode { get; set; }
        /// <summary>Вес длины пути от начальной точки</summary>
        public int WeightToStart { get; set; } = 0;
        /// <summary>Вес длины пути  до конечной точки</summary>
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