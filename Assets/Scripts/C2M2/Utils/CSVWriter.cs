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
        private string csvFilename;
        public NDLineGraph graph = null;
        // public double[] cellData;
        private SparseSolverTestv1 sim;
        private GameManager gm = null;
        // private float sTime;
        public bool single = false;
        private int numRows=0;
        private Stopwatch stopwatch;
        private long elapsed;
        private int times = 1;
        private float wtime;
        private float ctime;
        private long elapsedMilliseconds;
        private int size;

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
            size = sim.Neuron.nodes.Count;
            size = (size / 26) * 100;
            UnityEngine.Debug.Log("File size: " + size);
            DateTime date = DateTime.Now;
            String formatted = date.ToString("MM-dd-yyyy-hh-mm-ss");
            String fname = "/neuro_visor_recording_"+formatted;
            String path = Application.dataPath + "/";
            if (!Directory.Exists(path + "Binary_Files")) Directory.CreateDirectory(path + "Binary_Files");
            path += "Binary_Files/";
            filename = path + fname+".bin";
            csvFilename = path + fname + ".csv";
            // BinaryWriter bw = new BinaryWriter(File.Open(filename, append ? FileMode.Append : FileMode.Create));
            //
            // //TextWriter tw = new StreamWriter(filename, false);
            // bw.Write("Gating Variables,");
            // bw.Write("Time (ms)");
            // for (int i = 0; i < 110; i++)
            // { bw.Write(", Vert["+i+"]");
            // }
            //
            //
            // bw.Close();
            stopwatch = new Stopwatch();
            append = false;
            
            




        }

        public String getFileName()
        {
            return filename;
        }

        public void WriteToCSV(float sTime, double [] cellData, double [] M, double [] H, double []N)
        {
            Stopwatch stopWatch = new Stopwatch();
            
            stopWatch.Restart();
            
            
         
            if (cellData.Length!=0 & numRows<60*50)
            {
                numRows++;
                
                BinaryWriter bw = new BinaryWriter(File.Open(filename, append ? FileMode.Append : FileMode.Create));
                
              
                    bw.Write(sTime);

           
                    for (int j = 1; j <= times; j++)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            bw.Write(cellData[i%size] * sim.unitScaler);
                        }
                    }

           
                    for (int i = 0; i < size; i++)
                    {
                        bw.Write(M[i%size]);
                    }

         
                    for (int i = 0; i < size; i++)
                    {
                        bw.Write(H[i%size]);
                    }

          
                    for (int i = 0; i < size; i++)
                    {
                        bw.Write(N[i%size]);
                    }
                

                stopWatch.Stop();
                // tw.Write((","+elapsed));
                
                elapsedMilliseconds = stopWatch.ElapsedMilliseconds;
                // tw.Write(","+elapsedMilliseconds);
                wtime += elapsedMilliseconds;
                bw.Close();
                append = true;

            }
            else
            {
                //UnityEngine.Debug.Log("Update avg: " + (utime/100)+ " M: " + M.Length+ " H " + H.Length+ " N: " + N.Length);
                UnityEngine.Debug.Log("Write avg: " + (wtime));
            }
            
            
        }
        public void ConvertToCSV()
        {
            stopwatch.Restart();
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            using (StreamWriter csvWriter = new StreamWriter(csvFilename))
            {

                csvWriter.Write("Time (ms),");
                csvWriter.Write("Gating Variables");
                for (int i = 0; i < size; i++) csvWriter.Write($", Vert[{i}]");
                csvWriter.WriteLine();

                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
        
                    float sTime = reader.ReadSingle();
                    csvWriter.Write(sTime + ",");

             
                    for (int i = 0; i < size; i++)
                    {
                        double cellValue = reader.ReadDouble();
                        csvWriter.Write(cellValue + ",");
                    }
                    csvWriter.WriteLine();

         
                    csvWriter.Write("M,");
                    for (int i = 0; i < size; i++)
                    {
                        double mValue = reader.ReadDouble();
                        csvWriter.Write(mValue + (i < size-1 ? "," : ""));
                    }
                    csvWriter.WriteLine();

              
                    csvWriter.Write("H,");
                    for (int i = 0; i < size; i++)
                    {
                        double hValue = reader.ReadDouble();
                        csvWriter.Write(hValue + (i < size-1 ? "," : ""));
                    }

                    csvWriter.WriteLine();
                    
                    csvWriter.Write("N,");
                    for (int i = 0; i < size; i++)
                    {
                        double nValue = reader.ReadDouble();
                        csvWriter.Write(nValue + (i < size-1 ? "," : ""));
                    }
                    csvWriter.WriteLine();
                }
            }

            ctime = stopwatch.ElapsedMilliseconds;
            UnityEngine.Debug.Log("Conversion time: "+ ctime);
        }
    }

}