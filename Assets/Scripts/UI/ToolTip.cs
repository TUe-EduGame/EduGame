using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ToolTip : MonoBehaviour
{
    private static ToolTip instance;

    [SerializeField] private RectTransform canvasRectTransform;
    private Camera uiCamera;

    private TextMeshProUGUI tooltipText;
    private RectTransform backgroundRectTransform;

    private void Awake() {
        instance = this;
        backgroundRectTransform = GameObject.Find("Background").GetComponent<RectTransform>();
        tooltipText = GameObject.Find("Text").GetComponent<TextMeshProUGUI>();
        ShowToolTip("Let's go");
        HideToolTip();
    }

    private void Update() {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        
        if(localPoint.x + backgroundRectTransform.rect.width > canvasRectTransform.rect.width) {
            localPoint.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }
        if(localPoint.y + backgroundRectTransform.rect.height > canvasRectTransform.rect.height) {
            localPoint.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }
        transform.localPosition = localPoint;
    }

    private void ShowToolTip(string tooltipString) {
        gameObject.SetActive(true);
        tooltipText.SetText(tooltipString);
        tooltipText.ForceMeshUpdate();
        float textPaddingSize = 4f;
        Vector2 backgroundSize = tooltipText.GetRenderedValues(false) + new Vector2(textPaddingSize * 2f, textPaddingSize * 3f);
        backgroundRectTransform.sizeDelta = backgroundSize;        
    }

    private void HideToolTip() {
        gameObject.SetActive(false);
    }

    public static void ShowToolTip_Static(string tooltipString) {
        instance.ShowToolTip(tooltipString);
    }

    public static void HideToolTip_Static() {
        instance.HideToolTip();
    }
}
