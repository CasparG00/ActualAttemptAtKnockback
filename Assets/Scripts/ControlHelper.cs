using UnityEngine;

public class ControlHelper : MonoBehaviour
{
    public static bool ShowControls;

    public GameObject controlHelperWrapper;

    private void Update()
    {
        controlHelperWrapper.SetActive(ShowControls);
    }
}
