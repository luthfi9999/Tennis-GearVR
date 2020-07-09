using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ran = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System.Threading;
public class peramalan : MonoBehaviour {
    public Text zzz;
    string[] lines;
    string[] allData;
    string path;
    double[,] vij, wjk;
    double[] v0j, w0k;
    double[] z_inj, zj, y_ink, yk;
    int jInput, jHidden, jOutput;
    //public Text data;
    double timer2;
    float t1=0, t2=0;
    double timer;
    double seb, ses,temp,temp2;
    //public racketRotation racket;
    Quaternion last, rot, baru;
    public bool ready;
    Player player; 
    float[] datas;
    double[] datad;
    int counter = 0;
    int index2 = 0;
    int index = 0;
    bool hold = false;
    // Use this for initialization
    void Start () {
        player = this.GetComponent<Player>();
        // racket = gameObject.GetComponent<racketRotation>();
        path = Application.persistentDataPath;
        jInput = 20;
	    ses=0;
	    seb=0;
        jHidden = 5;
        jOutput = 4;
        allData = new string[300];
        lines = new string[jInput];
        vij = new double[jInput, jHidden];
        wjk = new double[jHidden, jOutput];
        v0j = new double[jHidden];
        w0k = new double[jOutput];
        datad = new double[jInput];
        z_inj = new double[jHidden];
        zj = new double[jHidden];
        y_ink = new double[jOutput];
        yk = new double[jOutput];
        //path = Application.persistentDataPath;
        datas = new float[20];
       // data.text = "";
        timer2 = 0;
        timer = 0;
        last.Set(0, 0, 0, 0);
        ready = false;
        // 
        // setTarget();
        getBobot();
    }
	
