using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatoScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // トリガーとの接触時に呼ばれるコールバック
       
	}

    void OnTriggerEnter(Collider hit)
    {
        // 接触対象はPlayerタグですか？
        if (hit.CompareTag("Ball"))
        {

            // このコンポーネントを持つGameObjectを破棄する
            Destroy(gameObject);
        }
    }
}
