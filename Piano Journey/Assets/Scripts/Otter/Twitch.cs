using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net.Sockets;
using System.ComponentModel;
using UnityEngine.UI;


public class Twitch : MonoBehaviour
{
    public string m_username; // 1
    public string m_password; 
    public string m_channelName; 

    private TcpClient m_twitchClient; // 2
    private StreamReader m_reader; // 3
    private StreamWriter m_writer; 
    private float m_reconnectTimer; // 4 
    private float m_reconnectAfter;

    [System.Serializable]
    public class ChatMessage
    {
        public string user;
        public string message;
    } 

    // Start is called before the first frame update
    void Start()
    {
        m_reconnectAfter = 60.0f;
        Connect();   
    }

    // Update is called once per frame
    void Update()
    {
         if (!m_twitchClient.Connected)
        {
            Connect();
        }

        if (m_twitchClient.Available == 0) // 1
        {
            m_reconnectTimer += Time.deltaTime; 
        }

        if (m_twitchClient.Available == 0 && m_reconnectTimer >= m_reconnectAfter) // 2
        {
            Connect(); 
            m_reconnectTimer = 0.0f; 
        }

        ReadChat();
    }

    private void Connect()
    {
        m_twitchClient = new TcpClient("irc.chat.twitch.tv", 6667); // 1
        m_reader = new StreamReader(m_twitchClient.GetStream()); // 2
        m_writer = new StreamWriter(m_twitchClient.GetStream()); 
        m_writer.WriteLine("PASS " + m_password); // 3
        m_writer.WriteLine("NICK " + m_username); 
        m_writer.WriteLine("USER " + m_username + " 8 *:" + m_username); 
        m_writer.WriteLine("JOIN #" + m_channelName); 
        m_writer.Flush(); 
        print("CONNECTED");
        
    }

    public ChatMessage ReadChat()
    {
        if (m_twitchClient.Available > 0) // 1
        {
            string message = m_reader.ReadLine();
            Debug.Log(message);
            

            if (message.Contains("PRIVMSG")) // 2
            {
                int splitPoint = message.IndexOf("!", 1); // 2
                string chatName = message.Substring(0, splitPoint); 
                chatName = chatName.Substring(1);

                //Get the message
                splitPoint = message.IndexOf(":", 1); 
                message = message.Substring(splitPoint + 1);
                ChatMessage chatMessage = new ChatMessage(); // 3
                chatMessage.user = chatName;
                chatMessage.message = message.ToLower();
                return chatMessage;
                
            }
        }
        else
        {
            print("NOT AVAILABLE");
        }
        return null; // 4
    }

}
