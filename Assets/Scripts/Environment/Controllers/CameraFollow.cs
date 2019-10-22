using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{

    public GameObject target;

    public float distance = 2;

    public float height = 1;

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        if (target)
        {
            transform.position = target.transform.position + (distance * -target.transform.forward) + (height * Vector3.up);
            transform.rotation = target.transform.rotation;
        }
    }
}
