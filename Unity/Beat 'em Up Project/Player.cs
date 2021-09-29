using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public PlayerControls controls;
    public Actor actor;
    public Camera mainCam;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Enable();
        controls.Gameplay.Jump.started += ctx => actor.Jump();
        controls.Gameplay.HeavyAttack.started += ctx => actor.HeavyAttack();
        controls.Gameplay.LightAttack.started += ctx => actor.LightAttack();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
        transform.position = actor.transform.position;
        mainCam.transform.rotation = new Quaternion();
        mainCam.transform.RotateAround(transform.position, Vector3.right, 40);
    }

    // Update is called once per frame
    void Update()
    {
        if(Application.isFocused)
        {
            //Mouse.current.WarpCursorPosition(new Vector2(Screen.width / 2, Screen.height / 2));
        }

        if(controls.UI.CloseGame.ReadValue<float>() > 0)
        {
            Application.Quit();
            if (Application.isEditor)
            {
                UnityEditor.EditorApplication.isPlaying = false;
            }
        }
    }

    void FixedUpdate()
    {
        float camRotationAngle = controls.Gameplay.Camera.ReadValue<Vector2>().x;
        mainCam.transform.RotateAround(transform.position, Vector3.up, camRotationAngle * 1.5f);
        Vector3 moveDir = new Vector3(controls.Gameplay.Movement.ReadValue<Vector2>().x, 0, controls.Gameplay.Movement.ReadValue<Vector2>().y);
        moveDir = Vector3.ClampMagnitude(moveDir, 1); 
        Vector3 camRotation = mainCam.transform.rotation.eulerAngles;
        camRotation.x = 0;
        camRotation.z = 0;
        actor.MoveTowards(Quaternion.Euler(camRotation) * moveDir);




        if (Vector3.Distance(transform.position, actor.transform.position) >= 10)
        {
            float lerpPoint = Mathf.Clamp(1 / (Vector3.Distance(transform.position, actor.transform.position) + 1f), 0.1f, 1);
            transform.position = Vector3.Lerp(transform.position, actor.transform.position, lerpPoint);
        }
        else
        {
            transform.position = actor.transform.position;
        }
    }
}
