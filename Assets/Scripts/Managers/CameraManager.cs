using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineCamera vc_cam;

    private CinemachineBasicMultiChannelPerlin noiseControl;
    private CoroutineExcuter coroutineExcuter;

    void Awake()
    {
        coroutineExcuter = new CoroutineExcuter(this);
        noiseControl = vc_cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public void ShakeScreen(float duration, float amp)
    {
        coroutineExcuter.Excute(coroutineShake(duration, amp));
    }
    IEnumerator coroutineShake(float duration, float amp)
    {
        noiseControl.AmplitudeGain = amp;
        yield return new WaitForLoop(duration, (t)=>{
            noiseControl.AmplitudeGain = Mathf.Lerp(amp, 0, t);
        });
        noiseControl.AmplitudeGain = 0;
    }
}
