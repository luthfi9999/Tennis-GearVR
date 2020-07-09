using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ran = UnityEngine.Random;
using System.Threading;
public class NeuralNetwork : MonoBehaviour {
    bool done;
    string[,] bobotvij,bobotwjk;
    double[,] data;
    string[] lines,bobotv0j,bobotw0k;
    public Text text;
    string path;
    float persen;
    double[,] vij, wjk,wjkS;
    double[] v0j, w0k, vj;
    double[] z_inj, zj, y_ink, yk;
    double[,] tk, delta_wjk, delta_vij;
    double[] sk, s_netj, sj,delta_v0j,delta_w0k;
    double a,mse,error;
    int jTrain,jInput,jHidden, jOutput,iterasi, epohMax;
   
    // Use this for initialization
    void Start () {
        done = false;
        path = Application.persistentDataPath;
        jTrain = 20;
        jInput = 20;
        jHidden =5;
        jOutput = 4;
        a = 0.8;
        mse = 1;
        persen = 0;
        iterasi = 0;
        error = 0.05;
        epohMax = 1000;
        bobotvij = new string[jInput,jHidden];
        bobotwjk = new string[jHidden, jOutput];
        bobotv0j = new string[jHidden];
        bobotw0k = new string[jOutput];
        lines = new string[jInput];
        data = new double[jTrain,jInput];
        vij = new double[jInput, jHidden];
        wjk = new double[jHidden, jOutput];
        wjkS = new double[jOutput, jHidden];
        v0j = new double[jHidden];
        w0k = new double[jOutput];
        vj = new double[jHidden];
        z_inj = new double[jHidden];
        zj = new double[jHidden];
        y_ink = new double[jOutput];
        yk = new double[jOutput];
        tk = new double[jTrain, jOutput];
        delta_wjk = new double[jHidden, jOutput];
        delta_vij = new double[jInput, jHidden];
        sk = new double[jOutput];
        delta_w0k = new double[jOutput];
        sj = new double[jHidden];
        s_netj = new double[jHidden];
        delta_v0j = new double[jHidden];
        //inisiasi
        setData();
        setTarget();
       
        bobotAwal();


    }

