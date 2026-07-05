using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isWalkingLeftHash;
    int isWalkingRightHash;
    int isWalkingBackwardsHash;
    int isTurningRightHash;
    int isTurningLeftHash;
    int isRunningHash;
    int isJumpingHash;
    int isGrabbingHash;
    int isWavingHash;
    int isSplittingHash;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isWalkingLeftHash = Animator.StringToHash("isWalkingLeft");
        isWalkingRightHash = Animator.StringToHash("isWalkingRight");
        isWalkingBackwardsHash = Animator.StringToHash("isWalkingBack");
        isTurningRightHash = Animator.StringToHash("isTurningRight");
        isTurningLeftHash = Animator.StringToHash("isTurningLeft");
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");
        isGrabbingHash = Animator.StringToHash("isGrabbingBook");
        isWavingHash = Animator.StringToHash("isWaving");
        isSplittingHash = Animator.StringToHash("isSplitting");
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isWalkingLeft = animator.GetBool(isWalkingLeftHash);
        bool isWalkingRight = animator.GetBool(isWalkingRightHash);
        bool isWalkingBackwards = animator.GetBool(isWalkingBackwardsHash);
        bool isTurningRight = animator.GetBool(isTurningRightHash);
        bool isTurningLeft = animator.GetBool(isTurningLeftHash);
        bool isJumping = animator.GetBool(isJumpingHash);
        bool isGrabbing = animator.GetBool(isGrabbingHash);
        bool isWaving = animator.GetBool(isWavingHash);
        bool isSplitting = animator.GetBool(isSplittingHash);
        bool forward = Input.GetKey("w");
        bool left = Input.GetKey("a");
        bool right = Input.GetKey("d");
        bool backwards = Input.GetKey("s");
        bool turningRight = Input.GetKey("e");
        bool turningLeft = Input.GetKey("q");
        bool running = Input.GetKey("left shift");
        bool jump = Input.GetKeyDown("space");
        bool grab = Input.GetKeyDown("mouse 0");
        bool wave = Input.GetKeyDown("f");
        bool split = Input.GetKeyDown("r");

        if (!isWalking && forward)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (isWalking && !forward)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (left && !isWalkingLeft)
        {
            animator.SetBool(isWalkingLeftHash, true);
        }

        if (isWalkingLeft && !left)
        {
            animator.SetBool(isWalkingLeftHash, false);
        }

        if (right && !isWalkingRight)
        {
            animator.SetBool(isWalkingRightHash, true);
        }

        if (isWalkingRight && !right)
        {
            animator.SetBool(isWalkingRightHash, false);
        }

        if (backwards && !isWalkingBackwards)
        {
            animator.SetBool(isWalkingBackwardsHash, true);
        }

        if (turningRight && !isTurningRight)
        {
            animator.SetBool(isTurningRightHash, true);
        }

        if (isTurningRight && !turningRight)
        {
            animator.SetBool(isTurningRightHash, false);
        }

        if (turningLeft && !isTurningLeft)
        {
            animator.SetBool(isTurningLeftHash, true);
        }

        if (isTurningLeft && !turningLeft)
        {
            animator.SetBool(isTurningLeftHash, false);
        }

        if (isWalkingBackwards && !backwards)
        {
            animator.SetBool(isWalkingBackwardsHash, false);
        }
        
        if (!isRunning && (running && forward))
        {
            animator.SetBool(isRunningHash, true);
        }
        
        if (isRunning && !running || !forward)
        {
            animator.SetBool(isRunningHash, false);
        }
        if (jump)
        {
            animator.SetBool(isJumpingHash, true);
        } else if (isJumping)
        {
            animator.SetBool(isJumpingHash, false);
        }

        if (grab)
        {
            animator.SetBool(isGrabbingHash, true);
        } else if (isGrabbing)
        {
            animator.SetBool(isGrabbingHash, false);
        }

        if (wave)
        {
            animator.SetBool(isWavingHash, true);
        } else if (isWaving)
        {
            animator.SetBool(isWavingHash, false);
        }
        
        if (split)
        {
            animator.SetBool(isSplittingHash, true);
        } else if (isSplitting)
        {
            animator.SetBool(isSplittingHash, false);
        }
    }
}
