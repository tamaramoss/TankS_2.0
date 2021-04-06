using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Projectile", menuName = "Projectile")]
public class Projectile : ScriptableObject
{
    public new string name;
    public float moveSpeed;
    public bool homing;
    public float rotateSpeed;
}
