using UnityEngine;

public static class IsPlayerExtension
{
    private const string _PLAYER_TAG = "Player";

    public static bool IsPlayer(this GameObject go)
    {
        return go.CompareTag(_PLAYER_TAG);
    }
}
