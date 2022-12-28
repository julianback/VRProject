using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private InputActionProperty _showButton; // Y Button
    [SerializeField] private InputActionProperty _thumbstick;
    [SerializeField] private Transform _head;
    public float spawnDistance = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        _menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Hide the menu if the thumbstick is moved to teleport or move
        if (_thumbstick.action.IsPressed())
        {
            _menu.SetActive(false);
        }

        if (_showButton.action.WasPressedThisFrame())
        {
            _menu.SetActive(!_menu.activeSelf);

            _menu.transform.position = _head.position + new Vector3(_head.forward.x, 0, _head.forward.z).normalized * spawnDistance;
        }

        if (_menu.activeSelf)
        {
            _menu.transform.LookAt(new Vector3(_head.position.x, _menu.transform.position.y, _head.position.z));
            _menu.transform.forward *= -1;
        }
    }
}
