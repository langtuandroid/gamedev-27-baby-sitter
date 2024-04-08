using System.Linq;
using UnityEditor;
using UnityEngine;

public class MissingReferencesFinder : MonoBehaviour 
{
	private const string MenuRoot = "Webelinx/Missing References/";
	
	[MenuItem(MenuRoot + "Search in scene", false, 50)]
	public static void FindMissingReferencesInCurrentScene()
	{
		var sceneObjects = GetSceneObjects();
		FindMissingReferences(EditorApplication.currentScene, sceneObjects);
	}
	
	[MenuItem(MenuRoot + "Search in all scenes", false, 51)]
	public static void MissingSpritesInAllScenes()
	{
		foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled))
		{
			EditorApplication.OpenScene(scene.path);
			FindMissingReferencesInCurrentScene();
		}
	}

	[MenuItem(MenuRoot + "Search in assets", false, 52)]
	public static void MissingSpritesInAssets()
	{
		var allAssets = AssetDatabase.GetAllAssetPaths().Where(path => path.StartsWith("Assets/")).ToArray();
		var objs = allAssets.Select(a => AssetDatabase.LoadAssetAtPath(a, typeof(GameObject)) as GameObject).Where(a => a != null).ToArray();
		
		FindMissingReferences("Project", objs);
	}
	
	private static void FindMissingReferences(string context, GameObject[] objects)
	{
		foreach (var go in objects)
		{
			var components = go.GetComponents<Component>();
			
			foreach (var c in components)
			{
				if (!c)
				{
					Debug.LogError("Missing Component in GO: " + GetFullPath(go), go);
					continue;
				}
				
				SerializedObject so = new SerializedObject(c);
				var sp = so.GetIterator();
				
				while (sp.NextVisible(true))
				{
					if (sp.propertyType == SerializedPropertyType.ObjectReference)
					{
						if (sp.objectReferenceValue == null
						    && sp.objectReferenceInstanceIDValue != 0)
						{
							ShowError(context, go, c.GetType().Name, ObjectNames.NicifyVariableName(sp.name));
						}
					}
				}
			}
		}
	}
	
	private static GameObject[] GetSceneObjects()
	{
		return Resources.FindObjectsOfTypeAll<GameObject>()
			.Where(go => string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go))
			       && go.hideFlags == HideFlags.None).ToArray();
	}
	
	private static void ShowError (string context, GameObject go, string componentName, string propertyName)
	{
		var ERROR_TEMPLATE = "Missing Ref in: [{3}]{0}. Component: {1}, Property: {2}";
		
		Debug.LogError(string.Format(ERROR_TEMPLATE, GetFullPath(go), componentName, propertyName, context), go);
	}
	
	private static string GetFullPath(GameObject go)
	{
		return go.transform.parent == null
			? go.name
				: GetFullPath(go.transform.parent.gameObject) + "/" + go.name;
	}
}