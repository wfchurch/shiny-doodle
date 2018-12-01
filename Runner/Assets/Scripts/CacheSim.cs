using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CacheState { waiting, caching, done };

public class CacheSim : MonoBehaviour {

    public string model;
    public string manufacturer;
    public float mHz;       // (this in inverted) seconds per cycle
    public int capacity;        // number of bytes the cache can hold
    public int bits;
    public float volts;

    public int size;            // how many bytes are in the cache

    public bool dirtyUI = false;

    public CacheState cacheState = CacheState.done;

    public Queue<CodeSimInst> cacheItems;       // the cache of code sims

    // string override
    public override string ToString()
    {
        return string.Format("CacheSim - size:{1} mHz:{2} NumItems:" + cacheItems.Count.ToString(), capacity, mHz);
    }

    // initializer
    public void Init( string initModel = "GenericCache", int initCapacity = 20, float initMHz = 2f )
    {
        model = initModel;
        capacity = initCapacity;
        mHz = initMHz;

        size = 0;

        cacheItems = new Queue<CodeSimInst>(capacity);
    }

    void Update()
    {
        if (cacheState == CacheState.waiting)
        {
            foreach (CodeSimInst cacheItem in cacheItems)
            {
                if(cacheItem.codeSimState == CodeSimState.waiting)
                {
                    //Debug.Log("StartCachingItem - cacheItem:" + cacheItem.ToString());
                    StartCachingItem(cacheItem);
                    break;
                }
                cacheState = CacheState.done;
            }
        }
    }
    
    private void StartCachingItem(CodeSimInst codeSimInst)
    {
        //Debug.Log("Caching::" + codeSimInst.ToString());

        //set states
        cacheState = CacheState.caching;
        codeSimInst.DoReadTask(mHz, ItemDoneCaching);
    }

    public void ItemDoneCaching( CodeSimInst codeSimInst )
    {
        //Debug.Log(string.Format("Cached - {0}", codeSim.ToString()));

        cacheState = CacheState.waiting;
    }

    // add a code sim to the cache
    public void WriteCache( CodeSimInst codeSimInst )
    {
        if( RoomInCache(codeSimInst) )
        {
            //Debug.Log(string.Format("WriteCache - {0}", codeSim.ToString()));

            size += codeSimInst.codeSim.size;

            // set state
            codeSimInst.codeSimState = CodeSimState.waiting;
            cacheItems.Enqueue(codeSimInst);

            if( cacheState == CacheState.done )
            {
                // set state
                cacheState = CacheState.waiting;
            }

            dirtyUI = true;
        }
        else
        {
            // let user know cache is full.
            Debug.Log("The Cache is full.");
        }
    }

    public bool ReadCache( out CodeSimInst codeSimInst )
    {
        if( HasItemCached() )
        {
            codeSimInst = cacheItems.Dequeue();
            //Debug.Log(string.Format("ReadCache - {0}", codeSim.ToString()));

            // don't need to set state
            size -= codeSimInst.codeSim.size;
            dirtyUI = true;
            return true;
        }
        else
        {
            // don't need to set state

            codeSimInst = null;
            return false;
        }
    }

    public bool RoomInCache( CodeSimInst codeSimInst)
    {
        if( size < capacity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool HasItemCached()
    {
        if(cacheItems.Count > 0)
        {
            if(cacheItems.Peek().codeSimState == CodeSimState.done) return true;
        }
        return false;
    }

}
