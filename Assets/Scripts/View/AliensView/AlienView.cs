using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="AlienView", menuName = "ScriptableObjects/AlienView")]
public class AlienView : ScriptableObject
{
    public int id;
    public int currentCosmetic;
    public Sprite sprite;

}
