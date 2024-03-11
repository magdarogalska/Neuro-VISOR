using System.IO;
using UnityEngine;
using System.Collections.Generic;
using C2M2.NeuronalDynamics.Interaction.UI;


namespace C2M2.Utils
{
    
    public class CSVWriter : MonoBehaviour
    {
        private string filename;
        public List<Vector3> graphData;
        public NDLineGraph graph;
        private int i = 0;


        void Awake()
        {
            graph = GetComponentInParent<NDLineGraph>();
            
            
        }

        public void AddData(Vector3 data)
        {
            graphData.Add(data);

        }
        
        public void GenerateRandomData(int count)
        {
            graphData.Clear(); 

            
            for (int i = 0; i < count; i++)
            {
                float randomX = Random.Range(0f, 10f); 
                float randomY = Random.Range(0f, 10f); 

                Vector3 dataPoint = new Vector3(randomX, randomY, 0f);
                graphData.Add(dataPoint);
            }
        }
        
        
        private void Start()
        {   
            
            filename = Application.dataPath + "/test.csv";
            // GenerateRandomData(10);
            
            
        }

        private void Update()
        {
            graphData.Add(graph.positions[i]);
            // i++;
        }
        
        

        public void WriteToCSV()
        {
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
            
        }
    }

}