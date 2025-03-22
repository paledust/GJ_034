using UnityEngine;

public class PlayerDummy : MonoBehaviour
{
    private PlayerController player;
    void Start()
    {
        player = PlayerManager.Instance.currentPlayer;
    }
    void Update()
    {
        transform.localPosition = player.transform.position;
        transform.localRotation = player.transform.rotation;     
    }
}
