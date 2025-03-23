using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class TilingGameObjects : EditorWindow
{
    private Vector2 startPos;
    private Vector2 tileSize;
    private Vector2Int tileCount;
    private Transform root;
    private GameObject prefab;
    [MenuItem("Tools/Others/Tile GameObject")]
    public static void ShowWindow(){
        GetWindow<TilingGameObjects>("Tile GameObjects");
    }

    void OnGUI(){
        root = EditorGUILayout.ObjectField("Root", root, typeof(Transform), true) as Transform;
        prefab = EditorGUILayout.ObjectField("Prefab", prefab, typeof(GameObject), false) as GameObject;
        tileSize = EditorGUILayout.Vector2Field("Tile Size", tileSize); 
        startPos = EditorGUILayout.Vector2Field("Start Pos", startPos);
        tileCount = EditorGUILayout.Vector2IntField("Tile Count", tileCount);

        if(GUILayout.Button("Tile GameObjects")){
            for(int i=0; i<tileCount.y; i++){
                for(int j=0;j<tileCount.x;j++)
                {
                    Vector2 pos = startPos+new Vector2(j*tileSize.x/(tileCount.x+0.0f), i*tileSize.y/(tileCount.y+0.0f));
                    GameObject go = PrefabUtility.InstantiatePrefab(prefab, root) as GameObject;
                    go.transform.localPosition = pos;
                }
            }
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}