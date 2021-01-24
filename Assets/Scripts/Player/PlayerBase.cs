using StateMachine;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Primary class for the character controller
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerBase : MonoBehaviour
{
    //Might replace with SOs
    [SerializeField]
    private float dashCooldownMin = 0;
    [SerializeField]
    private float dashCooldownMax = 0;
    [SerializeField]
    private float dashSpeedMin = 0;
    [SerializeField]
    private float dashSpeedMax = 0;
    [SerializeField]
    private float dashTime = 0;

    private bool isChargingDash = false;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashAmount = 0;
    private float dashStrength = 0;
    private float dashTimer = 0;
    private readonly float activationValue = 0.15f;

    //Splat Variables
    private Vector4 channelMask = new Vector4(1, 0, 0, 0);

    private int splatsX = 1;
    private int splatsY = 1;
    [SerializeField]
    private float splatScale = 1.0f;
    [SerializeField]
    private MeshRenderer playerMeshRend;
    [SerializeField]
    private Material[] colourMaterials;


    public Rigidbody Rigid { get; private set; }
    public PlayerInput PlayerInput { get; private set; }
    public StateMachine<PlayerBase> StateMachine { get; private set; }
    public Vector2 MovementValue { get; private set; }

    private void Awake()
    {
        Rigid = GetComponent<Rigidbody>();
        PlayerInput = GetComponent<PlayerInput>();
        StateMachine = new StateMachine<PlayerBase>(this);
        playerMeshRend.material = colourMaterials[0];
    }

    private void Update()
    {
        StateMachine.Update();
        DashCharging();
        Dash();
        ColourChanger();
    }

    private void ColourChanger()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //orange/yellow
        {
            channelMask = new Vector4(1, 0, 0, 0);
            playerMeshRend.material = colourMaterials[0];
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) //red
        {
            channelMask = new Vector4(0, 1, 0, 0);
            playerMeshRend.material = colourMaterials[1];
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) //green
        {
            channelMask = new Vector4(0, 0, 1, 0);
            playerMeshRend.material = colourMaterials[2];
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) //blue
        {
            channelMask = new Vector4(0, 0, 0, 1);
            playerMeshRend.material = colourMaterials[3];
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Splat();
        }
    }


    private void OnMove(InputValue inputValue)
    {
        StateMachine.ChangeState(MoveState.Instance);
        MovementValue = inputValue.Get<Vector2>();
    }

    private void OnDash(InputValue inputValue)
    {
        float value = inputValue.Get<float>();

        if (value > activationValue && canDash)
            isChargingDash = true;
        else
        {
            if (canDash)
            {
                dashStrength.CalculateFromPercentage(dashSpeedMin, dashSpeedMax, dashAmount);
                StartCoroutine(DashCooldown());
                dashAmount = 0;
                dashTimer = dashTime;
                isChargingDash = false;
                isDashing = true;
            }
        }
    }

    private void OnPower()
    {

    }

    private void OnPause()
    {

    }

    private void DashCharging()
    {
        if (isChargingDash && dashAmount < 1)
        {
            if (dashAmount < 1)
                dashAmount += Time.deltaTime;
            else
                dashAmount = 1;
        }
    }

    private void Dash()
    {
        if (isDashing)
        {
            if (dashTimer > 0)
            {
                dashTimer -= Time.deltaTime;
                Rigid.MovePosition(transform.position + transform.forward * dashStrength * Time.fixedDeltaTime);
            }
            else
            {
                isDashing = false;
                dashTimer = dashTime;
            }

        }
    }

    private void Splat()
    {
        //Get how many splats are in the splat atlas
        splatsX = SplatManagerSystem.instance.splatsX;
        splatsY = SplatManagerSystem.instance.splatsY;

        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out RaycastHit hit))//, scoreLayer))
        {
            if (hit.collider.CompareTag("Paintable"))
            {
                Vector3 leftVec = Vector3.Cross(hit.normal, Vector3.up);
                float randScale = Random.Range(0.5f, 1.5f);

                GameObject newSplatObject = new GameObject();
                newSplatObject.transform.position = hit.point;
                if (leftVec.magnitude > 0.001f)
                {
                    newSplatObject.transform.rotation = Quaternion.LookRotation(leftVec, hit.normal);
                }
                newSplatObject.transform.RotateAround(hit.point, hit.normal, Random.Range(-180, 180));
                newSplatObject.transform.localScale = new Vector3(randScale, randScale * 0.5f, randScale) * splatScale;

                Splat newSplat;
                newSplat.splatMatrix = newSplatObject.transform.worldToLocalMatrix;
                newSplat.channelMask = channelMask;

                float splatscaleX = 1.0f / splatsX;
                float splatscaleY = 1.0f / splatsY;
                float splatsBiasX = Mathf.Floor(Random.Range(0, splatsX * 0.99f)) / splatsX;
                float splatsBiasY = Mathf.Floor(Random.Range(0, splatsY * 0.99f)) / splatsY;

                newSplat.scaleBias = new Vector4(splatscaleX, splatscaleY, splatsBiasX, splatsBiasY);

                SplatManagerSystem.instance.AddSplat(newSplat);

                GameObject.Destroy(newSplatObject);
            }
        }
    }

    private IEnumerator DashCooldown()
    {
        canDash = false;
        float cooldown = 0;
        cooldown.CalculateFromPercentage(dashCooldownMin, dashCooldownMax, dashAmount);
        yield return new WaitForSeconds(cooldown);
        canDash = true;
    }
}
