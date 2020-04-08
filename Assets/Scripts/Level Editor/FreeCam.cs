using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCam : MonoBehaviour {
    private float xAxis, yAxis, zoom;
    private Camera cam;

    public float speed = 10f;

    private void Start() {
        cam = GetComponent<Camera>();
    }

    private void Update() {
        xAxis = Input.GetAxis("Horizontal");
        yAxis = Input.GetAxis("Vertical");
        zoom = Input.GetAxis("Mouse ScrollWheel") * 10;
        transform.Translate(new Vector3(xAxis * -speed, yAxis * -speed, 0.0f));
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -20, 20),
            Mathf.Clamp(transform.position.y, 20, 20),
            Mathf.Clamp(transform.position.z, -20, 20));
        if (zoom < 0 && cam.orthographicSize >= -25)
            cam.orthographicSize -= zoom * -speed;
        if (zoom > 0 && cam.orthographicSize <= -5)
            cam.orthographicSize += zoom * speed;
    }
}
