using System;
using System.IO;
using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;
using C2M2.NeuronalDynamics.Interaction.UI;
using Random = UnityEngine.Random;


namespace C2M2.Utils
{
    
    public class CSVWriter : MonoBehaviour
    {
        private string filename;
        public List<Vector3> graphData;
        public NDLineGraph graph;
        
        


        void Awake()
        {
            graph = GetComponentInParent<NDLineGraph>();
            
            


        }

        public void AddData(Vector3 data)
        {
            graphData.Add(data);

        }
        
        
        // public void GenerateRandomData(int count)
        // {
        //     graphData.Clear(); 
        //
        //     
        //     for (int i = 0; i < count; i++)
        //     {
        //         float randomX = Random.Range(0f, 10f); 
        //         float randomY = Random.Range(0f, 10f); 
        //
        //         Vector3 dataPoint = new Vector3(randomX, randomY, 0f);
        //         graphData.Add(dataPoint);
        //     }
        // }
        
        //Start is called on the frame when a script is enabled just before any of the Update methods are called the first time. This function can be a coroutine.
        private void Start()
        {
            
            DateTime date = DateTime.Now;
            String formatted = date.ToString("MM-dd-yyyy-hh-mm-ss");
            String fname = "/neuro_visor_recording_"+formatted+".csv";
            filename = Application.dataPath + fname;
            


        }

        private void Update()
        {
            UnityEngine.Debug.Log("Graphing vertex: "+graph.ndgraph.FocusVert);
            graphData.Add(graph.positions[graph.ndgraph.FocusVert]);
            

        }
        
        

        public void WriteToCSV()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            if (graphData.Count > 0)
            {
                TextWriter tw = new StreamWriter(filename, false);
                tw.WriteLine("Time (ms) , Voltage (mV)");
                tw.Close();
                tw = new StreamWriter(filename, true);
                for (int i = 0; i < graphData.Count; i++)
                {
                    tw.WriteLine(graphData[i].x+","+graphData[i].y);
                }

                tw.Close();
            }
            stopWatch.Stop();
            

            // Format and display the TimeSpan value.
            long elapsedMilliseconds = stopWatch.ElapsedMilliseconds;

            string elapsedTime = $"{elapsedMilliseconds} ms";

            
            UnityEngine.Debug.Log("Writing to CSV RunTime: " + elapsedTime);
            
            
        }
    }

}