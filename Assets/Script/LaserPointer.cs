using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPointer : MonoBehaviour
{

    [SerializeField]
    private Transform _rightPointer = null;
    [SerializeField]
    private Transform _leftPointer = null;

    [SerializeField]
    private LineRenderer _LaserPointerRenderer;

    // オブジェクトを掴む
    private GameObject _grabObject = null;
    private Vector3 _grabOffset;
    private float _grabDistance = 0;

    [SerializeField]
    private float _maxDistance = 100.0f;

    private Vector3 _gravPrevFramePos;

    // 現在アクティブな左右のどちらかのコントロールを得る
    private Transform GetController()
    {
        var controller = OVRInput.GetActiveController();
        return (controller == OVRInput.Controller.RTrackedRemote ? _rightPointer : _leftPointer);
    }

    // Update is called once per frame
    void Update()
    {
        var controller = GetController();

        // コントローラーから前に伸ばしたRayを作成
        var pointerRay = new Ray(controller.position, controller.transform.forward);

        // レーザーの起点
        _LaserPointerRenderer.SetPosition(0, pointerRay.origin);

        // Rayがヒットした位置を取得
        RaycastHit hitInfo;
        if (Physics.Raycast(pointerRay, out hitInfo, _maxDistance))
        {
            // ヒットしたオブジェクトを取得
            GameObject hitObj = hitInfo.collider.gameObject;

            // Rayがヒットしたらそこまで
            _LaserPointerRenderer.SetPosition(1, hitInfo.point);


            // トリガーボタンを押した時
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            {
                if (hitObj.name != "Plane")
                { // 地面は掴めない
                    _grabObject = hitObj;
                    _grabDistance = hitInfo.distance;
                    _grabOffset = hitObj.transform.position - hitInfo.point; // ヒットした場所からオブジェクト中心までの距離
                    _gravPrevFramePos = hitObj.transform.position;
                }
            }

        }else{
            // Rayがヒットしなかったら向いている方向にMaxDistance伸ばす
            _LaserPointerRenderer.SetPosition(1, pointerRay.origin + pointerRay.direction * _maxDistance);
        }

        // 掴んだオブジェクトを移動
        if (_grabObject != null)
        {
            // 上下タッチで距離変更
            Vector2 pt = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad); ///タッチパッドの位置
            if (pt.y > +0.6 && (-0.5 < pt.x && pt.x < +0.5))  //上側？
            {
                _grabDistance += 0.1f;
            }
            else if (pt.y < -0.9 && (-0.5 < pt.x && pt.x < +0.5)) // 下側？
            {
                _grabDistance -= 0.1f;
                if (_grabDistance < 0.1) { _grabDistance = 0.1f; }
            }

            // 移動
            _gravPrevFramePos = _grabObject.transform.position;
            _grabObject.transform.position = pointerRay.GetPoint(_grabDistance) + _grabOffset;
            _grabObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        // 掴んだオブジェクトを離す
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            // トリガーボタンを離した時
            Vector3 force = _grabObject.transform.position - _gravPrevFramePos;
            _grabObject.GetComponent<Rigidbody>().velocity = force * 30f;
            _grabObject = null;
        }

    }
}