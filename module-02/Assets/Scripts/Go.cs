using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Go")]public class Go : Action
{
    public override void Respond(GameController controller, string word)
    {
        if(controller.player.CanChangeLocation(controller, word))
        {
            controller.DisplayLocation();
        } else
        {
            controller.currentText.text = "The connection is blocked or doesn't exist.\n" + controller.player.currentLocation.GetConnections();
        }
    }
}
