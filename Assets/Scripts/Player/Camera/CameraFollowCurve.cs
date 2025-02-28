using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollowCurve : MonoBehaviour
{
    [SerializeField] private Transform forwardPoint;
    [SerializeField] private Transform neutralPoint;
    [SerializeField] private Transform backwardPoint;

    private const float MaxDownwardRotation = 80f;
    private const float MaxUpwardRotation = -80f;

    [SerializeField] private SharedFloat cameraXRotation;

    private void LateUpdate()
    {
        FollowSpine();
    }

    private void FollowSpine()
    {
        float i = Mathf.InverseLerp(0, MaxDownwardRotation, cameraXRotation.Value);
        Vector3 center = (neutralPoint.position + forwardPoint.position) * 0.5f;

        float arcShape = 0.35f;
        center -= new Vector3(0, arcShape, 0);
        var neckNeutralRelCenter = neutralPoint.position - center;
        var neckForwardRelCenter = forwardPoint.position - center;
        
        transform.position = Vector3.Slerp(neckNeutralRelCenter, neckForwardRelCenter, i);
        transform.position += center;
        
        
        if (i == 0) //if looking down factor is 0
        {
            float y = Mathf.InverseLerp(0, MaxUpwardRotation, cameraXRotation.Value);
            
            Vector3 center2 = (neutralPoint.position + backwardPoint.position) * 0.5f;
            center2 -= new Vector3(0, arcShape, 0);

            var neckNeutralRelCenter2 = neutralPoint.position - center2;
            var neckBackwardRelCenter = backwardPoint.position - center2;
            
            transform.position = Vector3.Slerp(neckNeutralRelCenter2, neckBackwardRelCenter, y);
            transform.position += center2;
        }
    }
}
