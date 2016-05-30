using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class RamSim : MonoBehaviour {

	public string model;
	public string manufacturer;
	public float mHz;       		// (this in inverted) seconds per cycle
	public int capacity;        	// number of bytes ram can hold
	public int bits;
	public float volts;

	public int size;            	// how many bytes are in ram

	public List<CodeSimInst> ramItems;

    public bool dirtyUI = false;

    // initializer
    public void Init( string initModel = "GenericCache", string initManufacturer = "GenericCo", int initCapacity = 20, float initMHz = 2f )
	{
		model = initModel;
		manufacturer = initManufacturer;
		capacity = initCapacity;
		mHz = initMHz;

		size = 0;

		ramItems = new List<CodeSimInst>(capacity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Load( CodeSim codeSim )
	{
        CodeSimInst codeSimInst = gameObject.AddComponent<CodeSimInst>(); 
        codeSimInst.codeSim = codeSim;
        ramItems.Add (codeSimInst);
        ramItems.Sort();
		size += codeSimInst.codeSim.size;

        dirtyUI = true;
    }

	public CodeSimInst Read( string srchModel )
	{
        foreach( CodeSimInst codeSimInst  in ramItems )
        {
            if( codeSimInst.codeSim.model == srchModel )
            {
                CodeSimInst newInst = codeSimInst.codeSim.GetCopy();
                return newInst;
            }
        }
        
        return null;
	}

    public void Remove( string srchModel)
    {
        foreach (CodeSimInst codeSimInst in ramItems)
        {
            if (codeSimInst.codeSim.model == srchModel)
            {
                ramItems.Remove(codeSimInst);
                dirtyUI = true;
                break;
            }
        }
    }

}
