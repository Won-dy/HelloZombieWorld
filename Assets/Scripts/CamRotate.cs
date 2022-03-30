using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamRotate : MonoBehaviour
{
    public static float rotSpeed = 200f; // 회전속도변수
    public Slider mouseSpeed;
    float mx = 0;
    float my = 0;
    void Start()
    {
        rotSpeed = 200f;
        mouseSpeed.value = rotSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        // 마우스 감도 변경
        rotSpeed = mouseSpeed.value;

        //게임 상태가 '게임 중' 상태일 때만 조작할 수 있게 한다.
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        // 마우스 입력을 받는다.
        float mouse_X = Input.GetAxis("Mouse X"); 
        float mouse_Y = Input.GetAxis("Mouse Y");
        //회전값 변수에 마우스 입력 값만큼 미리 누적을 시킨다.
        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;
        //상하이동 회전변수(my)의 값을 -90~90도 사이로 제한한다.
        my = Mathf.Clamp(my, -90f, 90f);
        //회전방향으로 물체를 회전시킨다. 
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
