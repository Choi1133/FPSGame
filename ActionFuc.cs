using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionFuc : MonoBehaviour
{
	//이펙트의 위치를 적용할 빈 오브젝트 선언
	public Transform footStepPos;
	public Transform ShootFirePos;
	public Transform CartridgePos;
	public Text bulletShow;
	public int bulletNum;

	private void Awake()
	{
		bulletNum = 30;
	}

	private void FootStep(GameObject effect) //오브젝트를 매개 변수로 받는 함수 선언 
	{
		GameObject newEffect = Instantiate(effect, footStepPos.position, Quaternion.identity);//오브젝트를 반환 하는 함수(오브젝트, 위치, 회전값)
		Destroy(newEffect, 1.0f); // 오브젝트 생성 후 1초 후에 없어지게 한다
	}

	private void ShootFire(GameObject effect)
	{
		GameObject newEffect = Instantiate(effect, ShootFirePos.position, ShootFirePos.rotation);
		Destroy(newEffect, 0.5f);
	}

	private void Cartridge(GameObject effect)
	{
		GameObject newEffect = Instantiate(effect, CartridgePos.position, CartridgePos.rotation);
		Destroy(newEffect, 2.5f);
	}

	private void MinBullet(int num)
	{
		bulletNum -= num;

		bulletShow.text = "" + bulletNum;
	}

	public void ReloadingBullet(int bullet)
	{
		bulletNum = bullet;

		bulletShow.text = "" + bulletNum;
	}
}
