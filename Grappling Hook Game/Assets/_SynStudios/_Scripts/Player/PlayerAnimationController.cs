using System.Collections;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator playerAnimator;

    bool playerJumpedRecently;

    int inputXHash,
        inputYHash;


    void Awake()
    {
        playerAnimator = transform.Find("playerModel").GetComponent<Animator>();

        inputXHash = Animator.StringToHash("InputX");
        inputYHash = Animator.StringToHash("InputY");
    }

    void OnEnable()
    {
        PlayerInput.OnJump += Jump;
        PlayerInput.OnStartGrapple += StartGrapple;
    }

    void OnDisable()
    {
        PlayerInput.OnJump -= Jump;
        PlayerInput.OnStartGrapple -= StartGrapple;
    }

    void StartGrapple()
    {
        if (playerJumpedRecently) return;
        Jump();
    }

    void Jump()
    {
        if (!PlayerController.Grounded) return;

        if (GameManager.Instance.currentState != GameManager.GameState.Playing)
            return;

        PlayJumpAnimation();
    }

    void PlayJumpAnimation() => StartCoroutine(nameof(JumpedRecently));

    IEnumerator JumpedRecently()
    {
        playerAnimator.SetTrigger("jumpTrigger");
        yield return new WaitForSeconds(0.3f);
        Debug.Log("jumpedRecently");
        playerJumpedRecently = true;
    }

    void PlayLandingEffect() => StartCoroutine(nameof(LandingEffect));

    IEnumerator LandingEffect()
    {
        yield return new WaitForSeconds(0.2f);
        Instantiate(GameAssets.Instance.pf_LandingEffect, transform.position + Vector3.down, Quaternion.Euler(-90, 0, 0));
    }

    void Update()
    {
        if (playerJumpedRecently && PlayerController.Grounded)
        {
            playerAnimator.SetTrigger("landingTrigger");
            SoundManager.Instance.PlaySound(SoundManager.Sound.Landing);            
            PlayLandingEffect();
            Debug.Log("Landing!");
            playerJumpedRecently = false;
        }

        float inputValueX = PlayerInput.InputDirection.x;
        float inputValueY = PlayerInput.InputDirection.y;

        if (PlayerInput.IsRunning)
        {
            inputValueX = 2f * PlayerInput.InputDirection.x;
            inputValueY = 2f * PlayerInput.InputDirection.y;
        }

        playerAnimator.SetFloat(inputXHash, inputValueX);
        playerAnimator.SetFloat(inputYHash, inputValueY);
    }
}
