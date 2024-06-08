using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Examine")] public class Examine : Action
{
    public override void Respond(GameController controller, string word) {
        if(CheckItems(controller, controller.player.currentLocation.items, word)) {
            return;
        }
        if(CheckItems(controller, controller.player.inventory, word)) {
            return;
        }
        controller.currentText.text = "You can't see " + word + '\n';
    }

    private bool CheckItems(GameController controller, List<Item> items, string word) {
        foreach(Item i in items) {
            if(i.itemName.ToLower() == word.ToLower()) {
                if(i.InteractWith(controller, "examine")) {
                    return true;
                }
                controller.currentText.text = i.itemName + '\n' + i.description;
                return true;
            }
        }
        return false;
    }
}
