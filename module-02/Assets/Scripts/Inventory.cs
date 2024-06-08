using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Inventory")] public class Inventory : Action
{
    public override void Respond(GameController controller, string word)
    {
        if(controller.player.inventory.Count == 0) {
            controller.currentText.text = "There are no items in the inventory.";
            return;
        }
        string result = "Inventory:\n";
        foreach(Item i in controller.player.inventory) {
            result += i.itemName + '\n';
        }
        controller.currentText.text = result;
    }
}
