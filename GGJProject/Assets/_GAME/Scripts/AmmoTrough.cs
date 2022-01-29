using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

public class AmmoTrough : NetworkBehaviour, IUseable
{
    private const string AmmoPickupConfirm = "AmmoPickupConfirmFX";
    public float drinkDuration = 3.0f;
    public UnityEvent OnDrinkStart = new UnityEvent();
    public UnityEvent OnDrinkEnd = new UnityEvent();

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    [Command(requiresAuthority = false)]
    public void Use(NetworkIdentity identity)
    {
        if (identity != null)
        {
            ClientUse(identity);
        }
    }

    [ClientRpc]
    private void ClientUse(NetworkIdentity identity)
    {
        StartCoroutine(DrinkCallback(identity));
    }

    private IEnumerator DrinkCallback(NetworkIdentity identity)
    {
        AlpacaController player = identity.gameObject.GetComponent<AlpacaController>();
        if (player != null)
        {
            player.BeginDrink(player.transform.position,
                transform.position - player.transform.position);
        
            OnDrinkStart?.Invoke();
        
            float t = 0.0f;
            while (t < drinkDuration)
            {
                t += Time.deltaTime;
                yield return null;
            }
        
            player.GiveAmmo();
            player.EndDrink();
            OnDrinkEnd?.Invoke();
        }
        else
        {
            Debug.LogWarning("NO ALPACA CONTROLLER");
        }

        yield return null;
    }
}