using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    public float speed;
    private float upRot;
    private float mouseX;
    private float mouseY;
    private float calcX;
    private float calcY;
    private float how;
    private float ver;
    public float rotationRestriction;
    public float rotationSpeed;
    private Rigidbody rb;
    private Transform cameraValues;
    public float interactRayDistance;
    public LayerMask interactionLayers;
    public Text interactionText;
    private bool piecesGathethered;

    private CapsuleCollider col;
    private int passcodeNumbersCollected;
    private bool key1Collected;
    private bool freeze;
    //level 2 items
    private bool tokenCollected;
    private int index;
    public GameObject screwdriver;
    public Transform dispensePoint;
    private bool dispensed;

    public GameObject torch;
    public Transform spawn2;
    [SerializeField] private string hori;
    [SerializeField] private string vert;
    [SerializeField] private float speed1;
    private CharacterController char_;
    [SerializeField] private float m;



    [SerializeField] private string imputX, inputY;
    [SerializeField] private float s;
    private float clampX;

    [SerializeField] private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<CapsuleCollider>();
        lookC(); clampX = 0.0f;
        rb = GetComponent<Rigidbody>();
        piecesGathethered = false;
        char_ = GetComponent<CharacterController>();
        player = GetComponent<Transform>();
        key1Collected = false;
        freeze = false;
        index = 0;
        dispensed = false;
    }

    // Update is called once per frame
    void Update()
    {
        cameraValues = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        mouseY = Mathf.Clamp(mouseY, -rotationRestriction, rotationRestriction);
        mouseX += Input.GetAxis("Mouse X") / 10 * rotationSpeed;
        mouseY += Input.GetAxis("Mouse Y") / 10 * rotationSpeed;
        how = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
        if (passcodeNumbersCollected == 4)
        {
            piecesGathethered = true;
        }
        if(freeze == false)
        {
            move();
        }
        if(index == 4 && tokenCollected == true && dispensed == false)
        {
            Debug.Log("disperse");
            Instantiate(screwdriver, dispensePoint.position, Quaternion.identity);
            dispensed = true;
        }

        //rot();
        transform.eulerAngles = new Vector3(-mouseY, mouseX, 0f);
        if (Input.GetMouseButtonDown(0))
        {
           
           // Debug.Log("Interaction process initiated");
            RaycastHit hit;
            if (Physics.Raycast(cameraValues.position, cameraValues.forward, out hit, interactRayDistance, interactionLayers))
            {
                if (hit.collider.gameObject.CompareTag("safe") && piecesGathethered == false)
                {
                    Debug.Log("Should display text");
                    interactionText.text = "Can't open without combination...";
                    Animator anim = interactionText.GetComponent<Animator>();
                    anim.SetTrigger("fade");
                }
                if (hit.collider.gameObject.CompareTag("safe") && piecesGathethered == true)
                {
                    Animator animi = hit.collider.gameObject.GetComponent<Animator>();
                    animi.SetTrigger("open");
                }
                if (hit.collider.gameObject.CompareTag("drawer"))
                {
                    Animator animi = hit.collider.gameObject.GetComponent<Animator>();
                    animi.SetTrigger("interact");
                }
                if (hit.collider.gameObject.CompareTag("passcodeNumber"))
                {
                    Destroy(hit.collider.gameObject);
                    passcodeNumbersCollected += 1;
                }
                if (hit.collider.gameObject.CompareTag("key1"))
                {
                    Debug.Log("key");
                    Destroy(hit.collider.gameObject);
                    key1Collected = true;
                    interactionText.text = "The key is labelled 4...";
                    Animator anim = interactionText.GetComponent<Animator>();
                    anim.SetTrigger("fade");
                }
                if (hit.collider.gameObject.CompareTag("door4") && key1Collected == true)
                {
                    StartCoroutine(torchFlicker());
                    Animator anim = hit.collider.gameObject.GetComponent<Animator>();
                    anim.SetTrigger("open");
                }
                if (hit.collider.gameObject.CompareTag("raincoat"))
                {
                    Destroy(hit.collider.gameObject);
                    tokenCollected = true;
                    interactionText.text = "There is a token in the pocket...";
                    Animator anim = interactionText.GetComponent<Animator>();
                    anim.SetTrigger("fade");
                }
                if (hit.collider.gameObject.CompareTag("3vending") && index != 0)
                {
                    index = 0;
                }
                if (hit.collider.gameObject.CompareTag("3vending") && index == 0)
                {
                    index = 1;
                }
                if (hit.collider.gameObject.CompareTag("4vending") && index != 1)
                {
                    index = 0;
                }

                if (hit.collider.gameObject.CompareTag("4vending") && index == 1)
                {
                    index += 1;
                }
                if (hit.collider.gameObject.CompareTag("7vending") && index != 2)
                {
                    index = 0;
                }
                if (hit.collider.gameObject.CompareTag("7vending") && index == 2)
                {
                    index += 1;
                }
                if (hit.collider.gameObject.CompareTag("1vending") && index != 3)
                {
                    index = 0;
                }
                if (hit.collider.gameObject.CompareTag("1vending") && index == 3)
                {
                    index += 1;
                }
                
                if (hit.collider.gameObject.CompareTag("2vending") || hit.collider.gameObject.CompareTag("5vending") || hit.collider.gameObject.CompareTag("6vending") || hit.collider.gameObject.CompareTag("8vending") || hit.collider.gameObject.CompareTag("9vending"))
                {
                    index = 0;
                }
                Debug.Log(index);
            }
        }
    }
    private void move()
    {
        float h = Input.GetAxis("Horizontal") * speed;
        float v = Input.GetAxis("Vertical") * speed;

        Vector3 forw = transform.forward * v;
        Vector3 right = transform.right * h;


        char_.SimpleMove(forw + right);

    }
    private void
        lookC()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void rot()
    {
        float x = Input.GetAxis("Mouse X") * s * Time.deltaTime;

        float y = Input.GetAxis("Mouse Y") * s * Time.deltaTime;
        transform.Rotate(Vector3.left * y);
        player.Rotate(Vector3.up * x);
        clampX += y;

        if (clampX > 90.0f)
        {
            clampX = 90.0f;
            y = 0.0f;
            clamp(270.0f);
        }
        else if (clampX < -90.0f)
        {
            clampX = -90.0f;
            y = 0.0f;
            clamp(90.0f);
        }
    }
    private void clamp(float v)
    {
        Vector3 eul = transform.eulerAngles;
        eul.x = v;
        eul.z = 0;
        transform.eulerAngles = eul;
    }
    IEnumerator torchFlicker()
    {
        yield return new WaitForSeconds(1);
        torch.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        torch.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        torch.SetActive(false);
        freeze = true;
        yield return new WaitForSeconds(0.3f);
        torch.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        torch.SetActive(false);
        col.enabled = false;
        transform.position = spawn2.position;
        yield return new WaitForSeconds(0.3f);
        col.enabled = false;
        transform.position = spawn2.position;
        yield return new WaitForSeconds(0.1f);
        transform.position = spawn2.position;
        col.enabled = true;
        yield return new WaitForSeconds(2f);
        transform.position = spawn2.position;
        torch.SetActive(true);
        freeze = false;
        yield return new WaitForSeconds(0.2f);
        torch.SetActive(false);
        freeze = false;
        yield return new WaitForSeconds(0.3f);
        torch.SetActive(true);
    }
}
