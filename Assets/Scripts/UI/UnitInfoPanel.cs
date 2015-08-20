using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnitInfoPanel : MonoBehaviour 
{
    public static UnitInfoPanel Instance;

    public Text UnitName;
    public Slider UnitHp;
    public Slider UnitEnergy;
    public Text UnitType;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowUnitInfo(BaseUnit unit)
    {
        gameObject.SetActive(true);
        UnitName.text = unit.UnitName;
        UnitHp.maxValue = unit.MaxHp;
        UnitHp.value = unit.Hp;
        UnitEnergy.maxValue = unit.MaxEnergy;
        UnitEnergy.value = unit.Energy;
        UnitType.text = unit.Type.ToString();
    }

    public void HideUnitInfo()
    {
        gameObject.SetActive(false);
    }
}
