using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem instance;

    [SerializeField]
    private Tooltip tooltip;
    private void Awake()
    {
        instance = this;
    }

    public void Show(string content, string header = "", int durability = 0)
    {
        tooltip.SetText(content, header, durability);
        tooltip.gameObject.SetActive(true);
    }

    public void Hide()
    {
        tooltip.gameObject.SetActive(false);
    }


}
