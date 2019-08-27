using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_look : MonoBehaviour
{
    [SerializeField] private string imputX, inputY;
    [SerializeField] private float s;
    private float clampX;

    [SerializeField] private Transform player;

    private void Awake()
    {
        lookC();clampX = 0.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   private void Update()
    {
        rot();

    }

    private void
        lookC()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void rot()
    {
        float x = Input.GetAxis(imputX) * s * Time.deltaTime;

        float y = Input.GetAxis(inputY) * s * Time.deltaTime;
        transform.Rotate(Vector3.left * y);
        player.Rotate(Vector3.up * x);
        clampX += y;

        if(clampX > 90.0f)
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

    private void clamp( float v)
    {
        Vector3 eul = transform.eulerAngles;
        eul.x = v;
        transform.eulerAngles = eul;
    }

}
