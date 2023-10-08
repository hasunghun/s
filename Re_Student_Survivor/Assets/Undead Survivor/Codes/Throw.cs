using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject projectilePrefab; // "구" 프리팹을 연결할 변수
    public float throwSpeed = 10f; // "구" 던지는 속도

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnAttack();
        }
    }

    void OnAttack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        if (closestEnemy != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            Vector2 direction = (closestEnemy.transform.position - transform.position).normalized;
            rb.velocity = direction * throwSpeed;

            // 추가: 적이 죽은 위치 가져오기
            Vector3 deathPosition = closestEnemy.GetComponent<Enemy>().GetDeathPosition();

            Goo projectileScript = projectile.AddComponent<Goo>();
            projectileScript.targetEnemy = closestEnemy;

            // 추가: 적이 죽은 위치 저장
            projectileScript.hitPosition = deathPosition;
        }
    }
}
