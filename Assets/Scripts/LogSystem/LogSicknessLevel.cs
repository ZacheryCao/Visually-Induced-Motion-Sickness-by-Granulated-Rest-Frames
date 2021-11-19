using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Valve.VR.InteractionSystem;
using GoogleSheetsToUnity;
using Valve.VR;

namespace LogSystem
{
    [RequireComponent(typeof(ReportHMDModel))]
    [RequireComponent(typeof(IPAddressReport))]
    public class LogSicknessLevel : Log
    {
        public enum Condition
        {
            GRF,
            NoGRF,
            none
        }
        public Condition condition;
        private string userID = "-1";
        private string controllerModel = "";
        private string hmdModel = "";
        private bool init = false;

        private string associatedSheet = "";
        private string associatedWorksheet = "";

        private string externalIP;
        private string localIP;

        private string siknesslevel = "";
        private string deltatime = "";
        private string averagespeed = "";
        private string waypointID = "";
        private string amountrotation = "";
        private string totalrotationtime = "";



        private void Awake()
        {
            ButtonsControllerMainScreen.OnSetUserID += BufferUserID;
            ReportHMDModel.OnReportHMDModel += BufferHMDModel;
            IPAddressReport.OnReportIpAddress += BufferIpAddresses;
            ReportHMDModel.OnReportControllerModel += BufferControllerModel;
        }
        private void Start()
        {
            associatedWorksheet = condition.ToString();
            BufferConditionInfoExperiment(condition);
        }

        private void Update()
        {
            if(!init && externalIP!=null)
            {
                StartCoroutine(BufferInitInfo());
                init = true;
            }
        }

        private void OnDestroy()
        {
            Close();
            ButtonsControllerMainScreen.OnSetUserID -= BufferUserID;
            ReportHMDModel.OnReportHMDModel -= BufferHMDModel;
            IPAddressReport.OnReportIpAddress -= BufferIpAddresses;
            ReportHMDModel.OnReportControllerModel -= BufferControllerModel;
        }

        IEnumerator BufferInitInfo()
        {
            yield return new WaitForSeconds(0.5f);
            Save(false);
        }

        private void BufferIpAddresses(string _externalIP, string _localIP)
        {
            externalIP = _externalIP;
            localIP = _localIP;
        }

        private void BufferHMDModel(string _hmdModel)
        {
            hmdModel = _hmdModel;
        }

        private void BufferControllerModel(string _controllerModel)
        {
            controllerModel = _controllerModel;
        }

        protected void BufferConditionInfoExperiment(Condition _condition)
        {
            Create(userID, _condition.ToString(), associatedWorksheet);
            condition = _condition;
            string header = "Date;Condition;ExternalIP;InternalIP;UserID;HMDModel;ControllerModel;Waypoint_ID;SiknessLevel;DeltaTime;Average Speed (m/s);Amount of Body Rotation (º)";
            WriteLine(header);
        }

        public void BufferUserID(string _id)
        {
            userID = _id;
        }


        public void LogPerformance(List<string> dataList)
        {
            waypointID = dataList[0];
            siknesslevel = dataList[1];
            deltatime = dataList[2];
            averagespeed = dataList[3];
            amountrotation = dataList[4];
            totalrotationtime = dataList[5];
            Save(false);
        }

        public void LogEarlyQuit()
        {
            Save(true);
        }

        protected override void Save(bool earlyQuit)
        {
            SaveLocal(earlyQuit);
            SaveRemote(earlyQuit);
        }

        private void SaveRemote(bool earlyQuit)
        {
            List<string> list = new List<string>();

            list.Add(System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            list.Add(condition.ToString());
            list.Add(externalIP);
            list.Add(localIP);
            list.Add(userID);
            list.Add(hmdModel);
            list.Add(controllerModel);
            list.Add(waypointID);
            if (earlyQuit)
            {
                list.Add("Early Quit");
            }
            else
            {
                list.Add(siknesslevel.ToString());
                list.Add(deltatime.ToString());
                list.Add(averagespeed.ToString());
                list.Add(amountrotation.ToString());
                list.Add(totalrotationtime.ToString());
            }
            SpreadsheetManager.Append(new GSTU_Search(associatedSheet, associatedWorksheet), new ValueRange(list), null);
        }

        private void SaveLocal(bool earlyQuit)
        {
            String line = "";
            line += System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ";";
            line += condition.ToString() + ";";
            line += externalIP + ";" + localIP + ";";
            line += userID + ";" + hmdModel + ";" + controllerModel + ";";
            if (earlyQuit)
            {
                line += "Early Quit;";
            }
            else
            {
                line += waypointID.ToString() + ";" + siknesslevel.ToString() + ";";
                line += deltatime.ToString() + ";" + averagespeed.ToString() + ";";
                line += amountrotation.ToString() + ";" + totalrotationtime.ToString() + ";";
            }
            WriteLine(line);
            Flush();
        }
    }
}