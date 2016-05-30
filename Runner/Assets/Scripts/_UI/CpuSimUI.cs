using UnityEngine;
using System.Collections;

public class CpuSimUI : MonoBehaviour {

    private CpuSim cpuSim;
    private RectTransform cpuSimUIRect;
    private RectTransform topCodeSimUIRect;
    private CodeSimUI topCodeSimUI;
    private RectTransform botCodeSimUIRect;
    private CodeSimUI botCodeSimUI;

    private float border = 5;
    
    // Use this for initialization
    void Start () {

	}

    public void Init( CpuSim initCpuSim )
    {
        cpuSim = initCpuSim;
        
        cpuSimUIRect = GetComponent<RectTransform>();

        topCodeSimUIRect = Instantiate(Resources.Load("CodeSimMeter", typeof(RectTransform))) as RectTransform;
        topCodeSimUIRect.SetParent(cpuSimUIRect, false);
        Vector3 topPos = new Vector3(topCodeSimUIRect.localPosition.x, -(cpuSimUIRect.sizeDelta.y * 0.5f - border - topCodeSimUIRect.sizeDelta.y * 2));
        topCodeSimUIRect.localPosition = topPos;
        topCodeSimUI = topCodeSimUIRect.GetComponent<CodeSimUI>();
        topCodeSimUIRect.gameObject.SetActive(false);

        botCodeSimUIRect = Instantiate(Resources.Load("CodeSimMeter", typeof(RectTransform))) as RectTransform;
        botCodeSimUIRect.SetParent(cpuSimUIRect, false);
        Vector3 botPos = new Vector3(botCodeSimUIRect.localPosition.x, -(cpuSimUIRect.sizeDelta.y * 0.5f - border - botCodeSimUIRect.sizeDelta.y));
        botCodeSimUIRect.localPosition = botPos;
        botCodeSimUI = botCodeSimUIRect.GetComponent<CodeSimUI>();
        botCodeSimUIRect.gameObject.SetActive(false);
    }    
	
	// Update is called once per frame
	void Update () {
        if (cpuSim.dirtyUI)
        {
            if(cpuSim.codeSimInst != null)
            {
                topCodeSimUI.Init(cpuSim.codeSimInst);
                topCodeSimUIRect.gameObject.SetActive(true);
            }
            else
            {
                topCodeSimUIRect.gameObject.SetActive(false);
                topCodeSimUI.RemoveRef();
            }

            if(cpuSim.opCodeSimInst != null)
            {
                botCodeSimUI.Init(cpuSim.opCodeSimInst);
                botCodeSimUIRect.gameObject.SetActive(true);
            }
            else
            {
                botCodeSimUIRect.gameObject.SetActive(false);
                botCodeSimUI.RemoveRef();
            }
        }
        cpuSim.dirtyUI = false;
	}

}
