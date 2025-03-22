using System.Collections;
using UnityEngine;

public class PlayerDummy : MonoBehaviour
{
    [SerializeField] private Color hitColor;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Color originColor;
    private PlayerController player;
    void Awake()
    {
        originColor = spriteRenderer.color;
    }
    void Start()
    {
        player = PlayerManager.Instance.currentPlayer;
    }
    void Update()
    {
        transform.localPosition = player.transform.position;
        transform.localRotation = player.transform.rotation;   
        spriteRenderer.color = player.IsCooling?hitColor:originColor;
    }
}
