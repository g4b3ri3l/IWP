using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(Collider2D))]
public class CamZone : MonoBehaviour
{
    #region Fields

    [SerializeField] private CinemachineVirtualCamera virtualCamera = null;

    #endregion


    #region Monobehavior

    private void Start()
    {
        virtualCamera.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            virtualCamera.enabled = false;
        }
    }

    private void OnValidate()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    #endregion

}