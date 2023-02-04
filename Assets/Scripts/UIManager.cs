using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager> //Singleton Manager user for displaying messages
{

    [SerializeField] private StatusPopup statusPopup;
    [SerializeField] private TextMeshProUGUI collectibleText;

    private int collectiblesCount;

    private void Start()
    {
        collectiblesCount = 0;
        UpdateCollectibleText();
    }

    private void OnEnable()
    {
        CollectibleOne.OnCollectibleOneCollected += ShowCollectibleOnePopup;
        CollectibleTwo.OnCollectibleTwoCollected += ShowCollectibleTwoPopup;

        ClickMovement.OnSpeedIncreased += ShowSpeedIncreasedPopup;
        ClickMovement.OnSpeedDecreased += ShowSpeedDecreasedPopup;
    }

    private void OnDisable()
    {
        CollectibleOne.OnCollectibleOneCollected -= ShowCollectibleOnePopup;
        CollectibleTwo.OnCollectibleTwoCollected -= ShowCollectibleTwoPopup;

        ClickMovement.OnSpeedIncreased -= ShowSpeedIncreasedPopup;
        ClickMovement.OnSpeedDecreased -= ShowSpeedDecreasedPopup;
    }

    private void UpdateCollectibleText()
    {
        if (collectibleText == null)
            return;

        collectibleText.text = "Collectibles : " + collectiblesCount.ToString();
    }

    #region Popups

    void ShowCollectibleOnePopup()
    {
        if(statusPopup == null)
            return;

        statusPopup.SetPopup("Collected: A");
        statusPopup.gameObject.SetActive(true);

        //Updates the collectibles display on top bar
        collectiblesCount++;
        UpdateCollectibleText();
    }

    void ShowCollectibleTwoPopup()
    {
        if (statusPopup == null)
            return;

        statusPopup.SetPopup("Collected: B");
        statusPopup.gameObject.SetActive(true);

        //Updates the collectibles display on top bar
        collectiblesCount++;
        UpdateCollectibleText();
    }

    void ShowSpeedIncreasedPopup()
    {
        if (statusPopup == null)
            return;

        statusPopup.SetPopup("Speed Increased");
        statusPopup.gameObject.SetActive(true);
    }

    void ShowSpeedDecreasedPopup()
    {
        if (statusPopup == null)
            return;

        statusPopup.SetPopup("Speed Decreased");
        statusPopup.gameObject.SetActive(true);
    }

    #endregion
}
