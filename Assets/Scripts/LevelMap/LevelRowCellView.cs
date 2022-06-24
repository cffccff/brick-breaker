using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

    public class LevelRowCellView : MonoBehaviour, IPointerClickHandler
    {
   
    public GameObject container;
        public TextMeshProUGUI text;
        public GameObject star;
        public Image unlockImage;
        public int level=-1;
   

    /// <summary>
    /// This function just takes the Demo data and displays it
    /// </summary>
    /// <param name="data"></param>
    public void SetData(LevelData data)
        {
            // this cell was outside the range of the data, so we disable the container.
            // Note: We could have disable the cell gameobject instead of a child container,
            // but that can cause problems if you are trying to get components (disabled objects are ignored).
            container.SetActive(data != null);

            if (data != null)
            {
                if (data.levelTxt == "1")
                {
                    text.text = "Tutorial";
                    level = 1;
                }
                else
                {
                    // set the text if the cell is inside the data range
                    text.text = data.levelTxt;
                }
            if (data.isUnLock == true)
            {
                unlockImage.enabled = false;
               
            }
            else
            {
                unlockImage.enabled = true;
                star.SetActive(false);
            }
            if(data.isPass == true)
            {
                star.SetActive(true);
            }
            else
            {
                star.SetActive(false);
            }
            if (level != 1)
            {
                level = int.Parse(data.levelTxt);
            }
           


        }
        }



        public void OnPointerClick(PointerEventData eventData)
        {
      
        if (unlockImage.enabled == false)
        {
            PlayerPrefs.SetInt("SelectedLevel", level);
            
           
            SceneManager.LoadScene("GamePlay");
        }

    }
    }


