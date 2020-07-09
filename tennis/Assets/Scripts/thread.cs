using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
public class thread : MonoBehaviour
{
    bool done = false;
    // Use this for initialization
    void Start()
    {
        ThreadStart childref = new ThreadStart(CallToChildThread);
        Debug.Log("In Main: Creating the Child thread");

        Thread childThread = new Thread(childref);
        childThread.Start();
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("haha");


    }
    public static void CallToChildThread()
    {
        try
        {
            Debug.Log("Child thread starts");

            // do some work, like counting to 10
            for (int counter = 0; counter <= 100000000000000; counter++)
            {
                Thread.Sleep(500);
                Debug.Log(counter);
            }

            Debug.Log("Child Thread Completed");
        }
        catch (ThreadAbortException e)
        {
            Debug.Log("Thread Abort Exception");
        }
        finally
        {
            Debug.Log("Couldn't catch the Thread Exception");
        }
    }
}
