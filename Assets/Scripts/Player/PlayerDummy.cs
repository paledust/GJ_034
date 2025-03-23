using System.Collections;
using UnityEngine;

public class PlayerDummy : MonoBehaviour
{
    [SerializeField] private Color hitColor;
    [SerializeField] private Animation m_anime;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool useOffset = false;
    private Color originColor;
    private PlayerController player;
    private Vector2 offset;
    void Awake()
    {
        originColor = spriteRenderer.color;
    }
    void Start()
    {
        player = PlayerManager.Instance.currentPlayer;
        offset = Vector2.zero;
        if(useOffset)
        {
            offset = transform.localPosition - player.transform.position;
        }
    }
    void Update()
    {
        transform.localPosition = (Vector2)player.transform.position + offset;
        transform.localRotation = player.transform.rotation;   
        spriteRenderer.color = player.IsCooling?hitColor:originColor;
    }
    public void DefeatTarget()=>m_anime.Play();
}
