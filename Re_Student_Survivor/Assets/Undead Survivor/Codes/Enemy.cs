using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriteRenderer;
    WaitForFixedUpdate wait;

    // 추가: 블랙홀 프리팹 인덱스와 히트 포지션 변수
    public int blackHolePrefabIndex;
    public Vector3 hitPosition;
    private Vector3 deathPosition;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }
    private void LateUpdate()
    {
        if (!GameManager.Instance.isLive)
            return;

        if (!isLive)
            return;

        spriteRenderer.flipX = target.position.x < rigid.position.x;
    }
    private void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriteRenderer.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
            return;

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0) {
            //.. 살았고 피격판정
            //애니메이션, 넉백
            anim.SetTrigger("Hit");
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            //.. 죽음
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriteRenderer.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.Instance.kill++;
            GameManager.Instance.GetExp();


            if (GameManager.Instance.isLive)
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    //코루틴 - 비동기
    IEnumerator KnockBack()
    {

        //yield - 코루틴 반환
        //yield return null; // 1프레임 쉬기
        
        //yield return new WaitForSeconds(2f);//2초 쉬기 - new 계속하면 성능문제

        //하나의 물리 프레임을 딜레이할 것
        yield return wait;

        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        //순간적인 힘 - Impulse


    }

    void Dead()
    {
        // 적이 죽었을 때 hitPosition에 저장된 위치에 블랙홀 생성
        GameObject blackHole = PoolManager.Instance.Get(blackHolePrefabIndex);
        blackHole.transform.position = hitPosition; // 저장된 위치로 설정
        blackHole.SetActive(true);

        deathPosition = transform.position;

        gameObject.SetActive(false);
    }

    public Vector3 GetDeathPosition()
    {
        return deathPosition;
    }
}