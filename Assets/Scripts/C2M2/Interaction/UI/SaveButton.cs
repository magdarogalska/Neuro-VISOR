﻿using UnityEngine;
using C2M2.Utils;
using C2M2.NeuronalDynamics.Simulation;
using UnityEngine.UI;
namespace C2M2.Interaction.UI

{
    public class SaveButton : MonoBehaviour
    {
        private CSVWriter csv;
        private GameManager gm = null;
        private SparseSolverTestv1 sim;
        

        private void Start()
        {
            gm = GameManager.instance;
            sim = (SparseSolverTestv1)gm.activeSims[0];
        }
        public void StartCSV(bool single)
        {   //restrict user from saving multiple csv files, disable button after clicking
            csv = sim.graphManager.graphs[0].gameObject.AddComponent<CSVWriter>();
            csv.single = single;
            
            Debug.Log("Button Clicked!");
        }
        public void StopCSV()
        {
            Destroy(csv);
            Debug.Log("Stop button Clicked!");
        }
    }

}