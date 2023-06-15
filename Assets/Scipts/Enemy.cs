using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    private GameObject playerGO;
    private Rigidbody rb;
    private PlayerController playerController;

    void Start()
    {
        playerGO = GameObject.Find("wizard");
        rb = GetComponent<Rigidbody>();
        playerController = GameObject.Find("wizard").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerController.isGameOver) return;
        
        rb.AddForce(transform.forward * moveSpeed);
        transform.LookAt(playerGO.transform);
    }
}
