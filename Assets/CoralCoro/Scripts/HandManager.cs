using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance { get; private set; }

    public Vector3 lefthandPosition { get; private set; }
    public Vector3 righthandPosition { get; private set; }
    public Collider lefthandCollider { get; private set; }
    public Collider righthandCollider { get; private set; }
}
