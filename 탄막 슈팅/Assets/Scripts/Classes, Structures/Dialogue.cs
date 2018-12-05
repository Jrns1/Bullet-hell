using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Line {

    public string script;
    public string speaker;

    public Line(string _script, string _speaker)
    {
        script = _script;
        speaker = _speaker;
    }

}
