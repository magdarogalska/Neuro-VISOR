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
        public List<Vector3> graphData;
        public NDLineGraph graph;
        public double[] cellData;
        private SparseSolverTestv1 sim;
        private GameManager gm = null;
        private float sTime;
        void Awake()
        {
            graph = GetComponentInParent<NDLineGraph>();
            

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
            for (int i = 0; i < size; i++)
            {
                tw.Write(", Vert["+i+"]");
            }
            tw.Close();
            //String txt_name = "/neuro_visor_recording_"+formatted+".txt";
            //String txtname = Application.dataPath + txt_name;
            //tw = new StreamWriter(txtname, false);
            //tw.WriteLine("Start of the recording: ");
            //tw.Close();
            //write start and end time of the recording, start and end times of the neurons
            //in the room?, the information from the indices, the neuron and vertex indices
            //from which the data is saved from



        }

        private void Update()
        {
            UnityEngine.Debug.Log("Graphing vertex: "+graph.ndgraph.FocusVert);
            cellData = sim.Get1DValues();
            sTime = sim.GetSimulationTime()*1000;
            // if (graphData.Count > 500)
            // {
            //     WriteToCSV();
            //     graphData.Clear();
            //     
            // }
            // else
            // { 
            // graphData.Add(graph.positions[graph.ndgraph.FocusVert]);
            // }
            WriteToCSV();
            
            
            

        }
        
        

        public void WriteToCSV()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            // if (graphData.Count > 0)
            // {
            //     
            //     TextWriter tw = new StreamWriter(filename, true);
            //     tw.Write("\n"+graphData[0].x+","+graphData[0].y);
            //     for (int i = 0; i < 1000; i++)
            //     {
            //         tw.Write(","+graphData[0].y);
            //     }
            //
            //     tw.Close();
            // }
            if (cellData.Length > 0)
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