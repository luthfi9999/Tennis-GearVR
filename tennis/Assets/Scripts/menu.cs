using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// Kelas untuk GUI pada menu utama
public class menu : MonoBehaviour {
    public Image pengambilanData;
    public Image Pelatihan;
    public Image Pengujian;
    public Image Game;
    public Color warnaAwal;
    public Color warnaAkhir;
    bool hold = false;
    int index = 1;
    // Use this for initialization
    void Start () {
        
        changeColor();
        
    }
	
	// Update is called once per frame
	void Update () {
        OVRInput.Update();
        if  (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            if (!hold)
            {
                index++;
                if (index == 5)
                {
                    index = 1;
                }
                changeColor();
                hold = true;
            }

        }
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
        {
            changeScene();
            
        }
        if (!OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
        {
            hold = false;
        }
            // warnaAwal = pengambilanData.colors;

    }
    //fungsi untuk merubah scene
    void changeScene()
    {
        SceneManager.LoadScene(index);
    }
    //fungsi untuk merubah warna tiap pilihan
    void changeColor()
    {
        if (index==1)
        { 
                pengambilanData.color = warnaAkhir;
                Pelatihan.color = warnaAwal;
                Pengujian.color = warnaAwal;
                Game.color = warnaAwal;
        }
        else if (index == 2)
        {
            pengambilanData.color = warnaAwal;
            Pelatihan.color = warnaAkhir;
            Pengujian.color = warnaAwal;
            Game.color = warnaAwal;
        }
        else if (index == 3)
        {
            pengambilanData.color = warnaAwal;
            Pelatihan.color = warnaAwal;
            Pengujian.color = warnaAkhir;
            Game.color = warnaAwal;
        }
        else
        {
            pengambilanData.color = warnaAwal;
            Pelatihan.color = warnaAwal;
            Pengujian.color = warnaAwal;
            Game.color = warnaAkhir;
        }
    }
}
