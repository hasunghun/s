using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    public GameObject projectilePrefab; // "��" �������� ������ ����
    public float throwSpeed = 10f; // "��" ������ �ӵ�

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

            // �߰�: ���� ���� ��ġ ��������
            Vector3 deathPosition = closestEnemy.GetComponent<Enemy>().GetDeathPosition();

            Goo projectileScript = projectile.AddComponent<Goo>();
            projectileScript.targetEnemy = closestEnemy;

            // �߰�: ���� ���� ��ġ ����
            projectileScript.hitPosition = deathPosition;
        }
    }
}
