﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class NewNodeWindow : EditorWindow
{
	string type;
	public Action OnCreateNode;

	void OnGUI()
	{
		GUILayout.Label("Base Settings", EditorStyles.boldLabel);
		type = EditorGUILayout.TextField("Node Type", type);
		if (GUILayout.Button("Create"))
		{
			OnCreateNode();
		}
	}
}

public class GraphEditorWindow : EditorWindow
{

    List<Rect> windows = new List<Rect>();
    List<int> windowsToAttach = new List<int>();
    List<int> attachedWindows = new List<int>();

    [MenuItem("Window/Behavior Tree")]
    static void ShowEditor()
    {
        EditorWindow.GetWindow<GraphEditorWindow>();
    }

	string myString;

    void OnGUI()
    {
		GUILayout.Label("Base Settings", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField("Text Field", myString);

        if (windowsToAttach.Count == 2)
        {
            attachedWindows.Add(windowsToAttach[0]);
            attachedWindows.Add(windowsToAttach[1]);
            windowsToAttach = new List<int>();
        }

        if (attachedWindows.Count >= 2)
        {
            for (int i = 0; i < attachedWindows.Count; i += 2)
            {
                DrawNodeCurve(windows[attachedWindows[i]], windows[attachedWindows[i + 1]]);
            }
        }

        BeginWindows();

        if (GUILayout.Button("Create Node"))
        {
            var newNodeWindow = EditorWindow.GetWindow<NewNodeWindow>();
			newNodeWindow.OnCreateNode = () =>
			{
				windows.Add(new Rect(10, 10, 100, 100));
			};
        }

		var style = new GUIStyle();
		//style.alignment = TextAnchor.MiddleCenter;
		
        for (int i = 0; i < windows.Count; i++)
        {
			GUI.Box(windows[i], "Sequence", style); //GUI.Window(i, windows[i], DrawNodeWindow, "Window " + i);
        }

        EndWindows();
    }


    void DrawNodeWindow(int id)
    {
        if (GUILayout.Button("Attach"))
        {
            windowsToAttach.Add(id);
        }

        GUI.DragWindow();
    }


    void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 50;
        Vector3 endTan = endPos + Vector3.left * 50;
        Color shadowCol = new Color(0, 0, 0, 0.06f);

        for (int i = 0; i < 3; i++)
        {// Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
    }
}
