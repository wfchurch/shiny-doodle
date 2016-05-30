using UnityEngine;
using System.Collections.Generic;

public class RamSimUI : MonoBehaviour {

    private RamSim ramSim;

    private RectTransform ramSimUIRect;
    private List<RectTransform> codeSimUIRectList;

    //private int numRamItems;

    private float border = 5;

    public void SetRamSim(RamSim newRamSim)
    {
        ramSim = newRamSim;
        ramSimUIRect = GetComponent<RectTransform>();
        float bottomPos = -((Main.instance.mainUI.sizeDelta.y + ramSimUIRect.sizeDelta.y) * 0.5f - border); // TODO: this calculation needs to take the anchor of the ProcMeterUI into account.
        //Debug.Log(string.Format("RamUI.bottomPos:{0}", bottomPos));

        codeSimUIRectList = new List<RectTransform>(ramSim.capacity);
        for (int i = 0; i < ramSim.capacity; i++)
        {
            //Debug.Log("Create CodeSimUI");
            RectTransform newCodeSimUIRect = Instantiate(Resources.Load("CodeSimMeter", typeof(RectTransform))) as RectTransform;
            codeSimUIRectList.Add(newCodeSimUIRect);
            newCodeSimUIRect.SetParent(ramSimUIRect, false);
            //Debug.Log(string.Format("CodeSimUI.localPos:{0}", newCodeSimUIRect.localPosition));
            newCodeSimUIRect.localPosition = new Vector3(newCodeSimUIRect.localPosition.x, bottomPos + ((i+1) * newCodeSimUIRect.sizeDelta.y));
            //Debug.Log(string.Format("CodeSimUI.localPos:{0}", newCodeSimUIRect.localPosition));
            newCodeSimUIRect.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if( ramSim != null)
        {
            if (ramSim.dirtyUI)
            {
                int i = 0;

                foreach (CodeSimInst codeSimInst in ramSim.ramItems)
                {
                    codeSimUIRectList[i].gameObject.SetActive(true);
                    codeSimUIRectList[i].GetComponent<CodeSimUI>().Init(codeSimInst);
                    //Debug.Log(string.Format("ramSimUI showing:{0}", i));
                    //numRamItems = ++i;
                    i++;
                }

                for (int k = i; k < codeSimUIRectList.Count; k++)
                {
                    //Debug.Log(string.Format("ramSimUI hiding:{0}", k));
                    codeSimUIRectList[k].gameObject.SetActive(false);
                }

                ramSim.dirtyUI = false;
            }
        }
    }

}
