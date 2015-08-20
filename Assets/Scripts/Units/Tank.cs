using UnityEngine;
using System.Collections;

public class Tank : BaseUnit 
{
    public Transform Turret;
    public ParticleSystem AttackSystem;

    void OnAttack(BaseUnit unit)
    {
        base.NavMeshAgent.Stop();
        Debug.Log(base.UnitName + " -> " + unit.UnitName);
        Turret.LookAt(unit.transform);
        AttackSystem.Play();
    }
}
