using System;
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
        // public double[] cellData;
        private SparseSolverTestv1 sim;
        private GameManager gm = null;
        // private float sTime;
        public bool single = false;
        private int numRows=0;
        private Stopwatch stopwatch;
        private long elapsed;
        private int times = 10;
        private float wtime;
        private float utime;
        private long elapsedMilliseconds;

        private bool append = false;
        // void Awake()
        // {   
        //     gm = GameManager.instance;
        //     sim = (SparseSolverTestv1)gm.activeSims[0];
        //     
        // }
        
        //Start is called on the frame when a script is enabled just before any of the Update methods are called the first time. This function can be a coroutine.
        public void Start()
        {   
            gm = GameManager.instance;
            sim = (SparseSolverTestv1)gm.activeSims[0];
            int size = sim.Neuron.nodes.Count;
            DateTime date = DateTime.Now;
            String formatted = date.ToString("MM-dd-yyyy-hh-mm-ss");
            String fname = "/neuro_visor_recording_"+formatted+".csv";
            String path = Application.dataPath + "/";
            if (!Directory.Exists(path + "CSV_Files")) Directory.CreateDirectory(path + "CSV_Files");
            path += "CSV_Files/";
            filename = path + fname;
            // TextWriter tw = new StreamWriter(filename, false);
            
            // tw.Write("Time (ms)");
            //
            // for (int i = 0; i < 800*times; i++)
            // { tw.Write(", Vert["+i+"]");
            // }
            // tw.Write(", Update ms");
            // tw.Write(", Write ms");
            
            
            // tw.Close();
            stopwatch = new Stopwatch();
            
            
            




        }

        // private void Update()
        // {
        //     stopwatch.Restart();
        //     cellData = sim.Get1DValues();
        //     sTime = sim.GetSimulationTime()*1000;
        //     stopwatch.Stop();
        //     elapsed = stopwatch.ElapsedMilliseconds;
        //     WriteToCSV();
        //     
        //     
        //     
        //
        // }
        
        

        public void WriteToCSV(float sTime, double [] cellData)
        {
            Stopwatch stopWatch = new Stopwatch();
            
            stopWatch.Restart();
            
            
            //100 rows * 100 vertices
            if (cellData.Length!=0 & numRows<=1)
            {
                numRows++;
                
                TextWriter tw = new StreamWriter(filename, append);
                tw.Write(+sTime);
                for (int j = 1; j <= times;j++)
                {


                    for (int i = 0; i < 600; i++)
                    {
                        tw.Write("," + cellData[i] * sim.unitScaler);
                    }
                }
                tw.Write("\n");


                stopWatch.Stop();
                // tw.Write((","+elapsed));
                utime += elapsed;
                elapsedMilliseconds = stopWatch.ElapsedMilliseconds;
                // tw.Write(","+elapsedMilliseconds);
                wtime += elapsedMilliseconds;
                tw.Close();
                append = true;
            }
            else
            {
                UnityEngine.Debug.Log("Update avg: " + (utime/100));
                UnityEngine.Debug.Log("Write avg: " + (wtime/100));
            }
            
            
            
            
            
            
            // string elapsedTime = $"{elapsedMilliseconds} ms";
            //
            //
            // UnityEngine.Debug.Log("Writing to CSV RunTime: " + elapsedTime);
            
            
        }
    }

}