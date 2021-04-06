using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{
    private Camera main;
    private Vector2 mousePosition;
    private PlayerColor color;
    public ArrowScript arrow;
    public MenuScript menu;
    void Start()
    {
        main = Camera.main;
        color = GameObject.Find("PlayerColor").GetComponent<PlayerColor>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        // make a ray from the camera to screen mouse position 
        var ray = main.ScreenPointToRay(mousePosition);
        Physics.Raycast(ray, out var hit);

        if (hit.collider)
        {
            switch (hit.collider.name)
            {
                case "Red":
                    color.Color = hit.collider.name;
                    arrow.SelectRed();
                    break;
                case "Blue":
                    color.Color = hit.collider.name;
                    arrow.SelectBlue();
                    break;
                case "Yellow":
                    color.Color = hit.collider.name;
                    arrow.SelectYellow();
                    break;
                case "Green":
                    color.Color = hit.collider.name;
                    arrow.SelectGreen();
                    break;
                case "Start":
                    menu.PlayGame();
                    break;
                case "Quit":
                    menu.Exit();
                    break;
                default:
                    break;
            }
        }
    }
    
}
