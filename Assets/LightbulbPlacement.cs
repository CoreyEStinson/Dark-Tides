using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightbulbPlacement : MonoBehaviour
{
    public bool hasBulb = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && hasBulb)
        {
            //reset scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
