using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public float moveX = 0.0f; //X方向の距離
    public float moveY = 0.0f; //Y方向の距離
    public float times = 0.0f; //時間
    public float wait = 0.0f; //停止時間

    public bool isMoveWhenOn = false; //乗った時にうごくアクションにするかどうか
    public bool isCanMove = true; //アクション中かどうかのフラグ

    Vector3 startPos; //初期位置の記憶
    Vector3 endPos; //ゴール地点
    bool isReverse = false; //移動方向の反転フラグ

    float movep = 0; //Lerpメソッドに使う移動の補完値。始点からゴール地点までの現在移動量の割合（0～1の間）


    // Start is called before the first frame update
    void Start()
    {
        startPos  = transform.position; //初期位置の記憶
        endPos = new Vector2(startPos.x + moveX, startPos.y + moveY); //ゴール地点の座標の設定

        //もしプレイヤーが乗ってから動くフラグがtrueなら
        if (isMoveWhenOn)
        {
            //最初は動かさない
            isCanMove = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //動いても良いフラグがtrueの場合
        if(isCanMove)
        {
            float distance = Vector2.Distance(startPos, endPos); //始点からゴール地点までの距離計測

            float ds = distance / times; //1秒あたりの移動距離目標

            float df = ds * Time.deltaTime; //その時々の1フレームあたりに進むべき移動量

            //全体の距離(distance)に対して、1フレームに進んだ距離の割合を蓄積
            movep += df / distance;

            //もし逆方向への移動フラグがtrueなら（反転）
            if(isReverse)
            {
                transform.position = Vector2.Lerp(endPos, startPos, movep); //逆方向への移動
            }
            else//逆方向フラグがfalseなら（順転）
            {
                transform.position = Vector2.Lerp(startPos,endPos, movep); //順方向への移動
            } 

            //移動の割合が100%に届いた＝ゴールに到着したら
            if(movep >= 1.0f)
            {
                movep = 0.0f; //移動の割合はリセット
                isReverse = !isReverse; //逆方向への移動フラグを反転
                isCanMove = false; //一旦停止

                //乗った時に動くフラグがfalseなのであれば
                if(isMoveWhenOn == false)
                {
                    //時間差で再始動(Moveメソッドをwait秒後に発動）
                    Invoke("Move", wait);
                }
            }

        }
    }

    //止まっているフラグをtrueに戻すメソッド
    public void Move()
    {
        isCanMove = true;
    }

    //動いているフラグをfalseにして止めるメソッド
    public void Stop()
    {
        isCanMove = false;
    }

    //何かと接触した時の動き
    void OnCollisionEnter2D(Collision2D collision)
    {
        //相手がプレイヤーだったら
        if(collision.gameObject.tag == "Player")
        {
            //プレイヤーの親はMovingBlockという事にする（プレイヤーがブロックの子オブジェクトになる＝ブロックについてくる）
            collision.transform.SetParent(transform);

            //もしプレイヤーが乗ってから動くフラグがtrueだった場合
            if (isMoveWhenOn)
            {
                isCanMove = true; //移動を始める
            }
        }    
    }

    //何かがブロックから離れた時
    void OnCollisionExit2D(Collision2D collision)
    {
        //相手がプレイヤーだったら
        if(collision.gameObject.tag == "Player")
        {
            //プレイヤーが離れたら親子関係は解除
            collision.transform.SetParent(null);
        }    
    }

    //移動のシミュレーションをギズモを使って描画
    void OnDrawGizmosSelected()
    {
        //描画するにあたっての初期地点
        Vector2 fromPos;

        //記録した初期地点が(0,0,0)だったら
        //ブロックの位置がfromPos
        if(startPos == Vector3.zero)
        {
            fromPos = transform.position;
        }
        else //でなければ初期地点が描画上も初期地点
        {
            fromPos = startPos;
        }

        //移動の線を描画
        Gizmos.DrawLine(fromPos,new Vector2(fromPos.x + moveX, fromPos.y + moveY));

        //移動の先のブロックの形=自分の形そのもの
        Vector2 size = GetComponent<SpriteRenderer>().size;

        //初期地点のブロックの形を描画
        Gizmos.DrawWireCube(fromPos, new Vector2(size.x, size.y));

        //ゴール地点の座標を確認
        Vector2 toPos = new Vector3(fromPos.x + moveX, fromPos.y + moveY);
        //ゴール地点のブロックの形を描画
        Gizmos.DrawWireCube(toPos, new Vector2(size.x, size.y));
    }
}
