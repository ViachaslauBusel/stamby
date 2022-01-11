using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MyGrid))]
public class MyGridEditor : Editor
{
    private SerializedProperty m_gridSize;
    private SerializedProperty m_cellSize;
    private SerializedProperty m_offsetRow;

    private bool m_workWithGrid = false;

    private void OnEnable()
    {
        m_workWithGrid = true;
        m_gridSize = serializedObject.FindProperty("m_gridSize");
        m_cellSize = serializedObject.FindProperty("m_cellSize");
        m_offsetRow = serializedObject.FindProperty("m_offsetRow");

        SceneView.beforeSceneGui += BeforeOnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.beforeSceneGui -= BeforeOnSceneGUI;
        m_workWithGrid = false;
    }

    private void BeforeOnSceneGUI(SceneView obj)
    {
        MyGrid myGrid = (MyGrid)serializedObject.targetObject;
        if (Event.current.type == EventType.MouseDown && m_workWithGrid)
        {
            Vector3 mousePosition = Event.current.mousePosition;
            mousePosition.y = Camera.current.pixelHeight - mousePosition.y;
            Vector3 position = Camera.current.ScreenToWorldPoint(mousePosition);


            Selection.activeGameObject = myGrid.GetCellOBJ(position);

            Event.current.Use();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_gridSize);
        EditorGUILayout.PropertyField(m_cellSize);
        EditorGUILayout.PropertyField(m_offsetRow);
        if (GUILayout.Button("GENERATE") && EditorUtility.DisplayDialog("WARNING", "¬нимание содержимое €чеек будет уничтожено, продолжить?", "yes", "no"))
        {
            MyGrid myGrid = (MyGrid)serializedObject.targetObject;
            for (int i = myGrid.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(myGrid.transform.GetChild(i).gameObject);
            }
            for (int row=0; row<myGrid.GridSize.y; row++)
            {
                for (int column = 0; column < myGrid.GridSize.x; column++)
                {
                    GameObject cellOBJ = new GameObject();
                    cellOBJ.name = $"Cell_{row}_{column}";
                    cellOBJ.transform.SetParent(myGrid.transform);
                    Vector3 position = new Vector3(column * myGrid.CellSize.x + (myGrid.OffsetRow * row),
                                                             row * myGrid.CellSize.y,
                                                             0.0f);
                    position += new Vector3(myGrid.CellSize.x / 2.0f, myGrid.CellSize.y / 2.0f, 0.0f);
                    cellOBJ.transform.localPosition = position;

                    cellOBJ.AddComponent<Cell>();
                }
            }
        }
        GUILayout.Space(20.0f);
        if(GUILayout.Button("mark as finish"))
        {
            Cell cell = Selection.activeGameObject?.GetComponent<Cell>();
            if(cell != null)
            {
                cell.MarkAsFinish();
            }
        }
        serializedObject.ApplyModifiedProperties();
    }

}
