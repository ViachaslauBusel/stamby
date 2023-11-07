using GameField;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinder
{
    public class Node
    {
        public Cell Cell { get; set; }
        /// <summary>The node from which we came to this node</summary>
        public Node CameFrom { get; set; }
        /// <summary>Next node on the way to the finish line</summary>
        public Node NextNode { get; set; }
        /// <summary>Weight of path length from starting point</summary>
        public int WeightToStart { get; set; } = 0;
        /// <summary>Weight of path length to end point</summary>
        public int WeightToFinish { get; set; } = 0;
        public int Weight => (WeightToFinish + WeightToFinish);

        public Node Last()
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