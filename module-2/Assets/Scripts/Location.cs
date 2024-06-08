using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location : MonoBehaviour
{
    public string locationName;
    [TextArea] public string description;
    public Connection[] connections;
    public List<Item> items = new List<Item>();

    public string GetConnections() {
        string result = string.Empty;
        foreach (Connection c in connections)
        {
                result += c.description + '\n';
        }
        return result;
    }

    public Connection GetConnection(string location) {
        foreach (Connection c in connections) {
            if(c.connectionName.ToString().ToLower() == location.ToLower()) return c;
        }
        return null;
    }

    public string GetItems() {
        if(items.Count == 0) return string.Empty;
        string result = "You see:\n";

        foreach (Item i in items) {
            if(i.isEnabled) result += i.description + '\n';
        }
        return result;
    }

    internal bool HasItem(Item item) {
        foreach(Item i in items) {
            if(i == item && i.isEnabled) return true;
        }
        return false;
    }
}
