using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class StatusPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float animDuration = 1f;
    [SerializeField] private float textAnimElevationValue = 70f;

    private Vector2 defaultTextPosition;

    Camera mainCamera;
    Coroutine showCoroutine;
    private void Awake()
    {
        mainCamera = Camera.main;
        defaultTextPosition = new Vector2(text.rectTransform.anchoredPosition.x, text.rectTransform.anchoredPosition.y);
    }
    private void Update()
    {
        if (mainCamera == null)
            return;

        //Ensures text always faces the camera
        transform.LookAt(mainCamera.transform);
        transform.Rotate(Vector3.up * 180);
    }

    private void OnEnable()
    {
        if(showCoroutine != null)
            StopCoroutine(showCoroutine);

        showCoroutine = StartCoroutine(ShowPopup());
    }

    //Popup Animation
    IEnumerator ShowPopup()
    {
        text.rectTransform.anchoredPosition = new Vector2(defaultTextPosition.x, defaultTextPosition.y);
        canvasGroup.DOFade(1f, animDuration);
        text.rectTransform.DOAnchorPos3DY(text.rectTransform.anchoredPosition.y + textAnimElevationValue, animDuration);

        yield return new WaitForSeconds(animDuration);

        canvasGroup.DOFade(0f, animDuration);        

        yield return new WaitForSeconds(animDuration);

        gameObject.SetActive(false);
    }

    public void SetPopup(string prompt)
    {
        if (showCoroutine != null)
            StopCoroutine(showCoroutine);

        gameObject.SetActive(false);
        text.text = prompt;
    }
}
