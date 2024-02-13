using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    Animator myanimator;

    Rigidbody myRigidbody;
    
    //HPの設定
    private float maxHP = 100;
    private float nowHP;
    public Slider slider;

    static public bool isEnd = false;

    public GameObject effectPrefab;

    //private float skipCollision = 0.0f;（衝突判定スキップ時間）

    //------------------------------------------------------------------------------------------------------------------
    //スタート
    //------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        Application.targetFrameRate = 60;

        this.myanimator = GetComponent<Animator>();

        this.myRigidbody = GetComponent<Rigidbody>();

        //Sliderを最大にする
        slider.value = 1;
        nowHP = maxHP;
    }

    /*void FixedUpdate()自由落下を強化
    {
        Vector3 gravity = -9.81f * 2.0f * Vector3.up;
        this.myRigidbody.AddForce(gravity, ForceMode.Acceleration);
    }*/

    //------------------------------------------------------------------------------------------------------------------
    //アップデート
    //------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        /*skipCollision -= Time.deltaTime;(衝突判定スキップ時間を減ずる)*/

        if (isEnd == false)
        {
            //プレーヤーの速度
            float speedx = Mathf.Abs(this.myRigidbody.velocity.x);

            //**********************************************************************
            //移動と待機
            //**********************************************************************

            //移動
            if (myanimator.GetCurrentAnimatorStateInfo(0).IsName("bb_front_A")==false)
            {
                if (Input.GetKey(KeyCode.D))
                {
                    myRigidbody.velocity = new Vector3(2.0f, 0f, 0f);
                    myanimator.SetInteger("speed", 1);
                }

                if (Input.GetKey(KeyCode.A))
                {
                    myRigidbody.velocity = new Vector3(-2.0f, 0f, 0f);
                    myanimator.SetInteger("speed", -1);
                }
            }
            //待機モーション
            if (speedx < 0.5f)
            {
                myanimator.SetInteger("speed", 0);
            }
            
            //**********************************************************************
            //攻撃、ガードのアニメーション
            //**********************************************************************

            //左パンチ
            if(Input.GetKeyDown(KeyCode.Y))
            {
                myanimator.SetTrigger("leftP");
                GameObject.Find("voiceBGM").GetComponent<AudioSource>().Play();
            }
            //右パンチ
            if (Input.GetKeyDown(KeyCode.U))
            {
                myanimator.SetTrigger("rightP");
                GameObject.Find("voiceBGM").GetComponent<AudioSource>().Play();
            }
            //左キック
            if (Input.GetKeyDown(KeyCode.G))
            {
                myanimator.SetTrigger("leftK");
                GameObject.Find("voiceBGM").GetComponent<AudioSource>().Play();
            }
            //右キック
            if (Input.GetKeyDown(KeyCode.H))
            {
                myanimator.SetTrigger("rightK");
                GameObject.Find("voiceBGM").GetComponent<AudioSource>().Play();
            }
            //ガード
            if(Input.GetKeyDown(KeyCode.L))
            {
                myanimator.SetTrigger("guard");
                myanimator.SetBool("guardEnd", false);
            }
            if (Input.GetKeyUp(KeyCode.L))
            {
                myanimator.SetBool("guardEnd", true);
            }

            //**********************************************************************
            //敗北（HPゼロ）時の処理
            //**********************************************************************
            if (nowHP<=0)
            {
                isEnd = true;
                myanimator.SetBool("die", true);
                GameObject.Find("Canvas").GetComponent<UIController>().gameLose();
            }
        }

        if (isEnd)
        {
            myanimator.SetInteger("speed", 0);
            myRigidbody.isKinematic = true;

            //タイトルへ戻る
            if (Input.GetKeyDown(KeyCode.Return))
            {
                isEnd = false;
                SceneManager.LoadScene("title");
            }
        }
    }

    //------------------------------------------------------------------------------------------------------------------
    //衝突判定
    //------------------------------------------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision other)
    {
        if(isEnd==false)
        {
            //ガードのモーションの時はダメージを無視
            if (myanimator.GetCurrentAnimatorStateInfo(0).IsName("bb_front_A"))
            {
                //ダメージ無効
            }
            else
            {
                //敵のキック攻撃を受けた時
                if (other.gameObject.tag == "guardAttack" && nowHP > 0)
                {
                    /*
                    //Debug.Log("ダメージ: " + Time.time);
                    //衝突箇所の全てを表示
                    //foreach (ContactPoint point in other.contacts)
                    //{
                    //    Debug.Log(point.point);
                    //}
                    //if(skipCollision < 0.0f)衝突判定スキップが０より小さい場合はダメージ
                    //{
                    //skipCollision = 0.6f;スキップ時間
                    */

                    //ダメージアニメーション
                    myanimator.SetTrigger("damage");

                    this.myRigidbody.AddForce(new Vector3(-200.0f, 0f, 0f), ForceMode.Impulse);

                    nowHP -= 1f;

                    //ダメージエフェクト生成
                    foreach(ContactPoint point in other.contacts)
                    {
                        Instantiate(effectPrefab, (Vector3)point.point, Quaternion.identity);
                    }

                    GetComponent<AudioSource>().Play();

                    //HPをSliderに反映
                    slider.value = (float)nowHP / (float)maxHP;
                }

                //敵のパンチ攻撃を受けた時
                if (other.gameObject.tag == "guardPunch" && nowHP > 0)
                {
                    //ダメージアニメーション
                    myanimator.SetTrigger("damage");

                    this.myRigidbody.AddForce(new Vector3(-200.0f, 0f, 0f), ForceMode.Impulse);

                    nowHP -= 3f;

                    //ダメージエフェクト生成
                    foreach (ContactPoint point in other.contacts)
                    {
                        Instantiate(effectPrefab, (Vector3)point.point, Quaternion.identity);
                    }

                    GetComponent<AudioSource>().Play();

                    //HPをSliderに反映
                    slider.value = (float)nowHP / (float)maxHP;
                }
            }
        }
    }
}
