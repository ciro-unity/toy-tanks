using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathAuthoring))]
public class PathEditor : Editor
{
	PathAuthoring pathScript;
	SerializedProperty speed;

	string arrayDataString = "waypoints.Array.data[{0}]";
	string arraySizeString = "waypoints.Array.size";

	private void OnEnable()
	{
		pathScript = (PathAuthoring)target;
	}


	//Recover the array of Vector3s from the serializedObject's properties
	private Vector3[] GetWaypoints()
	{
		int arrayCount = serializedObject.FindProperty(arraySizeString).intValue;
		Vector3[] wpArray = new Vector3[arrayCount];

		for(int i=0; i<arrayCount; i++)
		{
			wpArray[i] = serializedObject.FindProperty(string.Format(arrayDataString, i)).vector3Value;
		}

		return wpArray;
	}


	private void SetWaypoint(int index, Vector3 values)
	{
		serializedObject.FindProperty(string.Format(arrayDataString, index)).vector3Value = values;
	}


	//Draw the default inspector, and more!
	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		Vector3[] wpArray = GetWaypoints();

		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Number of Waypoints");
		int wpNumber = EditorGUILayout.IntField(wpArray.Length);
		if(GUI.changed)
		{
			if(wpNumber<2) wpNumber = 2; //waypoints need to be 2 or more
			serializedObject.FindProperty(arraySizeString).intValue = wpNumber;
			wpArray = GetWaypoints(); //need to get the array again because it might be shorter now
		}
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.Space();

		GUILayout.Label("Waypoints", EditorStyles.boldLabel);
		EditorGUILayout.Space();

		for(int i=0; i<wpArray.Length; i++)
		{
			EditorGUILayout.BeginHorizontal();
			Vector3 res = EditorGUILayout.Vector3Field("WP " + i, wpArray[i]);
			EditorGUILayout.EndHorizontal();

			//this happens if the User has changed Vector3 values in the Inspector
			if(GUI.changed)
			{
				SetWaypoint(i, res);
			}
		}

		//Button to reset all waypoints
		EditorGUILayout.Space();
		if(GUILayout.Button("Reset Waypoints"))
		{
			pathScript.Reset();
			EditorApplication.Beep();

			//force both the custom Inspector and the Scene View to show the changes
			Repaint();
			SceneView.RepaintAll();
		}

		//write the changes in the serialized object
		serializedObject.ApplyModifiedProperties();
	}


	//draw the arrow gizmos for each waypoint
	public void OnSceneGUI()
	{
		Vector3[] wpArray = GetWaypoints();

		Handles.color = new Color32(156, 39, 176, 255);
		Handles.ArrowCap(0, wpArray[0], Quaternion.LookRotation(wpArray[1]-wpArray[0], Vector3.up), 1f);
		for(int i = 0; i<wpArray.Length; i++)
		{
			//draw the gizmos
			EditorGUI.BeginChangeCheck();
			Vector3 gizmoPos = Handles.PositionHandle(wpArray[i], Quaternion.identity);
			
			//has the gizmo been moved?
			if(EditorGUI.EndChangeCheck())
			{
				SetWaypoint(i, gizmoPos);
			}

			//dotted lines and arrows
			Handles.SphereCap (0, wpArray[i], Quaternion.identity, .1f);
			if(i<wpArray.Length-1)
			{
				Handles.DrawDottedLine(wpArray[i], wpArray[i+1], 5f);
			}
			else
			{
				Handles.DrawDottedLine(wpArray[i], wpArray[0], 5f);
			}

			serializedObject.ApplyModifiedProperties();
		}

		//numeric labels
		Handles.BeginGUI();
		for(int i = 0; i < wpArray.Length; i++)
		{
			Vector2 wpPoint = HandleUtility.WorldToGUIPoint(wpArray[i]);
			Rect guiRect = new Rect(wpPoint.x-15f, wpPoint.y+30f, 30f, 20f);
			GUI.Box(guiRect, i.ToString());
		}
		Handles.EndGUI();
	}
}