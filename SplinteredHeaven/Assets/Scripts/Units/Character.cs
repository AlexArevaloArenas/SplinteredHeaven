using UnityEngine;
using UnityEngine.UI;
public class Character : Unit
{
    protected WeaponData[] _weapons;

    public Character(CharacterData characterData, Image hpBar) : base(characterData, hpBar)
    {
        //_health = characterData.health;
        //_maxHealth = characterData.health;
        //_modules = characterData.modules;
    }

}
