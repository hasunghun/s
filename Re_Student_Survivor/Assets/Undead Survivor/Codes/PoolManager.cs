using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static PoolManager Instance { get; private set; }

    // 프리팹과 리스트는 1:1 대응, 프리팹 2개면 리스트 2개
    // .. 프리팹들을 보관할 변수
    public GameObject[] prefabs;

    // .. 풀 담당 리스트들
    List<GameObject>[] pools;

    private void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
            pools[i] = new List<GameObject>();
    }

    public GameObject Get(int index)
    {
        GameObject select = null;

        // ... 선택한 풀의 놀고 있는(비활성화 된) 게임오브젝트 접근

          // ... 발견하면 select 변수에 할당


        foreach(GameObject item in pools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

       // ... 놀고 있는 오브젝트가 없으면 
         
         // ... 새롭게 할당
         if (!select)
        {
            //pool manager안에 넣겠다 원본을 복사해서
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        
        return select;
    }
}
