using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private Text headerField;

    [SerializeField]
    private Text contentField;

    [SerializeField]
    private Text durabilityField;

    [SerializeField]
    private LayoutElement layoutElement;

    [SerializeField]
    private int maxCharacter;

    [SerializeField]
    private RectTransform rectTransform;

    public void SetText(string content, string header ="", int durability = 0)
    {
        if(header=="")
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }
        if (durability == 0)
        {
            durabilityField.gameObject.SetActive(false);
        }
        else
        {
            durabilityField.gameObject.SetActive(true);
            durabilityField.text = "Durability : "+durability;
        }

        contentField.text = content;

        int headerLenth = headerField.text.Length;
        int ContentLenth = contentField.text.Length;

        layoutElement.enabled = (headerLenth > maxCharacter || ContentLenth > maxCharacter) ? true : false;
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;
       
        float pivotX = mousePosition.x / Screen.width;
        float pivotY = mousePosition.y / Screen.height;
        rectTransform.pivot = new Vector2(pivotX, pivotY);

        transform.position = mousePosition;
    }
}
