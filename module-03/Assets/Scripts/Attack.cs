using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    public Player target = null;
    public Card source = null;
    public Image attackImage = null;

    public AudioSource iceAudio = null;
    public AudioSource fireAudio = null;
    public AudioSource destroyAudio = null;

    public void EndTrigger() {
        if(target.HasMirror()) {
            target.SetMirror(false);
            target.smashAudio.Play();
            if (target.isPlayer) {
                GameController.instance.CastAttack(source, GameController.instance.enemy);
                return;
            }
            GameController.instance.CastAttack(source, GameController.instance.player);
            Destroy(gameObject);
            return;
        }
        int damage = source.cardData.damage;
        if (!target.isPlayer) {
            if((source.cardData.damageType == DamageType.Fire && target.isFire) || (source.cardData.damageType == DamageType.Ice && !target.isFire)) {
                damage /= 2;
            }
        }
        target.health -= damage;
        target.HitAnimation();
        GameController.instance.UpdateHealth();
        if(target.health <= 0) {
            target.health = 0;
            if(target.isPlayer) {
                target.dieAudio.Play();
            } else {
                GameController.instance.monsterAudio.Play();
            }
        }
        GameController.instance.isPlayable = true;
        Destroy(gameObject);
        GameController.instance.NextPlayersTurn();
    }
}
