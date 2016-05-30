using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;

public enum Who { player, corp, both };

public class Main : MonoBehaviour {

    public static Main instance;

    public bool runActive;

    private CpuSim playerCpu;
    private RamSim playerRam;

    private CpuSim corpCpu;
    private RamSim corpRam;

    public RectTransform mainUI;
    public Text runEnd;

	//public CpuSimUI playerCpuSimUI;
    //public RamSimUI playerRamSimUI;
	//public CacheSimUI playerCacheSimUI;

    //public CpuSimUI corpCpuSimUI;
    //public RamSimUI corpRamSimUI;
    //public CacheSimUI corpCacheSimUI;

    private CodeSimInst opCodeSimInst;

    private int codeID = 0;
    
    void Awake()
    {
        instance = this;
    }
    
    // Use this for initialization
	void Start () {

        //Debug.Log(System.Environment.Version);

        mainUI = GameObject.Find("RunCanvas").GetComponent<RectTransform>();
        runEnd = GameObject.Find("RunEndedText").GetComponent<Text>();
        runEnd.gameObject.SetActive(false);

        // Runner Sims
        CacheSim playerCache = gameObject.AddComponent<CacheSim>();
        playerCache.Init("PlayerCache", 20, 1.25f);

        playerRam = gameObject.AddComponent<RamSim>();
        playerRam.Init("PlayerRam", "GenCo", 20, 1f);

        playerCpu = gameObject.AddComponent<CpuSim>();
        playerCpu.Init( 0, "PlayerCPU", 2f );
        playerCpu.SetCache(playerCache);

        // Runner UIs
        CacheSimUI playerCacheUI = mainUI.FindChild("PlayerCachePanel").GetComponent<CacheSimUI>();
        playerCacheUI.SetCacheSim(playerCache);
        RamSimUI playerRamUI = mainUI.FindChild("PlayerRamPanel").GetComponent<RamSimUI>();
        playerRamUI.SetRamSim(playerRam);
        CpuSimUI playerCpuUI = mainUI.FindChild("PlayerCpuPanel").GetComponent<CpuSimUI>();
        playerCpuUI.Init(playerCpu);

        LoadRam(playerRam, Who.player);


        // Corp Sims
        CacheSim corpCache = gameObject.AddComponent<CacheSim>();
        corpCache.Init("CorpCache", 20, 1.25f);

        corpRam = gameObject.AddComponent<RamSim>();
        corpRam.Init("CorpRam", "GenCo", 20, 1f);

        corpCpu = gameObject.AddComponent<CpuSim>();
        corpCpu.Init(0, "CorpCPU", 2f);
        corpCpu.SetCache(corpCache);

        // Corp UIs
        CacheSimUI corpCacheUI = mainUI.FindChild("CorpCachePanel").GetComponent<CacheSimUI>();
        corpCacheUI.SetCacheSim(corpCache);
        RamSimUI corpRamUI = mainUI.FindChild("CorpRamPanel").GetComponent<RamSimUI>();
        corpRamUI.SetRamSim(corpRam);
        CpuSimUI corpCpuUI = mainUI.FindChild("CorpCpuPanel").GetComponent<CpuSimUI>();
        corpCpuUI.Init(corpCpu);

    }

    void Update()
    {
        if(!runActive)
        {
            // End the run
            runEnd.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    //private CacheSimUI CreateCacheUI(CacheSim cacheSim)
    //{
    //    RectTransform cacheSimPanel = Instantiate(Resources.Load("CacheSimPanel", typeof(RectTransform))) as RectTransform;
    //    cacheSimPanel.SetParent(Main.instance.mainUI, false);
    //    CacheSimUI cacheSimUI = cacheSimPanel.gameObject.GetComponent<CacheSimUI>();
    //    //cacheSimPanel.localPosition = new Vector3(pos, cacheSimPanel.localPosition.y);
    //    cacheSimUI.SetCacheSim(cacheSim);

    //    return cacheSimUI;
    //}

    //private CpuSimUI CreateCpuUI(CpuSim cpuSim)
    //{
    //    RectTransform cpuSimUIRect = Instantiate(Resources.Load("CpuSimPanel", typeof(RectTransform))) as RectTransform;
    //    cpuSimUIRect.SetParent(Main.instance.mainUI, false);
    //    CpuSimUI cpuSimUI = cpuSimUIRect.gameObject.GetComponent<CpuSimUI>();
    //    //cpuSimUIRect.localPosition = new Vector3(pos, cpuSimUIRect.localPosition.y);
    //    cpuSimUI.Init(cpuSim);

    //    return cpuSimUI;
    //}

    void LoadRam(RamSim ramSim, Who who)
    {
        if(who == Who.player)
        {
            ramSim.Load(gameObject.AddComponent<CSDebugData>());
            ramSim.Load(gameObject.AddComponent<CSDebug>());
        }
        else if(who == Who.corp)
        {

        }
    }

    public void DoPointerClick( CodeSimInst codeSimInst )
    {
        CodeSimInst newCodeSimInst = codeSimInst.codeSim.GetCopy();
        newCodeSimInst.id = codeID++;

        if(opCodeSimInst != null)
        {
            Debug.Log("Operand choosen::" + newCodeSimInst.codeSim.ToString());
            if (newCodeSimInst.codeSim.type == CodeType.codeOp)
            {
                Debug.Log("Choose an operand for::" + newCodeSimInst.codeSim.ToString());
                opCodeSimInst.opCodeInst = newCodeSimInst;
                opCodeSimInst = newCodeSimInst;
            }
            else
            {
                opCodeSimInst.opCodeInst = newCodeSimInst;
                opCodeSimInst = null;
            }
        }
        else if( newCodeSimInst.codeSim.type == CodeType.codeOp )
        {
            Debug.Log("Choose an operand for::" + newCodeSimInst.codeSim.ToString());
            opCodeSimInst = newCodeSimInst;
        }
            
        playerCpu.GetCache().WriteCache(newCodeSimInst);
                
    }
   
}
