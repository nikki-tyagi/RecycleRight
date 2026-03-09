using System;
using UnityEngine;

public class Core_MiniTriggerHelper : MonoBehaviour
{
    internal Action<Collider> onTriggerEnter;
    internal Action<Collider> onTriggerStay;
    internal Action<Collider> onTriggerExit;
    [SerializeField]private bool debug= false;
    private void OnTriggerEnter(Collider other)
    {
        if (debug)
        {
            Debug.Log($"OnTriggerEnter {other.name}");
        }
        onTriggerEnter?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        if (debug)
        {
            Debug.Log($"OnTriggerStay {other.name}");
        }
        onTriggerStay?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (debug)
        {
            Debug.Log($"OnTriggerExit {other.name}");
        }
        onTriggerExit?.Invoke(other);
    }
}