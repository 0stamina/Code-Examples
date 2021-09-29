using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public RuntimeAnimatorController[] animatorControllers;
    public Animator animator;
    public GameObject model;
    public MeshCollider hurtBox;

    public LayerMask groundLayer;
    public Vector3 movementVector;
    public Vector3 jumpVector;
    public bool grounded;
    public bool jumping;
    public float movementSpeed;
    public float gravity;
    public uint comboString;

    public float jumpingTimer;
    public float comboTimer;

    public string debugOutput;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!grounded && !jumping)
        {
            movementVector.y = gravity;
        }

        if(jumping)
        {
            jumpVector = movementVector;
            movementVector.y = movementSpeed * 1.5f;

            if(jumpingTimer >= 0)
            {
                jumpingTimer -= Time.fixedDeltaTime;
            }
            else
            {
                jumping = false;
            }
        }

        if (comboTimer >= 0)
        {
            comboTimer -= Time.fixedDeltaTime;
        }
        else
        {
            comboString = 0;
        }

        Vector3 movementSegment = movementVector * Time.fixedDeltaTime;


        int siz;
        RaycastHit[] hits;

        //Check For Walls
        {
            //TODO: fix walls 

            Vector3 pushBack = Vector3.zero;
            Vector3 deltaPos = transform.position + movementSegment;
            Vector3 circPos = deltaPos + Vector3.up * 0.60f;
            for (int angle = 0; angle < 360; angle += 10)
            {
                hits = new RaycastHit[15];
                var point = (circPos) + Quaternion.Euler(0, angle, 0) * Vector3.forward * 0.75f;
                var point2 = (circPos) + Quaternion.Euler(0, angle + 10, 0) * Vector3.forward * 0.75f;
                siz = Physics.RaycastNonAlloc(point, (point - point2).normalized, hits, (point - point2).magnitude, groundLayer);
                if (siz > 0)
                {
                    for (int i = 0; i < siz; i++)
                    {
                        if(Vector3.Angle(Vector3.up,hits[i].normal) > 45 && Vector3.Angle(Vector3.up, hits[i].normal) <= 91)
                        {
                            Vector3 closest = Vector3.zero;
                            Vector3 surfaceNormal = hits[i].normal;
                            surfaceNormal.y = 0;

                            Vector3 rayDir = -hits[i].normal;
                            rayDir.y = 0;
                            rayDir.Normalize();

                            RaycastHit[] tempHits = new RaycastHit[15];
                            int iSiz = Physics.RaycastNonAlloc(circPos, rayDir, tempHits, 0.75f, groundLayer);
                            if(iSiz == 0)
                            {
                                tempHits = new RaycastHit[15];
                                iSiz = Physics.RaycastNonAlloc(circPos, -rayDir, tempHits, 0.75f, groundLayer);
                            }

                            for(int o = 0; o < iSiz; o++)
                            {
                                if(hits[i].collider == tempHits[o].collider )
                                {
                                    closest = tempHits[o].point;
                                }
                            }

                            if(closest != Vector3.zero)
                            {
                                surfaceNormal = (surfaceNormal * 0.75f) - (circPos - closest);
                            }
                            else
                            {
                                surfaceNormal = Vector3.zero;
                            }

                            Vector3 pushCheck = surfaceNormal;

                            if (Mathf.Abs(pushCheck.x) > Mathf.Abs(pushBack.x))
                            {
                                pushBack.x = pushCheck.x;
                            }

                            if (Mathf.Abs(pushCheck.z) > Mathf.Abs(pushBack.z))
                            {
                                pushBack.z = pushCheck.z;
                            }
                        }
                    }
                }
            }

            pushBack.y = 0;

            movementSegment += pushBack;
        }

        //Check For Floors
        {
            hits = new RaycastHit[15];
            siz = Physics.RaycastNonAlloc(movementSegment + transform.position, Vector3.up, hits, 1f, groundLayer);
            SortHitinfoByHeight(hits, siz);
            if (siz > 0)
            {
                for (int i = 0; i < siz; i++)
                {
                    if (Vector3.Angle(Vector3.up, hits[i].normal) <= 45)
                    {
                        Vector3 deltaPos = transform.position + movementSegment;
                        movementSegment.y -= (deltaPos - hits[i].point).y - 0.001f;
                        grounded = true;
                        break;
                    }
                }
            }
            else
            {
                grounded = false;
            }

            if(!jumping)
            {
                hits = new RaycastHit[15];
                siz = Physics.RaycastNonAlloc(movementSegment + transform.position, Vector3.down, hits, 0.75f, groundLayer);
                SortHitinfoByHeight(hits, siz);
                if (siz > 0)
                {
                    for (int i = siz - 1; i >= 0; i--)
                    {
                        if (Vector3.Angle(Vector3.up, hits[i].normal) <= 45)
                        {
                            Vector3 deltaPos = transform.position + movementSegment;
                            movementSegment.y -= (deltaPos - hits[i].point).y - 0.001f;
                            grounded = true;
                            break;
                        }
                    }
                }
                else
                {
                    grounded = false;
                }
            }
        }

        //Check For Ceilings
        {
            hits = new RaycastHit[15];
            siz = Physics.RaycastNonAlloc(movementSegment + transform.position, Vector3.up, hits, 2f, groundLayer);
            SortHitinfoByHeight(hits, siz);
            if (siz > 0)
            {
                for (int i = 0; i < siz; i++)
                {
                    if (Vector3.Angle(Vector3.up, hits[i].normal) > 91)
                    {
                        movementSegment = Vector3.zero;
                        jumping = false;
                        break;
                    }
                }
            }

            hits = new RaycastHit[15];
            siz = Physics.RaycastNonAlloc(movementSegment + transform.position, Vector3.down, hits, 10f, groundLayer);
            SortHitinfoByHeight(hits, siz);
            if (siz > 0)
            {
                for (int i = siz - 1; i >= 0; i--)
                {
                    if (Vector3.Angle(Vector3.up, hits[i].normal) > 91)
                    {
                        movementSegment = Vector3.zero;
                        jumping = false;
                        break;
                    }

                    if (Vector3.Angle(Vector3.up, hits[i].normal) <= 45)
                    {
                        break;
                    }
                }
            }
        }
        
        transform.position += movementSegment;
        movementVector = Vector3.zero;
    }

    public void SortHitinfoByHeight(RaycastHit[] arr, int siz)
    {
        if(siz > arr.Length)
        {
            siz = arr.Length;
        }

        for(int count = 1; count < siz; count++)
        {
            int i = count;
            int iParent = (i + 1) / 2 - 1;

            while (i > 0)
            {
                if (iParent < 0)
                    break;
                if (arr[i].point.y < arr[iParent].point.y)
                    break;

                RaycastHit temp = arr[iParent];
                arr[iParent] = arr[i];
                arr[i] = temp;

                i = iParent;
            }
        }

        for (int count = siz - 1; count > 0; count--)
        {
            int i = 0;
            int iLeft = 2 * i + 1;
            int iRight = 2 * i + 2;

            RaycastHit temp = arr[count];
            arr[count] = arr[i];
            arr[i] = temp;

            while(i < count)
            {
                if (count <= iLeft)
                    break;
                if (count <= iRight || arr[iRight].point.y < arr[iLeft].point.y)
                {
                    if (arr[iLeft].point.y <= arr[i].point.y)
                        break;

                    temp = arr[iLeft];
                    arr[iLeft] = arr[i];
                    arr[i] = temp;

                    i = iLeft;
                }
                else
                {
                    if (arr[iRight].point.y <= arr[i].point.y)
                        break;

                    temp = arr[iRight];
                    arr[iRight] = arr[i];
                    arr[i] = temp;

                    i = iRight;
                }
            }
        }
    }

    public void LightAttack()
    {
        comboTimer = 0.5f;
        comboString = (comboString << 2) + 1;
    }

    public void HeavyAttack()
    {
        comboTimer = 0.5f;
        comboString = (comboString << 2) + 2;
    }

    public void MoveTowards(Vector3 direction)
    {
        direction = Quaternion.FromToRotation(direction, new Vector3(direction.x, 0, direction.z)) * direction;
        direction = Vector3.ClampMagnitude(direction, 1);
        if (grounded)
        {
            direction *= movementSpeed;
            direction.y = movementVector.y;
            movementVector = direction;
            int i = (int)Mathf.Ceil(Mathf.Clamp(movementVector.magnitude / (movementSpeed / 2f), 0, 2));
            animator.runtimeAnimatorController = animatorControllers[i];
            if (direction.magnitude != 0)
            {
                model.transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            }
        }
        else
        {
            direction *= 0.2f;
            Vector3 sum = Vector3.ClampMagnitude(direction + new Vector3(jumpVector.x, 0, jumpVector.z), movementSpeed);
            jumpVector.x = sum.x;
            jumpVector.z = sum.z;
            movementVector = jumpVector;
        }
    }

    public void Jump()
    {
        if(!jumping && grounded)
        {
            animator.runtimeAnimatorController = animatorControllers[3];
            jumping = true;
            jumpingTimer = 0.3f;
        }
    }
}
