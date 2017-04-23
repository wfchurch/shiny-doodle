using UnityEngine;
using System;
using System.Collections;

public enum CodeType { codeOp, codeNoop, data }

public class CodeSim : MonoBehaviour {

    //public int id = 0;
    public string model = "";
    public string manufacturer = "";
    public int cycles = 0;
    public int size = 0;
    public int bit = 128;
    public CodeType type = CodeType.data;

    public override string ToString()
    {
        return string.Format("T:{0} Model:" + model + " Manufacturer:" + manufacturer + " Cycles:{1}", type, cycles);
    }

    public CodeSimInst GetCopy()
    {
        CodeSimInst newInst = gameObject.AddComponent<CodeSimInst>();
        newInst.codeSim = this;
        return newInst;
    }

    public virtual void Exec(CodeSim opCode)
    {
        if (type == CodeType.codeOp)
        {
            if (opCode != null)
            {
                Debug.Log("CODE/OP - " + this.ToString() + "/OP - " + opCode.ToString());
            }
            else
            {
                Debug.Log("CODE/NOOP - " + this.ToString());
            }
        }
        else
        {
            // Error to run data as code
            Debug.Log("DATA - " + this.ToString());
        }
    }

    public virtual void Exec()
    {

    }

    public void InvalidOperatorMsg(CodeSim opCode)
    {
        Debug.Log("Invalid Operator:" + opCode.ToString());
    }

    public void UsrMsg(string msg)
    {
        Debug.Log(this.ToString() + " - " + msg);
    }

}
