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

     
    public List<String> m_SongList = new List<String>();

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

    public void ReadChat() // 1
    {
        if (m_twitchClient.Available > 0)
        {
            string message = m_reader.ReadLine();

            if (message.Contains("PRIVMSG"))
            {
                //Get the users name by splitting it from the string
                var splitPoint = message.IndexOf("!", 1);
                var chatName = message.Substring(0, splitPoint);
                chatName = chatName.Substring(1);

                //Get the users message by splitting it from the string
                splitPoint = message.IndexOf(":", 1);
                message = message.Substring(splitPoint + 1);
                print(String.Format("{0}: {1}", chatName, message));

                GameInputs(message);
            }
            
        }
    }


    private void GameInputs(string ChatInputs)
    {
        Debug.Log(ChatInputs);
        var msg = ChatInputs.IndexOf("-");
        var commandString = ChatInputs.Substring(0,msg);
        var commandSong = ChatInputs.Substring(msg+1);
        Debug.Log("command: " + commandString);
        Debug.Log("command: " + commandSong);
        if(commandString.ToLower() == "!sr")
        {
            Debug.Log("ADD COMMAND");
            Debug.Log(commandSong);
            m_SongList.Add(commandSong);
            Debug.Log(m_SongList.Count);
        }
        if(commandString.ToLower() == "!del")
        {
            Debug.Log("DELETE COMMAND");
            m_SongList.Remove(commandSong);
            Debug.Log(m_SongList.Count);
        }
    }
}
