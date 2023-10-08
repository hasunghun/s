using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goo : MonoBehaviour
{
    public GameObject targetEnemy; // �� ������Ʈ
    public int blackHolePrefabIndex = 4; // ��Ȧ ������ �ε���
    public float blackHoleDuration = 5f; // ��Ȧ ���� �ð�
    public float blackHoleForce = 20f; // ��Ȧ�� ������� ��

    // �߰�: ������ �¾��� �� ��ġ ����
    public Vector3 hitPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == targetEnemy)
        {
            // ��Ȧ�� ������ Ȱ��ȭ
            GameObject blackHole = PoolManager.Instance.Get(blackHolePrefabIndex);
            blackHole.transform.position = transform.position;
            blackHole.SetActive(true);

            // Point Effector 2D ������Ʈ ����
            PointEffector2D effector = blackHole.GetComponent<PointEffector2D>();

            if (effector != null)
            {
                effector.forceMagnitude = blackHoleForce;

                // ��Ȧ ��Ȱ��ȭ ����
                StartCoroutine(DeactivateBlackHole(blackHole));
            }
            else
            {
                Debug.LogWarning("PointEffector2D ������Ʈ�� ã�� �� �����ϴ�.");
            }

            // "��" �ı�
            Destroy(gameObject);
        }
    }

    private IEnumerator DeactivateBlackHole(GameObject blackHole)
    {
        yield return new WaitForSeconds(blackHoleDuration);

        // ��Ȧ�� ��Ȱ��ȭ�ϰ� Ǯ�� ��ȯ
        blackHole.SetActive(false);
    }
}