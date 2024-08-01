using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillBox : MonoBehaviour
{
    public Vector3 respawnPoint;
    public Transform playerT;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.CompareTag("Player")) // Check if the collided object has the "Player" tag
        {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);        }
    }
}
