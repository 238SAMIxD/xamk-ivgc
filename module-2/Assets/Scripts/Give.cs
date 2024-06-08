using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Give")] public class Give : Action
{
    public override void Respond(GameController controller, string word) {
        if (controller.player.HasItemByName(word)) {
            if (CanGive(controller, controller.player.currentLocation.items, word)) {
                return;
            }
            controller.currentText.text = "Nothing takes the " + word;
            return;
        }
        controller.currentText.text = "You don't have " + word;
    }

    private bool CanGive(GameController controller, List<Item> items, string word) {
        foreach (Item i in items) {
            if (i.isEnabled && controller.player.CanGiveTo(controller, i)) {
                if (i.InteractWith(controller, "give", word)) {
                    return true;
                }
            }
        }
        return false;
    }
}
