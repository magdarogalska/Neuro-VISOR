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
        public double[] cellData;
        private SparseSolverTestv1 sim;
        private GameManager gm = null;
        private float sTime;
        public bool single = false;
        void Awake()
        {   
            gm = GameManager.instance;
            sim = (SparseSolverTestv1)gm.activeSims[0];
            if (single)
            {
                //get graph
                UnityEngine.Debug.Log("Got the graph");
                graph = GetComponent<NDLineGraph>();


            }
            
            

        }
        
        //Start is called on the frame when a script is enabled just before any of the Update methods are called the first time. This function can be a coroutine.
        private void Start()
        {   gm = GameManager.instance;
            sim = (SparseSolverTestv1)gm.activeSims[0];
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
            if (single == false)
            {
                for (int i = 0; i < size; i++)
                {
                    tw.Write(", Vert["+i+"]");
                }
            }
            else
            {
                tw.Write(", Vert["+graph.ndgraph.FocusVert+"]");
            }
            
            tw.Close();
            



        }

        private void Update()
        {
            
            cellData = sim.Get1DValues();
            sTime = sim.GetSimulationTime()*1000;
            
            WriteToCSV();
            
            
            

        }
        
        

        public void WriteToCSV()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            
            if (single)
            {
                TextWriter tw = new StreamWriter(filename, true);
                tw.Write("\n"+sTime);
                tw.Write(","+cellData[graph.ndgraph.FocusVert]*sim.unitScaler);
            }
            else if (cellData.Length > 0)
            {
                
                TextWriter tw = new StreamWriter(filename, true);
                tw.Write("\n"+sTime);
                for (int i = 0; i < cellData.Length; i++)
                {
                    tw.Write(","+cellData[i]*sim.unitScaler);
                }
            
                tw.Close();
            }
            
            
            stopWatch.Stop();
            
            long elapsedMilliseconds = stopWatch.ElapsedMilliseconds;
            
            string elapsedTime = $"{elapsedMilliseconds} ms";
            
            
            UnityEngine.Debug.Log("Writing to CSV RunTime: " + elapsedTime);
            
            
        }
    }

}