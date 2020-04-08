using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class BotAI : MonoBehaviour {
    private Target self;
    [SerializeField]
    private NavMeshAgent agent;
    private bool canShoot = true;
    [SerializeField]
    private List<Target> targets = new List<Target>();

    public Transform target;
    public GameObject gun;
    public Transform eyes;
    public Transform barrel;
    public LayerMask targetLayers;
    public float rotationSpeed = 15f;
    public float reloadTime = 1.3f;

    private void Start() {
        self = GetComponent<Target>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        UpdateTargets();
        if (target == null && targets.Count == 0) agent.isStopped = true;
        if (target == null) target = GetNearestTarget();
        LockAtTarget();
        FollowTarget();
        ResetPath();
        if(canShoot) Shoot();

        //Debug
        //var direction = (target.position - transform.position).normalized;
        //var shootVector = Vector3.Slerp(transform.forward, direction, 0.08f);
        //Debug.DrawRay(eyes.position, shootVector * 100f, Color.green);
        //Debug.DrawRay(eyes.position, transform.forward * 100f, Color.yellow);
    }

    private void LockAtTarget() {
        if (target == null) return;
        var direction = (target.position - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0,transform.localEulerAngles.y, 0);
    }

    private void UpdateTargets(){
        targets = FindObjectsOfType<Target>().ToList();
        if (targets.Contains(self))
            targets.Remove(self);
    }

    private void ResetPath() {
        if (agent.isStopped && target == null) return;
        if (agent.isStopped) agent.ResetPath();
    }

    private void FollowTarget() {
        if(target != null)
        agent.SetDestination(target.position);
    }

    private void Shoot() {
        if (target == null) return;
        var direction = (target.position - transform.position).normalized;
        var shootVector = Vector3.Slerp(transform.forward, direction, 0.7f);
        var ray = new Ray(eyes.position, shootVector);
        RaycastHit hit;
        var muzzleflash = ObjectPooler.SharedInstance.GetPooledObject(1);
        if (muzzleflash != null) {
            muzzleflash.transform.position = barrel.position;
            muzzleflash.SetActive(true);
        }
        LeanTween.rotateAroundLocal(gun, Vector3.right, 360f, reloadTime - 0.15f).setEase(LeanTweenType.easeSpring);
        StartCoroutine(Reload());
        if(Physics.Raycast(ray, out hit, 100f, targetLayers)) {
            if (hit.collider != null) {
                if (hit.collider.CompareTag("Player")) {
                    hit.collider.GetComponent<Health>().Kill();
                    target = null;
                    var particles = ObjectPooler.SharedInstance.GetPooledObject(2);
                    particles.transform.position = hit.point;
                    particles.SetActive(true);
                } else if (hit.collider.CompareTag("Bot")) {
                    var bot = hit.collider.GetComponent<BotAI>();
                    bot.Kill();
                    target = null;
                    var particles = ObjectPooler.SharedInstance.GetPooledObject(2);
                    particles.transform.position = hit.point;
                    particles.SetActive(true);
                } else {
                    var particles = ObjectPooler.SharedInstance.GetPooledObject(0);
                    particles.transform.position = hit.point;
                    particles.SetActive(true);
                }
            }
        }
    }

    private Transform GetNearestTarget() {
        var position = transform.position;
        var result = targets.OrderBy(o => (o.transform.position - position).sqrMagnitude).FirstOrDefault().transform;
        if (result == null) Debug.LogWarning("Couldn't find target");
        return result;
    }

    public void Kill() {
        Destroy(gameObject);
        GameManager.instance.botSpawner.StartCoroutine("Respawn");
    }

    private IEnumerator Reload() {
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        canShoot = true;
    }

    private void OnCollisionEnter(Collision collision) {
        var otherBot = collision.gameObject.GetComponent<BotAI>();
        if (otherBot != null) DestroyIfStuck(otherBot);
    }

    private void OnCollisionStay(Collision collision) {
        var otherBot = collision.gameObject.GetComponent<BotAI>();
        if (otherBot != null) DestroyIfStuck(otherBot);
    }

    private void DestroyIfStuck(BotAI target) {
        target.Kill();
    }

}
