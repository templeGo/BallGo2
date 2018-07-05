using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour {


    public UnityEngine.UI.Text TargetCount;

        public void Update()
        {
            int count = GameObject.FindGameObjectsWithTag("Target").Length;
        TargetCount.text = "Target :" + count.ToString();
        }
    }
