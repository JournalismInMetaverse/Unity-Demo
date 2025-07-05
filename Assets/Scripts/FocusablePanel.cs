// 2. FocusablePanel.cs (SIMPLIFIED)
// The linkID has been removed.

using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class FocusablePanel : MonoBehaviour
{
    [Header("Focus Settings")]
    [Tooltip("The font size the summary text should have when it is in focus.")]
    public float focusedFontSize = 36;
    
    [Header("Component References")]
    [Tooltip("The main TextMeshPro component for the summary text.")]
    public TextMeshProUGUI summaryTextComponent;
    [Tooltip("A list of GameObjects (like other text fields) that will only be visible when the panel is in focus.")]
    public List<GameObject> detailObjects;
    
    private FocusablePanelManager manager;
    private Vector3 startPosition, startScale;
    private Quaternion startRotation;
    private float originalFontSize;
    private Transform focusMarker;
    private bool isMoving = false;
    private bool isFocused = false;
    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.position;
        startRotation = rectTransform.rotation;
        startScale = rectTransform.localScale;

        if (summaryTextComponent != null)
        {
            originalFontSize = summaryTextComponent.fontSize;
        }

        if (detailObjects != null)
        {
            foreach (var obj in detailObjects)
            {
                obj.SetActive(false);
            }
        }
    }

    public void RegisterManager(FocusablePanelManager mgr)
    {
        manager = mgr;
    }

    public void WasClicked()
    {
        if (manager != null)
        {
            manager.OnPanelClicked(this);
        }
    }

    public void MoveToFocus(Transform target)
    {
        focusMarker = target;
        isFocused = true;
        isMoving = true;
        
        if(summaryTextComponent != null)
        {
            summaryTextComponent.fontSize = focusedFontSize;
        }

        if (detailObjects != null)
        {
            foreach (var obj in detailObjects)
            {
                obj.SetActive(true);
            }
        }
    }

    public void MoveToStart()
    {
        isFocused = false;
        isMoving = true;
        
        if(summaryTextComponent != null)
        {
            summaryTextComponent.fontSize = originalFontSize;
        }

        if (detailObjects != null)
        {
            foreach (var obj in detailObjects)
            {
                obj.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (!isMoving) return;

        Vector3 targetPosition = isFocused ? focusMarker.position : startPosition;
        Quaternion targetRotation = isFocused ? focusMarker.rotation : startRotation;
        Vector3 targetScale = isFocused ? focusMarker.localScale : startScale;
        
        rectTransform.position = Vector3.Lerp(rectTransform.position, targetPosition, Time.deltaTime * 10f);
        rectTransform.rotation = Quaternion.Slerp(rectTransform.rotation, targetRotation, Time.deltaTime * 10f);
        rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, targetScale, Time.deltaTime * 10f);
        
        if (Vector3.Distance(rectTransform.position, targetPosition) < 0.01f)
        {
            rectTransform.position = targetPosition;
            rectTransform.rotation = targetRotation;
            rectTransform.localScale = targetScale;
            isMoving = false;
        }
    }
}