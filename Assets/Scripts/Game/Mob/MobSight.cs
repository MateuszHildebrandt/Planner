using UnityEngine;
using UnityEngine.AI;

namespace Mob
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class MobSight : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float distance = 7;
        [Header("Debug")]
        [SerializeField] bool playerObserved;

        private CircleCollider2D detectionCircle;
        private MobController mobController;

        internal bool GetPlayerObserved() => playerObserved;

        private void Awake()
        {
            detectionCircle = GetComponent<CircleCollider2D>();
            mobController = GetComponentInParent<MobController>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (mobController.CompareWithTags(collision.gameObject.tag))
            {
                playerObserved = false;
                Vector3 direction = collision.transform.position - transform.position;

                RaycastHit2D[] raycastHit;
                raycastHit = Physics2D.RaycastAll(transform.position, direction.normalized, distance);
                //Debug.DrawRay(transform.position, direction.normalized, Color.red);

                foreach (var item in raycastHit)
                {
                    if (item.collider != null)
                    {
                        if (mobController.CompareWithTags(item.collider.tag))
                        {
                            if (CalculatePathLength(item.transform.position) <= detectionCircle.radius)
                            {
                                playerObserved = true;
                                mobController.target = item.transform;
                            }
                        }
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (mobController.CompareWithTags(collision.tag))
                playerObserved = false;
        }

        private float CalculatePathLength(Vector3 targetPosition)
        {
            NavMeshPath navMeshPath = new NavMeshPath();

            if (mobController.Agent.enabled)
                mobController.Agent.CalculatePath(targetPosition, navMeshPath);

            Vector3[] allWayPoints = new Vector3[navMeshPath.corners.Length + 2];
            allWayPoints[0] = transform.position;
            allWayPoints[allWayPoints.Length - 1] = targetPosition;

            for (int i = 0; i < navMeshPath.corners.Length; i++)
                allWayPoints[i + 1] = navMeshPath.corners[i];

            float pathLength = 0f;
            for (int i = 0; i < allWayPoints.Length - 1; i++)
                pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);

            return pathLength;
        }
    }
}
