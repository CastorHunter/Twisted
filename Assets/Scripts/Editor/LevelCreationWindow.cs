using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelCreationWindow : EditorWindow
{
    
    private int columns, rows;
    
    [MenuItem("Level/LevelCreationWindow")]
    public static void CreatWindows()
    {
        var myWIn = GetWindow<LevelCreationWindow>(false,"LevelCreationWindow");
        myWIn.Show();
    }

    private void OnGUI()
    {
        var boardManager = FindAnyObjectByType<BoardManager>();
        EditorGUILayout.LabelField("");
        if(GUILayout.Button("Generate Board")) //Be sure there is no board in the scene
        {
            boardManager.CreateBoard(columns, rows);
        }
        if(GUILayout.Button("Delete Board")) //Be sure there is no board in the scene
        {
            boardManager.DeleteBoard();
        }
        
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Columns : " + columns.ToString());
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("(-) Columns"))
        {
            columns--;
        }
        if(GUILayout.Button("(+) Columns"))
        {
            columns++;
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.LabelField("");
        EditorGUILayout.LabelField("Rows : " +rows.ToString());
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("(-) Rows"))
        {
            rows--;
        }
        if(GUILayout.Button("(+) Rows"))
        {
            rows++;
        }
        EditorGUILayout.EndHorizontal();
    }
}
