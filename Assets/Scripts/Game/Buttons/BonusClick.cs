using UnityEngine;

public class BonusClick : MonoBehaviour
{
    Bunch bunch;
    private void Awake()
    {
        bunch = gameObject.GetComponentInParent<Bunch>();
    }
    private void OnMouseUpAsButton()
    {
        bunch.ClickBonus();
    }
}
