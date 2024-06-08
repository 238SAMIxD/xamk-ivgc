using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Get")] public class Get : Action
{
    public override void Respond(GameController controller, string word)
    {
        foreach(Item i in controller.player.currentLocation.items)
        {
            if(i.isEnabled && i.itemName.ToLower() == word.ToLower()) {
                if(i.canBeTaken) {
                    controller.player.inventory.Add(i);
                    controller.player.currentLocation.items.Remove(i);
                    controller.currentText.text = "You took the " + i.itemName;
                }
                return;
            }
        }
        controller.currentText.text = "You can't get this";
    }
}
