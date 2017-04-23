using UnityEngine;
using System.Collections;

public class CpuSim : MonoBehaviour {

    public int id;
    public string model;
    public string manufacturer;
    public float mHz;
    public int bits;
    public float volts;

    public CodeSimInst codeSimInst = null;
    public CodeSimInst opCodeSimInst = null;

    private CacheSim cacheSim;

    public bool processing = false;
    public bool dirtyUI = false;

    // Use this for initialization
    void Start () {
	    // init to no cpu
	}

    public void Init( int newID, string newModel = "GenericCPU", float newMhz = 2, int newBits = 8, float newVolts = 5f )
    {
        id = newID;
        model = newModel;
        mHz = newMhz;
        bits = newBits;
        volts = newVolts;
    }

    public void SetCache( CacheSim newCacheSim)
    {
        cacheSim = newCacheSim;
    }

    public CacheSim GetCache()
    {
        return cacheSim;
    }

    // Update is called once per frame
	void Update () {
        if (!processing)
        {
            if (cacheSim.HasItemCached())
            {
                ReadCache();
            }
        }
    }

    void ReadCache()
    {
        if ( cacheSim.ReadCache(out codeSimInst) )
        {
            codeSimInst.ResetTask();
            dirtyUI = true;

            if (opCodeSimInst == null && codeSimInst.codeSim.type == CodeType.codeOp)
            {
                opCodeSimInst = codeSimInst;
                codeSimInst = null;
            }
            else
            {
                StartProcess();
            }
        }
    }

    void StartProcess()
    {
        processing = true;

        if (opCodeSimInst != null)
        {
            //Debug.Log("Processing::" + opCodeSimInst.codeSim.ToString() + " on::" + codeSimInst.codeSim.ToString());
            float time = opCodeSimInst.DoCyclesTask(mHz, EndProcess);
            codeSimInst.DoTimeTask(time, null);
        }
        else
        {
            //Debug.Log("Processing::" + codeSimInst.ToString());
            codeSimInst.DoCyclesTask(mHz, EndProcess);
        }
    }

    void EndProcess( CodeSimInst endCodeSimInst )
    {
        if( opCodeSimInst != null)
        {
            opCodeSimInst.codeSim.Exec(codeSimInst.codeSim); // execute the code sim.
            if(codeSimInst.codeSim.type == CodeType.codeOp)
            {
                codeSimInst.ResetTask();
                opCodeSimInst = codeSimInst;
                codeSimInst = null;
            }
            else
            {
                opCodeSimInst = null;
                codeSimInst = null;
            }
        }
        else
        {
            codeSimInst.codeSim.Exec(); // execute the code sim.
            codeSimInst = null;
        }
        
        processing = false;
        dirtyUI = true;
    }
    
        
}
