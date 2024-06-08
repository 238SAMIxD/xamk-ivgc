using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    [TextArea] public string description;
    public bool canBeTaken;
    public bool isEnabled = true;
    public bool playerCanTalkTo = false;
    public bool playerCanGiveTo = false;
    public bool playerCanRead = false;
    public Interaction[] interactions;
    public Item targetItem = null;

    public bool InteractWith(GameController controller, string keyword, string word = "") {
        foreach (Interaction i in interactions) {
            if(i.action.keyword.ToLower() == keyword.ToLower()) {
                if (word != "" && word.ToLower() != i.textToMatch.ToLower()) continue;
                foreach(Item disableItem in i.itemsToDisable) {
                    disableItem.isEnabled = false;
                }
                foreach (Item enableItem in i.itemsToEnable) {
                    enableItem.isEnabled = true;
                }
                foreach (Connection disableConnection in i.connectionsToDisable) {
                    disableConnection.isActive = false;
                }
                foreach (Connection enableConnection in i.connectionsToEnable) {
                    enableConnection.isActive = true;
                }
                if(i.teleportTo != null) {
                    controller.player.Teleport(controller, i.teleportTo);
                }
                controller.currentText.text = i.response;
                controller.DisplayLocation(true);
                return true;
            }
        }
        return false;
    }
}
