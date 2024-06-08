using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType {
    Fire,
    Ice,
    Both,
    Destruct
}

[CreateAssetMenu(menuName = "CardGame/Card", fileName = "Card")] public class CardData : ScriptableObject
{
    public string cardTitle;
    public string cardDescription;
    public int cost;
    public int damage;
    public DamageType damageType;
    public Sprite cardImage;
    public Sprite frameImage;
    public int count;
    public bool isDefensive = false;
    public bool isMirror = false;
    public bool isMultiple = false;
}
