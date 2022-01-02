using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveMovement : MonoBehaviour{
    //Deklarasi variabel
    float currentPosZ;
    private int nilaiValve;
    public string JenisGerak;
    public string SumbuPutar;
    public float gradient,constanta;
    private float desiredPosCoord;
    private float moveSpeed=1.0f;
    public Transform Translasi;
    public float AdjustX;
    public float AdjustY;
    public float AdjustZ;
    public int AdjustRot;

    //Method update nilai persentase
    public void UpdateValue(int valveValue){
        nilaiValve=valveValue;
    }

    // Update is called once per frame
    void Update()
    {
        //Jika JenisGerak Translasi
        if (JenisGerak=="Trans"){
            currentPosZ=Translasi.position.z;
            Debug.Log(currentPosZ);
            desiredPosCoord=(gradient*nilaiValve)+constanta;
            float jarakPindah=desiredPosCoord-currentPosZ;
            Translasi.Translate(0f,0f,moveSpeed*jarakPindah*Time.deltaTime);
        }
        //Jika JenisGerak Rotasi
        else if (JenisGerak=="Rot"){
            //Pemilihan sumbu putar
            if (SumbuPutar=="x"){
                //0,-90
                transform.rotation=Quaternion.Euler(AdjustRot+90+nilaiValve*0.90f,AdjustY,AdjustZ);
            }
            else if (SumbuPutar=="y"){
                transform.rotation=Quaternion.Euler(AdjustX,AdjustRot+90+nilaiValve*0.90f,AdjustZ);
            }
            else if (SumbuPutar=="z"){
                transform.rotation=Quaternion.Euler(AdjustX,AdjustY,AdjustRot+90+nilaiValve*0.90f);
            }
            
        }
    }
}
