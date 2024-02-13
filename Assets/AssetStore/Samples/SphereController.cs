using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
	// 状態の番号
	private int StateNumber = 0;

	// 経過時間(秒)
	private float TimeCounter = 0.0f;

	// 移動する速度
	private float Speed = 3.0f;

	//Unityちゃんのオブジェクト
	private GameObject UnityChan;

	//--------------------------------------------------------------------------------
	// 角度クリップ
	//--------------------------------------------------------------------------------
	
    private float ClipDirection360( float direction) {
        if( direction < 0.0f) {
            return 360.0f + direction;
        }
        if( direction >= 360.0f) {
            return direction - 360.0f;
        }
        return direction;
    }

    private float ClipDirection180( float direction) {
        direction = ClipDirection360( direction);
        direction = direction + 180.0f;
        if( direction >= 360.0f) {
            direction = direction - 360.0f;
        }
        return direction - 180.0f;
    }

	//--------------------------------------------------------------------------------
	// 方向・角度
	//--------------------------------------------------------------------------------
	
    private float GetDirection( float x1, float y1, float x2, float y2) {
        if( (x2 - x1) == 0.0f && (y2 - y1) == 0.0f) {
            return 0.0f;
        } else {
            return ClipDirection360( (((Mathf.PI / 2.0f) - Mathf.Atan2( y1 - y2, x1 - x2)) * (180.0f / Mathf.PI)) + 180.0f);
        }
    }

    private float GetLength( float x1, float y1, float x2, float y2) {
        return Mathf.Sqrt( ((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2)));
    }

	//--------------------------------------------------------------------------------
	// スタート
	//--------------------------------------------------------------------------------
	
    void Start()
    {
		//Unityちゃんのオブジェクトを取得
		this.UnityChan = GameObject.Find ( "UnityChan");        
    }

	//--------------------------------------------------------------------------------
	// アップデート
	//--------------------------------------------------------------------------------

    void Update()
    {
			// 座標・回転の取得
			Vector3 Position = transform.position;
			Vector3 Rotation = transform.rotation.eulerAngles;

 			// ステートマシン ※ステートとは[状態]のこと
			switch( StateNumber) {
				// アイドリング
				case  0 :	{	// 1秒経ったか？
								if( TimeCounter > 1.0f) {
									// 状態の遷移
									StateNumber = 1;
								}
							}	break;

				// 逃げる
				case  1 :	{	// デバッグ
								//Debug.Log( "逃げる方向: " + GetDirection( UnityChan.transform.position.x, UnityChan.transform.position.z, Position.x, Position.z));

								// 一時的に変数へ代入
								float direction = GetDirection( UnityChan.transform.position.x, UnityChan.transform.position.z, Position.x, Position.z);

								// directionの方向へ移動
								Position.x += (Mathf.Sin( direction * Mathf.Deg2Rad) * Speed) * Time.deltaTime;
								Position.z += (Mathf.Cos( direction * Mathf.Deg2Rad) * Speed) * Time.deltaTime;
							
								// 一時的に変数へ代入
								float length = GetLength( UnityChan.transform.position.x, UnityChan.transform.position.z, Position.x, Position.z);

								// 離れすぎ？（10メートル）
								if( length > 10.0f) {
									// タイマー クリアー
									TimeCounter = 0.0f;

									// 状態の遷移
									StateNumber = 0;								
								}				
							}	break;


				default :	break;
			}

			// 座標の更新
			transform.position = Position;

			// タイマー
			TimeCounter += Time.deltaTime;				
    }
}
