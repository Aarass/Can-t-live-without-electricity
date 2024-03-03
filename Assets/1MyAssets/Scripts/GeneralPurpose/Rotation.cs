using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField]
    Vector3 axis;
    [SerializeField]
    float speed;
    Vector3 rotatedAmount;
    Vector3 original;
    void Start()
    {
        rotatedAmount = Vector3.zero;
        original = transform.rotation.eulerAngles;
    }
    private void Update()
    {
        rotatedAmount += axis * speed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(original + rotatedAmount);
    }
}
