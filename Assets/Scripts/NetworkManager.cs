using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NetMQ;
using NetMQ.Sockets;
using UnityEngine;

public class NetworkManager : MonoBehaviour {

	void StartServer()
	{
		Debug.Log("Start Server!");
		using (PullSocket pullSocket = new PullSocket())
		using (PublisherSocket publisherSocket = new PublisherSocket())
		{
			pullSocket.Bind("tcp://*:20002");
			publisherSocket.Bind("tcp://*:20001");
			
			while (true)
			{
				string str = pullSocket.ReceiveFrameString();
				Debug.Log("[Server] Get! :: " + str);
				publisherSocket.SendFrame("Hello Client!");
				Debug.Log("[Server] Send Hello Client!");
			}
		}
		Debug.Log("Close Server!");
	}

	void StartClient()
	{
		Debug.Log("Client is On!!");
		using (var pushSocket = new PushSocket())
		using (var subscriberSocket = new SubscriberSocket())
		{
			pushSocket.Connect("tcp://localhost:20002");
			subscriberSocket.Connect("tcp://localhost:20001");
			subscriberSocket.Subscribe("topicA");
			
			Thread.Sleep(TimeSpan.FromSeconds(2));
			
			Debug.Log("Send !!");
			pushSocket.SendFrame("Hello Server First!");
			while (true)
			{
				string str = subscriberSocket.ReceiveFrameString();
				Debug.Log("[Client] Get! :: " + str);
				pushSocket.SendFrame("Hello Server!");
				Debug.Log("[Client] Send Hello Server!");
				
//				Thread.Sleep(TimeSpan.FromSeconds(1));
			}
			
		}
	}
	
	// Use this for initialization
	void Start () {
		new Thread(StartServer).Start();
		Thread.Sleep(TimeSpan.FromSeconds(3));
		new Thread(StartClient).Start();

//		thread.Starat();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
