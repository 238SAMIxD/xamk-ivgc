using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Say")] public class Say : Action
{
    public override void Respond(GameController controller, string word) {
        if (CanSay(controller, controller.player.currentLocation.items, word)) {
            return;
        }
        controller.currentText.text = "Nothing responds.";
    }

    private bool CanSay(GameController controller, List<Item> items, string word) {
        foreach(Item i in items) {
            if(i.isEnabled && controller.player.CanTalkTo(controller, i)) {
                if(i.InteractWith(controller, "say", word)) {
                    return true;
                }
            }
        }
        return false;
    }
}
