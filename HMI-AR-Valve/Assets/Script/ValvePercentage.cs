﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

namespace M2MqttUnity.Program
{
    public class ValvePercentage : M2MqttUnityClient
    {
        //Deklarasi variabel
        public InputField masukanAlamatBroker;
        public InputField masukanPortBroker;
        public Button connectButton;
        public Button disconnectButton;
        public Button testPublishButton;
        public Text messageValue;
        public string topiknya;
        private ValveMovement valveMovement;
        private int valveValue;
        private List<string> eventMessages = new List<string>();
        private bool updateUI = false;
        private string MQTTPublishInput;


        //Method komunikasi dengan script valvemovement
        protected override void Awake(){
            valveMovement=GameObject.FindObjectOfType<ValveMovement>();
        }

        //Method membaca input untuk publish
        public void ReadMQTTInput(string s)
        {
            MQTTPublishInput=s;
        }

        //Method Publish
        public void TestPublish()
        {
            client.Publish(topiknya, System.Text.Encoding.UTF8.GetBytes(MQTTPublishInput), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, false);
            updateUI = true;
        }

        //Method mengatur Address Broker
        public void SetBrokerAddress(string brokerAddress)
        {
            if (masukanAlamatBroker && !updateUI)
            {
                this.brokerAddress = brokerAddress;
            }
        }

        //Method Mengatur Port Broker
        public void SetBrokerPort(string brokerPort)
        {
            if (masukanPortBroker && !updateUI)
            {
                int.TryParse(brokerPort, out this.brokerPort);
            }
        }

        //Method Connecting ke Broker dan Port
        protected override void OnConnecting()
        {
            base.OnConnecting();
            updateUI = true;
        }

        //Method setelah Terkoneksi ke Broker
        protected override void OnConnected()
        {
            base.OnConnected();
            updateUI = true;
        }

        //Method Subscribe
        protected override void SubscribeTopics()
        {
            client.Subscribe(new string[] {topiknya}, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        }
        
        //Method Unsubscribe
        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new string[] {topiknya});
        }

        //Method ConnectionFailed
        protected override void OnConnectionFailed(string errorMessage)
        {
            updateUI = true;
        }

        //Method Disconnect
        protected override void OnDisconnected()
        {
            updateUI = true;
        }

        //Method Connection Lost
        protected override void OnConnectionLost()
        {
            updateUI = true;
        }

        //Method Update Tombol UI
        private void UpdateUI()
        {    
            if (client == null)
            {
                if (connectButton != null)
                {
                    connectButton.interactable = true;
                    disconnectButton.interactable = false;
                    testPublishButton.interactable = false;
                }
            }
            else
            {
                if (testPublishButton != null)
                {
                    testPublishButton.interactable = client.IsConnected;
                }
                if (disconnectButton != null)
                {
                    disconnectButton.interactable = client.IsConnected;
                }
                if (connectButton != null)
                {
                    connectButton.interactable = !client.IsConnected;
                }
            }
            if (masukanAlamatBroker != null && connectButton != null)
            {
                masukanAlamatBroker.interactable = connectButton.interactable;
                masukanAlamatBroker.text = brokerAddress;
            }
            
            if (masukanPortBroker != null && connectButton != null)
            {
                masukanPortBroker.interactable = connectButton.interactable;
                masukanPortBroker.text = brokerPort.ToString();
            }

            if (connectButton != null)
            {
                connectButton.interactable = connectButton.interactable;
            }
            
            updateUI = false;
            
        }
        
        //Method mulai
        protected override void Start()
        {
            updateUI = true;
            base.Start();
        }
        
        //Method decode
        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            StoreMessage(msg);
        }
        
        //Method penyimpanan pesan
        private void StoreMessage(string eventMsg)
        {
            eventMessages.Add(eventMsg);
        }
        
        //Method tampilkan pesan
        private void ProcessMessage(string msg)
        {
            updateUI = true;
            messageValue.text=msg+"%";
            //Konversi ke int
            valveValue=Convert.ToInt32(msg);
            valveMovement.UpdateValue(valveValue);
        }
        
        //Method Update pesan
        protected override void Update()
        {
            base.Update(); // call ProcessMqttEvents()

            if (eventMessages.Count > 0)
            {
                foreach (string msg in eventMessages)
                {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }
            if (updateUI)
            {
                UpdateUI();
            }
        }
        
        //Method forced quit
        private void OnDestroy()
        {
            Disconnect();
        }

    }
}