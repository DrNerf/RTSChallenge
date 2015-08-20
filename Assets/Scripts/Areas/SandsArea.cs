using UnityEngine;
using System.Collections;

public class SandsArea : BaseArea 
{
    public GameObject TextPlaceholder;

    void OnUnitEnter(BaseUnit unit)
    {
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        TextPlaceholder.SetActive(true);
        yield return new WaitForSeconds(9);
        TextPlaceholder.SetActive(false);
    }
}
