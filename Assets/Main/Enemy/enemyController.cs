using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyController : MonoBehaviour
{
    //��ԁi�X�e�[�g�p�^�[���j
    private int stateNumber = 0;

    //�ėp�^�C�}�[
    private float timeCounter = 0f;

    private Animator myanimator;

    private Rigidbody myRigidbody;

    private GameObject player;

    public GameObject effectPrefab;

    //�i�ޑ��x
    private int stepVelocity = 2;

    //�U���J�n�̊ԍ���
    private float enemyLength = 1.5f;

    //HP�ݒ�
    private float maxHP = 170;
    private float nowHP;
    public Slider slider;

    //------------------------------------------------------------------------------------------------------------------
    //�X�^�[�g
    //------------------------------------------------------------------------------------------------------------------
    void Start()
    {
        this.myanimator = GetComponent<Animator>();

        this.myRigidbody = GetComponent<Rigidbody>();

        this.player = GameObject.Find("UnityChan");

        //Slider���ő�ɂ���
        slider.value = 1;
        //HP���ő�HP�Ɠ����l��
        nowHP = maxHP;
    }

    //------------------------------------------------------------------------------------------------------------------
    //�����ƕ��������߂�֐�
    //------------------------------------------------------------------------------------------------------------------

    //���������߂�
    float getLength(Vector3 current, Vector3 target)
    {
        return Mathf.Sqrt(((current.x - target.x) * (current.x - target.x)) + ((current.z - target.z) * (current.z - target.z)));
    }

    //���������߂� ���I�C���[�i-180�`0�`+180)
    float getEulerAngle(Vector3 current, Vector3 target)
    {
        Vector3 value = target - current;
        return Mathf.Atan2(value.x, value.z) * Mathf.Rad2Deg; //���W�A�����I�C���[
    }

    //���������߂� �����W�A��
    float getRadian(Vector3 current, Vector3 target)
    {
        Vector3 value = target - current;
        return Mathf.Atan2(value.x, value.z);
    }

    //------------------------------------------------------------------------------------------------------------------
    //�A�b�v�f�[�g
    //------------------------------------------------------------------------------------------------------------------
    void Update()
    {
        //�^�C�}�[���Z
        timeCounter += Time.deltaTime;

        //���������߂�
        float direction = getEulerAngle(this.transform.position, player.transform.position);

        //���������߂�
        float length = getLength(this.transform.position, player.transform.position);

        //**************************************************************************************************************
        //���������ԏ���
        //**************************************************************************************************************

        int stepPattern = Random.Range(1, 3);
        int enemyPattern = Random.Range(4, 9);
        
        //�ҋ@
        if (stateNumber == 0)
        {
            //�v���[���[�̕���������
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //0.5�b�o��
            if (timeCounter > 0.5f)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //��Ԃ̑J�ځi�O�i�A��ށj
                stateNumber = stepPattern;
            }
            //�v���[���[���߂���
            else if (length < enemyLength)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //�A�j���[�V�����@�U��
                this.myanimator.SetTrigger("rightK");

                //Debug.Log("�U��: " + Time.time);

                //��Ԃ̑J�ځi�؂�ւ��X�e�[�g�j
                stateNumber = 3;
            }
        }

        //�O�i
        else if (stateNumber == 1)
        {
            //�v���[���[�̕���������
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);
            //�A�j���[�V�����O�i
            myanimator.SetInteger("speed", 1);
            //�ړ�
            myRigidbody.velocity = transform.forward * stepVelocity;

            //4�b�o��
            if (timeCounter > 4.0f)
            {
                timeCounter = 0f;

                //�A�j���[�V�����@�ҋ@
                this.myanimator.SetInteger("speed", 0);

                //��Ԃ̑J�ځi�ҋ@�j
                stateNumber = 0;
            }
            //�v���[���[���߂���
            else if (length < enemyLength)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //�A�j���[�V�����@�U��
                this.myanimator.SetTrigger("rightK");

                //��Ԃ̑J�ځi�؂�ւ��X�e�[�g�j
                stateNumber = 3;
            }
        }

        //���
        else if (stateNumber == 2)
        {
            //�v���[���[�̕���������
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);
            //�A�j���[�V�������
            myanimator.SetInteger("speed", -1);
            //�ړ�
            myRigidbody.velocity = transform.forward * -stepVelocity;

            //0.5�b�o��
            if (timeCounter > 0.5f)
            {
                timeCounter = 0f;

                //�A�j���[�V�����@�ҋ@
                this.myanimator.SetInteger("speed", 0);

                //��Ԃ̑J�ځi�ҋ@�j
                stateNumber = 0;
            }
            //�v���[���[���߂���
            else if (length < enemyLength)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //�A�j���[�V�����@�U��
                this.myanimator.SetTrigger("rightK");

                //��Ԃ̑J�ځi�؂�ւ��X�e�[�g�j
                stateNumber = 3;
            }
        }

        //���[�V�����؂�ւ�
        else if (stateNumber == 3)
        {
            //���[�V�����I���
            if (timeCounter > 0.2f)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //��Ԃ̑J�ځ@��ނ��U��4�ʂ�̃����_��
                stateNumber = enemyPattern;
            }
        }

        //���p���`
        else if (stateNumber == 4)
        {
            //�v���[���[�̕���������
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //1�b�o��
            if (timeCounter > 1.0f)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //��Ԃ̑J�ځi�O�i�A��ށj
                stateNumber = stepPattern;
            }
            //�v���[���[���߂���
            else if (length < enemyLength)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //�A�j���[�V�����@�U��
                this.myanimator.SetTrigger("leftP");

                //��Ԃ̑J�ځi�؂�ւ��X�e�[�g�j
                stateNumber = 3;
            }
        }

        //�E�p���`
        else if (stateNumber == 5)
        {
            //�v���[���[�̕���������
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //1�b�o��
            if (timeCounter > 1.0f)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //��Ԃ̑J�ځi�O�i�A��ށj
                stateNumber = stepPattern;
            }
            //�v���[���[���߂���
            else if (length < enemyLength)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //�A�j���[�V�����@�U��
                this.myanimator.SetTrigger("rightP");

                //��Ԃ̑J�ځi�؂�ւ��X�e�[�g�j
                stateNumber = 3;
            }
        }

        //���L�b�N
        else if (stateNumber == 6)
        {
            //�v���[���[�̕���������
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //1�b�o��
            if (timeCounter > 1.0f)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //��Ԃ̑J�ځi�O�i�A��ށj
                stateNumber = stepPattern;
            }
            //�v���[���[���߂���
            else if (length < enemyLength)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //�A�j���[�V�����@�U��
                this.myanimator.SetTrigger("leftK");

                //��Ԃ̑J�ځi�؂�ւ��X�e�[�g�j
                stateNumber = 3;
            }
        }

        //�E�L�b�N
        else if (stateNumber == 7)
        {
            //�v���[���[�̕���������
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //1�b�o��
            if (timeCounter > 1.0f)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //��Ԃ̑J�ځi�O�i�A��ށj
                stateNumber = stepPattern;
            }
            //�v���[���[���߂���
            else if (length < enemyLength)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //�A�j���[�V�����@�U��
                this.myanimator.SetTrigger("rightK");

                //��Ԃ̑J�ځi�؂�ւ��X�e�[�g�j
                stateNumber = 3;
            }
        }

        //���
        else if (stateNumber == 8)
        {
            //�v���[���[�̕���������
            this.transform.rotation = Quaternion.Euler(0f, direction, 0f);

            //1�b�o��
            if (timeCounter > 1.0f)
            {
                //�^�C�}�[���Z�b�g
                timeCounter = 0f;

                //��Ԃ̑J�ځi�O�i�A��ށj
                stateNumber = stepPattern;
            }
            //�v���[���[���߂���
            else if (length < enemyLength)
            {
                //�A�j���[�V�����@���
                myanimator.SetInteger("speed", -1);

                myRigidbody.velocity = transform.forward * -stepVelocity;

                if (timeCounter>1.5f)
                {
                    timeCounter = 0f;
                    //��Ԃ̑J�ځi�؂�ւ��X�e�[�g�j
                    stateNumber = 3;
                }
            }
        }

        //**************************************************************************************************************
        //�Q�[���I�[�o�[�Ď�
        //**************************************************************************************************************

        if (playerController.isEnd)
        {
            //�A�j���[�V�����@�ҋ@
            this.myanimator.SetInteger("speed", 0);

            myRigidbody.isKinematic = true;

            //�X�e�[�g�p�^�[�����~
            stateNumber = -1;
        }
    }

    //------------------------------------------------------------------------------------------------------------------
    //�Փ˔���
    //------------------------------------------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision other)
    {
        if(playerController.isEnd==false)
        {
            //�L�b�N�U�����󂯂��Ƃ�
            if (other.gameObject.tag == "unityAttack" && nowHP > 0)
            {
                //�_���[�W�A�j���[�V����
                this.myanimator.SetTrigger("damage");

                myRigidbody.AddForce(new Vector3(250.0f, 0f, 0f), ForceMode.Impulse);

                nowHP-=0.35f;

                //�_���[�W�G�t�F�N�g����
                foreach (ContactPoint point in other.contacts)
                {
                    Instantiate(effectPrefab, (Vector3)point.point, Quaternion.identity);
                }

                GetComponent<AudioSource>().Play();

                timeCounter = 0f;

                //��Ԃ̑J�ځi�؂�ւ��X�e�[�g�j
                stateNumber = 3;

                //HP��Slider�ɔ��f
                slider.value = (float)nowHP / (float)maxHP;
            }

            //�p���`�U�����󂯂���
            if (other.gameObject.tag == "unityPunch" && nowHP > 0)
            {
                //�_���[�W�A�j���[�V����
                myanimator.SetTrigger("damage");

                myRigidbody.AddForce(new Vector3(250.0f, 0f, 0f), ForceMode.Impulse);

                nowHP -= 5.2f;

                //�_���[�W�G�t�F�N�g����
                foreach (ContactPoint point in other.contacts)
                {
                    Instantiate(effectPrefab, (Vector3)point.point, Quaternion.identity);
                }

                GetComponent<AudioSource>().Play();

                timeCounter = 0f;

                //��Ԃ̑J�ځi�؂�ւ��X�e�[�g�j
                stateNumber = 3;

                //HP��Slider�ɔ��f
                slider.value = (float)nowHP / (float)maxHP;
            }

            //�X�e�[�W�̑��ǂɐڐG��
            if (other.gameObject.tag == "wall" && nowHP > 0)
            {
                timeCounter = 0f;

                //��Ԃ̑J�ځi�O�i�j
                stateNumber = 1;
            }

            //�s�k�iHP�[���j���̏���
            if (nowHP <= 0)
            {
                //�m�b�N�_�E���A�j���[�V����
                this.myanimator.SetBool("die", true);

                //�X�e�[�g�p�^�[�����~
                stateNumber = -1;

                myRigidbody.isKinematic = true;

                //�����e�L�X�g�\��
                GameObject.Find("Canvas").GetComponent<UIController>().gameWin();

                playerController.isEnd = true;
            }
        }        
    }
}
