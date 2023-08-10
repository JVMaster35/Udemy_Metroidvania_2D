using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullMapCameraController : MonoBehaviour
{

    [SerializeField] private MapCameraController mapCameraController;

    public float zoomSpeed;
    public float minZoom, maxZoom;
    public float moveModifier;

    private float startSize;

    private Camera fullMapCam;

    // Start is called before the first frame update
    void Start()
    {
        fullMapCam = GetComponent<Camera>();

        startSize = fullMapCam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        FullMapControls();
    }

    public void FullMapControls()
    {
        transform.position += new Vector3(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"), 
            0f).normalized * fullMapCam.orthographicSize * Time.unscaledDeltaTime * moveModifier;

        if(Input.GetKey(KeyCode.E))
        {
            fullMapCam.orthographicSize -= zoomSpeed * Time.unscaledDeltaTime;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            fullMapCam.orthographicSize += zoomSpeed * Time.unscaledDeltaTime;
        }

        fullMapCam.orthographicSize = Mathf.Clamp(fullMapCam.orthographicSize, minZoom, maxZoom);
    }

    private void OnEnable()
    {
        transform.position = mapCameraController.transform.position;
    }
}
