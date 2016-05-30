using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CacheSimUI : MonoBehaviour {

    private CacheSim cacheSim;

    private RectTransform cacheSimUIRect;
    private List<RectTransform> codeSimUIRectList;

    private int numCacheItems;

    private float border = 5;
    private float startPos;
    private float bottomPos;
    private float dropSpeed = 400f;

    // Use this for initialization
    //void Start()
    //{
    //    cacheSimUIRect = GetComponent<RectTransform>();
    //    bottomPos = -(cacheSimUIRect.sizeDelta.y * 0.5f - border); // TODO: this calculation needs to take the anchor of the ProcMeterUI into account.
    //}

    public void SetCacheSim( CacheSim newCacheSim )
    {
        cacheSim = newCacheSim;
        cacheSimUIRect = GetComponent<RectTransform>();
        bottomPos = -((Main.instance.mainUI.sizeDelta.y + cacheSimUIRect.sizeDelta.y) * 0.5f - border); // TODO: this calculation needs to take the anchor of the ProcMeterUI into account.
        startPos = ((Main.instance.mainUI.sizeDelta.y + cacheSimUIRect.sizeDelta.y) * 0.5f - border);

        codeSimUIRectList = new List<RectTransform>(cacheSim.capacity);
        for ( int i = 0; i < cacheSim.capacity; i++)
        {
            //Debug.Log("Create CodeSimUI");
            RectTransform newCodeSimUIRect = Instantiate(Resources.Load("CodeSimMeter", typeof(RectTransform))) as RectTransform;
            codeSimUIRectList.Add( newCodeSimUIRect );
            newCodeSimUIRect.SetParent(cacheSimUIRect, false);
            newCodeSimUIRect.localPosition = new Vector3(newCodeSimUIRect.localPosition.x, startPos);
            newCodeSimUIRect.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(cacheSim != null)
        {
            if (cacheSim.dirtyUI)
            {
                UpdateCodeSimUIList();
                cacheSim.dirtyUI = false; // set dirty flag to false (everything should be in order until something else changes)
            }
        }
	}

    private void UpdateCodeSimUIList()
    {
        int i = 0;

        foreach ( CodeSimInst codeSimInst in cacheSim.cacheItems )
        {
            codeSimUIRectList[i].gameObject.SetActive(true);
            codeSimUIRectList[i].GetComponent<CodeSimUI>().Init(codeSimInst);
            //Debug.Log(string.Format("showing:{0}@:{1}", i, codeSimUIRectList[i].localPosition.y));
            numCacheItems = ++i;
        }

        if (i > 0) Animate();

        for( int k = i; k < codeSimUIRectList.Count; k++ )
        {
            ResetCodeSimUIRect(codeSimUIRectList[k]);
            //Debug.Log(string.Format("hiding:{0}@:{1}", k, codeSimUIRectList[i].localPosition.y));
            codeSimUIRectList[k].gameObject.SetActive(false);
        }
    }

    private void ResetCodeSimUIRect(RectTransform codeSimUIRect)
    {
        codeSimUIRect.localPosition = new Vector3(codeSimUIRect.localPosition.x, startPos);
        //Debug.Log("ResetCodeSimUIRect - " + codeSimUIRect.localPosition.ToString());
    }

    private void Animate()
    {
        Vector3 endPos;

        for( int i = 0; i < numCacheItems; i++ )
        {
            endPos = CalculateEndPosition(codeSimUIRectList[i], i);
            //Debug.Log(codeSimUIRectList[i].localPosition.ToString() + ":" + endPos.ToString());
            StartCoroutine(DropCodeSimUIRect(codeSimUIRectList[i], endPos));
        }
    }

    private Vector3 CalculateEndPosition( RectTransform codeSimUIRect, int queueSlot )
    {
        return new Vector3(codeSimUIRect.localPosition.x, bottomPos + (queueSlot + 1) * codeSimUIRect.sizeDelta.y);
    }

    private IEnumerator DropCodeSimUIRect( RectTransform codeSimUIRect, Vector3 endPos )
    {
        while (codeSimUIRect.localPosition != endPos)
        {
            codeSimUIRect.localPosition = Vector3.MoveTowards(codeSimUIRect.localPosition, endPos, dropSpeed * Time.deltaTime);
            //Debug.Log(string.Format("DropCacheItem{0} - startPos:{1} endPos:{2}",codeSimUIRect.GetComponent<CodeSimUI>().GetCodeSim().id, codeSimUIRect.localPosition.y, endPos.y));
            yield return null;
        }
    }
    
} 
