using SimpleAudioSystem;
using UnityEngine;

public class Intro : MonoBehaviour
{
    [SerializeField] private string ambClip;
    void Start()
    {
        AudioManager.Instance.PlayAmbience(ambClip, true, 1, true);
    }
    public void NextLevel()
    {
        GameManager.Instance.SwitchingScene("Main");
    }
}
