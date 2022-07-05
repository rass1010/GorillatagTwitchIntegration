using BepInEx;
using BepInEx.Configuration;
using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Utilla;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;
using System.Reflection;
using System.Collections;

namespace GorillaTagTwitchIntegration
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	[BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")] // Make sure to add Utilla 1.5.0 as a dependency!
	[ModdedGamemode]
	public class Plugin : BaseUnityPlugin
    {
		public GameObject canvas;
		public Text chat;
		public Text Timer;
		public GameObject gameObject;
		public float normalGravity;
		public float normalDeltaTime;
		public float normalFixedDeltaTime;
		public float normalJumpMultiplier;
		public float normalMaxJump;
		bool isConnectedToTwitch = false;

		Events eventClass;

		List<string> eventNames = new List<string>();
		List<Action> events = new List<Action>();

		public int vote1;
		public int vote2;
		public int vote3;
		public int vote4;

		Text choice1t;
		Text choice2t;
		Text choice3t;
		Text choice4t;

		bool inAllowedRoom = false;
		TcpClient Twitch;
		StreamReader Reader;
		StreamWriter Writer;

		const string URL = "irc.chat.twitch.tv";
		const int PORT = 6667;

		string User = "";

		string OAuth = "";
		string Channel = "";

		float PingCounter = 0;
		float time = 1;

		public static Plugin instance;
		ConfigEntry<string> us;
		ConfigEntry<string> oa;
		ConfigEntry<string> ch;

		private void ConnectToTwitch()
        {
            Twitch = new TcpClient(URL, PORT);
			Reader = new StreamReader(Twitch.GetStream());
			Writer = new StreamWriter(Twitch.GetStream());

			Writer.WriteLine("PASS " + OAuth);
			Writer.WriteLine("NICK " + User.ToLower());
			Writer.WriteLine("JOIN #" + Channel.ToLower());
			Writer.Flush();
        }

        void Awake()
        {
			HarmonyPatches.ApplyHarmonyPatches();
			var customFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "TwitchInfo.cfg"), true);
			us = customFile.Bind("Configuration", "UserName", "", "username");
			oa = customFile.Bind("Configuration", "OAuth", "", "oauth");
			ch = customFile.Bind("Configuration", "ChannelName", "", "channelname");
			User = us.Value;
			OAuth = oa.Value;
			Channel = ch.Value;
			instance = this;
			Utilla.Events.GameInitialized += GameInitialized;
		}

		private void GameInitialized(object sender, EventArgs e)
		{
			normalJumpMultiplier = GorillaLocomotion.Player.Instance.jumpMultiplier;
			normalMaxJump = GorillaLocomotion.Player.Instance.maxJumpSpeed;
			normalGravity = Physics.gravity.y;
			normalDeltaTime = Time.timeScale;
			normalFixedDeltaTime = Time.fixedDeltaTime;

			var fileStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GorillaTagTwitchIntegration.Assets.ui");
			var bundleLoadRequest = AssetBundle.LoadFromStreamAsync(fileStream);
			var myLoadedAssetBundle = bundleLoadRequest.assetBundle;

			var assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>("UiCanvas");

			GameObject uiCanvas = assetLoadRequest.asset as GameObject;
			canvas = Instantiate(uiCanvas);

			chat = canvas.transform.FindChild("Event").GetComponent<Text>();
			Timer = canvas.transform.FindChild("Time").GetComponent<Text>();
			choice1t = canvas.transform.FindChild("choice1").GetComponent<Text>();
			choice2t = canvas.transform.FindChild("choice2").GetComponent<Text>();
			choice3t = canvas.transform.FindChild("choice3").GetComponent<Text>();
			choice4t = canvas.transform.FindChild("choice4").GetComponent<Text>();

			eventClass = canvas.AddComponent<Events>();
			eventClass.RegisterEvents();

			ConnectToTwitch();
		}

		public void AddEvent(string name, Action Event)
        {
			eventNames.Add(name);
			events.Add(Event);
        }

		void Update()
        {
            if (inAllowedRoom) 
			{
				if (time > 0)
				{
					time -= Time.deltaTime;
					Timer.text = $"{time.ToString()}s";
				}
				else
				{
					vote1 = 0;
					vote2 = 0;
					vote3 = 0;
					vote4 = 0;
					time = 30;

					chooseEvents();
				}
				
			}

			PingCounter += Time.deltaTime;
			if (PingCounter > 60)
			{
				Writer.WriteLine("PING " + URL);
				Writer.Flush();
				PingCounter = 0;
			}
			if (Twitch.Available > 0)
			{
				string message = Reader.ReadLine();

				if (message.Contains("PRIVMSG"))
				{
					int splitpoint = message.IndexOf("!");
					string chatter = message.Substring(1, splitpoint - 1);

					splitpoint = message.IndexOf(":", 1);
					string msg = message.Substring(splitpoint + 1);

					print($"{chatter}: {msg}");
					if (inAllowedRoom)
					{
						if (msg.Equals("1"))
						{
							vote1 += 1;
						}
						if (msg.Equals("2"))
						{
							vote2 += 1;
						}
						if (msg.Equals("3"))
						{
							vote3 += 1;
						}
						if (msg.Equals("4"))
						{
							vote4 += 1;
						}
					}
				}
			}

		}

		void chooseEvents()
        {
			int rand1 = UnityEngine.Random.Range(0, events.Count);
			int rand2 = UnityEngine.Random.Range(0, events.Count);
			int rand3 = UnityEngine.Random.Range(0, events.Count);
			int rand4 = UnityEngine.Random.Range(0, events.Count);
			choice1t.text = $"1: {eventNames[rand1]}";
			StartCoroutine(addDelay(events[rand1], 1));
			choice2t.text = $"2: {eventNames[rand2]}";
			StartCoroutine(addDelay(events[rand2], 2));
			choice3t.text = $"3: {eventNames[rand3]}";
			StartCoroutine(addDelay(events[rand3], 3));
			choice4t.text = $"4: {eventNames[rand4]}";
			StartCoroutine(addDelay(events[rand4], 4));
		}

		IEnumerator addDelay(Action delay, int vote)
        {
			yield return new WaitForSeconds(30);
			if(vote == 1)
            {
				if (vote1 > vote2)
				{
					if (vote1 > vote3)
					{
						if (vote1 > vote4)
						{
							delay();
						}
					}
				}
			}
			else if(vote == 2)
            {
				if (vote2 > vote1)
				{
					if (vote2 > vote3)
					{
						if (vote2 > vote4)
						{
							delay();
						}
					}
				}
			}
			else if (vote == 3)
			{
				if (vote3 > vote1)
				{
					if (vote3 > vote2)
					{
						if (vote3 > vote4)
						{
							delay();
						}
					}
				}
			}
			else if (vote == 4)
			{
				if (vote4 > vote1)
				{
					if (vote4 > vote2)
					{
						if (vote4 > vote3)
						{
							delay();
						}
					}
				}
			}
		}

		[ModdedGamemodeJoin]
		private void RoomJoined(string gamemode)
		{
			// The room is modded. Enable mod stuff.
			inAllowedRoom = true;
			time = 0;
		}

		[ModdedGamemodeLeave]
		private void RoomLeft(string gamemode)
		{
			// The room was left. Disable mod stuff.
			inAllowedRoom = false;
			StopAllCoroutines();
			eventClass.stopAllIE();
			Physics.gravity = new Vector3(0, normalGravity, 0);
			Time.timeScale = normalDeltaTime;
			GorillaLocomotion.Player.Instance.transform.localScale = new Vector3(1, 1, 1);
			Time.fixedDeltaTime = normalFixedDeltaTime;

			eventClass.TreeRoom.GetComponent<MeshRenderer>().enabled = true;
			eventClass.Forest1.GetComponent<MeshRenderer>().enabled = true;
			eventClass.Forest2.SetActive(true);
			eventClass.Caves.GetComponent<MeshRenderer>().enabled = true;
			eventClass.Canyons.GetComponent<MeshRenderer>().enabled = true;
			eventClass.City1.GetComponent<MeshRenderer>().enabled = true;
			eventClass.City2.GetComponent<MeshRenderer>().enabled = true;
			eventClass.City3.GetComponent<MeshRenderer>().enabled = true;
			eventClass.City4.GetComponent<MeshRenderer>().enabled = true;
			eventClass.Mountain1.GetComponent<MeshRenderer>().enabled = true;
			eventClass.Mountain2.GetComponent<MeshRenderer>().enabled = true;
		}
	}
}
