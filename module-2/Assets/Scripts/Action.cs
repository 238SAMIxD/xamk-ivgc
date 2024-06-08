using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public string keyword;
    public string[] argsReq, argsOpt;
    public string description;
    public abstract void Respond(GameController controller, string word);
}
