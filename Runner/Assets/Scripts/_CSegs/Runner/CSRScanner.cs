using UnityEngine;
using System;
using System.Collections;

// Port scanner attack
public class CSRScanner : CodeSim
{
    int randSeed = 546;

    public CSRScanner()
    {
        model = "GenCoScanner";
        manufacturer = "GenCo";
        cycles = 2;
        size = 2;
        type = CodeType.codeOp;
    }

    public override void Exec(CodeSim opCode)
    {
        if (opCode.GetType() == Type.GetType("CSCFirewall"))
        {
            string msg = "Fail!";
            float randValue;
            CSCFirewall fw = (CSCFirewall)opCode;
            for (int i = 0; i < fw.openPorts; i++)
            {
                randSeed++;
                UnityEngine.Random.seed = randSeed;
                randValue = UnityEngine.Random.value;
                //Debug.Log(randValue.ToString() + " < " + fw.openPortChance.ToString());
                if (randValue < fw.openPortChance)
                {
                    //Debug.Log(UnityEngine.Random.value.ToString() + " < " + fw.openPortChance.ToString());
                    fw.open = true;
                    msg = "Success!";
                    break;
                }
            }
            UsrMsg(msg);
        }
        else
        {
            InvalidOperatorMsg( opCode );
        }

        
    }

}
