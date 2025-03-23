using System.Collections.Generic;
using UnityEngine;

public static class Service{
    public const string PLAYER_TAG = "Player";
    public const float MAX_GAME_TIME = 3f;
#region HelperFunction
    public static Vector2 ConstraintInBoundry(Vector2 pos, Vector4 bound, float boundExtend)
    {
        if(pos.x>bound.x+boundExtend)
            pos.x = bound.y;
        if(pos.x<bound.y-boundExtend)
            pos.x = bound.x;
        if(pos.y>bound.z+boundExtend)
            pos.y = bound.w;
        if(pos.y<bound.w-boundExtend)
            pos.y = bound.z;
        return pos;
    }
    /// <summary>
    /// Return a list of all active and inactive objects of T type in loaded scenes.
    /// </summary>
    /// <typeparam name="T">Object Type</typeparam>
    /// <returns></returns>
    public static T[] FindComponentsOfTypeIncludingDisable<T>(){
        int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
        var MatchObjects = new List<T> ();

        for(int i=0; i<sceneCount; i++){
            var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt (i);
            
            var RootObjects = scene.GetRootGameObjects ();

            foreach (var obj in RootObjects) {
                var Matches = obj.GetComponentsInChildren<T> (true);
                MatchObjects.AddRange (Matches);
            }
        }

        return MatchObjects.ToArray ();
    }
    public static void Shuffle<T>(ref T[] elements){
        var rnd = new System.Random();
        for(int i=0; i<elements.Length; i++){
            int index = rnd.Next(i+1);
            T tmp = elements[i];
            elements[i] = elements[index];
            elements[index] = tmp;
        }
    }
#endregion
}