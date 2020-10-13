using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform player;

    private void Update() {
        transform.position = player.transform.position;
    }
}