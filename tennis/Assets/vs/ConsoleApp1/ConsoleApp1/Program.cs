using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.IO;
using System.Text;
namespace ConsoleApp1
{
    class Program
    {
        static bool done = false;
        static string[,] dataString, bobotvij, bobotwjk;
        static double[,] data;
        static string[] lines, bobotv0j, bobotw0k;
        static double max, min;
        static double beta;
        static string[] ok;
        static double[,] vij, wjk, wjkS;
        static double[] v0j, w0k, vj;
        static double[] z_inj, zj, y_ink, yk;
        static double[,] tk, delta_wjk, delta_vij;
        static double[] sk, s_netj, sj, delta_v0j, delta_w0k, dataR;
        static double a, mse, error;
        static int jTrain, jInput, jHidden, jOutput, iterasi, epohMax;
        static string[] mses = new string[9];
        static void Main(string[] args)
        {
            jTrain = 15;
            jInput = 16;
            jHidden = 16;
            jOutput = 3;
            a = 0.2;
            mse = 1;
            iterasi = 0;
            error = 0.0001;
            epohMax = 1000;
            ok = new string[100];
            bobotvij = new string[jInput, jHidden];
            bobotwjk = new string[jHidden, jOutput];
            bobotv0j = new string[jHidden];
            bobotw0k = new string[jOutput];
            dataString = new string[jTrain, jInput];
            lines = new string[jInput];
            data = new double[jTrain, jInput];
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
            dataR = new double[jInput];
            s_netj = new double[jHidden];
            delta_v0j = new double[jHidden];
            max = 0;
            process();
            // hitung_z_inj();
        }
        static void process()
        {
            setData();
            setTarget();
            nguyenWidrow();
            
                while (iterasi < 10000)
                {
                for (int i = 0; i < jTrain; i++)
                    {
                    feedforward(i);
                    backpropagation(i);
                    perubahanBobot(i);                  
                    }
                iterasi++;
                }
                //mse = 1;
           
            setBobot();
            System.IO.File.WriteAllLines(@"D:\unity\pleaseBisa\Assets\Scripts\tes.txt", ok);
            Console.Write("ok");
        }
        static void setData()
        {
            // set data dari txt
            int counter = 0;
            var fileStream = new FileStream(@"D:\unity\pleaseBisa\Assets\Scripts\data.txt", FileMode.Open, FileAccess.Read);
            //string[] ok = new string[jTrain];
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;

                while ((line = streamReader.ReadLine()) != null)
                {

                    lines = line.Split('|');


                    for (int j = 0; j < lines.Length; j++)
                    {

                        data[counter, j] = Double.Parse(lines[j]);
                        //ok[counter] += data[counter,j].ToString() + "|";
                        //data[counter, j] = Double.Parse(dataString[counter, j]);
                    }

                    counter++;
                    if (counter == jTrain)
                    {
                        break;
                    }
                }


            }
        }
        static void setTarget()
        {
            // set target dari txt
            int counter = 0;
            var fileStream = new FileStream(@"D:\unity\pleaseBisa\Assets\Scripts\target.txt", FileMode.Open, FileAccess.Read);
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
                       // ok[counter] += tk[counter, k].ToString() + "|";
                    }

