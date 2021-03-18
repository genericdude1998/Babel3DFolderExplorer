using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour
{
    public Camera cam;
    public Transform body;
    public float mouseSens = 100f;
    public float clickReach = 10.0f;
    public float sizeOfTheCursor = 100f;

    private Canvas playerCanvas;
    private Image playerCanvasCursorDisplayer;
    public Sprite cursor;

    private float xRot = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        playerCanvas = GetComponentInChildren<Canvas>();
        playerCanvasCursorDisplayer = GetComponentInChildren<Image>();

        playerCanvasCursorDisplayer.sprite = cursor;
        playerCanvasCursorDisplayer.color = Color.white;

        Cursor.lockState = CursorLockMode.Locked;

    }

    private void OnGUI()
    {
        playerCanvas.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90, 90);

        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        body.Rotate(Vector3.up * mouseX);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            TryClick();
        }

        if (Input.GetKey(KeyCode.Mouse0)) 
        {
            playerCanvasCursorDisplayer.color = Color.red;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            playerCanvasCursorDisplayer.color = Color.white;
        }

    }

    void TryClick()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        int UIInteractibleLayerMask = 1 << 10;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, clickReach, UIInteractibleLayerMask))
        {
            TerminalInteractible interactible;
            interactible = hit.collider.GetComponent<TerminalInteractible>();
            interactible.PressButton();
        }
    }
}
