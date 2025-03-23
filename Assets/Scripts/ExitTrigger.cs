using UnityEngine;
using DG.Tweening;
using SimpleAudioSystem;

public class ExitTrigger : MonoBehaviour
{
    [SerializeField] private Transform renderTrans;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private string exitClip;
    private bool hasPlayer;
    private bool hasExit=false;
    void OnEnable()
    {
        EventHandler.E_OnCheckTarget += CheckExit;
    }
    void OnDisable()
    {
        EventHandler.E_OnCheckTarget -= CheckExit;
    }
    void CheckExit()
    {
        if(hasPlayer)
        {
            hasExit = true;

            AudioManager.Instance.PlaySoundEffect(audioSource, exitClip, 1);
            EventHandler.Call_OnExitGame();
            renderTrans.DOKill();
            renderTrans.DOScale(250, 1f).SetEase(Ease.OutQuad)
            .OnComplete(()=>Application.Quit());
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(!hasExit && other.tag == Service.PLAYER_TAG)
        {
            renderTrans.DOKill();
            renderTrans.DOScale(10, 0.2f);
            hasPlayer = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {   
        if(!hasExit && other.tag == Service.PLAYER_TAG)
        {
            renderTrans.DOKill();
            renderTrans.DOScale(1, 0.2f);
            hasPlayer = false;
        }
    }
}
