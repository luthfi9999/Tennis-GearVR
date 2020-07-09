using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Kelas untuk mengambil quaternion dari gearvr controller
public class rotation : MonoBehaviour {
    public Text data;
    double timer2;
    double timer;
    Quaternion last,rot,baru;
    bool hold;
    double start;
    float[,] datas;
    string[] datastr;
    string[] datastr2;
    int counter = 0;
    bool ready = false;
    int index = 0;
    int index2 = 0;
    bool saved = false;
    string path;
    // Use this for initialization
    void Start () {

        path = Application.persistentDataPath;
        datas = new float[20,20];
        datastr = new string[20];
        datastr2 = new string[20];
        data.text = "";
        timer2 = 0;
        timer = 0;
        last.Set(0, 0, 0, 0);
        hold = false;
        
    }

    // Update is called once per frame
    void Update() {
        OVRInput.Update();
        //data.text = path;
        timer += (double)Time.deltaTime;
        if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            index2 = 0;
            ready = true;
        }
        if (!OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            hold=false;
        }
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {

            if (ready)
            {
                getData();
            }
         
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            if (!hold)
            {
                if (index < 20)
                {
                    counter = 0;
                    data.text = "";
                    index++;
                }
                else
                {
                    data.text = "indexH";
                    check();
                    saveData();
                    saveTarget();
                    SceneManager.LoadScene(0);
                }
                hold = true;
            }

            //data.text = "okay";
        }
     
        if (OVRInput.Get(OVRInput.Button.Back))
        {
            hold = false;
            Array.Clear(datas, 0, 300);
            counter = 0;
            index = 0;
            index2 = 0;
            data.text = "";
            saved = false;
            Array.Clear(datastr, 0, 20);
            System.IO.File.WriteAllLines(path + "/tes.txt", datastr);
            //data.text = "ok ok";
        }

    }
    void check()
    {
        for(int i = 0; i < 20; i++)
        {
            for(int j = 0; j < 20; j++)
            {
                if (datas[i, j] == null)
                {
                    datas[i, j] = 0;
                }
            }
        }
    }
    void getData()
    {
        if (timer - timer2 >= 0.04 && counter < 6 && index < 20)
        {
            if (counter == 0)
            {
                last = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
            }
            else
            {
                data.text += index.ToString()+" ";
                baru = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
                rot = baru * Quaternion.Inverse(last);
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {
                        data.text += " x"+index+"="+Math.Round(rot.x,2).ToString();
                        datas[index, index2++] = rot.x;
                    }
                    else if (i == 1)
                    {
                        data.text += " y" + index + "=" + Math.Round(rot.y, 2).ToString();
                        datas[index, index2++] = rot.y;
                    }
                    else if (i == 2)
                    {
                        data.text += " z" + index + "=" + Math.Round(rot.z, 2).ToString();
                        datas[index, index2++] = rot.z;
                    }
                    else
                    {
                        data.text += " w" + index + "=" + Math.Round(rot.w, 2).ToString();
                        datas[index, index2++] = rot.w;
                    }
                }
                data.text += "\n";
                last = baru;
            }
            //data.text += "waktu: " + timer + "\t";
            //data.text += rot + "\n";
            timer2 = timer;
            counter++;
        }
    }
    void saveTarget()
    {
        for (int i = 0; i < 20; i++)
        {
            if (i < 5)
            {
                datastr2[i] += "1|0|0|0";
            }
            else if (i < 10)
            {
                datastr2[i] += "0|1|0|0";
            }
            else if (i < 15)
            {
                datastr2[i] += "0|0|1|0";
            }
            else
            {
                datastr2[i] += "0|0|0|1";
            }
        }
        data.text = "target saved";
        System.IO.File.WriteAllLines(path + "/target.txt", datastr2);
    }
    void saveData()
    {
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                if (j == 19)
                {
                    datastr[i] += datas[i, j].ToString();
                }
                else
                {
                    datastr[i] += datas[i, j].ToString() + "|";
                }
            }
        }
        data.text = "saved";
        System.IO.File.WriteAllLines(path + "/data.txt", datastr);
        saved = true;
    }
    void printData()
    {
        var fileStream = new FileStream(path + "/data.txt", FileMode.Open, FileAccess.Read);
        string[] lines = new string[40];
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                lines = line.Split('|');
                for (int j = 0; j < 20; j++)
                {
                    //wjk[counter, j] = Double.Parse(lines[j]);
                    data.text += lines[j];
                    //ok[counter] += wjk[counter, j].ToString() + "|";
                }
                data.text += "|||";
                counter++;
                if (counter == 20)
                {
                    break;
                }
            }
        }
    }
}
