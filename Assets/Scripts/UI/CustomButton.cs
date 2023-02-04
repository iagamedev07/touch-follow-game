using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform buttonRect;
    [SerializeField] private float animDuration = 0.1f;

    private void Awake()
    {
        buttonRect = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        buttonRect.DOScale(new Vector3(0.8f, 0.8f, 0.8f), animDuration);
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        buttonRect.DOScale(new Vector3(1f, 1f, 1f), animDuration);
    }
}
