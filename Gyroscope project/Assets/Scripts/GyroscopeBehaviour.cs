using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class GyroscopeBehaviour : MonoBehaviour
{
    //We have a bool to see if gyroscope is enabled in the phone being used.
    private bool gyroEnabled;

    //A private gyroscope objects gets created, called gyro.
    private Gyroscope gyro;

    //A camera GameObject its added to the scene.
    private GameObject cameraContainer;

    //Quaternions are implemented because we need it to calculate later.
    private Quaternion rot;

    List<float[]> info = new List<float[]>();

    //Starts when script is run.
    void Start()
    {
        Debug.Log(Application.dataPath);
        //cameraContainer gets defined as a new game object, with the name "Camera Container"
        cameraContainer = new GameObject("Camera Container");

        //
        cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        gyroEnabled = EnableGyro();
    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
            rot = new Quaternion(0, 0, 1, 0);

            return true;
        }

        return false;
    }

    private void Update()
    {
        if (gyroEnabled)
        {
            if (startDataSave)
                info.Add(new float[] {gyro.gravity.x, gyro.gravity.y, gyro.gravity.z });

            transform.localRotation = gyro.attitude * rot;
        }
    }


    public void SaveData()
    {
        string fileName = Application.dataPath + "/GyroscopeInfo.csv";

        StreamWriter sw = new StreamWriter(fileName, false);
        sw.WriteLine("GravityX, GravityY, GravityZ");

        for(int i = 0; i < info.Count; i++)
        {
            sw.WriteLine(info[i][0] + "," + info[i][1] + "," + info[i][2]);
        }
        sw.Close();
        
        info.Clear();

    }

    bool startDataSave = false;
    public void ButtonClicked()
    {
        Debug.Log("Started");
        startDataSave = !startDataSave;

        if (!startDataSave)
            SaveData();
    }
}
