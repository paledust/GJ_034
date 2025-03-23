using SimpleAudioSystem;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private string roomToneClip;
    void Awake()
    {
        PlayerManager.Instance.FindPlayer();
    }
    void Start()
    {
        AudioManager.Instance.PlayAmbience(roomToneClip, false, 1, true);
    }
}
