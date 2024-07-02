using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    private bool isSizeIncreased = false;

    public void Initialize(bool isSizeIncreased)
    {
        this.isSizeIncreased = isSizeIncreased;
    }

    public bool GetIsSizeIncreased()
    {
        return isSizeIncreased;
    }
}
