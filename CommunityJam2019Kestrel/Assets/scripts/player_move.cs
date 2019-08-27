using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_move : MonoBehaviour
{

    [SerializeField] private string hori;
    [SerializeField] private string vert;
    [SerializeField] private float speed;
    private CharacterController char_;
    [SerializeField] private float m;


    private void Awake()
    {
        char_ = GetComponent<CharacterController>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }
    private void move()
    {
        float h = Input.GetAxis(hori) * speed;
        float v = Input.GetAxis(vert) * speed;

        Vector3 forw = transform.forward * v;
        Vector3 right = transform.right * h;


        char_.SimpleMove(forw + right);

    }
}
