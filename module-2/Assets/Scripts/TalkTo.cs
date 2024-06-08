using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/TalkTo")] public class TalkTo : Action
{
    public override void Respond(GameController controller, string word) {
       if(CanTalkTo(controller, controller.player.currentLocation.items, word)) {
            return;
        }
       controller.currentText.text = "The " + word + " does not respond.";
    }

    private bool CanTalkTo(GameController controller, List<Item> items, string word) {
        foreach (Item i in items) {
            if (i.isEnabled && i.itemName.ToLower() == word.ToLower()) {
                if (controller.player.CanTalkTo(controller, i)) {
                    if (i.InteractWith(controller, "talkto")) {
                        return true;
                    }
                    controller.currentText.text = "The " + i.itemName + " does not respond.\n";
                    return true;
                }
            }
        }
        return false;
    }
}
