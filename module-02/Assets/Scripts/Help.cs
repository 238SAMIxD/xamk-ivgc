using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/Help")] public class Help : Action
{
    public Action[] commands;
    public override void Respond(GameController controller, string word)
    {
        controller.logText.text += "Type a command\nAvailable commands:\n";
        foreach (Action c in commands)
        {
            controller.logText.text += c.keyword + "\t";
            foreach (string a in c.argsReq)
            {
                controller.logText.text += "<" + a + ">\t";
            }
            foreach (string a in c.argsOpt)
            {
                controller.logText.text += "[" + a + "]\t";
            }
            controller.logText.text += " " + c.description + '\n';
        }
    }
}
