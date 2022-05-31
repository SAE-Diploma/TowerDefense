using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{

    [SerializeField] protected float range;
    [SerializeField] protected int damage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float projectileSpeed;

    GameObject enemiesParent;
    List<GameObject> enemiesInRange;
    GameObject turret;
    Shoot turretShoot;

    [SerializeField] bool shot = false;

    protected virtual void Start()
    {
        enemiesParent = GameObject.Find("Enemies");
        if (enemiesParent == null)
        {
            Debug.LogError("Enemies parent not found!");
            return;
        }
        enemiesInRange = new List<GameObject>();
        turret = GetTurret();
        turretShoot = turret.GetComponent<Shoot>();
    }

    protected virtual void Update()
    {
        // make sure that the enemiesParent was found
        if (enemiesParent == null)
        {
            Debug.LogError("Enemies parent not found!");
            return;
        }

        // get the enemy to aim at
        GetEnemiesInRange(ref enemiesInRange);
        if (enemiesInRange.Count > 0)
        {
            GameObject enemy = GetProritizedEnemy(enemiesInRange);
            Debug.DrawLine(transform.position, enemy.transform.position, Color.red, Time.deltaTime);
            aimAtEnemy(enemy);
            if (!shot)
            {
                turretShoot.ShootAt(enemy, damage, projectileSpeed);
                shot = true;
                StartCoroutine(ShootCooldown(attackSpeed));
            }
        }
    }

    /// <summary>
    /// get all enemies that are in range.
    /// </summary>
    /// <param name="enemies">reference to the enemiesInRange list</param>
    protected void GetEnemiesInRange(ref List<GameObject> enemies)
    {
        enemies.Clear();
        foreach (Transform enemy in enemiesParent.transform)
        {
            Vector3 dir = new Vector3(enemy.position.x, 0, enemy.position.z) - new Vector3(transform.position.x, 0, transform.position.z);
            if (dir.sqrMagnitude < Mathf.Pow(range, 2))
            {
                enemies.Add(enemy.gameObject);
            }
        }
    }

    /// <summary>
    /// get the enemy GameObject to shoot at.
    /// </summary>
    /// <param name="enemiesInRange">all enemies in Range</param>
    /// <returns>Enemy to shoot at</returns>
    protected GameObject GetProritizedEnemy(List<GameObject> enemiesInRange)
    {
        // ToDoo:
        // Consider aming on the enemy that has the highes progression score
        return enemiesInRange[0];
    }

    /// <summary>
    /// get the child with the Name "Turret"
    /// </summary>
    /// <returns>the GameObject of the Turret or null if not found</returns>
    public GameObject GetTurret()
    {
        foreach (Transform child in transform)
        {
            if (child.name == "Turret")
            {
                return child.gameObject;
            }
        }
        Debug.LogError("Turret not found!");
        return null;
    }

    public void aimAtEnemy(GameObject enemy)
    {
        turret.transform.LookAt(enemy.transform);
    }

    private IEnumerator ShootCooldown(float attackSpeed)
    {
        yield return new WaitForSeconds(attackSpeed);
        shot = false;
    }
}
