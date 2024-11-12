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
        private SparseSolverTestv1 sim;
        private GameManager gm = null;
        private long elapsed;
        private int times = 1;
        private float wtime;
        private float ctime;
        private long elapsedMilliseconds;
        private int size;

        private bool append = false;
        public void Start()
        {   
            gm = GameManager.instance;
            sim = (SparseSolverTestv1)gm.activeSims[0];
            size = sim.Neuron.nodes.Count;
            DateTime date = DateTime.Now;
            String formatted = date.ToString("MM-dd-yyyy-hh-mm-ss");
            String fname = "/neuro_visor_recording_"+formatted;
            String path = Application.dataPath + "/";
            if (!Directory.Exists(path + "Binary_Files")) Directory.CreateDirectory(path + "Binary_Files");
            path += "Binary_Files/";
            filename = path + fname+".bin";
            csvFilename = path + fname + ".csv";
            append = false;
            
            




        }

        public void WriteToCSV(float sTime, double [] cellData, double [] M, double [] H, double []N)
        {
            Stopwatch stopWatch = new Stopwatch();
            
            stopWatch.Restart();



            if (cellData.Length != 0)
            {


                BinaryWriter bw = new BinaryWriter(File.Open(filename, append ? FileMode.Append : FileMode.Create));


                bw.Write(sTime);


                for (int j = 1; j <= times; j++)
                {
                    for (int i = 0; i < size; i++)
                    {
                        bw.Write(cellData[i] * sim.unitScaler);
                    }
                }


                for (int i = 0; i < size; i++)
                {
                    bw.Write(M[i]);
                }


                for (int i = 0; i < size; i++)
                {
                    bw.Write(H[i]);
                }


                for (int i = 0; i < size; i++)
                {
                    bw.Write(N[i]);
                }


                stopWatch.Stop();
                
                bw.Close();
                append = true;

            }


        }
        public void ConvertToCSV()
        {
            
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            using (StreamWriter csvWriter = new StreamWriter(csvFilename))
            {

                csvWriter.Write("Time (ms),");
                for (int i = 0; i < size; i++) csvWriter.Write($"Vert[{i}],");
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
            
        }
    }

}