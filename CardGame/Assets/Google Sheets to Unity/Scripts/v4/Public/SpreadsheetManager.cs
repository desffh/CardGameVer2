﻿using System.Collections;
using System.Text;
using GoogleSheetsToUnity;
using GoogleSheetsToUnity.ThirdPary;
using TinyJSON;
using UnityEngine;
using UnityEngine.Networking;

public delegate void OnSpreedSheetLoaded(GstuSpreadSheet sheet);
namespace GoogleSheetsToUnity
{
    /// <summary>
    /// Partial class for the spreadsheet manager to handle all Public functions
    /// </summary>
    public partial class SpreadsheetManager
    {
        static GoogleSheetsToUnityConfig _config;
        /// <summary>
        /// Reference to the config for access to the auth details
        /// </summary>
        public static GoogleSheetsToUnityConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = (GoogleSheetsToUnityConfig)Resources.Load("GSTU_Config");
                }

                return _config;
            }
            set { _config = value; }
        }

        /// <summary>
        /// Read a public accessable spreadsheet
        /// </summary>
        /// <param name="searchDetails"></param>
        /// <param name="callback">event that will fire after reading is complete</param>
        public static void ReadPublicSpreadsheet(GSTU_Search searchDetails, OnSpreedSheetLoaded callback)
        {
            if (string.IsNullOrEmpty(Config.API_Key))
            {
                Debug.Log("Missing API Key, please enter this in the confie settings");
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("https://sheets.googleapis.com/v4/spreadsheets");
            sb.Append("/" + searchDetails.sheetId);
            sb.Append("/values");
            sb.Append("/" + searchDetails.worksheetName + "!" + searchDetails.startCell + ":" + searchDetails.endCell);
            sb.Append("?key=" + Config.API_Key);

            if (Application.isPlaying)
            {
                new Task(Read(UnityWebRequest.Get(sb.ToString()), searchDetails.titleColumn, searchDetails.titleRow, callback));
            }
#if UNITY_EDITOR
            else
            {
                EditorCoroutineRunner.StartCoroutine(Read(UnityWebRequest.Get(sb.ToString()), searchDetails.titleColumn, searchDetails.titleRow, callback));
            }
#endif


            /// <summary>
            /// Wait for the Web request to complete and then process the results
            /// </summary>
            /// <param name="www"></param>
            /// <param name="titleColumn"></param>
            /// <param name="titleRow"></param>
            /// <param name="callback"></param>
            /// <returns></returns>
            static IEnumerator Read(UnityWebRequest www, string titleColumn, int titleRow, OnSpreedSheetLoaded callback)
            {
                yield return www.SendWebRequest();  // 비동기적으로 웹 요청을 보냄

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    // 웹 요청이 성공하면 여기서 결과를 처리
                    string result = www.downloadHandler.text;
                    // 결과 처리 (예: callback 호출)
                }
            }


        }
    }
}