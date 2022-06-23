using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ReadCSV : MonoBehaviour
{
    public readonly int maxRow = 10;
    public readonly int maxCol = 16;
    private int[,] matrix;
    private int count;
    private void Start()
    {
        matrix = new int[maxRow, maxCol];
        ReadCSVFile();
        count = 0;
        Debug.Log("matrix 015:" + matrix[0, 15]);
        Debug.Log("matrix 115:" + matrix[1, 15]);
        Debug.Log("matrix 215:" + matrix[2, 15]);
        Debug.Log("matrix 315:" + matrix[3, 15]);
        Debug.Log("matrix 415:" + matrix[4, 15]);
        Debug.Log("matrix 515:" + matrix[5, 15]);
        Debug.Log("matrix 615:" + matrix[6, 15]);
        Debug.Log("matrix 715:" + matrix[7, 15]);
        Debug.Log("matrix 815:" + matrix[8, 15]);
        Debug.Log("matrix 915:" + matrix[9, 15]);
    }
    public void  ReadCSVFile()
    {
        string fileName = "Assets/CSV/level_1.csv";
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
          //  Debug.Log("Go to refer");
            //storing to variable
            var data_value = data_String.Split(',');


            foreach (string item in data_value)
            {
           //     Debug.Log(item);
            }
            for (int i = count; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    matrix[i, j] = int.Parse(data_value[j]);
                }

            }
            count++;
        }

        


    }


}
