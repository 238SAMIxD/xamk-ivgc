using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Location currentLocation;
    public List<Item> inventory = new List<Item>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanChangeLocation(GameController controller, string location) {
        Connection c = currentLocation.GetConnection(location);
        if (c != null) {
            if(c.isActive) {
                currentLocation = c.location;
                return true;
            }
        }
        return false;
    }

    public void Teleport(GameController controller, Location destination) {
        currentLocation = destination;
    }

    internal bool CanUseItem(GameController controller, Item item) {
        if(item.targetItem == null) {
            return true;
        }
        if(HasItem(item.targetItem)) {
            return true;
        }
        if(currentLocation.HasItem(item.targetItem)) {
            return true;
        }
        return false;
    }

    private bool HasItem(Item item)
    {
        foreach (Item i in inventory) {
            if (i == item && i.isEnabled) return true;
        }
        return false;
    }
    public bool HasItemByName(string word) {
        foreach (Item i in inventory) {
            if (i.itemName.ToLower() == word.ToLower() && i.isEnabled) return true;
        }
        return false;
    }

    internal bool CanTalkTo(GameController controller, Item item) {
        return item.playerCanTalkTo;
    }

    internal bool CanGiveTo(GameController controller, Item item) {
        return item.playerCanGiveTo;
    }

    internal bool CanRead(GameController controller, Item item) {
        return item.playerCanRead;
    }
}
