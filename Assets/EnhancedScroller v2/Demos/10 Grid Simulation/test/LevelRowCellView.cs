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
        private int level=-1;
     
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(PlayerPrefs.GetString("currentLevel"));
        }
    }
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
            if (data.isLock == true)
            {
                unlockImage.enabled = false;
                star.SetActive(true);
            }
            else
            {
                unlockImage.enabled = true;
                star.SetActive(false);
            }

          //  star.SetActive(data.isLock);
          //  unlockImage.enabled = !data.isLock;
            if (level != 1)
            {
                level = int.Parse(data.levelTxt);
            }

            
        }
        }



        public void OnPointerClick(PointerEventData eventData)
        {
            if(unlockImage.enabled ==false)
         
            SceneManager.LoadScene("Level"+level);
        }
    }



