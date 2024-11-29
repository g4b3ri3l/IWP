using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderRotater : MonoBehaviour
{
    [SerializeField] private float rotationDuration = 0.5f;
    [SerializeField] private AnimationCurve rotationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool isRotating = false;

    private void Update()
    {
        if (GameController.Instance.state == GameState.FreeRoam)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                RotateColliders(true);

            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                RotateColliders(false);
            }

        }
    }

    public void RotateColliders(bool clockwise)
    {
        if (isRotating) return;

        float rotationAngle = clockwise ? 90f : -90f;
        StartCoroutine(SmoothRotateColliders(rotationAngle));
    }

    private IEnumerator SmoothRotateColliders(float targetAngle)
    {
        isRotating = true;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, targetAngle, 0);

        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = rotationCurve.Evaluate(elapsedTime / rotationDuration);

            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null;
        }

        transform.rotation = targetRotation;
        isRotating = false;
    }



}
