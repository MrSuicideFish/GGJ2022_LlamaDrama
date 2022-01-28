using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;
using UnityEngine;

public class UIAlpacaTracker : MonoBehaviour
{
    private class NametagInfo
    {
        public string playerName;
        public AlpacaController player;
        public UIAlpacaNametag nametag;

        public NametagInfo(string playerName, AlpacaController player, UIAlpacaNametag nametag)
        {
            this.playerName = playerName;
            this.player = player;
            this.nametag = nametag;
        }
    }

    public Vector3 nametagOffset;
    
    private UIAlpacaNametag nametagPrefab = null;
    private List<NametagInfo> nametags;
    private Canvas canvasComponent;

    private void Awake()
    {
        nametags = new List<NametagInfo>();

        canvasComponent = this.GetComponent<Canvas>();
        if (canvasComponent != null)
        {
            canvasComponent.worldCamera = Camera.main;
            canvasComponent.planeDistance = 1.0f;
        }
    }

    public void CreateNametag(string playerName, AlpacaController alpaca)
    {
        if (nametagPrefab == null)
        {
            nametagPrefab = this.transform
                .GetChild(0)
                .GetComponent<UIAlpacaNametag>();
            nametagPrefab.gameObject.SetActive(false);
        }

        UIAlpacaNametag newNametag = UIAlpacaNametag.Instantiate(
            nametagPrefab,
            parent: this.transform);
        
        if (newNametag != null)
        {
            newNametag.SetPlayerName(playerName);
            newNametag.SetPlayerColor(alpaca.playerColor);
            newNametag.gameObject.SetActive(true);
            
            NametagInfo nametagInfo = new NametagInfo(playerName, alpaca, newNametag);
            nametags.Add(nametagInfo);
        }
    }

    private void Update()
    {
        UpdateNametags();
    }

    private void UpdateNametags()
    {
        foreach (NametagInfo nameTagInfo in nametags)
        {
            // check if off-screen
            bool isOffscreen = false;
            
            // determine player position
            Vector2 canvasPos;
            Vector3 playerWorldPos = nameTagInfo.player.headTarget.transform.position;
            Vector3 playerScreenPos = Camera.main
                .WorldToScreenPoint(playerWorldPos);

            isOffscreen = (playerScreenPos.x < 0 || playerScreenPos.x > Screen.width)
                          || (playerScreenPos.y < 0 || playerScreenPos.y > Screen.height);

            if (!isOffscreen)
            {
                RectTransformUtility
                    .ScreenPointToLocalPointInRectangle(
                        canvasComponent.transform as RectTransform,
                        (Vector2)(playerScreenPos + nametagOffset)
                        , canvasComponent.worldCamera, out canvasPos);
                    
                //enable name
                nameTagInfo.nametag.playerNameText.enabled = true;
                
                // move nametag with player (above head)
                nameTagInfo.nametag.RectTransform.anchoredPosition = canvasPos;

                // arrow point down
                nameTagInfo.nametag.arrowImg.transform.localEulerAngles = Vector3.zero;
                
                // hide warning message
                nameTagInfo.nametag.HideWarningMessage();
            }
            else
            {
                // move nametag to edge of screen
                playerScreenPos.x = Mathf.Clamp(playerScreenPos.x, 100, Screen.width - 100);
                playerScreenPos.y = Mathf.Clamp(playerScreenPos.y, 100, Screen.height - 100);

                RectTransformUtility
                    .ScreenPointToLocalPointInRectangle(
                        canvasComponent.transform as RectTransform,
                        (Vector2)(playerScreenPos + nametagOffset)
                        , canvasComponent.worldCamera, out canvasPos);

                // move nametag with player
                nameTagInfo.nametag.RectTransform.anchoredPosition = canvasPos;
                
                // disable name
                nameTagInfo.nametag.playerNameText.enabled = false;

                // adjust arrow to point to player direction
                Vector3 dollyTrackPos = GameManager.Instance.gameCameraDollyTrack.LookAtTargetPosition;
                Vector3 centerToPlayerDir = dollyTrackPos - playerWorldPos;
                centerToPlayerDir.y = 0;
                nameTagInfo.nametag.arrowImg.transform.localEulerAngles = new Vector3(
                    0, 0,
                    Vector3.Angle(Vector3.forward, centerToPlayerDir));
                
                // show warning message
                nameTagInfo.nametag.ShowWarningMessage();
            }
        }
    }
}