                    counter++;

                }
                //System.IO.File.WriteAllLines(@"D:\unity\pleaseBisa\Assets\Scripts\tes.txt", ok);

            }
        }
        static void nguyenWidrow()
        {
            Random ran = new Random();
            //tahap1
            //beta = (double) (0.7 * Math.Pow(jHidden, 0.0625));

            //tahap2
            for (int j = 0; j < jHidden; j++)
            {
                for (int i = 0; i < jInput; i++)
                {
                    vij[i, j] = ran.NextDouble();

                    //vj[j] += (double)(Math.Pow((double)vij[i, j], 2));
                }
                v0j[j] = ran.NextDouble();
                //ok[j] += v0j[j] + "|";
                //vj[j] = (double)Math.Pow((double)vj[j], 0.5);
            }
            //tahap3
            //for (int i = 0; i < jInput; i++)
            //{
            //    for (int j = 0; j < jHidden; j++)
            //    {
            //        vij[i,j] = (beta * vij[i,j]) / vj[j];
            //    }
            //}
            //tahap4
            for (int k = 0; k < jOutput; k++)
            {
                for (int j = 0; j < jHidden; j++)
                {
                    wjk[j, k] = ran.NextDouble();

                }
                w0k[k] = ran.NextDouble();
                //ok[k] += w0k[k] + "|";
            }

        }
        static void feedforward(int pointer)
        {
            // Debug.Log("iterasi ke" + iterasi);
            Array.Clear(z_inj, 0, z_inj.Length);
            Array.Clear(y_ink, 0, y_ink.Length);
            //string[] ok = new string[jHidden];
            for (int j = 0; j < jHidden; j++)
            {
                for (int i = 0; i < jInput; i++)
                {
                    z_inj[j] += (data[pointer, i] * vij[i, j]);

                    //ok[j] += data[pointer,i].ToString() + "|";
                }
                //ok[j] += z_inj[j].ToString() + "|";
                z_inj[j] += v0j[j];
                //Debug.Log(z_inj[j]);
                zj[j] = (1 / (1 + Math.Exp(-z_inj[j])));
                //zj[j]=Math.Round(zj[j], 8, MidpointRounding.ToEven);
                //Debug.Log(zj[j]);
                // Debug.Log(zj[j]);
            }
            for (int k = 0; k < jOutput; k++)
            {
                for (int j = 0; j < jHidden; j++)
                {
                    y_ink[k] += zj[j] * wjk[j, k];

                }
                y_ink[k] += w0k[k];
                //Debug.Log(y_ink[k]);
                //Debug.Log(y_ink[k]);
                //ok[k] += y_ink[k].ToString() + "|";
                yk[k] = 1 / (1 + Math.Exp(-y_ink[k]));
               // yk[k] = Math.Round(yk[k], 8, MidpointRounding.ToEven);
                // ok[k] += yk[k].ToString() + "|";
                //Debug.Log(yk[k]);
            }
            //System.IO.File.WriteAllLines(@"D:\unity\pleaseBisa\Assets\Scripts\tes.txt", ok);
        }
        static void hitung_z_inj()
        {
            double temp = 0;

        }
        static void backpropagation(int pointer)
        {
            Array.Clear(s_netj, 0, s_netj.Length);
            //string[] ok = new string[jHidden];
            for (int k = 0; k < jOutput; k++)
            {
                sk[k] = ((tk[pointer, k] - yk[k]) * yk[k] * (1 - yk[k]));
                // Debug.Log(tk[pointer, k]);
                //Debug.Log(yk[k]);
                // Debug.Log(sk[k]);
                for (int j = 0; j < jHidden; j++)
                {
                    delta_wjk[j, k] = a * sk[k] * zj[j];
                    //delta_wjk[j, k] = Math.Round(delta_wjk[j,k], 8, MidpointRounding.ToEven);
                }
                delta_w0k[k] = a * sk[k];
                //delta_w0k[k] = Math.Round(delta_w0k[k], 8, MidpointRounding.ToEven);
            }

            for (int j = 0; j < jHidden; j++)
            {
                for (int k = 0; k < jOutput; k++)
                {
                    s_netj[j] += sk[k] * wjk[j, k];
                    //s_netj[j] = Math.Round(s_netj[j], 8, MidpointRounding.ToEven);
                }
                sj[j] = s_netj[j] * zj[j] * (1 - zj[j]);
                //sj[j] = Math.Round(sj[j], 4, MidpointRounding.ToEven);
                delta_v0j[j] = a * sj[j];
                //delta_v0j[j] = Math.Round(delta_v0j[j], 8, MidpointRounding.ToEven);
                for (int i = 0; i < jInput; i++)
                {
                    delta_vij[i, j] = a * sj[j] * data[pointer, i];
                    //delta_vij[i, j] = Math.Round(delta_vij[i, j], 8, MidpointRounding.ToEven);
                }
            }
            //System.IO.File.WriteAllLines(@"D:\unity\pleaseBisa\Assets\Scripts\tes.txt", ok);
        }
        static void perubahanBobot(int pointer)
        {
            mse = 0;


            for (int j = 0; j < jHidden; j++)
            {
                for (int i = 0; i < jInput; i++)
                {
                    vij[i, j] += delta_vij[i, j];
                    //vij[i, j] = Math.Round(vij[i, j], 4, MidpointRounding.ToEven);
                }
                v0j[j] += delta_v0j[j];
                //v0j[j] = Math.Round(v0j[j], 4, MidpointRounding.ToEven);
            }
            for (int k = 0; k < jOutput; k++)
            {
                for (int j = 0; j < jHidden; j++)
                {
                    wjk[j, k] += delta_wjk[j, k];
                    //wjk[j, k] = Math.Round(wjk[j, k], 4, MidpointRounding.ToEven);
                }
                w0k[k] += delta_w0k[k];
                //w0k[k] = Math.Round(w0k[k], 4, MidpointRounding.ToEven);
                // mse += (double)Math.Pow(((double)tk[pointer, k] - (double)yk[k]), 2);

            }
            // mse /= jOutput;
        }
        static void setBobot()
        {
            string v0jS = "", w0kS = "";
            string[] vijS = new string[jInput];
            string[] wjkS = new string[jHidden];
            for (int j = 0; j < jHidden; j++)
            {
                for (int i = 0; i < jInput; i++)
                {
                    bobotvij[i, j] = vij[i,j].ToString();
                }
                bobotv0j[j] = v0j[j].ToString();
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
                    bobotwjk[j, k] = wjk[j, k].ToString();
                }
                bobotw0k[k] = w0k[k].ToString();
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
                    if (k < jOutput - 1)
                    {
                        wjkS[j] += bobotwjk[j, k] + "|";
                    }
                    else
                    {
                        wjkS[j] += bobotwjk[j, k];
                    }
                }
            }
            System.IO.File.WriteAllText(@"D:\unity\pleaseBisa\Assets\Scripts\bobotv0j.txt", v0jS);
            System.IO.File.WriteAllLines(@"D:\unity\pleaseBisa\Assets\Scripts\bobotvij.txt", vijS);
            System.IO.File.WriteAllLines(@"D:\unity\pleaseBisa\Assets\Scripts\bobotwjk.txt", wjkS);
            System.IO.File.WriteAllText(@"D:\unity\pleaseBisa\Assets\Scripts\bobotw0k.txt", w0kS);
        }
    }
}
