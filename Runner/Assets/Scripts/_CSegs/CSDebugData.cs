using UnityEngine;
using System.Collections;

public class CSDebugData : CodeSim {

	public CSDebugData () {
        model = "FooData";
        manufacturer = "Bar";
        cycles = 0;
        size = 8;
        type = CodeType.data;
    }
}