using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour {
    public event Action<string> OnStartDialogSequence;
    public event Action<int> OnPdaElementPickUp;
    public event Action OnKillzoneEnter;
    public event Action<PowerUpType> OnPowerUpPickUp;
    public event Action OnGunShoot;
    public event Action OnBotSpawn;
    public void StartDialogSequence(string sequenceName) => OnStartDialogSequence?.Invoke(sequenceName);
    public void PickUpPDAElement(int id) => OnPdaElementPickUp?.Invoke(id);
    public void KillOnEnter() => OnKillzoneEnter?.Invoke();
    public void PickUpPowerUp(PowerUpType type) => OnPowerUpPickUp?.Invoke(type);
    public void ShootGun() => OnGunShoot?.Invoke();
    public void SpawnBot() => OnBotSpawn?.Invoke();
}
