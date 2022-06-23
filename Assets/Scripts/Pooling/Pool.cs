using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MarchingBytes;
public class Pool : MonoBehaviour
{
    private readonly int maxRow = 10;
    private readonly int maxCol = 16;
    private int[,] matrix;
    private int count;

    private static Pool _instance;
    public static Pool Instance => _instance;
    private GameObject go;
    List<GameObject> goList = new List<GameObject>();
    private void Awake()
    {
        // this is not the first instance so destroy it!
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        //first instance should be kept and do NOT destroy it on load
        _instance = this;
       //    DontDestroyOnLoad(_instance);



    }
    // Start is called before the first frame update
    void Start()
    {
       
        matrix = new int[maxRow, maxCol];
        count = 0;
        LoadBlockFromCSV();
    }
   

    public void ReturnToPoolAction()
    {
        foreach (GameObject go in goList)
        {
            EasyObjectPool.instance.ReturnObjectToPool(go);
        }
        goList.Clear();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            go = EasyObjectPool.instance.GetObjectFromPool("brick1", new Vector2(5, 5), Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            go = EasyObjectPool.instance.GetObjectFromPool("brick2", new Vector2(10, 10), Quaternion.identity);
        }
    }
    public int[,] ReadCSVFile()
    {
        Debug.Log("Before filename");
        string level = PlayerPrefs.GetInt("SelectedLevel").ToString();
        Debug.Log("Level in this file:" + level);
        // string fileName = "Assets/CSV/level_1.csv";
        string fileName = "Assets/CSV/level_" + level + ".csv";
        Debug.Log("Pass filename");
        StreamReader streamReader = new StreamReader(fileName);
        bool endOfFile = false;

        while (!endOfFile)
        {

            string data_String = streamReader.ReadLine();

            if (data_String == null)
            {
                endOfFile = true;

                break;
            }
            //storing to variable
            var data_value = data_String.Split(',');
            for (int i = count; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = int.Parse(data_value[j]);
                }

            }
            count++;
        }

        return matrix;


    }
    public void LoadBlockFromCSV()
    {

        matrix = ReadCSVFile();

        SetUpBlock();

    }
    public void SetUpBlock()
    {
       
        float Yaxis = 10.5f;
        for (int i = 1; i < maxRow; i++)
        {
            float Xaxis = 0.5f;
            for (int j = 0; j < maxCol; j++)
            {

                if (matrix[i, j] == 1)
                {
                    // Instantiate(breakableBlockHit1, new Vector2(Xaxis, Yaxis), Quaternion.identity).transform.parent = blocks.transform;
                     go = EasyObjectPool.instance.GetObjectFromPool("brick1", new Vector2(Xaxis, Yaxis), Quaternion.identity);
                    goList.Add(go);
                    //  EnableBreakableBlockHit1(number);
                }
                else if (matrix[i, j] == 0)
                {
                   
                }
                else if (matrix[i, j] == -1)
                {
                     go = EasyObjectPool.instance.GetObjectFromPool("brick-1", new Vector2(Xaxis, Yaxis), Quaternion.identity);
                    goList.Add(go);
                }
                else if (matrix[i, j] == 2)
                {
                     go = EasyObjectPool.instance.GetObjectFromPool("brick2", new Vector2(Xaxis, Yaxis), Quaternion.identity);
                    goList.Add(go);
                }
                else if (matrix[i, j] == 3)
                {
                     go = EasyObjectPool.instance.GetObjectFromPool("brick3", new Vector2(Xaxis, Yaxis), Quaternion.identity);
                    goList.Add(go);
                }
                Xaxis += 1;
            }
            Yaxis--;
        }
    }
}
