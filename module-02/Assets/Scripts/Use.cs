using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Use")] public class Use : Action
{
    public override void Respond(GameController controller, string word)
    {
        if (UseItems(controller, controller.player.currentLocation.items, word)) {
            return;
        }
        if (UseItems(controller, controller.player.inventory, word)) {
            return;
        }
        controller.currentText.text = "The " + word + " does nothing\n";
    }

    private bool UseItems(GameController controller, List<Item> items, string word)
    {
        foreach (Item i in items) {
            if (i.itemName.ToLower() == word.ToLower()) {
                if (controller.player.CanUseItem(controller, i)) {
                    if (i.InteractWith(controller, "use")) {
                        return true;
                    }
                    controller.currentText.text = "Item " + i.itemName + " does nothing\n";
                    return true;
                }
            }
        }
        return false;
    }
}
