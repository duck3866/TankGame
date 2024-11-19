using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField, Min(3)] private int lineSegments = 60;
    [SerializeField, Min(1)] private float timeOfTheFlight = 5;
    
    [SerializeField] Vector2 boxSize = new Vector2(0.8f, 0.8f); // 너비와 높이를 적절하게 설정
    [SerializeField] float boxAngle = 0f; // 박스의 회전 각도, 필요시 변경 가능
    
    public void ShowTrajectoryLine(Vector3 startPoint, Vector3 startVelocity)
    {
        float timeStep = timeOfTheFlight / lineSegments;

        Vector3[] lineRendererPoints = CalculateTrajectoryLine(startPoint, startVelocity, timeStep);

        lineRenderer.positionCount = lineSegments;
        lineRenderer.SetPositions(lineRendererPoints);
    }

    private Vector3[] CalculateTrajectoryLine(Vector3 startPoint, Vector3 startVelocity, float timeStep)
    {
        Vector3[] lineRendererPoints = new Vector3[lineSegments];
        lineRendererPoints[0] = startPoint;
        
        for (int i = 1; i < lineSegments; i++)
        {
            Vector3 progressBeforeGravity = startVelocity * timeStep;
            Vector3 gravityOffset = Vector3.up * -0.5f * Physics.gravity.y * timeStep * timeStep;
            Vector3 nextPosition = startPoint + progressBeforeGravity - gravityOffset;
            //----------------
            Vector3 direction = (nextPosition - startPoint).normalized;
            float distance = Vector3.Distance(startPoint, nextPosition);
        
            RaycastHit raycastHit;
            if (Physics.Raycast(startPoint, direction, out raycastHit, distance))
            {
                if (raycastHit.collider.CompareTag("Block"))
                {
                    lineRendererPoints[i] = raycastHit.point;
                    for (int j = i + 1; j < lineSegments; j++)
                    {
                        lineRendererPoints[j] = raycastHit.point;
                    }
                    break;
                }
            }

            lineRendererPoints[i] = nextPosition;
            startPoint = nextPosition;
            startVelocity += Physics.gravity * timeStep;
        }

        return lineRendererPoints;
    }
    
}
