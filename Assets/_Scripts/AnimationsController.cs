using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsController : MonoBehaviour
{
    [SerializeField] private Animator handAnimator;
    [SerializeField] private Animator bodyAnimator;

    public bool ikActive = false;
    /*public Transform rightHandObj = null;
    public Transform leftHandObj = null;
    public Transform lookObj = null;*/

    private ItemManager item;
    private FpsController player;


    void Start() {
        item = GetComponentInParent<ItemManager>();
        player = GetComponentInParent<FpsController>();
    }

    private void Update() {
        if(!item.isLocalPlayer) {
            bodyAnimator.SetFloat("Horizontal", player.horizontal);
            bodyAnimator.SetFloat("Vertical", player.vertical);
            if (player.jumping) {
               bodyAnimator.SetTrigger("Jump");
            }
        }
    }

    //a callback for calculating IK
    void OnAnimatorIK() {
        if (handAnimator && item && item.isLocalPlayer) {
            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive) {
                // Set the right hand target position and rotation, if one has been assigned
                if (item.currentItem.rightHandPosition != null) {
                    handAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    handAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    handAnimator.SetIKPosition(AvatarIKGoal.RightHand, item.currentItem.rightHandPosition.position);
                    handAnimator.SetIKRotation(AvatarIKGoal.RightHand, item.currentItem.rightHandPosition.rotation);
                }

                if (item.currentItem.leftHandPosition != null) {
                    handAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    handAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    handAnimator.SetIKPosition(AvatarIKGoal.LeftHand, item.currentItem.leftHandPosition.position);
                    handAnimator.SetIKRotation(AvatarIKGoal.LeftHand, item.currentItem.leftHandPosition.rotation);
                }

            }

            //if the IK is not active, set the position and rotation of the hand and head back to the original position
            /*else {
                handAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                handAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                handAnimator.SetLookAtWeight(0);
                handAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                handAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                handAnimator.SetLookAtWeight(0);
            }*/
        }
    }
}
