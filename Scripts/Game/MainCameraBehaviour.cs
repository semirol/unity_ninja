using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraBehaviour : MonoBehaviour
{
    private Transform _playerTransform;
    private Vector3 _offset = new Vector3(0, 15, -6);

    void Start()
    {
        transform.rotation = Quaternion.Euler(70, 0, 0);
        TryInitPlayerTransform();
    }

    private void LateUpdate()
    {
        if (_playerTransform == null)
        {
            TryInitPlayerTransform();
        }
        else
        {
            transform.position = _playerTransform.position + _offset;
            // transform.position = Vector3.Lerp (transform.position, _playerTransform.position + _offset, Time.deltaTime * 10f);
        }
    }

    private void TryInitPlayerTransform()
    {
        try
        {
            _playerTransform = GameObject.Find("Player").transform;
        }
        catch
        {
            //ignore
        }
    }
}
