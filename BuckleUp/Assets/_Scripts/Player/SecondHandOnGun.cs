
using UnityEngine;
using System.Collections;
using RootMotion.FinalIK;

public class SecondHandOnGun : MonoBehaviour
{

    // References to the IK components
    public AimIK aim;
    public FullBodyBipedIK ik;
    public LookAtIK look;

    // Just quick shortcuts to the hand effectors for better readability
    private IKEffector leftHand { get { return ik.solver.leftHandEffector; } }
    private IKEffector rightHand { get { return ik.solver.rightHandEffector; } }

    private Quaternion leftHandRotationRelative;

    private Vector3 toLeftHandRelative;

    void Start()
    {
        // Disabling (and initiating) the IK components
        aim.enabled = false;
        ik.enabled = false;
        //look.enabled = false;

        ik.solver.OnPostUpdate += OnPostFBBIK; // Add to the OnPostUpdate delegate of the FBBIK solver

        // Find out how the left hand is positioned relative to the right hand rotation
        toLeftHandRelative = rightHand.bone.InverseTransformPoint(leftHand.bone.position);

        // Rotation of the left hand relative to the rotation of the right hand
        leftHandRotationRelative = Quaternion.Inverse(rightHand.bone.rotation) * leftHand.bone.rotation;
    }

    void LateUpdate()
    {
        //// Find out how the left hand is positioned relative to the right hand rotation
        //toLeftHandRelative = rightHand.bone.InverseTransformPoint(leftHand.bone.position);

        //// Rotation of the left hand relative to the rotation of the right hand
        //leftHandRotationRelative = Quaternion.Inverse(rightHand.bone.rotation) * leftHand.bone.rotation;

        // Match AimIK target with the LookAtIK target
        aim.solver.IKPosition = look.solver.IKPosition;

        // Update Aim IK. This will change the position and the rotation of the right hand that holds the gun, so we will need pin the left hand back to the gun handle
        aim.solver.Update();

        // Position the left hand on the gun
        leftHand.position = rightHand.bone.TransformPoint(toLeftHandRelative);
        leftHand.positionWeight = 1f;

        // Making sure the right hand won't budge during solving
        rightHand.position = rightHand.bone.position;
        rightHand.positionWeight = 1f;
        ik.solver.GetLimbMapping(FullBodyBipedChain.RightArm).maintainRotationWeight = 1f;

        // Update FBBIK
        ik.solver.Update();

        // Rotate the head
        look.solver.Update();
    }

    // Rotate the left hand after FBBIK has finished, called by the FBBIK solver
    private void OnPostFBBIK()
    {
        leftHand.bone.rotation = rightHand.bone.rotation * leftHandRotationRelative;
    }

}
