using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DamageHandler : MonoBehaviour
{
    private manager manager; 
    public Entity player;

    /*public void AttackTarget(Technique technique, Entity target, Entity caster);
    {
        int baseDamage = 0;
        if technique.variableDamage == true
        {
            baseDamage = UnityEngine.Random.Range(technique.damage, technique.lowerDamage);
        }
        else
        {
            baseDamage = technique.damage;
        }
        caster.Mana -= technique.manaCost; //implement treasure system relating to mana cost later
        int armourReduction = target.Armour - caster.ArmourPen;
        if armourReduction < 0
        {
            armourReduction = 0;
        }
        baseDamage -= armourReduction;
        TakeDamage(baseDamage, target, caster);
        
    }*/

   // this function assumes that the function that calls it checks mana.
    public void AttackTarget(Technique technique, Entity target, Entity caster)
    {
        int baseDamage;
        if (technique.variableDamage == true)
            baseDamage = UnityEngine.Random.Range(technique.damage, technique.lowerDamage);
        else
            baseDamage = technique.damage;
        if (caster.playableCharacter == false)
            caster.Mana -= technique.manaCost - 1;
        else
            caster.Mana -= technique.manaCost; //implement treasure system relating to mana cost later
        int armourReduction = target.Armour - caster.ArmourPen;
        if (armourReduction < 0)
            armourReduction = 0;
        baseDamage -= armourReduction;
        TakeDamage(baseDamage, target, caster, technique);

    }

    public void TakeDamage(int amount, Entity target, Entity caster, Technique technique)
    {
        int damage = manager.ProcessDamageTreasures(amount, target, caster);
        double damagedouble = Convert.ToDouble(damage);
        damagedouble *= AffinityTest(target, technique.damageType);
        target.Health -= Convert.ToInt32(damagedouble);
        manager.DamageTaken.Invoke(damage, target); //this does nothing, and will be replaced with a <int, Target> event
    }


    public double AffinityTest(Entity target, DamageType type)
    {   
        double mod = 1;
        if (type == DamageType.SLASHING)
        {
        mod = target.slashing;
        }
        if (type == DamageType.PIERCING)
        {
        mod = target.piercing;
        }
        if (type == DamageType.BLUDGEONING)
        {
        mod = target.bludgeoning;
        }
        if (type == DamageType.FIRE)
        {
        mod = target.fire;
        }
        if (type == DamageType.COLD)
        {
        mod = target.cold;
        }
        if (type == DamageType.FORCE)
        {
        mod = target.force;
        }
        if (type == DamageType.HOLY)
        {
        mod = target.holy;
        }
        if (type == DamageType.EVIL)
        {
        mod = target.evil;
        }
        if (type == DamageType.ALMIGHTY)
        {
        mod = 1;
        }
        return mod;
    }


    

 
    
}
