using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{
    //Deklarasi variabel
    private Slider sliderZ;
    private float rotMinVal=0.0f;
    private float rotMaxVal=90.0f;
    public string Axis;
    public int adjustx;
    public int adjusty;
    public int adjustz;
    //Method Start
    void Start()
    {
        //Objek slider
        sliderZ=GameObject.Find("Slider").GetComponent<Slider>();
        //Nilai rotasi minimal
        sliderZ.minValue=rotMinVal;
        //Nilai rotasi maksimal
        sliderZ.maxValue=rotMaxVal;
        //Update nilai slider
        sliderZ.onValueChanged.AddListener(RotateSliderUpdate);
    }
    //Method Update
    void RotateSliderUpdate(float value){
        //sumbu rotasi z
        if (Axis=="z"){
            //Update merubah posisi
            transform.localEulerAngles= new Vector3(adjustx,adjusty,value);
        }
        //sumbu rotasi y
        else if (Axis=="y"){
            //Update merubah posisi
            transform.localEulerAngles= new Vector3(adjustx,value,adjustz);
        }        
    }
}