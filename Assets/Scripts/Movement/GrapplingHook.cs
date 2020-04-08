using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;

public class GrapplingHook : MonoBehaviour {
    [Header("Grappling")]
    public Rope grapplingRope;
    public CharacterController controller;
    public Transform grappleTip;
    public Transform grappleHolder;
    public LayerMask whatToGrapple;
    public float maxDistance;
    public float minDistance;
    public float rotationSmooth;

    [Header("Raycasts")]
    public float raycastRadius;
    public int raycastCount;

    [Header("Physics")]
    public float pullForce;
    public float pushForce;
    public float yMultiplier;
    public float minPhysicsDistance;
    public float maxPhysicsDistance;

    private Vector3 _hit;
    private Camera playerCamera;

    private void Awake() {
        playerCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0) && grapplingRope.Grappling) {
            Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAA");
            grappleHolder.rotation = Quaternion.Lerp(grappleHolder.rotation, Quaternion.LookRotation(-(grappleHolder.position - _hit)), rotationSmooth * Time.fixedDeltaTime);

            var distance = Vector3.Distance(controller.transform.position, _hit);
            if (!(distance >= minPhysicsDistance) || !(distance <= maxPhysicsDistance)) return;
            controller.Move(controller.velocity + pullForce * Time.fixedDeltaTime * yMultiplier * Mathf.Abs(_hit.y - controller.transform.position.y) * (_hit - controller.transform.position).normalized);
            controller.Move(controller.velocity + pushForce * Time.fixedDeltaTime * controller.transform.forward);
        }
        else {
            grappleHolder.localRotation = Quaternion.Lerp(grappleHolder.localRotation, Quaternion.Euler(0, 0, 0), rotationSmooth * Time.fixedDeltaTime);
        }
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0) && RaycastAll(out var hitInfo)) {
            Debug.Log("Start grappling??!?!?!");
            grapplingRope.Grapple(grappleTip.position, hitInfo.point);
            _hit = hitInfo.point;
            grapplingRope.UpdateStart(grappleTip.position);
        }

        if (Input.GetMouseButtonUp(0)) {
            grapplingRope.UnGrapple();
            Debug.Log("STOP grappling??!?!?!");
        }

        grapplingRope.UpdateGrapple();
    }

    private bool RaycastAll(out RaycastHit hit)
    {
        var divided = raycastRadius / 2f;
        var possible = new List<RaycastHit>(raycastCount * raycastCount);
        var cam = playerCamera.transform;

        for (var x = 0; x < raycastCount; x++) {
            for (var y = 0; y < raycastCount; y++) {
                var pos = new Vector2(
                    Mathf.Lerp(-divided, divided, x / (float)(raycastCount - 1)),
                    Mathf.Lerp(-divided, divided, y / (float)(raycastCount - 1))
                );

                if (!Physics.Raycast(cam.position + cam.right * pos.x + cam.up * pos.y, cam.forward, out var hitInfo, maxDistance)) continue;

                var distance = Vector3.Distance(cam.position, hitInfo.point);
                if (hitInfo.transform.gameObject.layer != whatToGrapple) continue;
                if (distance < minDistance) continue;
                if (distance > maxDistance) continue;

                possible.Add(hitInfo);
            }
        }

        var arr = possible.ToArray();
        possible.Clear();

        if (arr.Length > 0) {
            var closest = new RaycastHit();
            var distance = 0f;
            var set = false;

            foreach (var hitInfo in arr) {
                var hitDistance = DistanceFromCenter(hitInfo.point);

                if (!set) {
                    set = true;
                    distance = hitDistance;
                    closest = hitInfo;
                }
                else if (hitDistance < distance) {
                    distance = hitDistance;
                    closest = hitInfo;
                }
            }

            hit = closest;
            return true;
        }

        hit = new RaycastHit();
        return false;
    }

    private float DistanceFromCenter(Vector3 point) {
        return Vector2.Distance(playerCamera.WorldToViewportPoint(point),
            new Vector2(0.5f, 0.5f));
    }
}

