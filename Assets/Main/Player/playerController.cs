using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    Animator myanimator;

    Rigidbody myRigidbody;
    
    //HP�̐ݒ�
    private float maxHP = 100;
    private float nowHP;
    public Slider slider;

    static public bool isEnd = false;

    public GameObject effectPrefab;

    //private float skipCollision = 0.0f;�i�Փ˔���X�L�b�v���ԁj

    //------------------------------------------------------------------------------------------------------------------
    //�X�^�[�g
    //------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        Application.targetFrameRate = 60;

        this.myanimator = GetComponent<Animator>();

        this.myRigidbody = GetComponent<Rigidbody>();

        //Slider���ő�ɂ���
        slider.value = 1;
        nowHP = maxHP;
    }

    /*void FixedUpdate()���R����������
    {
        Vector3 gravity = -9.81f * 2.0f * Vector3.up;
        this.myRigidbody.AddForce(gravity, ForceMode.Acceleration);
    }*/

    //------------------------------------------------------------------------------------------------------------------
    //�A�b�v�f�[�g
    //------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        /*skipCollision -= Time.deltaTime;(�Փ˔���X�L�b�v���Ԃ�������)*/

        if (isEnd == false)
        {
            //�v���[���[�̑��x
            float speedx = Mathf.Abs(this.myRigidbody.velocity.x);

            //**********************************************************************
            //�ړ��Ƒҋ@
            //**********************************************************************

            //�ړ�
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
            //�ҋ@���[�V����
            if (speedx < 0.5f)
            {
                myanimator.SetInteger("speed", 0);
            }
            
            //**********************************************************************
            //�U���A�K�[�h�̃A�j���[�V����
            //**********************************************************************

            //���p���`
            if(Input.GetKeyDown(KeyCode.Y))
            {
                myanimator.SetTrigger("leftP");
                GameObject.Find("voiceBGM").GetComponent<AudioSource>().Play();
            }
            //�E�p���`
            if (Input.GetKeyDown(KeyCode.U))
            {
                myanimator.SetTrigger("rightP");
                GameObject.Find("voiceBGM").GetComponent<AudioSource>().Play();
            }
            //���L�b�N
            if (Input.GetKeyDown(KeyCode.G))
            {
                myanimator.SetTrigger("leftK");
                GameObject.Find("voiceBGM").GetComponent<AudioSource>().Play();
            }
            //�E�L�b�N
            if (Input.GetKeyDown(KeyCode.H))
            {
                myanimator.SetTrigger("rightK");
                GameObject.Find("voiceBGM").GetComponent<AudioSource>().Play();
            }
            //�K�[�h
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
            //�s�k�iHP�[���j���̏���
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

            //�^�C�g���֖߂�
            if (Input.GetKeyDown(KeyCode.Return))
            {
                isEnd = false;
                SceneManager.LoadScene("title");
            }
        }
    }

    //------------------------------------------------------------------------------------------------------------------
    //�Փ˔���
    //------------------------------------------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision other)
    {
        if(isEnd==false)
        {
            //�K�[�h�̃��[�V�����̎��̓_���[�W�𖳎�
            if (myanimator.GetCurrentAnimatorStateInfo(0).IsName("bb_front_A"))
            {
                //�_���[�W����
            }
            else
            {
                //�G�̃L�b�N�U�����󂯂���
                if (other.gameObject.tag == "guardAttack" && nowHP > 0)
                {
                    /*
                    //Debug.Log("�_���[�W: " + Time.time);
                    //�Փˉӏ��̑S�Ă�\��
                    //foreach (ContactPoint point in other.contacts)
                    //{
                    //    Debug.Log(point.point);
                    //}
                    //if(skipCollision < 0.0f)�Փ˔���X�L�b�v���O��菬�����ꍇ�̓_���[�W
                    //{
                    //skipCollision = 0.6f;�X�L�b�v����
                    */

                    //�_���[�W�A�j���[�V����
                    myanimator.SetTrigger("damage");

                    this.myRigidbody.AddForce(new Vector3(-200.0f, 0f, 0f), ForceMode.Impulse);

                    nowHP -= 1f;

                    //�_���[�W�G�t�F�N�g����
                    foreach(ContactPoint point in other.contacts)
                    {
                        Instantiate(effectPrefab, (Vector3)point.point, Quaternion.identity);
                    }

                    GetComponent<AudioSource>().Play();

                    //HP��Slider�ɔ��f
                    slider.value = (float)nowHP / (float)maxHP;
                }

                //�G�̃p���`�U�����󂯂���
                if (other.gameObject.tag == "guardPunch" && nowHP > 0)
                {
                    //�_���[�W�A�j���[�V����
                    myanimator.SetTrigger("damage");

                    this.myRigidbody.AddForce(new Vector3(-200.0f, 0f, 0f), ForceMode.Impulse);

                    nowHP -= 3f;

                    //�_���[�W�G�t�F�N�g����
                    foreach (ContactPoint point in other.contacts)
                    {
                        Instantiate(effectPrefab, (Vector3)point.point, Quaternion.identity);
                    }

                    GetComponent<AudioSource>().Play();

                    //HP��Slider�ɔ��f
                    slider.value = (float)nowHP / (float)maxHP;
                }
            }
        }
    }
}
