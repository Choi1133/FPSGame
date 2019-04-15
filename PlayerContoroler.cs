using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerContoroler : MonoBehaviour
{

	private ActionFuc Act;
	private BossControler boss;
	private ZombieControler zombie;

	private Animator charAnim;
	private AnimatorStateInfo animStateInfo0;

	public int  currHp;
	private int maxHp = 500;

	private bool isAbleToMove = true;

	public Transform cameraTr;
	public GameObject blood;
	public GameObject blood1;

	public float fMoveSpeed;
	public float fMoveSpeedX;
	private float fAimingMoveX;
	private float fAimingMoveY;
	private static float fWalkSpeed = 1.0f;
	private static float fRunSpeed = 1.5f;
	public float fPlayerSpeed = 0.1f;
	public float fRotSpeed = 15.0f;

	private void Awake()
	{
		charAnim = this.GetComponentInChildren<Animator>();
		boss = GameObject.Find("Boss").GetComponent<BossControler>();
		zombie = GameObject.Find("Zombie").GetComponent<ZombieControler>();
		Act = GetComponent<ActionFuc>();

		currHp = maxHp;
	}

	private void Update()
	{
		// 0번 레이어(기본 레이어)의 애니메이션 정보를 가져온다.
		animStateInfo0 = charAnim.GetCurrentAnimatorStateInfo(0);

		isAbleToMove = animStateInfo0.IsName("Grounded");

		if(Input.GetKeyDown(KeyCode.R))
		{
			charAnim.SetBool("Reloading", true);
		}
		else
		{
			charAnim.SetBool("Reloading", false);
		}

		MoveControl();
		ActionControl();
	}

	private void FixedUpdate()
	{
		if (charAnim.GetBool("Aiming"))
		{
			this.transform.Translate(Vector3.forward * fAimingMoveY * fPlayerSpeed);
			this.transform.Translate(Vector3.right * fAimingMoveX * fPlayerSpeed);
		}
		else
		{
			this.transform.Translate(Vector3.forward * fMoveSpeed * fPlayerSpeed);
			this.transform.Translate(Vector3.right * fMoveSpeedX * fPlayerSpeed);
		}
	}

	private void MoveControl()
	{
		
		if (isAbleToMove)
		{
			if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
			{
				if (fMoveSpeed < fRunSpeed)
					fMoveSpeed += Time.deltaTime * 2.0f;
				else
					fMoveSpeed = fRunSpeed;
			}
			else if (Input.GetKey(KeyCode.W))
			{
				if (fMoveSpeed < fWalkSpeed)
				{
					fMoveSpeed += Time.deltaTime * 2.0f;
					if (fMoveSpeed > fWalkSpeed)
						fMoveSpeed = fWalkSpeed;
				}
				else if (fMoveSpeed > fWalkSpeed)   // 뛰다가 걷기로
				{
					fMoveSpeed -= Time.deltaTime * 2.0f;
					if (fMoveSpeed < fWalkSpeed)
						fMoveSpeed = fWalkSpeed;
				}
			}
			else if (Input.GetKey(KeyCode.S))
			{
				if (fMoveSpeed <= 0.0f)
				{
					fMoveSpeed -= Time.deltaTime * 2.0f;
					if (fMoveSpeed < -1.0f)
						fMoveSpeed = -1.0f;
				}
				else if (fMoveSpeed > 0.0f)
				{
					fMoveSpeed -= Time.deltaTime * 2.0f;
					if (fMoveSpeed < -1.0f)
						fMoveSpeed = -1.0f;
				}
			}
			else // 멈춤으로 넘어갈 때
			{
				if (fMoveSpeed > 0.0f)
				{
					fMoveSpeed -= Time.deltaTime * 2.0f;
					if (fMoveSpeed < 0.0f)
						fMoveSpeed = 0.0f;
				}
				else if (fMoveSpeed < 0.0f)
				{
					fMoveSpeed += Time.deltaTime * 2.0f;
					if (fMoveSpeed > 0.0f)
						fMoveSpeed = 0.0f;
				}

				// 후진 중 멈추는거 구현 (애니메이션 적용)
			}

			charAnim.SetFloat("Speed", fMoveSpeed);

			if (Input.GetKey(KeyCode.D))
			{
				if (fMoveSpeedX < fWalkSpeed)
				{
					fMoveSpeedX += Time.deltaTime * 2.0f;
					if (fMoveSpeedX > fWalkSpeed)
						fMoveSpeedX = fWalkSpeed;
				}
				else if (fMoveSpeedX > fWalkSpeed)   // 뛰다가 걷기로
				{
					fMoveSpeedX -= Time.deltaTime * 2.0f;
					if (fMoveSpeedX < fWalkSpeed)
						fMoveSpeedX = fWalkSpeed;
				}
			}
			else if (Input.GetKey(KeyCode.A))
			{
				if (fMoveSpeedX <= 0.0f)
				{
					fMoveSpeedX -= Time.deltaTime * 2.0f;
					if (fMoveSpeedX < -1.0f)
						fMoveSpeedX = -1.0f;
				}
				else if (fMoveSpeedX > 0.0f)
				{
					fMoveSpeedX -= Time.deltaTime * 2.0f;
					if (fMoveSpeedX < -1.0f)
						fMoveSpeedX = -1.0f;
				}
			}
			else // 멈춤으로 넘어갈 때
			{
				if (fMoveSpeedX > 0.0f)
				{
					fMoveSpeedX -= Time.deltaTime * 2.0f;
					if (fMoveSpeedX < 0.0f)
						fMoveSpeedX = 0.0f;
				}
				else if (fMoveSpeedX < 0.0f)
				{
					fMoveSpeedX += Time.deltaTime * 2.0f;
					if (fMoveSpeedX > 0.0f)
						fMoveSpeedX = 0.0f;
				}
			}

			charAnim.SetFloat("SpeedX", fMoveSpeedX);
		}
	}

	private void ActionControl()
	{
		if(Input.GetKey(KeyCode.Mouse1))
		{
			charAnim.SetBool("Aiming", true);

			if(charAnim.GetBool("Aiming") == true && Input.GetKey(KeyCode.W))
			{
				if (fAimingMoveY < fWalkSpeed)
				{
					fAimingMoveY += Time.deltaTime * 2.0f;
					if (fAimingMoveY > fWalkSpeed)
						fAimingMoveY = fWalkSpeed;
				}
				else if (fAimingMoveY > fWalkSpeed)   // 뛰다가 걷기로
				{
					fAimingMoveY -= Time.deltaTime * 2.0f;
					if (fAimingMoveY < fWalkSpeed)
						fAimingMoveY = fWalkSpeed;
				}
			}
			else if(charAnim.GetBool("Aiming") && Input.GetKey(KeyCode.S))
			{
				if (fAimingMoveY <= 0.0f)
				{
					fAimingMoveY -= Time.deltaTime * 2.0f;
					if (fAimingMoveY < -1.0f)
						fAimingMoveY = -1.0f;
				}
				else if (fAimingMoveY > 0.0f)
				{
					fAimingMoveY -= Time.deltaTime * 2.0f;
					if (fAimingMoveY < -1.0f)
						fAimingMoveY = -1.0f;
				}
			}
			else
			{
				if (fAimingMoveY > 0.0f)
				{
					fAimingMoveY -= Time.deltaTime * 2.0f;
					if (fAimingMoveY < 0.0f)
						fAimingMoveY = 0.0f;
				}
				else if (fAimingMoveY < 0.0f)
				{
					fAimingMoveY += Time.deltaTime * 2.0f;
					if (fAimingMoveY > 0.0f)
						fAimingMoveY = 0.0f;
				}
			}
			charAnim.SetFloat("Y", fAimingMoveY);

			if (charAnim.GetBool("Aiming") == true && Input.GetKey(KeyCode.A))
			{
				if (fAimingMoveX <= 0.0f)
				{
					fAimingMoveX -= Time.deltaTime * 2.0f;
					if (fAimingMoveX < -1.0f)
						fAimingMoveX = -1.0f;
				}
				else if (fAimingMoveX > 0.0f)
				{
					fAimingMoveX -= Time.deltaTime * 2.0f;
					if (fAimingMoveX < -1.0f)
						fAimingMoveX = -1.0f;
				}
			}
			else if (charAnim.GetBool("Aiming") && Input.GetKey(KeyCode.D))
			{
				if (fAimingMoveX < fWalkSpeed)
				{
					fAimingMoveX += Time.deltaTime * 2.0f;
					if (fAimingMoveX > fWalkSpeed)
						fAimingMoveX = fWalkSpeed;
				}
				else if (fAimingMoveX > fWalkSpeed)   // 뛰다가 걷기로
				{
					fAimingMoveX -= Time.deltaTime * 2.0f;
					if (fAimingMoveX < fWalkSpeed)
						fAimingMoveX = fWalkSpeed;
				}				
			}
			else
			{
				if (fAimingMoveX > 0.0f)
				{
					fAimingMoveX -= Time.deltaTime * 2.0f;
					if (fAimingMoveX < 0.0f)
						fAimingMoveX = 0.0f;
				}
				else if (fAimingMoveX < 0.0f)
				{
					fAimingMoveX += Time.deltaTime * 2.0f;
					if (fAimingMoveX > 0.0f)
						fAimingMoveX = 0.0f;
				}
			}
			charAnim.SetFloat("X", fAimingMoveX);

			if(charAnim.GetBool("Aiming") && Input.GetKey(KeyCode.Mouse0))
			{
				
				charAnim.SetBool("Shoot",true);

				if (Act.bulletNum <= 0)
				{
					charAnim.SetBool("Reloading", true);
				}

				Debug.DrawRay(cameraTr.position, cameraTr.forward * 100.0f, Color.green);

				RaycastHit temp;

				if (Physics.Raycast(cameraTr.position, cameraTr.forward, out temp, 100.0f)) //레이가 목표물에 충돌했다
				{
					//충돌된 객체가 보스라면
					if (temp.collider.tag == "Boss")
					{
						GameObject newBlood = Instantiate(blood);

						newBlood.transform.position = temp.point;

						newBlood.transform.forward = temp.normal;

						boss.currHP -= 5; 

						Destroy(newBlood, 0.5f);
					}

					//충돌된 객체가 좀비라면
					if (temp.collider.tag == "Zombie")
					{
						GameObject newBlood = Instantiate(blood1);

						newBlood.transform.position = temp.point;

						newBlood.transform.forward = temp.normal;

						zombie.currHP -= 5;

						Destroy(newBlood, 0.5f);
					}
				}
			}
			else if(charAnim.GetBool("Aiming") && Input.GetKeyUp(KeyCode.Mouse0))
			{
				charAnim.SetBool("Shoot", false);
			}

			if(currHp <= 0)
			{
				charAnim.SetBool("Dead", true);
			}
		}
		else if(Input.GetKeyUp(KeyCode.Mouse1))
		{
			charAnim.SetBool("Aiming", false);
		}
		else if(Input.GetKeyDown(KeyCode.R))
		{
			charAnim.SetBool("Reloading", true);		
		}
	}
}
