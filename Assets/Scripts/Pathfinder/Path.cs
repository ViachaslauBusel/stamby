using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinder
{
    public static class Path
    {
        public static Node Find(Vector3 startPoint, Vector3 endPoint)//, Packet packet = null)
        {
         //   node = null;
            // оординаты €чейки в котором находитьс€ начальна€ точка
            Cell startCell = MyGrid.Instance.GetCell(startPoint); 

            Cell endCell = MyGrid.Instance.GetCell(endPoint);

            // онечна€ точка находитьс€ в пределах текущего нода
            if (startCell == endCell)
            {
                //   Debug.Log.Debug($"Path.Find return:  онечна€ точка находитьс€ в пределах текущего нода");
                return new Node()
                {
                    Cell = startCell,
                    WeightToFinish = 0
                };
            }

            //¬ес длины пути до конечной точки
            int weightF = (int)(Mathf.Abs(endCell.Column - startCell.Column) + Mathf.Abs(endCell.Row - startCell.Row) * 10.0f);
            Node startNode = new Node()
            {
                Cell = startCell,
                WeightToFinish = weightF
            };


            //ќткрытый список узлов
            List<Node> open = new List<Node>() { startNode };
            //«акрытый список узлов
            List<Node> closed = new List<Node>();


            //ћаксимальное количество циклов поиска
            int cicle = 0, maxCicle = 50;
            Node current = null;
            while (open.Count > 0 && cicle++ < maxCicle)
            {
                //ѕоиск текущего узла из открытого списка, с минимальным весом
                current = open.OrderBy((n) => n.Weight).First();
                //≈сли конечна€ точка была достигнута
                if (current.WeightToFinish == 0)
                {

                    closed.Add(current);
                    break;
                }

                {//”зел, который был задействован в поиске ближайших узлов перемещаем из открытого в закрытый список узлов
                    open.Remove(current);
                    closed.Add(current);
                }


                //ѕеребираем всех соседей узла current
                foreach (Cell neigbourCell in MyGrid.Instance.GetAround(current.Cell))
                {
                    //ѕровер€ем доступен ли этот узел физический
                    if (!neigbourCell.AvailableMove)
                    {
                        //”зел не доступен, переходим к следующему соседу
                        continue;
                    }

                    //≈сли такой узел уже есть в списке закрытых узлов, переходим к следующему
                    if (closed.Any((n) => n.Cell == neigbourCell)) { continue; }

                    //¬ес длины пути от текущей точки до новго узла
                    int weightToStart = ((Mathf.Abs(neigbourCell.Row - current.Cell.Row) + Mathf.Abs(neigbourCell.Column - current.Cell.Column)) == 2) ? 14 : 10;//≈сли узел расположен ортаганально\ текущего узла вес 14 если перпендикул€рно| 10
                    //¬ес от старта до новго узла
                    weightToStart = current.WeightToStart + weightToStart;

                    Node neigbourInOpen = open.Find((n) => n.Cell == neigbourCell);
                    //≈сли сосед уже есть в списке open и его вес от старта меньше текущего, переходим к следующему соседу
                    if (neigbourInOpen != null && neigbourInOpen.WeightToStart < weightToStart) { continue; }


                   



                    //≈сли сосед уже есть в списке, обновить данные
                    if (neigbourInOpen != null)
                    {
                        neigbourInOpen.WeightToStart = weightToStart;
                        neigbourInOpen.CameFrom = current;
                    }
                    else//≈сли нет создать новый
                    {
                        //¬ес длины пути до конечной точки
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

            //—оздаем цепочку узлов от начального до конечного. 
            while (current.CameFrom != null)
            {
                //Ѕерем последний узел, и присваиваем его родителю следующий узел на себ€
                current.CameFrom.NextNode = current;
                current = current.CameFrom;
            }
           return current.NextNode;

        }
    }
}