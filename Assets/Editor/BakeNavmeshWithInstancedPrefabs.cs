using UnityEngine;
using UnityEditor;

public class BakeNavmeshWithInstancedPrefabs : MonoBehaviour {
	[MenuItem("TaZ/Bake navmesh with instanced prefabs")]
	public static void Bake () {
		Undo.IncrementCurrentGroup();
		Undo.SetCurrentGroupName("Bake navmesh");

		PrefabInstance.BakeAllInstancesWithUndo();
		NavMeshBuilder.BuildNavMesh();

		Undo.RevertAllInCurrentGroup();
	}
}
