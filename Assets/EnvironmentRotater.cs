using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentRotater : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;

    private Quaternion _targetRot;

    private void Awake()
    {
        _targetRot = transform.rotation;
    }



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _targetRot *= Quaternion.AngleAxis(-90, transform.forward);

        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            _targetRot *= Quaternion.AngleAxis(90, transform.forward);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRot, _rotationSpeed * Time.deltaTime);

    }
}
