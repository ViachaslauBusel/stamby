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
            //���������� ������ � ������� ���������� ��������� �����
            Cell startCell = MyGrid.Instance.GetCell(startPoint); 

            Cell endCell = MyGrid.Instance.GetCell(endPoint);

            //�������� ����� ���������� � �������� �������� ����
            if (startCell == endCell)
            {
                //   Debug.Log.Debug($"Path.Find return: �������� ����� ���������� � �������� �������� ����");
                return new Node()
                {
                    Cell = startCell,
                    WeightToFinish = 0
                };
            }

            //��� ����� ���� �� �������� �����
            int weightF = (int)(Mathf.Abs(endCell.Column - startCell.Column) + Mathf.Abs(endCell.Row - startCell.Row) * 10.0f);
            Node startNode = new Node()
            {
                Cell = startCell,
                WeightToFinish = weightF
            };


            //�������� ������ �����
            List<Node> open = new List<Node>() { startNode };
            //�������� ������ �����
            List<Node> closed = new List<Node>();


            //������������ ���������� ������ ������
            int cicle = 0, maxCicle = 50;
            Node current = null;
            while (open.Count > 0 && cicle++ < maxCicle)
            {
                //����� �������� ���� �� ��������� ������, � ����������� �����
                current = open.OrderBy((n) => n.Weight).First();
                //���� �������� ����� ���� ����������
                if (current.WeightToFinish == 0)
                {

                    closed.Add(current);
                    break;
                }

                {//����, ������� ��� ������������ � ������ ��������� ����� ���������� �� ��������� � �������� ������ �����
                    open.Remove(current);
                    closed.Add(current);
                }


                //���������� ���� ������� ���� current
                foreach (Cell neigbourCell in MyGrid.Instance.GetAround(current.Cell))
                {
                    //��������� �������� �� ���� ���� ����������
                    if (!neigbourCell.AvailableMove)
                    {
                        //���� �� ��������, ��������� � ���������� ������
                        continue;
                    }

                    //���� ����� ���� ��� ���� � ������ �������� �����, ��������� � ����������
                    if (closed.Any((n) => n.Cell == neigbourCell)) { continue; }

                    //��� ����� ���� �� ������� ����� �� ����� ����
                    int weightToStart = ((Mathf.Abs(neigbourCell.Row - current.Cell.Row) + Mathf.Abs(neigbourCell.Column - current.Cell.Column)) == 2) ? 14 : 10;//���� ���� ���������� ������������\ �������� ���� ��� 14 ���� ���������������| 10
                    //��� �� ������ �� ����� ����
                    weightToStart = current.WeightToStart + weightToStart;

                    Node neigbourInOpen = open.Find((n) => n.Cell == neigbourCell);
                    //���� ����� ��� ���� � ������ open � ��� ��� �� ������ ������ ��������, ��������� � ���������� ������
                    if (neigbourInOpen != null && neigbourInOpen.WeightToStart < weightToStart) { continue; }


                   



                    //���� ����� ��� ���� � ������, �������� ������
                    if (neigbourInOpen != null)
                    {
                        neigbourInOpen.WeightToStart = weightToStart;
                        neigbourInOpen.CameFrom = current;
                    }
                    else//���� ��� ������� �����
                    {
                        //��� ����� ���� �� �������� �����
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

            //������� ������� ����� �� ���������� �� ���������. 
            while (current.CameFrom != null)
            {
                //����� ��������� ����, � ����������� ��� �������� ��������� ���� �� ����
                current.CameFrom.NextNode = current;
                current = current.CameFrom;
            }
           return current.NextNode;

        }
    }
}