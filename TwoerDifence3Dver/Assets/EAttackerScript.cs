using UnityEngine;
using System.Collections;

public class EAttackerScript : MonoBehaviour
{
    public float speed = 5f;                   // 移動速度
    public float rotationSpeed = 200f;         // 回転速度
    public float[] segmentDistances = { 2f, 200f, 2f }; // 各セグメントの移動距離
    private int currentSegment = 0;            // 現在のセグメントインデックス
    private Vector3 startPosition;             // 各セグメントの開始位置
    private bool isTurning = false;            // 回転中フラグ
    private bool isPaused = false;             // 移動を一時停止するフラグ
    public bool isMove = false;                // 移動を再開するメソッドを受け取るフラグ
    public Animator animator;                  // animator
    public int enemyAttackerHp = 100;          // エネミーアタッカーのHP
    public int enemyAttackerPower = 10;        // エネミーアタッカーの攻撃力
    private float damageInterval = 1.0f;  // **ダメージを受ける間隔（秒）**
    private float lastDamageTime = 0f;    // **最後にダメージを受けた時間**

    void Start()
    {
        animator = GetComponent<Animator>(); // animatorのコンポーネントを取得
        startPosition = transform.position;  // 初期位置を設定
        isMove = true;                       // isMoveをtrueにしておく
    }

    void Update()
    {
        //Debug.Log(isPaused);

        if (isPaused) return; // 一時停止中なら処理しない

        if (!isTurning && currentSegment < segmentDistances.Length)
        {
            animator.SetBool("isWalk", true);    //移動しているとき歩行アニメーションを実行
            animator.SetBool("isAttack", false); //移動しているとき攻撃アニメーションを無効
            animator.SetBool("isIdel", false);   //移動しているとき待機アニメーションを無効

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

        float targetAngle = transform.eulerAngles.y + 90f; // 目標角度
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

    void EATakeDamage(int damage)
    {
        //Debug.Log("Enemy残りHP:" + enemyAttackerHp);

        // **一定時間ごとにダメージを与える**
        if (Time.time - lastDamageTime >= damageInterval)
        {
            enemyAttackerHp -= damage;
            lastDamageTime = Time.time; // **ダメージを受けた時間を更新**
        }
        if (enemyAttackerHp <= 0)
        {
            Die();
            isMove = false;  //isMoveをfalseにする
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PAttackerScript pAttackerScript = other.GetComponent<PAttackerScript>();

            isPaused = true;  // 移動を一時停止
            animator.SetBool("isWalk", false);  //移動しているとき歩行アニメーションを無効
            animator.SetBool("isAttack", true); //移動しているとき攻撃アニメーションを実行
            animator.SetBool("isIdel", false);  //移動しているとき待機アニメーションを実行

            EATakeDamage(pAttackerScript.playerAttackerPower);

            if (enemyAttackerHp <= 0)
            {
                Debug.Log("中身");
                pAttackerScript.PAResumeMovement();
            }
        }

        if (other.CompareTag("Enemy"))  //味方と衝突している間
        {
            if (transform.position.z > other.transform.position.z)
            {
                return;
            }
            isPaused = true;                      //移動を停止
            animator.SetBool("isWalk", false);    //移動しているとき歩行アニメーションを無効
            animator.SetBool("isAttack", false);  //移動しているとき攻撃アニメーションを無効
            animator.SetBool("isIdel", true);     //移動しているとき待機アニメーションを実行

            if (enemyAttackerHp <= 0)
            {
                EAResumeMovement();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EAResumeMovement();
        }
    }

    public void EAResumeMovement()
    {
        isPaused = false; // 移動を再開
        Debug.Log(isPaused);
        animator.SetBool("isWalk", true);    //移動しているとき歩行アニメーションを実行
        animator.SetBool("isAttack", false); //移動しているとき攻撃アニメーションを無効
        animator.SetBool("isIdel", false);   //移動しているとき待機アニメーションを無効
    }

    public IEnumerator CurrentTime()
    {
        yield return new WaitForSeconds(2f);
    }
}