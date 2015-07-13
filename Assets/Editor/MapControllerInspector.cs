using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(MapController))]
public class MapControllerInspector : Editor {
	
	public override void OnInspectorGUI() {
		//base.OnInspectorGUI();
		DrawDefaultInspector();
		
		if(GUILayout.Button("Regenerate")) {
			MapController tileMap = (MapController)target;
			tileMap.BuildMesh();
		}
	}
}
