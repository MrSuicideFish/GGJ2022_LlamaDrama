
using Mirror;
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
    void Use(NetworkIdentity identity);
}

public interface IDamageable
{
    NetworkIdentity GetNetworkIdentity();
    void Hit(AlpacaColor hitBy);
}

public class HitInfo
{
    public uint hitEntityId;
    public uint hitById;

    public HitInfo(uint hitEntitiy, uint hitBy)
    {
        this.hitEntityId = hitEntitiy;
        this.hitById = hitBy;
    }
}