using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class PowerUp : ScriptableObject
{
    public delegate void PowerupAction(PlayerBase player);
    public event PowerupAction OnPickup;
    public event PowerupAction OnTimerTick;
    public event PowerupAction OnEnd;

    [SerializeField]
    private Image powerUpImage = null;

    [SerializeField]
    private float duration = 0f;

    public Image PowerUpImage => powerUpImage;

    public float Duration => duration;

    private float elapsedTime;

    public void PickUp(PlayerBase player)
    {
        OnPickup?.Invoke(player);
    }

    public void Timer(PlayerBase player)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= duration)
            OnEnd?.Invoke(player);
    }
}
