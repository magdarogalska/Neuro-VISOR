﻿using System;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;
using C2M2.NeuronalDynamics.Interaction.UI;
using C2M2.NeuronalDynamics.Simulation;
using C2M2.Interaction;
using C2M2.Simulation;
using Debug = UnityEngine.Debug;



namespace C2M2.Utils
{
    
    public class CSVWriter : MonoBehaviour
    {
        private string filename;
        public NDLineGraph graph = null;
        public double[] cellData;
        private SparseSolverTestv1 sim;
        private GameManager gm = null;
        private float sTime;
        public bool single = false;
        private int numRows=0;
        private Stopwatch stopwatch;
        private long elapsed;
        private int times = 1;
        private float wtime;
        private float utime;
        void Awake()
        {   
            gm = GameManager.instance;
            sim = (SparseSolverTestv1)gm.activeSims[0];
            
        }
        
        //Start is called on the frame when a script is enabled just before any of the Update methods are called the first time. This function can be a coroutine.
        private void Start()
        {   
            int size = sim.Neuron.nodes.Count;
            
            DateTime date = DateTime.Now;
            String formatted = date.ToString("MM-dd-yyyy-hh-mm-ss");
            String fname = "/neuro_visor_recording_"+formatted+".csv";
            String path = Application.dataPath + "/";
            if (!Directory.Exists(path + "CSV_Files")) Directory.CreateDirectory(path + "CSV_Files");
            path += "CSV_Files/";
            filename = path + fname;
            TextWriter tw = new StreamWriter(filename, false);
            
            tw.Write("Time (ms)");
            
            for (int i = 0; i < size*times; i++)
            { tw.Write(", Vert["+i+"]");
            }
            tw.Write(", Update ms");
            tw.Write(", Write ms");
            
            
            tw.Close();
            stopwatch = new Stopwatch();
            
            




        }

        private void Update()
        {
            stopwatch.Restart();
            cellData = sim.Get1DValues();
            sTime = sim.GetSimulationTime()*1000;
            stopwatch.Stop();
            elapsed = stopwatch.ElapsedMilliseconds;
            WriteToCSV();
            
            
            

        }
        
        

        public void WriteToCSV()
        {
            Stopwatch stopWatch = new Stopwatch();
            long elapsedMilliseconds = 0;
            stopWatch.Restart();
            
            
            
            if (cellData.Length > 0 && numRows<1000)
            {
                numRows++;
                
                TextWriter tw = new StreamWriter(filename, true);
                tw.Write("\n"+sTime);
                for (int j = 1; j <= times;j++)
                {


                    for (int i = 0; i < cellData.Length; i++)
                    {
                        tw.Write("," + cellData[i] * sim.unitScaler);
                    }
                }


                stopWatch.Stop();
                tw.Write((","+elapsed));
                utime += elapsed;
                elapsedMilliseconds = stopWatch.ElapsedMilliseconds;
                tw.Write(","+elapsedMilliseconds);
                wtime += elapsedMilliseconds;
                tw.Close();
            }
            else
            {
                UnityEngine.Debug.Log("Update avg: " + (utime/1000));
                UnityEngine.Debug.Log("Write avg: " + (wtime/1000));
            }
            
            
            
            
            
            
            // string elapsedTime = $"{elapsedMilliseconds} ms";
            //
            //
            // UnityEngine.Debug.Log("Writing to CSV RunTime: " + elapsedTime);
            
            
        }
    }

}