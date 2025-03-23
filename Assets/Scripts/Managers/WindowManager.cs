using System.Collections.Generic;
using Newtonsoft.Json.Bson;
using SimpleAudioSystem;
using UnityEngine;

public enum WorldType
{
    Dodge,
    Posses,
    Split,
    Idle,
    Constant
}

public class WindowManager : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Color baseColor;
    [SerializeField] private List<Window> windowPool;
    [SerializeField] private ParticleSystem p_explode;
[Header("Enter Sound")]
    [SerializeField] private AudioSource sfxAudio;
    [SerializeField] private string defeatClip;
    [SerializeField] private string explodeClip;
    [SerializeField] private string enterClip;
    private WorldType[] worldTypes = new WorldType[]{WorldType.Dodge, WorldType.Posses, WorldType.Split, WorldType.Idle, WorldType.Constant};
    private List<float> costs = new List<float>();
    private float lastCost;
    private int count = 5;
    private int createIndex;
    private Window activeWindow;

    void OnEnable()
    {
        EventHandler.E_OnEnterWindow += OnEnterWindowHandler;
        EventHandler.E_OnCheckTarget += OnCheckTarget;
        EventHandler.E_OnWindowExplode += OnWindowExplodeHandler;
    }
    void OnDisable()
    {
        EventHandler.E_OnEnterWindow -= OnEnterWindowHandler;
        EventHandler.E_OnCheckTarget -= OnCheckTarget;
        EventHandler.E_OnWindowExplode -= OnWindowExplodeHandler;
    }
    void Start()
    {
        lastCost = Service.MAX_GAME_TIME;
        createIndex = windowPool.Count;
        for(int i=0; i<windowPool.Count; i++)
        {
            windowPool[i].CompleteReset(GetNewColorFromBase(), i, worldTypes[i%worldTypes.Length]);
        }
        createIndex = 0;
        Service.Shuffle(ref worldTypes);
    }
    void OnCheckTarget()
    {
        if(activeWindow!=null)
        {
            if(activeWindow.CheckTarget())
            {
                OnDeactivateWindow(activeWindow);

                AudioManager.Instance.PlaySoundEffect(sfxAudio, defeatClip, 0.5f);
                float cost = activeWindow.GetCost();
                if(cost < lastCost)
                {
                    costs.Add(cost);
                    lastCost = cost;
                    if(count == 0)
                    {
                        //Win the game and show cost

                    }
                }
                else
                {
                    costs.Clear();
                    lastCost = Service.MAX_GAME_TIME;
                    //Add windows to five max with delay
                    int need = 5-count;
                    for(int i=0; i<need; i++)
                    {
                        if(i==0) CreateRandomWindow(0.01f);
                        else CreateRandomWindow(Random.Range(0.2f, 0.6f));
                        count ++;
                    }
                }

                activeWindow.DefeatWindow();
                foreach(var go in windowPool)
                {
                    if(go!=activeWindow)
                        go.EnableHitbox();
                }
                activeWindow = null;

                PlayerManager.Instance.DefeatTarget();
            }
        }
    }
    void OnWindowExplodeHandler(Window window)
    {
        OnDeactivateWindow(window);
        AudioManager.Instance.PlaySoundEffect(sfxAudio, explodeClip, 0.4f);
        cameraManager.ShakeScreen(0.2f, 5);

        foreach(var go in windowPool)
        {
            if(go!=activeWindow)
                go.EnableHitbox();
        }
        activeWindow = null;

        p_explode.transform.position = window.transform.position;
        var shapeModule = p_explode.shape;
        shapeModule.scale = window.transform.localScale;
        p_explode.Play();

        PlayerManager.Instance.ResetPlayer();

        //Add windows to five max with delay
        int need = 5-count;
        for(int i=0; i<need; i++)
        {
            CreateRandomWindow(Random.Range(0.2f, 0.6f));
            count ++;
        }
    }
    float RandomSign()=>Random.value>0.5f?1:-1;
    void CreateRandomWindow(float delay)
    {
        Vector2 scale = new Vector2(Random.Range(10f, 20f), Random.Range(10f, 20f));
        Vector2 pos = new Vector2(RandomSign()*Random.Range(6f, 30f), RandomSign()*Random.Range(4f, 21f));
        var worldType = worldTypes[createIndex];
        createIndex++;

        if(createIndex >= worldTypes.Length)
        {
            createIndex = 0;
            Service.Shuffle(ref worldTypes);
        }
        StartCoroutine(CommonCoroutine.delayAction(()=>{
            var window = windowPool.Find(x=>!x.gameObject.activeSelf);
            window.transform.position = pos;
            window.transform.localScale = scale;
            window.gameObject.SetActive(true);
            window.CompleteReset(GetNewColorFromBase(), 1, worldType);
        }, delay));
    }
    void OnEnterWindowHandler(Window window)
    {
        if(activeWindow==null)
        {
            sfxAudio.Play();
            AudioManager.Instance.PlaySoundEffect(sfxAudio, enterClip, 1f);
            activeWindow = window;
            activeWindow.ActivateWindow();
            PlayerManager.Instance.MatchWindowBoundry(activeWindow);
        }
        foreach(var go in windowPool)
        {
            go.DisableHitbox();
        }
    }
    void OnDeactivateWindow(Window window)
    {
        count --;
        sfxAudio.Stop();
        foreach(var go in windowPool)
        {
            if(go.m_sortIndex>window.m_sortIndex)
            {
                go.DecrementSort();
            }
        }
    }
    Color GetNewColorFromBase()
    {
        float hOffset = RandomSign()*Random.Range(0.05f, 0.1f);
        float sOffset = Random.Range(-0.1f, 0.1f);
        float vOffset = Random.Range(-0.05f, 0.05f);
        
        Color.RGBToHSV(baseColor, out float h, out float s, out float v);

        h += hOffset;
        s += sOffset;
        v += vOffset;
                        
        h = Mathf.Clamp01(h);
        s = Mathf.Clamp01(s);
        v = Mathf.Clamp01(v);

        return Color.HSVToRGB(h, s, v);
    }
}