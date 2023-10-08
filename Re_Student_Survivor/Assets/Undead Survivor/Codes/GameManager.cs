using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //static - ����, �޸𸮿� ��ڴ�
    //inspector�� ��Ÿ���� ����
    public static GameManager Instance;

    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime = 2 * 10f;

    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };

    [Header("# Game Object")]
    public PoolManager pool;
    public Player player;
    public LevelUp uiLevelUp;
    public Result uiResult;
    public Transform uiJoy;
    public GameObject enemyCleaner;
    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }
    public void GameStart(int id)
    {
        playerId = id;

        health = maxHealth;

        player.gameObject.SetActive(true);

        //ù��° ĳ���� ����
        uiLevelUp.Select(playerId % 2);
        Resume();

        AudioManager.Instance.PlayBgm(true);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
    }
    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Lose);
    }
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.Instance.PlayBgm(false);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Win);
    }
    public void GameRetry()
    {
        SceneManager.LoadScene(0); // build setting�� scene ��ȣ
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (!isLive)
            return;
        gameTime += Time.deltaTime;

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory();
        }

    }
    public void GetExp()
    {
        if (!isLive)
            return;
        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)])//index bound error �� ��������
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    //�� ��ũ��Ʈ�� Update �迭 ������ isLive ���� �߰�
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0; //����Ƽ�� �ð� �ӵ� ����
        uiJoy.localScale = Vector3.zero;
    }
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1; //���� 2�� �ð��� �׸�ŭ ���� �귯��.
        uiJoy.localScale = Vector3.one;
    }
}