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


    public Rigidbody Rigid { get; private set; }
    public PlayerInput Input { get; private set; }
    public StateMachine<PlayerBase> StateMachine { get; private set; }
    public Vector2 MovementValue { get; private set; }

    private void Awake()
    {
        Rigid = GetComponent<Rigidbody>();
        Input = GetComponent<PlayerInput>();
        StateMachine = new StateMachine<PlayerBase>(this);
    }

    private void Update()
    {
        StateMachine.Update();
        DashCharging();
        Dash();
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
        Ray ray = new Ray(transform.position, -transform.up);

        if (Physics.Raycast(ray, out RaycastHit hit))//, scoreLayer))
        {
            if (hit.collider.CompareTag("Paintable"))
            {


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
