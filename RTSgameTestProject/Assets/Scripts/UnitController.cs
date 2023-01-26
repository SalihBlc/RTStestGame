using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitController : MonoBehaviour
{

    private UnityEngine.AI.NavMeshAgent navAgent;
    private Transform currentTarget;
    private float attackTimer;

    public UnitStats unitStats;
    public GameObject healthBar;
    public TextMeshProUGUI healthText;
    public GameObject bulletPrefab;
    public Transform firePoint;

    private float MaxHealth;
    private float CurrentHealth;

    private void Start()
    {
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navAgent.speed = unitStats.movementSpeed;
        navAgent.angularSpeed = unitStats.movementSpeed * 10f;

        attackTimer = unitStats.attackSpeed;

        MaxHealth = unitStats.health;
        CurrentHealth = MaxHealth;
        healthText.SetText($"{CurrentHealth}");
        healthBar.transform.localScale = new Vector3(((CurrentHealth / MaxHealth) * 100) / 100f, 1, 1);
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
        if (this.tag == "EnemyUnit")
        {
            var players = GameObject.FindGameObjectsWithTag("PlayerUnit");
            var playerTransforms = new List<Transform>();
            foreach (var player in players)
            {
                playerTransforms.Add(player.transform);
            }
            SetNewTarget(GetClosestPlayer(playerTransforms.ToArray()));

            var distance = (transform.position - currentTarget.position).magnitude;

            if (currentTarget.gameObject.activeSelf & distance <= unitStats.attackRange)
            {
                this.transform.LookAt(currentTarget);
                Attack();
            }
        }
        if (currentTarget != null && this.tag == "PlayerUnit")
        {
            navAgent.destination = currentTarget.position;

            var distance = (transform.position - currentTarget.position).magnitude;

            if (distance <= unitStats.attackRange)
            {
                this.transform.LookAt(currentTarget);
                Attack();
                navAgent.isStopped = true;
            }
        }
    }

    Transform GetClosestPlayer(Transform[] players)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform t in players)
        {
            float dist = Vector3.Distance(t.position, currentPos);
            if (dist < minDist)
            {
                tMin = t;
                minDist = dist;
            }
        }
        return tMin;
    }

    public void MoveUnit(Vector3 dest)
    {
        if (navAgent.isStopped)
            navAgent.isStopped = !navAgent.isStopped;
        currentTarget = null;
        navAgent.destination = dest;
    }

    public void SetSelected(bool isSelected)
    {
        transform.Find("Highlight").gameObject.SetActive(isSelected);
    }

    public void SetNewTarget(Transform enemy)
    {
        currentTarget = enemy;
    }

    public void Attack()
    {
        if (currentTarget.gameObject.activeSelf & attackTimer >= unitStats.attackSpeed)
        {
            GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();

            if (bullet != null)
                bullet.Seek(this, currentTarget.GetComponent<UnitController>());
            //RTSGameManager.UnitTakeDamage(this, currentTarget.GetComponent<UnitController>());
            attackTimer = 0;
        }

    }

    public void TakeDamage(UnitController enemy, float damage)
    {
        CurrentHealth -= damage;
        healthText.SetText($"{CurrentHealth}");
        healthBar.transform.localScale = new Vector3(((CurrentHealth / MaxHealth) * 100) / 100f, 1, 1);
        if (CurrentHealth <= 0)
            this.gameObject.SetActive(false);
    }
}
