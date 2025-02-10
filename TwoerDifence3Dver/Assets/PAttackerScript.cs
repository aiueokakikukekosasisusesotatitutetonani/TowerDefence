using UnityEngine;
using System.Collections;

public class PAttackerScript : MonoBehaviour
{
    public float speed = 5f;                   // 移動速度
    public float rotationSpeed = 200f;         // 回転速度
    public float[] segmentDistances = { 2f, 200f, 2f }; // 各セグメントの移動距離
    private int currentSegment = 0;            // 現在のセグメントインデックス
    private Vector3 startPosition;             // 各セグメントの開始位置
    private bool isTurning = false;            // 回転中フラグ
<<<<<<< HEAD
=======
    public bool isMove = false;                // Enemyの移動を再開するメソッドのフラグ
>>>>>>> origin/main
    private bool isPaused = false;             // 移動を一時停止するフラグ
    public Animator animator;                  // Animatorコンポーネント
    public int playerAttackerHp = 100;         // プレイヤーアタッカーのHP
    public int playerAttackerPower = 10;       // プレイヤーアタッカーの攻撃力
<<<<<<< HEAD
    private float damageInterval = 1.0f;  // **ダメージを受ける間隔（秒）**
    private float lastDamageTime = 0f;    // **最後にダメージを受けた時間**

    public EAttackerScript eAttackerScript;
=======
    private float damageInterval = 1.0f;       // **ダメージを受ける間隔（秒）**

    private Coroutine damageCoroutine; // ダメージ処理のコルーチンを保持

    public EAttackerScript eAttackerScript;
    public ETankScript eTankScript;
>>>>>>> origin/main

    void Start()
    {
        animator = GetComponent<Animator>(); // Animatorのコンポーネントを取得
        startPosition = transform.position;  // 初期位置を設定
    }

    void Update()
    {
<<<<<<< HEAD
=======
        //Debug.Log("Update内のフラグ:" + isPaused);
>>>>>>> origin/main

        if (isPaused) return; // 一時停止中なら処理しない

        if (!isTurning && currentSegment < segmentDistances.Length)
        {
            animator.SetBool("isWalk", true); //移動しているとき歩行アニメーションを実行
            animator.SetBool("isAttack", false); //移動しているとき攻撃アニメーションを無効

            float movedDistance = Vector3.Distance(startPosition, transform.position);
            if (movedDistance < segmentDistances[currentSegment] - 0.01f)  // 誤差補正
            {
                // 指定距離に達するまで移動
                float remainingDistance = segmentDistances[currentSegment] - movedDistance;
                float moveStep = Mathf.Min(speed * Time.deltaTime, remainingDistance); // 必要以上に進まないよう補正
                transform.position += transform.forward * moveStep;
            }
            else
            {
                // 次のセグメントがある場合のみ回転
                if (currentSegment < segmentDistances.Length - 1)
                {
                    StartCoroutine(Turn90Degrees());
                }
                else
                {
                    // 最後のセグメントが終わったらスクリプトを無効化して終了
                    enabled = false;
                }
            }
        }
    }

