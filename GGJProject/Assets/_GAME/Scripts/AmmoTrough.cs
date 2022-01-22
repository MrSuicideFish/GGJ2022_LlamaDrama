using System.Collections;
using Mirror;
using UnityEngine;

public class AmmoTrough : NetworkBehaviour, IUseable
{
    private const string AmmoPickupConfirm = "AmmoPickupConfirmFX";
    
    public float drinkDuration = 3.0f;

    public GameObject GetGameObject()
    {
        return this.gameObject;
    }

    public void Use(AlpacaController player)
    {
        StartCoroutine(DrinkCallback(player));
    }

    private IEnumerator DrinkCallback(AlpacaController player)
    {
        player.BeginDrink(player.transform.position,
            transform.position - player.transform.position);
        
        float t = 0.0f;
        while (t < drinkDuration)
        {
            t += Time.deltaTime;
            yield return null;
        }

        player.GiveAmmo();
        player.EndDrink();
        
        yield return null;
    }
}