    // Update is called once per frame
    void Update()
    {
        //inisialisasi awal
        if (!done)
        {
            if (iterasi < epohMax && error < mse)
            {
                training();

            }
            else
            {
                setBobot();
                done = true;
            }
            progress();
            iterasi++;

            if (done)
            {
                iterasi = 0;
                mse = 1;
                SceneManager.LoadScene(0);

            }
        }
    }
    //fungsi untuk melakukan pelatihan
    void training()
    {
        mse = 0;
            for (int i = 0; i < jTrain; i++)
            {
                feedforward(i);
                backpropagation(i);
                perubahanBobot(i);
            }
        mse /= jTrain;
    }
    //fungsi untuk mengambil target
    void setTarget()
    {
        // set target dari txt
        int counter = 0;
        var fileStream = new FileStream(path + "/target.txt", FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string line;
            //
            string[,] targetS = new string[jTrain, jOutput];
            while ((line = streamReader.ReadLine()) != null)
            {
                lines = line.Split('|');
                for (int k = 0; k < lines.Length; k++)
                {
                    targetS[counter, k] = lines[k];
                    tk[counter, k] = Double.Parse(targetS[counter, k]);
                }

                counter++;
                
            }

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
    //fungsi untuk mengambil bobot yang menghubungkan silnya masukan dengan sinyal tersembunyi yang sudah disimpan sebelumnya
    void getVij()
    {
        // set data dari txt
        int counter = 0;
        var fileStream = new FileStream(path+"/bobotvij.txt", FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                lines = line.Split('|');
                for (int j = 0; j < jHidden; j++)
                {
                    vij[counter, j] = Double.Parse(lines[j]);
                }
                counter++;
                if (counter == jInput)
                {
                    break;
                }
            }

        }
    }
    //fungsi untuk mengambil bobot yang menghubungkan sinyal tersembunyi dengan sinyal keluaran
    void getWjk()
    {
        // set data dari txt
        int counter = 0;
        var fileStream = new FileStream(path+"/bobotwjk.txt", FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                lines = line.Split('|');
                for (int j = 0; j < jOutput; j++)
                {
                    wjk[counter, j] = Double.Parse(lines[j]);

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
        var fileStream = new FileStream(path+"/bobotW0k.txt", FileMode.Open, FileAccess.Read);
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
                    Debug.Log(w0k[i]);
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
        var fileStream = new FileStream(path+"/bobotv0j.txt", FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string line;
            string[] dataS = new string[jInput];
            while ((line = streamReader.ReadLine()) != null)
            {
                lines = line.Split('|');

                for (int i = 0; i < jHidden; i++)
                {
                    v0j[i] = Double.Parse(lines[i]);

                }
                counter++;
                if (counter == 1)
                {
                    break;
                }
            }

        }
    }
    //fungsi untuk menyimpan bobot
    void setBobot()
    {
        string  v0jS="", w0kS="";
        string[] vijS = new string[jInput];
        string[] wjkS = new string[jHidden];
        for (int j = 0; j < jHidden; j++)
        {
            for (int i = 0; i < jInput; i++)
            {
                bobotvij[i, j] = vij[i, j].ToString();
            }
            bobotv0j[j]= v0j[j].ToString();
            if (j == jHidden - 1)
            {
                v0jS += bobotv0j[j];
            }
            else
            {
                v0jS += bobotv0j[j] + "|";
            }
        }
        for (int k = 0; k < jOutput; k++)
        {
            for (int j = 0; j < jHidden; j++)
            {
                bobotwjk[j,k]= wjk[j, k].ToString();
            }
            bobotw0k[k]= w0k[k].ToString(); 
            if (k == jOutput - 1)
            {
                w0kS += bobotw0k[k];
            }
            else
            {
                w0kS += bobotw0k[k] + "|";
            }
        }
        for (int i = 0; i < jInput; i++)
        {
            for (int j = 0; j < jHidden; j++)
            {
                if (j < jHidden - 1)
                {
                    vijS[i] += bobotvij[i, j] + "|";
                }
                else
                {
                    vijS[i] += bobotvij[i, j];
                }
            }
        }
        for (int j = 0; j < jHidden; j++)
        {
            for (int k = 0; k < jOutput; k++)
            {
                if (k < jOutput- 1)
                {
                    wjkS[j] += bobotwjk[j, k] + "|";
                }
                else
                {
                    wjkS[j] += bobotwjk[j, k];
                }
            }
        }
        System.IO.File.WriteAllText(path+ "/bobotv0j.txt", v0jS);
        System.IO.File.WriteAllLines(path+"/bobotvij.txt", vijS);
        System.IO.File.WriteAllLines(path+"/bobotwjk.txt", wjkS);
        System.IO.File.WriteAllText(path+"/bobotw0k.txt", w0kS);
    }
    //fungsi untuk mengambil data
    void setData()
    {
        // set data dari txt
        int counter = 0;
        var fileStream = new FileStream(path + "/data.txt", FileMode.Open, FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
        {
            string line;

            while ((line = streamReader.ReadLine()) != null)
            {
               
                lines = line.Split('|');
               

                for (int j = 0; j < lines.Length; j++)
                {
                    
                    data[counter, j] = Double.Parse(lines[j]);
                }

                counter++;
                if (counter == jTrain)
                {
                    break;
                }
            }
           

        }
    }   
    
    //fungsi untuk mencari bobot awal, dengan nilai 0-1
    void bobotAwal()
    {
        System.Random ran = new System.Random();
        for (int j = 0; j < jHidden; j++)
        {
            for (int i = 0; i < jInput; i++)
            {
                vij[i, j] = ran.NextDouble();
            }
            v0j[j] = ran.NextDouble();
        }
        for (int k = 0; k < jOutput; k++)
        {
            for (int j = 0; j < jHidden; j++)
            {
                wjk[j, k] = ran.NextDouble();

            }
            w0k[k] = ran.NextDouble();
        }

    }
    //fungsi untuk melakukan proses feedforward
    void feedforward(int pointer)
    {
        Array.Clear(z_inj, 0, z_inj.Length);
        Array.Clear(y_ink, 0, y_ink.Length);
        for (int j = 0; j < jHidden; j++)
        {
            for (int i = 0; i < jInput; i++)
            {
                z_inj[j] += (data[pointer, i] * vij[i, j]);
             
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
            yk[k] =  1/(1+Math.Exp(-y_ink[k]));
        }
    }
    //fungsi untuk melakukan fungsi backpropagation
    void backpropagation(int pointer)
    {
        Array.Clear(s_netj, 0, s_netj.Length);
        for (int k = 0; k < jOutput; k++)
        {
            sk[k] = ((tk[pointer, k] - yk[k]) * yk[k] * (1 - yk[k]));
            for (int j = 0; j < jHidden; j++)
            {
                delta_wjk[j, k] = a * sk[k] * zj[j];
            }
            delta_w0k[k] = a * sk[k];
        }
        
        for (int j = 0; j < jHidden; j++)
        {
            for (int k = 0; k < jOutput; k++)
            {
                s_netj[j] += sk[k] * wjk[j, k];
            } 
            sj[j] = s_netj[j] * zj[j] * (1 - zj[j]);
            delta_v0j[j] = a * sj[j];
            for (int i = 0; i < jInput; i++)
            {
                delta_vij[i, j] = a * sj[j] * data[pointer, i];
            }
        }
    }
    //fungsi untuk melakukan fungsi perubahan bobot
    void perubahanBobot(int pointer)
    {
        double temp = 0;

        for (int j = 0; j < jHidden; j++)
        {
            for (int i = 0; i < jInput; i++)
            {
                vij[i, j] += delta_vij[i, j];
            }
            v0j[j] += delta_v0j[j];
        }
        for (int k = 0; k < jOutput; k++)
        {
            for (int j = 0; j < jHidden; j++)
            {
                wjk[j, k] += delta_wjk[j, k];
            }
            w0k[k] += delta_w0k[k];
            temp += (double)Math.Pow(((double)tk[pointer, k] - (double)yk[k]), 2);

        }
        temp /= jOutput;
        mse += temp;
    }
       
    }


