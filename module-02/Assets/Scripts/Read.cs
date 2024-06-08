using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Read")] public class Read : Action
{
    public override void Respond(GameController controller, string word) {
        if (CanRead(controller, controller.player.currentLocation.items, word)) {
            return;
        }
        if (CanRead(controller, controller.player.inventory, word)) {
            return;
        }
        controller.currentText.text = "The " + word + " cannot be read.";
    }

    private bool CanRead(GameController controller, List<Item> items, string word) {
        foreach (Item i in items) {
            if (i.isEnabled && controller.player.CanRead(controller, i)) {
                if (i.InteractWith(controller, "read")) {
                    return true;
                }
            }
        }
        return false;
    }
}
