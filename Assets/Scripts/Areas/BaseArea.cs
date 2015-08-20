using UnityEngine;
using System.Collections;

public class BaseArea : MonoBehaviour 
{
    public bool OnlyFirstTime;
    public AreaType Type;

    private bool DidTrigger = false;

    void OnTriggerEnter(Collider other)
    {
        if (OnlyFirstTime && DidTrigger)
        {
            return;
        }

        UnitType unitType = 0;
        BaseUnit unit = other.GetComponent<BaseUnit>();
        switch (Type)
        {
            case AreaType.ForFriendly:
                unitType = UnitType.Friendly;
                break;
            case AreaType.ForHostile:
                unitType = UnitType.Hostile;
                break;
            case AreaType.ForBoth:
                if (unit != null)
                {
                    TriggerArea(unit);
                    return;
                }
                break;
            default:
                break;
        }

        if (unit != null && unit.Type == unitType)
        {
            TriggerArea(unit);
        } 
    }

    void TriggerArea(BaseUnit unit)
    {
        SendMessage("OnUnitEnter", unit);
        DidTrigger = true;
    }

    void OnUnitEnter(BaseUnit unit)
    {
        Debug.Log(unit.UnitName + " entered area!");
    }
}

public enum AreaType
{
    ForFriendly = 0,
    ForHostile = 1,
    ForBoth = 2
}
