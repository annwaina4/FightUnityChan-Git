using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyController : MonoBehaviour
{
    //状態（ステートパターン）
    private int stateNumber = 0;

    //汎用タイマー
    private float timeCounter = 0f;

    private Animator myanimator;

    private Rigidbody myRigidbody;

    private GameObject player;

    public GameObject effectPrefab;

    //進む速度
    private int stepVelocity = 2;

    //攻撃開始の間合い
    private float enemyLength = 1.5f;

    //HP設定
    private float maxHP = 170;
    private float nowHP;
    public Slider slider;

    //------------------------------------------------------------------------------------------------------------------
    //スタート
    //------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        this.myanimator = GetComponent<Animator>();

        this.myRigidbody = GetComponent<Rigidbody>();

        this.player = GameObject.Find("UnityChan");

        //Sliderを最大にする
        slider.value = 1;
        //HPを最大HPと同じ値に
        nowHP = maxHP;
    }

    //------------------------------------------------------------------------------------------------------------------
    //距離と方向を求める関数
    //------------------------------------------------------------------------------------------------------------------

    //距離を求める
    float getLength(Vector3 current, Vector3 target)
    {
        return Mathf.Sqrt(((current.x - target.x) * (current.x - target.x)) + ((current.z - target.z) * (current.z - target.z)));
    }

    //方向を求める ※オイラー（-180～0～+180)
    float getEulerAngle(Vector3 current, Vector3 target)
    {
        Vector3 value = target - current;
        return Mathf.Atan2(value.x, value.z) * Mathf.Rad2Deg; //ラジアン→オイラー
    }

    //方向を求める ※ラジアン
    float getRadian(Vector3 current, Vector3 target)
    {
        Vector3 value = target - current;
        return Mathf.Atan2(value.x, value.z);
    }

    //------------------------------------------------------------------------------------------------------------------
    //アップデート
    //------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        //タイマー加算
        timeCounter += Time.deltaTime;

        //方向を求める
        float direction = getEulerAngle(this.transform.position, player.transform.position);

        //距離を求める
        float length = getLength(this.transform.position, player.transform.position);

        //**************************************************************************************************************
        //ここから状態処理
        //**************************************************************************************************************

        int stepPattern = Random.Range(1, 3);
        int enemyPattern = Random.Range(4, 9);
        
        //待機
        if (stateNumber == 0)
        {
            //プレーヤーの方向を向く
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //0.5秒経過
            if (timeCounter > 0.5f)
            {
                //タイマーリセット
                timeCounter = 0f;

                //状態の遷移（前進、後退）
                stateNumber = stepPattern;
            }
            //プレーヤーが近い時
            else if (length < enemyLength)
            {
                //タイマーリセット
                timeCounter = 0f;

                //アニメーション　攻撃
                this.myanimator.SetTrigger("rightK");

                //Debug.Log("攻撃: " + Time.time);

                //状態の遷移（切り替えステート）
                stateNumber = 3;
            }
        }

        //前進
        else if (stateNumber == 1)
        {
            //プレーヤーの方向を向く
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);
            //アニメーション前進
            myanimator.SetInteger("speed", 1);
            //移動
            myRigidbody.velocity = transform.forward * stepVelocity;

            //4秒経過
            if (timeCounter > 4.0f)
            {
                timeCounter = 0f;

                //アニメーション　待機
                this.myanimator.SetInteger("speed", 0);

                //状態の遷移（待機）
                stateNumber = 0;
            }
            //プレーヤーが近い時
            else if (length < enemyLength)
            {
                //タイマーリセット
                timeCounter = 0f;

                //アニメーション　攻撃
                this.myanimator.SetTrigger("rightK");

                //状態の遷移（切り替えステート）
                stateNumber = 3;
            }
        }

        //後退
        else if (stateNumber == 2)
        {
            //プレーヤーの方向を向く
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);
            //アニメーション後退
            myanimator.SetInteger("speed", -1);
            //移動
            myRigidbody.velocity = transform.forward * -stepVelocity;

            //0.5秒経過
            if (timeCounter > 0.5f)
            {
                timeCounter = 0f;

                //アニメーション　待機
                this.myanimator.SetInteger("speed", 0);

                //状態の遷移（待機）
                stateNumber = 0;
            }
            //プレーヤーが近い時
            else if (length < enemyLength)
            {
                //タイマーリセット
                timeCounter = 0f;

                //アニメーション　攻撃
                this.myanimator.SetTrigger("rightK");

                //状態の遷移（切り替えステート）
                stateNumber = 3;
            }
        }

        //モーション切り替え
        else if (stateNumber == 3)
        {
            //モーション終わり
            if (timeCounter > 0.2f)
            {
                //タイマーリセット
                timeCounter = 0f;

                //状態の遷移　後退か攻撃4通りのランダム
                stateNumber = enemyPattern;
            }
        }

        //左パンチ
        else if (stateNumber == 4)
        {
            //プレーヤーの方向を向く
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //1秒経過
            if (timeCounter > 1.0f)
            {
                //タイマーリセット
                timeCounter = 0f;

                //状態の遷移（前進、後退）
                stateNumber = stepPattern;
            }
            //プレーヤーが近い時
            else if (length < enemyLength)
            {
                //タイマーリセット
                timeCounter = 0f;

                //アニメーション　攻撃
                this.myanimator.SetTrigger("leftP");

                //状態の遷移（切り替えステート）
                stateNumber = 3;
            }
        }

        //右パンチ
        else if (stateNumber == 5)
        {
            //プレーヤーの方向を向く
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //1秒経過
            if (timeCounter > 1.0f)
            {
                //タイマーリセット
                timeCounter = 0f;

                //状態の遷移（前進、後退）
                stateNumber = stepPattern;
            }
            //プレーヤーが近い時
            else if (length < enemyLength)
            {
                //タイマーリセット
                timeCounter = 0f;

                //アニメーション　攻撃
                this.myanimator.SetTrigger("rightP");

                //状態の遷移（切り替えステート）
                stateNumber = 3;
            }
        }

        //左キック
        else if (stateNumber == 6)
        {
            //プレーヤーの方向を向く
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //1秒経過
            if (timeCounter > 1.0f)
            {
                //タイマーリセット
                timeCounter = 0f;

                //状態の遷移（前進、後退）
                stateNumber = stepPattern;
            }
            //プレーヤーが近い時
            else if (length < enemyLength)
            {
                //タイマーリセット
                timeCounter = 0f;

                //アニメーション　攻撃
                this.myanimator.SetTrigger("leftK");

                //状態の遷移（切り替えステート）
                stateNumber = 3;
            }
        }

        //右キック
        else if (stateNumber == 7)
        {
            //プレーヤーの方向を向く
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //1秒経過
            if (timeCounter > 1.0f)
            {
                //タイマーリセット
                timeCounter = 0f;

                //状態の遷移（前進、後退）
                stateNumber = stepPattern;
            }
            //プレーヤーが近い時
            else if (length < enemyLength)
            {
                //タイマーリセット
                timeCounter = 0f;

                //アニメーション　攻撃
                this.myanimator.SetTrigger("rightK");

                //状態の遷移（切り替えステート）
                stateNumber = 3;
            }
        }

        //後退
        else if (stateNumber == 8)
        {
            //プレーヤーの方向を向く
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //1秒経過
            if (timeCounter > 1.0f)
            {
                //タイマーリセット
                timeCounter = 0f;

                //状態の遷移（前進、後退）
                stateNumber = stepPattern;
            }
            //プレーヤーが近い時
            else if (length < enemyLength)
            {
                //アニメーション　後退
                myanimator.SetInteger("speed", -1);

                myRigidbody.velocity = transform.forward * -stepVelocity;

                if (timeCounter>1.5f)
                {
                    timeCounter = 0f;
                    //状態の遷移（切り替えステート）
                    stateNumber = 3;
                }
            }
        }

        //**************************************************************************************************************
        //ゲームオーバー監視
        //**************************************************************************************************************

        if (playerController.isEnd)
        {
            //アニメーション　待機
            this.myanimator.SetInteger("speed", 0);

            myRigidbody.isKinematic = true;

            //ステートパターンを停止
            stateNumber = -1;
        }
    }

    //------------------------------------------------------------------------------------------------------------------
    //衝突判定
    //------------------------------------------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision other)
    {
        if(playerController.isEnd==false)
        {
            //キック攻撃を受けたとき
            if (other.gameObject.tag == "unityAttack" && nowHP > 0)
            {
                //ダメージアニメーション
                this.myanimator.SetTrigger("damage");

                myRigidbody.AddForce(new Vector3(250.0f, 0f, 0f), ForceMode.Impulse);

                nowHP-=0.35f;

                //ダメージエフェクト生成
                foreach (ContactPoint point in other.contacts)
                {
                    Instantiate(effectPrefab, (Vector3)point.point, Quaternion.identity);
                }

                GetComponent<AudioSource>().Play();

                timeCounter = 0f;

                //状態の遷移（切り替えステート）
                stateNumber = 3;

                //HPをSliderに反映
                slider.value = (float)nowHP / (float)maxHP;
            }

            //パンチ攻撃を受けた時
            if (other.gameObject.tag == "unityPunch" && nowHP > 0)
            {
                //ダメージアニメーション
                myanimator.SetTrigger("damage");

                myRigidbody.AddForce(new Vector3(250.0f, 0f, 0f), ForceMode.Impulse);

                nowHP -= 5.2f;

                //ダメージエフェクト生成
                foreach (ContactPoint point in other.contacts)
                {
                    Instantiate(effectPrefab, (Vector3)point.point, Quaternion.identity);
                }

                GetComponent<AudioSource>().Play();

                timeCounter = 0f;

                //状態の遷移（切り替えステート）
                stateNumber = 3;

                //HPをSliderに反映
                slider.value = (float)nowHP / (float)maxHP;
            }

            //ステージの側壁に接触時
            if (other.gameObject.tag == "wall" && nowHP > 0)
            {
                timeCounter = 0f;

                //状態の遷移（前進）
                stateNumber = 1;
            }

            //敗北（HPゼロ）時の処理
            if (nowHP <= 0)
            {
                //ノックダウンアニメーション
                this.myanimator.SetBool("die", true);

                //ステートパターンを停止
                stateNumber = -1;

                myRigidbody.isKinematic = true;

                //勝利テキスト表示
                GameObject.Find("Canvas").GetComponent<UIController>().gameWin();

                playerController.isEnd = true;
            }
        }        
    }
}
