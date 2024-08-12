using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
    public ItemData popupData;
    public GameObject itemPopup;

    [Header("Popup Item")]
    public Image popupIcon;
    public TextMeshProUGUI popupName;
    public TextMeshProUGUI popupDescription;
    public GameObject continueText;

    public bool isContinue;

    public void PopupGetItem()
    {
        popupData = CharacterManager.Instance.Player.itemData;
        continueText.SetActive(false);
        isContinue = false;
        popupName.text = popupData?.name;
        popupDescription.text = popupData?.description;
        popupIcon.sprite = popupData?.icon;

        itemPopup.SetActive(true);
        StartCoroutine(OnIsContinue());
    }

    IEnumerator OnIsContinue()
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(2f);

        continueText.SetActive(true);
        isContinue = true;
    }

    public void ClosePopup()
    {
        itemPopup.SetActive(false);
        Time.timeScale = 1f;
    }
}
