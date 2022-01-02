using UnityEngine;
using System.Collections;
using System.Net;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt.Utility;
using uPLibrary.Networking.M2Mqtt.Exceptions;
using UnityEngine.UI;
using System;
namespace M2MqttUnity.Program
{
	public class Main3 : M2MqttUnityClient {
		//Deklarasi variabel
	    private string brokerIpAddress;
		public InputField masukanPortBroker;
		public string topiknya;
		public string topikpublishnya;
		public string satuan;
		string msg = "";

		//Method membaca masukan address MQTT
	    public void ReadAddressInput(string s){
			brokerIpAddress=s;
		}
				
		//Method subscribe ke address MQTT
    	public void SubscribetoAddress()
    	{
        	client = new MqttClient(brokerIpAddress);
			// register to message received 
        	client.MqttMsgPublishReceived += client_MqttMsgPublishReceived; 
			string clientId = Guid.NewGuid().ToString(); 
			client.Connect(clientId); 
			// subscribe ke topik "Topic" dengan QoS 0 
			client.Subscribe(new string[] {topiknya}, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
    	} 

		//Method membaca masukan port MQTT
		public void SetBrokerPort(string brokerPort)
    	{
	        if (masukanPortBroker)
	        {
    	        int.TryParse(brokerPort, out this.brokerPort);
        	}
    	}

		//Method unsubscribe ke address MQTT
		public void UnsubscribetoAddress()
		{
			client.Unsubscribe(new string[] { topiknya });
		}

		//Method penerimaan pesan
		void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) 
		{ 
			try
			{
				msg = System.Text.Encoding.UTF8.GetString(e.Message).Trim();
			}
			catch{}
			//Publish pemberitahuan sudah diterima
			client.Publish(topikpublishnya, System.Text.Encoding.UTF8.GetBytes("TOPIC "+topikpublishnya+"RECEIEVED"), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
		} 

		// Update is called once per frame
		public Text ValueText;
		//Method Update
		protected override void Update () {
			ValueText.text=msg+satuan;
		}
	}
}