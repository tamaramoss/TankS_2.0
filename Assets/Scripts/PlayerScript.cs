using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;    
    public float aimSpeed;

    private Rigidbody rb;
    private Transform tankTopTransform;
    private Camera main;
    private Vector3 readMoveValue;
    private Vector2 readMouseValue;
    private float aimHeight;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        tankTopTransform = transform.Find("Top");
        main = Camera.main;
        aimHeight = GameManager.GetInstance().SpawnHeight;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyAim();
    }

    // Update the value, gets called with events
    public void OnMoveTank(InputAction.CallbackContext context)
    {
        readMoveValue = context.ReadValue<Vector2>();
    }
    
    // Rotate the tank and move him forward
    private void ApplyMovement()
    {
        var wantedVelocity = transform.forward * (readMoveValue.y * moveSpeed);
        rb.AddForce(wantedVelocity);
        
        var wantedRotationForce = Vector3.up * (readMoveValue.x * rotationSpeed);
        rb.AddTorque(wantedRotationForce);
    }

    // Called on click event
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;
        
        StartCoroutine("Shoot");
        SpawnPools.Instance.SpawnFromPool(GetProjectileName(context.action.ToString()),
            transform.Find("Top").Find("ProjectileSpawnPosition"));
    }

    // The action string looks like this:
    // action=PlayerTank/ShootMain[/Mouse/leftButton]
    // Pools named like action -> spawn right projectile with action name
    private string GetProjectileName(string actionString)
    {
        var startIdx = actionString.IndexOf('/');
        return actionString.Substring( startIdx + 1,
            (actionString.IndexOf('[') - startIdx) - 1);
    }

    // Some time between shooting
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1f);
    }

    // Called when new mouse input is detected
    public void OnAim(InputAction.CallbackContext context)
    {
        readMouseValue = context.ReadValue<Vector2>();
    }

    // Rotate the top of the tank towards the mouse
    private void ApplyAim()
    {
        // make a ray from the camera to screen mouse position 
        var ray = main.ScreenPointToRay(readMouseValue);
        var currentRotation = tankTopTransform.rotation;
        Quaternion targetRotation;

        if (Physics.Raycast(ray, out var hit))
        {
            // Fix the y value ( I want to mouse to be the same height ) and calculate a quaternion
            // with the direction from tank position to our hit point
            var hitPointFixedY = new Vector3(hit.point.x, aimHeight, hit.point.z);
            targetRotation = Quaternion.LookRotation(hitPointFixedY - tankTopTransform.position);
        }
        else
        {
            // If nothing was hit take ray direction
            targetRotation = Quaternion.LookRotation(ray.direction);
        }
        
        // Check if I need to rotate towards
        float angularDifference = Quaternion.Angle(currentRotation, targetRotation);
        if (angularDifference > 0)
        {
            // Only Y axis rotation wanted
            var qOnlyYAxisRotation = new Quaternion(0, targetRotation.y, 0, targetRotation.w);
            tankTopTransform.rotation = Quaternion.RotateTowards(tankTopTransform.rotation, qOnlyYAxisRotation, aimSpeed);
            // topRb.rotation = Quaternion.Slerp
            // (
            //     topRb.rotation,
            //     newR,
            //     aimSpeed * Time.fixedDeltaTime
            // );
        }
    }
    
    public void OnQuit(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(0);
    }
} 

