using UnityEngine;
using System.Collections;

// Packet Blocker
public class CSCFirewall : CodeSim {

    public int openPorts = 10;
    public float openPortChance = 0.10f;
    public bool open;

    public CSCFirewall()
    {
        model = "GenCoFirewall";
        manufacturer = "GenCo";
        cycles = 20;
        size = 5;
        type = CodeType.codeNoop;
    }

    public override void Exec()
    {
        if (!open) Main.instance.endRun = true;
    }
}
