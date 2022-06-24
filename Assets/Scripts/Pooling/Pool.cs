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
   public List<GameObject> goList = new List<GameObject>();
    private void Awake()
    {
        // this is not the first instance so destroy it!
        if (_instance != null && _instance != this)
        {
            _instance = this;
            return;
        }
        //first instance should be kept and do NOT destroy it on load
        _instance = this;
        matrix = new int[maxRow, maxCol];
        count = 0;
        LoadBlockFromCSV();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }
   
    public int GetTotalBreakableBrick()
    {
        int total = 0;
        for(int i = 0; i < goList.Count; i++)
        {
            if (goList[i].tag.StartsWith("Breakable")) total++;
        }
        return total;
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
      
    }
    public int[,] ReadCSVFile()
    {
        string level = PlayerPrefs.GetInt("SelectedLevel").ToString();
        string fileName = "Assets/CSV/level_" + level + ".csv";
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
        Debug.Log("List Go is populated with Count is:"+goList.Count);
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
