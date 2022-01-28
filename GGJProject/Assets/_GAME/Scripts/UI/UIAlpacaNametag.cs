using UnityEngine;
using System;
using UnityEngine.UI;

public class UIAlpacaNametag : MonoBehaviour
{
    public Text playerNameText;
    public Text warningText;
    public Image arrowImg;
    
    public RectTransform RectTransform { get; private set; }

    private void OnEnable()
    {
        RectTransform = this.GetComponent<RectTransform>();
    }

    public void SetPlayerName(string playerName)
    {
        playerNameText.text = playerName;
    }

    public void SetPlayerColor(AlpacaColor color)
    {
        Color rawColor = (color == AlpacaColor.BLUE)
            ? GameManager.Instance.blueAlpacaColor
            : GameManager.Instance.pinkAlpacaColor;

        playerNameText.color = rawColor;
        arrowImg.color = rawColor;
    }

    public void ShowWarningMessage()
    {
        if (!warningText.gameObject.activeInHierarchy)
        {
            warningText.gameObject.SetActive(true);    
        }
    }

    public void HideWarningMessage()
    {
        if (warningText.gameObject.activeInHierarchy)
        {
            warningText.gameObject.SetActive(false);    
        }
    }
}