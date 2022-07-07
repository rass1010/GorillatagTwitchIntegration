using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GorillaTagTwitchIntegration
{
	class Events : MonoBehaviour
	{

		public GameObject TreeRoom;
		public GameObject Forest1;
		public GameObject Forest2;
		public GameObject Caves;
		public GameObject Canyons;
		public GameObject City1;
		public GameObject City2;
		public GameObject City3;
		public GameObject City4;
		public GameObject Mountain1;
		public GameObject Mountain2;
		public float Maxjump;
		public float Jumpmulti;

		Plugin plugin;
		public static Events instance;

		public void RegisterEvents()
        {
            plugin = Plugin.instance;

			plugin.AddEvent("LowGravity", lowgravity);
			plugin.AddEvent("SideWaysGravity", sideWaysGravity);
			plugin.AddEvent("InvisBody", invisBody);
			plugin.AddEvent("ResetVelocity", ResetVelocity);
			plugin.AddEvent("NoGravity", nogravity);
			plugin.AddEvent("DoubleVelocity", doubleVelocity);
			plugin.AddEvent("DoubleGravity", doubleGravity);
			plugin.AddEvent("LaunchPlayer", launchPlayer);
			plugin.AddEvent("DoubleSpeed", doubleSpeed);
			plugin.AddEvent("SlowMotion", SlowMotion);
			plugin.AddEvent("RuberBanding", RuberBanding);
			plugin.AddEvent("BigPlayer", BigPlayer);
			plugin.AddEvent("ChangingGravity", ChangingGravity);
			plugin.AddEvent("CorruptWorld", CorruptWorld);
			plugin.AddEvent("ThickArms", ThickArms);
			plugin.AddEvent("Move'nt", Movent);
			plugin.AddEvent("SkipArmDay", SkipArmDay);
			plugin.AddEvent("SmallPlayer", SmallPlayer);
			plugin.AddEvent("SpeedBoost", SpeedBoost);

			findObjects();
		}

		void findObjects()
        {
			TreeRoom = GameObject.Find("Level/treeroom/tree/Uncover TreeAtlas/CombinedMesh-TreeAtlas-mesh/TreeAtlas-mesh-mesh/");
			Forest1 = GameObject.Find("Level/forest/Uncover ForestCombined/CombinedMesh-GameObject (1)-mesh/GameObject (1)-mesh-mesh/");
			Forest2 = GameObject.Find("Level/forest/SmallTrees/");
			Caves = GameObject.Find("Level/cave/");
			Canyons = GameObject.Find("Level/canyon/");
			City1 = GameObject.Find("Level/city/CosmeticsRoomAnchor/Mesh Bakers/Uncover RoomAtlasOnly/CombinedMesh-RoomAtlasOnly-mesh/RoomAtlasOnly-mesh-mesh/");
			City2 = GameObject.Find("Level/city/CosmeticsRoomAnchor/Mesh Bakers/Uncover HatlasandShinyHatlas/CombinedMesh-StaticHats-mesh/StaticHats-mesh-mesh/");
			City3 = GameObject.Find("Level/city/CosmeticsRoomAnchor/Mesh Bakers/Uncover DefaultMat/CombinedMesh-StaticHead-mesh/StaticHead-mesh-mesh/");
			City4 = GameObject.Find("Level/city/CosmeticsRoomAnchor/Mesh Bakers/Uncover NoLighting/CombinedMesh-NoLighting-mesh/NoLighting-mesh-mesh/");
			Mountain1 = GameObject.Find("Level/mountain/Geometry/mountainside/");
			Mountain2 = GameObject.Find("Level/mountain/Geometry/mountainsideice/");
		}

		public void lowgravity()
		{			
			Physics.gravity = new Vector3(0, plugin.normalGravity / 2, 0);
			plugin.chat.text = "LowGravity";
			StartCoroutine(resetGravity());
		}
		public void sideWaysGravity()
		{
			Physics.gravity = new Vector3(0, 0, plugin.normalGravity);
			plugin.chat.text = "SideWaysGravity";
			StartCoroutine(resetGravity());
		}
		public void invisBody()
		{
			GameObject clone_body = GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/gorilla");
			clone_body.GetComponent<SkinnedMeshRenderer>().forceRenderingOff = true;
			plugin.chat.text = "InvisBody";
			StartCoroutine(showBody());
		}
		public void ResetVelocity()
		{
			GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity.Equals(Vector3.zero);
			plugin.chat.text = "ResetVelocity";
		}
		public void nogravity()
		{
			Physics.gravity = new Vector3(0, 0, 0);
			plugin.chat.text = "NoGravity";
			StartCoroutine(resetGravity10s());
		}
		public void doubleVelocity()
		{
			plugin.chat.text = "DoubleVelocity";
			GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity = new Vector3(GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity.x * 2, GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity.y * 2, GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity.z * 2);
		}
		public void doubleGravity()
		{
			plugin.chat.text = "DoubleGravity";
			Physics.gravity = new Vector3(0, plugin.normalGravity * 2, 0);
			StartCoroutine(resetGravity());
		}
		public void launchPlayer()
		{
			plugin.chat.text = "LaunchPlayer";
			GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity = new Vector3(GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity.x, 20, GorillaLocomotion.Player.Instance.bodyCollider.attachedRigidbody.velocity.z);
		}
		public void doubleSpeed()
		{
			Time.timeScale = plugin.normalDeltaTime * 2;
			Time.fixedDeltaTime = plugin.normalFixedDeltaTime * 2;
			plugin.chat.text = "DoubleSpeed";
			StartCoroutine(resetTime());
		}
		public void SlowMotion()
		{
			Time.timeScale = plugin.normalDeltaTime / 2;
			Time.fixedDeltaTime = plugin.normalFixedDeltaTime / 2;
			plugin.chat.text = "SlowMotion";
			StartCoroutine(resetTime());
		}
		public void RuberBanding()
        {
			plugin.chat.text = "RuberBanding";
			StartCoroutine(RuberBandingIE());
        }
		public void BigPlayer()
        {
			plugin.chat.text = "BigPlayer";
			GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(2, 2, 2);
			StartCoroutine(ResetPlayerSize());
        }
		public void ChangingGravity()
        {
			plugin.chat.text = "ChangingGravity";
			StartCoroutine(ChangingGravityIE());
		}
		public void ThickArms()
        {
			plugin.chat.text = "ThickArms";
			GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/shoulder.L").transform.localScale = new Vector3(2, 2, 2);
			GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/shoulder.R").transform.localScale = new Vector3(2, 2, 2);
			StartCoroutine(ResetArm());
		}
		public void CorruptWorld()
        {
			plugin.chat.text = "CorruptWorld";
			TreeRoom.GetComponent<MeshRenderer>().enabled = false;
			Forest1.GetComponent<MeshRenderer>().enabled = false;
			Forest2.SetActive(false);
			Caves.GetComponent<MeshRenderer>().enabled = false;
			Canyons.GetComponent<MeshRenderer>().enabled = false;
			City1.GetComponent<MeshRenderer>().enabled = false;
			City2.GetComponent<MeshRenderer>().enabled = false;
			City3.GetComponent<MeshRenderer>().enabled = false;
			City4.GetComponent<MeshRenderer>().enabled = false;
			Mountain1.GetComponent<MeshRenderer>().enabled = false;
			Mountain2.GetComponent<MeshRenderer>().enabled = false;
			StartCoroutine(UnCorruptWorld());
		}
		public void Movent()
        {
			plugin.chat.text = "Move'nt";
			GorillaLocomotion.Player.Instance.disableMovement = true;
			StartCoroutine(EnableMovement());
		}
		public void SkipArmDay()
        {
			plugin.chat.text = "SkipArmDay";
			GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/shoulder.L").transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/shoulder.R").transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			StartCoroutine(ResetArm());
		}
		public void SmallPlayer()
        {
			plugin.chat.text = "SmallPlayer";
			GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
			StartCoroutine(ResetPlayerSize());
		}
		public void SpeedBoost()
        {
			Maxjump = GorillaLocomotion.Player.Instance.maxJumpSpeed;
			Jumpmulti = GorillaLocomotion.Player.Instance.jumpMultiplier;
			GorillaLocomotion.Player.Instance.maxJumpSpeed = 999f;
			GorillaLocomotion.Player.Instance.jumpMultiplier = 1.45f;
		}
		public IEnumerator ResetBoost()
        {
			yield return new WaitForSeconds(30);
			GorillaLocomotion.Player.Instance.maxJumpSpeed = Maxjump;
			GorillaLocomotion.Player.Instance.jumpMultiplier = Jumpmulti;
		}
		public IEnumerator UnCorruptWorld()
        {
			yield return new WaitForSeconds(10);
			TreeRoom.GetComponent<MeshRenderer>().enabled = true;
			Forest1.GetComponent<MeshRenderer>().enabled = true;
			Forest2.SetActive(true);
			Caves.GetComponent<MeshRenderer>().enabled = true;
			Canyons.GetComponent<MeshRenderer>().enabled = true;
			City1.GetComponent<MeshRenderer>().enabled = true;
			City2.GetComponent<MeshRenderer>().enabled = true;
			City3.GetComponent<MeshRenderer>().enabled = true;
			City4.GetComponent<MeshRenderer>().enabled = true;
			Mountain1.GetComponent<MeshRenderer>().enabled = true;
			Mountain2.GetComponent<MeshRenderer>().enabled = true;
		}
		public IEnumerator ChangingGravityIE()
        {
			yield return new WaitForSeconds(5);
			Physics.gravity = new Vector3(0, 0, plugin.normalGravity);
			yield return new WaitForSeconds(5);
			Physics.gravity = new Vector3(0, 0, -plugin.normalGravity);
			yield return new WaitForSeconds(5);
			Physics.gravity = new Vector3(plugin.normalGravity, 0, 0);
			yield return new WaitForSeconds(5);
			Physics.gravity = new Vector3(0, plugin.normalGravity, 0);
			yield return new WaitForSeconds(5);
			Physics.gravity = new Vector3(-plugin.normalGravity, 0, 0);
			yield return new WaitForSeconds(5);
			Physics.gravity = new Vector3(0, plugin.normalGravity, 0);
		}
		public IEnumerator resetTime()
		{
			yield return new WaitForSeconds(30);
			print("IEnumerator Working");
			Time.timeScale = plugin.normalDeltaTime;
			Time.fixedDeltaTime = plugin.normalFixedDeltaTime;
		}
		public IEnumerator resetGravity()
        {
			print("IEnumerator Working");
			yield return new WaitForSeconds(30);
			Physics.gravity = new Vector3(0, plugin.normalGravity, 0);
		}
		public IEnumerator showBody()
		{
			print("IEnumerator Working");
			yield return new WaitForSeconds(30);
			GameObject clone_body = GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/gorilla");
			clone_body.GetComponent<SkinnedMeshRenderer>().forceRenderingOff = false;
		}
		public IEnumerator resetGravity10s()
        {
			print("IEnumerator Working");
			yield return new WaitForSeconds(10);
			Physics.gravity = new Vector3(0, plugin.normalGravity, 0);
		}
		public IEnumerator RuberBandingIE()
        {
			Vector3 pos = GorillaLocomotion.Player.Instance.transform.position;
			yield return new WaitForSeconds(0.5f);
			GorillaLocomotion.Player.Instance.transform.position = pos;
			yield return new WaitForSeconds(0.5f);
			GorillaLocomotion.Player.Instance.transform.position = pos;
			yield return new WaitForSeconds(0.5f);
			GorillaLocomotion.Player.Instance.transform.position = pos;
			yield return new WaitForSeconds(0.5f);
			GorillaLocomotion.Player.Instance.transform.position = pos;
			yield return new WaitForSeconds(0.5f);
			GorillaLocomotion.Player.Instance.transform.position = pos;
			yield return new WaitForSeconds(0.5f);
			GorillaLocomotion.Player.Instance.transform.position = pos;
			yield return new WaitForSeconds(0.5f);
			GorillaLocomotion.Player.Instance.transform.position = pos;
			yield return new WaitForSeconds(0.5f);
			GorillaLocomotion.Player.Instance.transform.position = pos;
			yield return new WaitForSeconds(0.5f);
			GorillaLocomotion.Player.Instance.transform.position = pos;
			yield return new WaitForSeconds(0.5f);
			GorillaLocomotion.Player.Instance.transform.position = pos;
		}
		public IEnumerator ResetPlayerSize()
        {
			yield return new WaitForSeconds(30);
			GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1, 1, 1);
		}
		public IEnumerator ResetArm()
        {
			yield return new WaitForSeconds(30);
			GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/shoulder.L").transform.localScale = new Vector3(1, 1, 1);
			GameObject.Find("GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/shoulder.R").transform.localScale = new Vector3(1, 1, 1);
		}
		public IEnumerator EnableMovement()
        {
			yield return new WaitForSeconds(10);
			GorillaLocomotion.Player.Instance.disableMovement = false;
		}
		public void stopAllIE()
        {
			StopAllCoroutines();
        }
	}
}