	// Update is called once per frame
	void Update () {
        OVRInput.Update();
        //data.text = "LOB";
        timer += (double)Time.deltaTime;
        if (!OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
	     
            ready = true;
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (ready)
            {
                getData();
            }

        }
        if (!OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            hold = false;
        }
            if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            if (!hold)
            {
            
                zzz.text="";
                //zzz.text=(seb).ToString();
                seb=(double)Time.realtimeSinceStartup;
                zzz.text+="seb"+((double)(seb)).ToString()+"||";
                //zzz.text+=(seb).ToString();
                //data.text = "";
                index2 = 0;
                saveData();
                setData();
                counter = 0;
                peramalanku();
                 ses=(double)Time.realtimeSinceStartup;
                zzz.text+="ses"+((double)(ses)).ToString()+"||";
                //zzz.text+=(ses).ToString();
                temp=(double)(ses-seb);
                zzz.text+="temp"+((double)(temp)).ToString()+"||";
                index++;
                hold = true;
            }
        }
        if (OVRInput.Get(OVRInput.Button.Back))
        {
            index = 0;
            System.IO.File.WriteAllLines(path + "/dataku.txt", allData);
            Array.Clear(allData, 0, allData.Length);
            Array.Clear(datas, 0, datas.Length);
            SceneManager.LoadScene(0);
        }
    }
    //fungsi untuk mengambil data
    void setData()
    {
        // set data dari txt
        var fileStream = new FileStream(path + "/ramal.txt", FileMode.Open, FileAccess.Read);
        //string[] ok = new string[jTrain];
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string line;

            while ((line = streamReader.ReadLine()) != null)
            {

                lines = line.Split('|');


                for (int j = 0; j < lines.Length; j++)
                {

                    datad[j] = Double.Parse(lines[j]);
                   // ok[counter] += data[counter, j].ToString() + "|";
                    //data[counter, j] = Double.Parse(dataString[counter, j]);
                }
            }


        }
    }
    //fungsi untuk menyimpan data
    void saveData()
    {
        //data.text += "saveData";
        string ramal="";
        for (int i = 0; i < 20; i++)
        {
            if (i == 19)
            {
                allData[index] += datas[i];
                ramal += datas[i];
            }
            else
            {
                allData[index] += datas[i] + "|";
                ramal += datas[i]+"|";
            }
            
        }
        System.IO.File.WriteAllText(path + "/ramal.txt", ramal);
    }
    //fungsi untuk mengambil data
    void getData()
    {
        //data.text += "get";
        if (timer - timer2 >= 0.04 && counter < 6 && index2 < 20)
        {
            if (counter == 0)
            {
		ses=timer;
                last = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
            }
            else
            {
                baru = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
                rot = baru * Quaternion.Inverse(last);
                for (int i = 0; i < 4; i++)
                {
                    if (i == 0)
                    {                        
                        datas[index2++] = rot.x;
                    }
                    else if (i == 1)
                    {
                        datas[index2++] = rot.y;
                    }
                    else if (i == 2)
                    {
                        datas[index2++] = rot.z;
                    }
                    else
                    {
                        datas[index2++] = rot.w;
                    }
                }
                last = baru;
            }
            timer2 = timer;
            counter++;
        }
    }
   // fungsi untuk mengambil bobot yang sudah disimpan sebelumnya

    void getBobot()
    {
        getVij();
        getV0j();
        getWjk();
        getW0k();
    }
    void getVij()
    {
        // fungsi untuk mengambil bobot yang menghubungkan silnya masukan dengan sinyal tersembunyi yang sudah disimpan sebelumnya
        int counter = 0;
        var fileStream = new FileStream(path + "/bobotvij.txt", FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                lines = line.Split('|');
                for (int j = 0; j < jHidden; j++)
                {
                    vij[counter, j] = Double.Parse(lines[j]);
                    //ok[counter] += vij[counter,j].ToString() + "|";
                }
                counter++;
                if (counter == jInput)
                {
                    break;
                }
            }

        }
    }
    void getWjk()
    {
        // fungsi untuk mengambil bobot yang menghubungkan sinyal tersembunyi dengan sinyal keluaran
        int counter = 0;
        var fileStream = new FileStream(path + "/bobotwjk.txt", FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                lines = line.Split('|');
                for (int j = 0; j < jOutput; j++)
                {
                    wjk[counter, j] = Double.Parse(lines[j]);
                    //ok[counter] += wjk[counter, j].ToString() + "|";
                }
                counter++;
                if (counter == jHidden)
                {
                    break;
                }
            }
        }
    }
    //fungsi untuk mendapatkan bobot bias sinyal keluaran
    void getW0k()
    {
        int counter = 0;
        var fileStream = new FileStream(path + "/bobotW0k.txt", FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string line;
            string[] dataS = new string[jInput];
            while ((line = streamReader.ReadLine()) != null)
            {
                lines = line.Split('|');
                for (int i = 0; i < jOutput; i++)
                {
                    w0k[i] = Double.Parse(lines[i]);
                    //Debug.Log(w0k[i]);
                }
                counter++;
                if (counter == 1)
                {
                    break;
                }
            }

        }
    }
    //fungsi untuk mendapatkan bobot bias sinyal tersembunyi
    void getV0j()
    {
        int counter = 0;
        var fileStream = new FileStream(path + "/bobotv0j.txt", FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string line;
            string[] dataS = new string[jInput];
            while ((line = streamReader.ReadLine()) != null)
            {
                lines = line.Split('|');
                //ok[counter] += line.ToString() + "|";
                for (int i = 0; i < jHidden; i++)
                {
                    v0j[i] = Double.Parse(lines[i]);
                    //ok[counter] += v0j[i].ToString() + "|";
                }
                counter++;
                if (counter == 1)
                {
                    break;
                }
            }

        }
    }
    //fungsi untuk melakukan peramalan
    public void peramalanku()
    {
        //data.text += "ramal";
        double maxx = -100;
        int hasil = 0;
        Array.Clear(z_inj, 0, z_inj.Length);
        Array.Clear(y_ink, 0, y_ink.Length);

        //string[] ok = new string[jHidden];
        for (int j = 0; j < jHidden; j++)
        {
            for (int i = 0; i < jInput; i++)
            {
                z_inj[j] += datad[i] * vij[i, j];
                //ok[j] += z_inj[j].ToString() + "|";
            }
            z_inj[j] += v0j[j];
            zj[j] = (1 / (1 + Math.Exp(-z_inj[j])));
        }
        for (int k = 0; k < jOutput; k++)
        {
            for (int j = 0; j < jHidden; j++)
            {
                y_ink[k] += zj[j] * wjk[j, k];
            }
            y_ink[k] += w0k[k];
            yk[k] = (1 / (1 + Math.Exp(-y_ink[k])));
            if (yk[k] > maxx)
            {
                maxx = yk[k];
                hasil = k;
            }

        }
        if (hasil == 0)
        {
            allData[index] += "lob" + "|";
            allData[index] += yk[0].ToString();
            //data.text += "Lob";
            playerTennis.state = 0;
            player.hitted=true;
        }
        else if (hasil == 1)
        {
            allData[index] += "spin" + "|";
            allData[index] += yk[1].ToString();
           // data.text += "spin";
            playerTennis.state= 1;
            player.tech = 0;
            player.hitted=true;
        }
        else if (hasil == 2)
        {
            allData[index] += "smash" + "|";
            allData[index] += yk[2].ToString();
           // data.text += "smash";
            playerTennis.state = 2;
            player.hitted=true;
        }
        else
        {
            allData[index] += "back" + "|";
            allData[index] += yk[2].ToString();
            //data.text += "back";
            playerTennis.state = 3;
            player.tech = 1;
            player.hitted=true;
        }
        temp2=ses;
        double a = temp2 - temp;
        //data.text += "Kecepatan ramal="+a.ToString();
	seb=timer;
    t2 = Time.realtimeSinceStartup;
   // data.text += (t2 - t1).ToString(); 
    }

}
