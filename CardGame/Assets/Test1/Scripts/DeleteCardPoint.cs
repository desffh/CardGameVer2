using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCardPoint : MonoBehaviour
{
    [SerializeField] public Transform deleteCardPoints;

    private void Awake()
    {
        deleteCardPoints = GetComponent<Transform>();
    }
}
