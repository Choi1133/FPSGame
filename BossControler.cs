using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossControler : MonoBehaviour
{
	public int currHP;
	public Transform targetTrans;
	public float rotSpeed;
	private int maxHP = 1000;
	private Animator anim;
	public Animator animGetSet
	{
		get
		{
			return anim;
		}
		set
		{
			anim = animGetSet;
		}

	}
	private float moveSpeed = 1.6f;
	private bool isMove = false;
	private bool isAttack = false;

	public float rayAngle; //레이 방향각도
	public float rayDistance; //레이 길이

	private Ray leftRay;
	private Ray rightRay;

	private Transform rayPosition; //레이 위치

	private float leftRayDistance; //왼쪽 레이 충돌거리
	private float rightRayDistance; //오른쪽 레이 충돌거리

	private float distance;

	private SphereCollider coll;

	private PlayerContoroler player;

	private void Awake()
	{
		currHP = maxHP;
		anim = this.GetComponent<Animator>();
		isMove = true;
		rayPosition = this.transform.Find("RayPosition"); // 레이의 위치 셋팅
		coll = this.GetComponentInChildren<SphereCollider>();
	}

	//히트 스캔
	private void FixedUpdate()
	{
		// 레이의 방향을 컨트롤 해서 동체의 움직임을 제어
		float InputH = Input.GetAxis("Horizontal");
		rayPosition.localRotation = Quaternion.Euler(0.0f, -InputH * 20.0f, 0.0f);

		// 왼쪽 앵글 계산
		float leftFrontRad = (90.0f + rayAngle) * Mathf.Deg2Rad;   // 앵글각도 라디안값으로 변환
		Vector3 dirToLeftFront = new Vector3(Mathf.Cos(leftFrontRad), 0.0f, Mathf.Sin(leftFrontRad)); // 레이 방향 셋팅

		// 왼쪽 레이의 원점과 방향 셋팅
		// (방향: 동체의 로컬 정보를 사용해서 월드 공간에서의 실제 방향을 계산한다.)
		leftRay.origin = rayPosition.position; // 레이의 시작은 지정한 위치에서 부터 시작
		leftRay.direction = rayPosition.TransformDirection(dirToLeftFront);// 왼쪽 레이의 방향

		// 오른쪽 앵글 계산
		float rightFrontRad = (90.0f - rayAngle) * Mathf.Deg2Rad;   // 앵글각도 라디안값
		Vector3 dirToRightFront = new Vector3(Mathf.Cos(rightFrontRad), 0.0f, Mathf.Sin(rightFrontRad)); // 레이 방향

		// 오른쪽 레이의 원점과 방향 셋팅
		rightRay.origin = rayPosition.position;
		rightRay.direction = rayPosition.TransformDirection(dirToRightFront);

		// 레이의 길이 초기화
		leftRayDistance = rayDistance;
		rightRayDistance = rayDistance;

		RaycastHit rayHit; //레이캐스트 선언

		// 왼쪽 레이 검사
		if (Physics.Raycast(leftRay, out rayHit, rayDistance))
			leftRayDistance = rayHit.distance;
		// 오른쪽 레이 검사
		if (Physics.Raycast(rightRay, out rayHit, rayDistance))
			rightRayDistance = rayHit.distance;

		// 왼쪽 오른쪽 충돌 거리에 따른 비율로 회전량을 계산한다. 
		float steering = (rightRayDistance - leftRayDistance) / rayDistance;
		// 결과 값이 양수라면 오른쪽 회전, 음수라면 왼쪽 회전, 0은 직진

		if (isMove)
		{
			anim.SetFloat("MoveSpeed", moveSpeed);
			this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
			this.transform.Rotate(0.0f, steering * rotSpeed * Time.deltaTime, 0.0f);
		}
	}

	private void Update()
	{
		distance = Vector3.Distance(targetTrans.position, this.transform.position);

		if (distance <= 3.0f)
		{
			isMove = false;
			isAttack = true;
			anim.SetBool("Attack1", true);
			anim.SetBool("Attack2", false);
			anim.SetBool("Attack3", false);
			moveSpeed = 0.0f;
		}
		else if (distance <= 4.0f)
		{
			isMove = false;
			isAttack = true;
			anim.SetBool("Attack1", false);
			anim.SetBool("Attack2", true);
			anim.SetBool("Attack3", false);
			moveSpeed = 0.0f;
		}
		else if (distance <= 5.0f)
		{
			isMove = false;
			isAttack = true;
			anim.SetBool("Attack1", false);
			anim.SetBool("Attack2", false);
			anim.SetBool("Attack3", true);
			moveSpeed = 0.0f;
		}
		else if (distance <= 7.0f)
		{
			isMove = false;
			isAttack = false;
			moveSpeed = 1.6f;
			anim.SetBool("Attack1", false);
			anim.SetBool("Attack2", false);
			anim.SetBool("Attack3", false);
			anim.SetFloat("MoveSpeed", moveSpeed);
			this.transform.LookAt(targetTrans);
			this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
		}
		else if (currHP <= 0)
		{
			isMove = false;
			anim.SetBool("Attack1", false);
			anim.SetBool("Attack2", false);
			anim.SetBool("Attack3", false);
			anim.SetBool("isDie", true);
			Destroy(this.gameObject, 3.0f);
		}
		else
		{
			anim.SetBool("Attack1", false);
			anim.SetBool("Attack2", false);
			anim.SetBool("Attack3", false);
			isMove = true;
			isAttack = false;
		}	
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;

		// 충돌 검사에 의해서 줄어든 레이를 그린다.
		Gizmos.DrawLine(leftRay.origin,
			leftRay.origin + leftRay.direction * leftRayDistance);

		Gizmos.DrawLine(rightRay.origin,
			rightRay.origin + rightRay.direction * rightRayDistance);
	}
}
