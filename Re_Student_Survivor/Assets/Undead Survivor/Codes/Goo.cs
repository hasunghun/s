using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goo : MonoBehaviour
{
    public GameObject targetEnemy; // 적 오브젝트
    public int blackHolePrefabIndex = 4; // 블랙홀 프리팹 인덱스
    public float blackHoleDuration = 5f; // 블랙홀 지속 시간
    public float blackHoleForce = 20f; // 블랙홀이 끌어당기는 힘

    // 추가: 적에게 맞았을 때 위치 저장
    public Vector3 hitPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == targetEnemy)
        {
            // 블랙홀을 가져와 활성화
            GameObject blackHole = PoolManager.Instance.Get(blackHolePrefabIndex);
            blackHole.transform.position = transform.position;
            blackHole.SetActive(true);

            // Point Effector 2D 컴포넌트 설정
            PointEffector2D effector = blackHole.GetComponent<PointEffector2D>();

            if (effector != null)
            {
                effector.forceMagnitude = blackHoleForce;

                // 블랙홀 비활성화 예약
                StartCoroutine(DeactivateBlackHole(blackHole));
            }
            else
            {
                Debug.LogWarning("PointEffector2D 컴포넌트를 찾을 수 없습니다.");
            }

            // "구" 파괴
            Destroy(gameObject);
        }
    }

    private IEnumerator DeactivateBlackHole(GameObject blackHole)
    {
        yield return new WaitForSeconds(blackHoleDuration);

        // 블랙홀을 비활성화하고 풀에 반환
        blackHole.SetActive(false);
    }
}