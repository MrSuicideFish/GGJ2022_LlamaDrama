
using UnityEngine;

public enum AlpacaColor
{
    PINK,
    BLUE,
    BOTH
}

public interface IUseable
{
    GameObject GetGameObject();
    void Use(AlpacaController player);
}