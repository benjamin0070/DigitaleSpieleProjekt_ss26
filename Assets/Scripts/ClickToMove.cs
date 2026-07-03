using UnityEngine;
using UnityEngine.AI;

namespace Unity.AI.Navigation.Samples
{
    // Use physics raycast hit from mouse click to set agent destination
    [RequireComponent(typeof(NavMeshAgent))]
    public class ClickToMove : MonoBehaviour
    {
        private NavMeshAgent m_Agent;
        private RaycastHit m_HitInfo = new RaycastHit();
        private Animator m_Animator;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_Agent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
                    m_Agent.destination = m_HitInfo.point;
            }
            if (m_Agent.velocity.magnitude != 0f)
            {
                m_Animator.SetBool("Running", true);
            }
            else
            {
                m_Animator.SetBool("Running", false);
            }
        }

        // void OnAnimatorMove()
        // {
            // if (m_Animator.GetBool("Running"))
            // {
                // m_Agent.speed = (m_Animator.deltaPosition / Time.deltaTime).magnitude;
            // }
        // }
    }
}