    private IEnumerator Turn90Degrees()
    {
        isTurning = true;

        float targetAngle = transform.eulerAngles.y - 90f; // 目標角度
        float currentAngle = transform.eulerAngles.y;

        while (Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle)) > 0.1f)
        {
            float step = rotationSpeed * Time.deltaTime;
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, step);
            transform.rotation = Quaternion.Euler(0, currentAngle, 0);
            yield return null;
        }

        // 正確な90度回転
        transform.rotation = Quaternion.Euler(0, targetAngle, 0);

        // 新しいセグメントの開始位置を更新
        startPosition = transform.position;

        // 次の移動フェーズ
        currentSegment++;

        isTurning = false;
    }

    void PATakeDamage(int damage)
    {
<<<<<<< HEAD
        playerAttackerHp -= damage;
        //Debug.Log("Player残りHP:" + playerAttackerHp);
        if (playerAttackerHp <= 0)
        {
            Die();
            eAttackerScript.EAResumeMovement(); // 移動再開
=======
       
        playerAttackerHp -= damage;
        //Debug.Log(playerAttackerHp);

        if (playerAttackerHp <= 0)
        {
            Die();
>>>>>>> origin/main
        }
    }

    public void Die()
    {
<<<<<<< HEAD
        Destroy(gameObject);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            isPaused = true;  // 移動を一時停止
            animator.SetBool("isWalk", false); //移動しているとき歩行アニメーションを無効
            animator.SetBool("isAttack", true); //移動しているとき攻撃アニメーションを実行

            // **一定時間ごとにダメージを与える**
            if (Time.time - lastDamageTime >= damageInterval)
            {
                PATakeDamage(eAttackerScript.enemyAttackerPower);
                lastDamageTime = Time.time; // **ダメージを受けた時間を更新**
            }
=======
        transform.Translate(-4f, 0f, 0f);
        Destroy(gameObject, 1f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AEnemy"))
        {
            eAttackerScript = other.GetComponent<EAttackerScript>();

            isPaused = true;  // 移動を一時停止
            //Debug.Log("OnTriggerEnter内: isPaused = " + isPaused);
            animator.SetBool("isWalk", false);  //移動アニメーションを無効
            animator.SetBool("isAttack", true); //攻撃アニメーションを実行
            animator.SetBool("isIdel", false);  //待機アニメーションを無効

            // すでにダメージ処理が動いていなければ開始
            if (damageCoroutine == null)
            {
                Debug.Log("DamageOverTimeを処理");
                damageCoroutine = StartCoroutine(ADamageOverTime());
            }
        }

        if (other.CompareTag("TEnemy"))
        {
            eTankScript = other.GetComponent<ETankScript>();

            isPaused = true;  // 移動を一時停止
            //Debug.Log("OnTriggerEnter内: isPaused = " + isPaused);
            animator.SetBool("isWalk", false);  //移動アニメーションを無効
            animator.SetBool("isAttack", true); //攻撃アニメーションを実行
            animator.SetBool("isIdel", false);  //待機アニメーションを無効

            // すでにダメージ処理が動いていなければ開始
            if (damageCoroutine == null)
            {
                Debug.Log("DamageOverTimeを処理");
                damageCoroutine = StartCoroutine(TDamageOverTime());
            }
        }

        if (other.CompareTag("Player"))
        {
            if (transform.position.z > other.transform.position.z)
            {
                return;
            }
            isPaused = true;                       //移動停止  
            animator.SetBool("isWalk", false);     //移動しているとき歩行アニメーションを無効
            animator.SetBool("isAttack", false);   //移動しているとき歩行アニメーションを無効
            animator.SetBool("isIdel", true);      //移動しているとき待機アニメーションを実行

        }

        if (other.CompareTag("EnemyTower"))
        {
            isPaused = true;                      //移動を停止
            animator.SetBool("isWalk", false);    //移動しているとき歩行アニメーションを無効
            animator.SetBool("isAttack", true);  //移動しているとき攻撃アニメーションを無効
            animator.SetBool("isIdel", false);     //移動しているとき待機アニメーションを実行
        }
    }

    // 一定時間ごとにダメージを受ける Coroutine
    private IEnumerator ADamageOverTime()
    {
        while (playerAttackerHp > 0) // HPが0になるまでループ
        {
            Debug.Log("while");
            PATakeDamage(eAttackerScript.enemyAttackerPower);
            yield return new WaitForSeconds(damageInterval); // 指定時間待つ
        }

        // HPが0になったら敵の移動を再開
        Debug.Log("HP0後");
        eAttackerScript.EAResumeMovement();
        damageCoroutine = null; // コルーチンの参照をリセット
    }

    private IEnumerator TDamageOverTime()
    {
        while (playerAttackerHp > 0) // HPが0になるまでループ
        {
            Debug.Log("while");
            PATakeDamage(eTankScript.enemyTackPower);
            yield return new WaitForSeconds(damageInterval); // 指定時間待つ
        }

        // HPが0になったら敵の移動を再開
        Debug.Log("HP0後");
        eTankScript.ETResumeMovement();
        damageCoroutine = null; // コルーチンの参照をリセット
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            // ダメージを受ける Coroutine を停止
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }

            PAResumeMovement(); // 通常の移動を再開
        }

        if (other.CompareTag("TEnemy"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }

            PAResumeMovement(); // 通常の移動を再開
>>>>>>> origin/main
        }
    }

    public void PAResumeMovement()
    {
<<<<<<< HEAD
        isPaused = false; // 移動を再開
        animator.SetBool("isWalk", true); //移動しているとき歩行アニメーションを実行
        animator.SetBool("isAttack", false); //移動しているとき攻撃アニメーションを無効
=======
        Debug.Log("playerの移動再開");
        isPaused = false; // 移動を再開
        animator.SetBool("isWalk", true);    //移動しているとき歩行アニメーションを実行
        animator.SetBool("isAttack", false); //移動しているとき攻撃アニメーションを無効
        animator.SetBool("isIdel", false);   //移動しているとき待機アニメーションを無効
>>>>>>> origin/main
    }
}