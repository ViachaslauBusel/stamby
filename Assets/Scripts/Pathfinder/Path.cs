using GameField;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Pathfinder
{
    public static class Path
    {
        public static Node Find(this GameGrid myGrid, Vector3 startPoint, Vector3 endPoint)
        {
            //Coordinates of the cell in which the starting point is located
            Cell startCell = myGrid.GetCell(startPoint); 

            Cell endCell = myGrid.GetCell(endPoint);

            //The endpoint is equals current node
            if (startCell == endCell)
            {
                return new Node()
                {
                    Cell = startCell,
                    WeightToFinish = 0
                };
            }

            //Weight of path length to end point
            int weightF = (int)(Mathf.Abs(endCell.Column - startCell.Column) + Mathf.Abs(endCell.Row - startCell.Row) * 10.0f);
            Node startNode = new Node()
            {
                Cell = startCell,
                WeightToFinish = weightF
            };


            //Open node list
            List<Node> open = new List<Node>() { startNode };
            //Private node list
            List<Node> closed = new List<Node>();


            //Maximum number of search cycles
            int cicle = 0, maxCicle = 50;
            Node current = null;
            while (open.Count > 0 && cicle++ < maxCicle)
            {
                //Finding the current node from an open list, with minimum weight
                current = open.OrderBy((n) => n.Weight).First();
                //If the end point has been reached
                if (current.WeightToFinish == 0)
                {

                    closed.Add(current);
                    break;
                }

                {//The node that was involved in the search for nearby nodes is moved from the open to the closed list of nodes
                    open.Remove(current);
                    closed.Add(current);
                }

                //Iterate over all neighbors of the current node
                foreach (Cell neigbourCell in myGrid.GetAround(current.Cell))
                {
                    //Checking whether this node is physically accessible
                    if (!neigbourCell.AvailableMove)
                    {
                        //Node is not reachable, move on to next neighbor
                        continue;
                    }

                    //If such a node is already in the list of closed nodes, move on to the next one
                    if (closed.Any((n) => n.Cell == neigbourCell)) { continue; }

                    //Weight of the path length from the current point to the new node
                    int weightToStart = ((Mathf.Abs(neigbourCell.Row - current.Cell.Row) + Mathf.Abs(neigbourCell.Column - current.Cell.Column)) == 2) ? 14 : 10;//Если узел расположен ортаганально\ текущего узла вес 14 если перпендикулярно| 10
                    //Weight from start to new node
                    weightToStart = current.WeightToStart + weightToStart;

                    Node neigbourInOpen = open.Find((n) => n.Cell == neigbourCell);
                    //If the neighbor is already in the open list and its weight from the start is less than the current one, move on to the next neighbor
                    if (neigbourInOpen != null && neigbourInOpen.WeightToStart < weightToStart) { continue; }


                    //If the neighbor is already on the list, update the data
                    if (neigbourInOpen != null)
                    {
                        neigbourInOpen.WeightToStart = weightToStart;
                        neigbourInOpen.CameFrom = current;
                    }
                    else//If not, create a new one
                    {
                        //Weight of path length to end point
                        int weightToFinish = (int)(Mathf.Abs(endCell.Row - neigbourCell.Row) + Mathf.Abs(endCell.Column - neigbourCell.Column) * 10.0f);
                        Node newNode = new Node()
                        {
                            Cell = neigbourCell,
                            CameFrom = current,
                            WeightToStart = current.WeightToStart + weightToStart,
                            WeightToFinish = weightToFinish
                        };
                        open.Add(newNode);
                    }
                }
            }

            current = closed.OrderBy((n) => n.WeightToFinish).First();

            //We create a chain of nodes from the beginning to the end.
            while (current.CameFrom != null)
            {
                //We take the last node and assign the next node to its parent
                current.CameFrom.NextNode = current;
                current = current.CameFrom;
            }
           return current.NextNode;

        }
    }
}