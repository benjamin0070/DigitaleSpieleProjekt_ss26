using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
   // Rigidbody of the player.
   private Rigidbody rb; 

   // Movement along X and Y axes.
   private float movementX;
   private float movementY;

   // Speed at which the player moves.
   public float speed = 1;
   public float forwardSpeed = 10;
   public float backwardSpeed = 0;
   public float sidewaysSpeed = 5;

   public float controlLimitX = -25;

   private Transform cameraTransform;

   public PinParent pinParent;

   public TextMeshProUGUI scoreText;
   public int score = 0;

   // Start is called before the first frame update.
   void Start()
   {
      // Get and store the Rigidbody component attached to the player.
      rb = GetComponent<Rigidbody>();
      cameraTransform = Camera.main.transform;
      SetScoreText();
   }
 
   // This function is called when a move input is detected.
   void OnMove(InputValue movementValue)
    {
      // Convert the input value into a Vector2 for movement.
        Vector2 movementVector = movementValue.Get<Vector2>();

      // Store the X and Y components of the movement.
        movementX = movementVector.x; 
        movementY = movementVector.y; 
    }

   // FixedUpdate is called once per fixed frame-rate frame.
   void FixedUpdate() 
   {
      Vector3 camForward = cameraTransform.forward;
      Vector3 camRight = cameraTransform.right;

      camForward.y = 0f;
      camRight.y = 0f;
      camForward.Normalize();
      camRight.Normalize();

      if (rb.position.x > controlLimitX)
      {
         //select the right speed for the inputs
         float currentForwardSpeed = movementY > 0f ? forwardSpeed : backwardSpeed;

         Vector3 forwardMovement = camForward * movementY * currentForwardSpeed;
         Vector3 sideMovement = camRight * movementX * sidewaysSpeed;

         Vector3 movement = forwardMovement + sideMovement;

         // Apply force to the Rigidbody to move the player.
         rb.AddForce(movement * speed);
      } else
      {
         score = pinParent.FallenPinsCount();
         SetScoreText();
      }
   }

   void SetScoreText() 
   {
      scoreText.text =  "Score: " + score.ToString();
   }
}
