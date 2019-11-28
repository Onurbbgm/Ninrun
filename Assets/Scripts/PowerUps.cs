using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public float bonusSpeed = 3f;
    public float bonusSpeedDuration = 10f;
    public float bonusInvincibilityDuration = 10f;
    public float bonusMagnetRadius = 20f;
    public float bonusMagnetDuration = 10f;

    void FixedUpdate()
    {
        transform.Rotate((Vector3.up + Vector3.forward + Vector3.right) * 200f * Time.fixedDeltaTime);
    }
}
