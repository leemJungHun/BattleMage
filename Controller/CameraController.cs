using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject _player;
    Vector3 cameraPos = new Vector3(0, 0, -10);

    private void FixedUpdate()
    {
        if(_player.transform.position.y <= 0)
        {

            transform.position = cameraPos;
            return;
        }
        transform.position = _player.transform.position + cameraPos;
    }
}
