using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
	//회전량
	private float turnSpeedX = 100.0f;
	private float turnSpeedY = 100.0f;

	private float x = 0.0f;
	private float y = 0.0f;

	//타겟과 카메라의 거리
	private float dist = -1.5f;

	//타겟과 카메라의 Transform 
	public Transform TargetTransform;
	
	private void Awake()
	{
		Cursor.lockState = CursorLockMode.Locked; // 마우스 커서 숨기기

		Vector3 angle = transform.eulerAngles;

		x = angle.y;
		y = angle.x;
	}
		
	private void LateUpdate()
	{
		if(TargetTransform)
		{
			//회전속도 정하기
			x += Input.GetAxis("Mouse X") * turnSpeedX * 0.02f;
			y -= Input.GetAxis("Mouse Y") * turnSpeedY * 0.02f;

			//y축 회전량 제한(카메라가 y축 회전을 할 때 완전히 뒤로 넘어가는 것을 방지)
			if(y < -90.0f)
			{
				y = -90.0f;
			}
			else if(y > 90.0f)
			{
				y = 90.0f;
			}

			//카메라 위치, 회전 변환 계산
			Quaternion cameraRot = Quaternion.Euler(y, x, 0);
			Vector3 cameraPos = cameraRot * new Vector3(0, 0, dist) + TargetTransform.position + new Vector3(0.0f, 2.2f, 0.0f);
			TargetTransform.rotation = Quaternion.Euler(0, x, 0);

			this.transform.rotation = cameraRot;
			this.transform.position = cameraPos;
		}
	}
}
