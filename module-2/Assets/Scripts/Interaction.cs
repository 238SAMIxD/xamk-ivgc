using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable] public class Interaction
{
    public Action action;
    [TextArea] public string response;
    public string textToMatch;
    public List<Item> itemsToDisable = new List<Item>();
    public List<Item> itemsToEnable = new List<Item>();
    public List<Connection> connectionsToDisable = new List<Connection>();
    public List<Connection> connectionsToEnable = new List<Connection>();
    public Location teleportTo = null;
}
