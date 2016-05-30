using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CodeSimUI : MonoBehaviour {

    private CodeSimInst codeSimInst;
	private bool sleep = false;

    public Scrollbar progBar;

    void Start()
    {
        progBar = GetComponent<Scrollbar>();
    }

    void Update()
    {
        if( codeSimInst != null)
        {
            if (codeSimInst.codeSimState == CodeSimState.busy || !sleep)
            {
                switch (codeSimInst.codeSimState)
                {
                    case CodeSimState.busy:
                        progBar.size = Mathf.Lerp(0f, 1f, codeSimInst.GetProgress());
                        sleep = false;
                        break;
                    case CodeSimState.waiting:
                        progBar.size = 0f;
                        sleep = true;
                        break;
                    case CodeSimState.done:
                        progBar.size = 1f;
                        sleep = true;
                        break;
                }
            }
        }
    }

	public void Init( CodeSimInst newCodeSimInst )
	{
        sleep = false;
        codeSimInst = newCodeSimInst;
        GetComponentInChildren<Text>().text = codeSimInst.codeSim.model;
	}

    public CodeSim GetCodeSim()
    {
        return codeSimInst.codeSim;
    }

    public void RemoveRef()
    {
        sleep = false;
        progBar.size = 0f;
        codeSimInst = null;
    }

    public void DoPointerClick()
    {
        Main.instance.DoPointerClick(codeSimInst);
    }

}
