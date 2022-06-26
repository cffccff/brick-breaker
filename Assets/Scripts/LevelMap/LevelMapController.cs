using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelMapController : MonoBehaviour, IEnhancedScrollerDelegate
{
    /// <summary>
    /// Internal representation of our data. Note that the scroller will never see
    /// this, so it separates the data from the layout using MVC principles.
    /// </summary>
    private List<LevelData> _data;
    /// <summary>
    /// This is our scroller we will be a delegate for
    /// </summary>
    public EnhancedScroller scroller;
    public EnhancedScroller.TweenType vScrollerTweenType = EnhancedScroller.TweenType.immediate;
    public float vScrollerTweenTime = 0f;

    public EnhancedScroller.TweenType hScrollerTweenType = EnhancedScroller.TweenType.immediate;
    public float hScrollerTweenTime = 0f;
    /// <summary>
    /// This will be the prefab of each cell in our scroller. The cell view will
    /// hold references to each row sub cell
    /// </summary>
    public EnhancedScrollerCellView cellViewPrefab;
    public TextMeshProUGUI totalStarTxt;
    public int numberOfCellsPerRow = 4;
    public static int totalLevel = 40;
    /// <summary>
    /// for set up list like 1 2 3 4 8 7 6 5 9 10 11 12 16 15 14 13 .....
    /// hold references to each row sub cell
    /// </summary>
    bool isRevesed = true;
    int count = 0;
    private List<LevelData> list1 = new List<LevelData>();
    /// <summary>
    /// Be sure to set up your references to the scroller after the Awake function. The 
    /// scroller does some internal configuration in its own Awake function. If you need to
    /// do this in the Awake function, you can set up the script order through the Unity editor.
    /// In this case, be sure to set the EnhancedScroller's script before your delegate.
    /// 
    /// In this example, we are calling our initializations in the delegate's Start function,
    /// but it could have been done later, perhaps in the Update function.
    /// </summary>
    void Start()
    {
        // tell the scroller that this script will be its delegate
        scroller.Delegate = this;

        // load in a large set of data
        LoadData();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(PlayerPrefs.GetInt("currentLevel"));
        }
    }
    /// <summary>
    /// Populates the data with a lot of records
    /// </summary>
    private void LoadData()
    {
        // set up some simple data
        _data = new List<LevelData>();
        int currentLevel;
        int totalStar = 0;
        //PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("currentLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("currentLevel");
        }
        else
        {
           
            PlayerPrefs.SetInt("currentLevel", 1);
            currentLevel = PlayerPrefs.GetInt("currentLevel");

        }
        //order display level and populated the list
        for (int i = 1; i <= totalLevel; i++)
        {

            if (isRevesed == true)
            {
                list1.Add(new LevelData() { levelTxt = i.ToString(), isUnLock = false, isPass = false });
                count++;


            }
            else
            {
                _data.Add(new LevelData() { levelTxt = i.ToString(), isUnLock = false, isPass = false });

                count++;

            }
            if (count == 4)
            {

                isRevesed = !isRevesed;
                count = 0;
                list1.Reverse();
                _data.AddRange(list1);
                list1.Clear();
            }

        }
        _data.Reverse();
        //set logic active/pass of status of a level in _data level
        if (currentLevel == 1)
        {
            
            for (int i = 0; i < _data.Count-1; i++)
            {
                if(i==_data.Count - 4)
                {
                    _data[i].isPass = false;
                    _data[i].isUnLock = true;
                }
                else
                {
                    _data[i].isPass = false;
                    _data[i].isUnLock = false;
                }
               
            }
        }
        else
        {
            for (int i = 0; i < _data.Count; i++)
            {
                if (int.Parse(_data[i].levelTxt) < currentLevel)
                {
                    totalStar += 3;
                    _data[i].isPass = true;
                    _data[i].isUnLock = true;
                }
                else if (int.Parse(_data[i].levelTxt) == currentLevel)
                {
                    _data[i].isPass = false;
                    _data[i].isUnLock = true;
                }
                else
                {
                    _data[i].isPass = false;
                    _data[i].isUnLock = false;
                }
            }
        }
        // tell the scroller to reload now that we have the data
        scroller.ReloadData();
        totalStarTxt.text = totalStar.ToString();
        ViewCurrentLevel();
    }
    public void ViewCurrentLevel()
    {
        int jumpDataIndex = PlayerPrefs.GetInt("currentLevel");
        jumpDataIndex = (40 - jumpDataIndex) / 4;
       



        Debug.Log(jumpDataIndex);
        // extract the integer from the input text

        // jump to the index
        scroller.JumpToDataIndex(jumpDataIndex, 0f, 0f, false,vScrollerTweenType, vScrollerTweenTime, null, EnhancedScroller.LoopJumpDirectionEnum.Closest);
          
       
    }
    #region EnhancedScroller Handlers

    /// <summary>
    /// This tells the scroller the number of cells that should have room allocated.
    /// For this example, the count is the number of data elements divided by the number of cells per row (rounded up using Mathf.CeilToInt)
    /// </summary>
    /// <param name="scroller">The scroller that is requesting the data size</param>
    /// <returns>The number of cells</returns>
    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return Mathf.CeilToInt((float)_data.Count / (float)numberOfCellsPerRow);
    }

    /// <summary>
    /// This tells the scroller what the size of a given cell will be. Cells can be any size and do not have
    /// to be uniform. For vertical scrollers the cell size will be the height. For horizontal scrollers the
    /// cell size will be the width.
    /// </summary>
    /// <param name="scroller">The scroller requesting the cell size</param>
    /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
    /// <returns>The size of the cell</returns>
    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 300f;
    }

    /// <summary>
    /// Gets the cell to be displayed. You can have numerous cell types, allowing variety in your list.
    /// Some examples of this would be headers, footers, and other grouping cells.
    /// </summary>
    /// <param name="scroller">The scroller requesting the cell</param>
    /// <param name="dataIndex">The index of the data that the scroller is requesting</param>
    /// <param name="cellIndex">The index of the list. This will likely be different from the dataIndex if the scroller is looping</param>
    /// <returns>The cell for the scroller to use</returns>
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        // first, we get a cell from the scroller by passing a prefab.
        // if the scroller finds one it can recycle it will do so, otherwise
        // it will create a new cell.
        LevelCellView cellView = scroller.GetCellView(cellViewPrefab) as LevelCellView;

        cellView.name = "Cell " + (dataIndex * numberOfCellsPerRow).ToString() + " to " + ((dataIndex * numberOfCellsPerRow) + numberOfCellsPerRow - 1).ToString();

        // pass in a reference to our data set with the offset for this cell
        cellView.SetData(ref _data, dataIndex * numberOfCellsPerRow);

        // return the cell to the scroller
        return cellView;
    }

    #endregion
}

