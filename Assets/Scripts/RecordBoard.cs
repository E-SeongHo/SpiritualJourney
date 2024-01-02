using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecordBoard : MonoBehaviour
{
    public Canvas map;
    public Canvas record;

    private int playTime;
    private List<float> poneglyphActivated;

    private void Start()
    {
        playTime = (int)GameManager.Instance.PlayTime;
        DisplayRecords();
        poneglyphActivated = GameManager.Instance.ActivateTimes;
    }

    private void DisplayRecords()
    {
        int[] timeSpent = ParseTime(playTime);

        string timeRecord = string.Format("YOU SPENT \n {0}h {1}m {2}s", timeSpent[0], timeSpent[1], timeSpent[2]);
        
        if(poneglyphActivated != null)
        {
            string numActivated = poneglyphActivated.Count.ToString();
        
            List<string> timePones = new List<string>();
            for(int i = 0; i < poneglyphActivated.Count; i++)
            {
                string timePone = (i+1).ToString() + " " + poneglyphActivated[i].ToString();
                timePones.Add(timePone);
            }
        }

        record.GetComponent<TMP_Text>().text = timeRecord;

        RawImage displayImage = map.GetComponent<RawImage>();
        displayImage.texture = GameManager.Instance.endStatus;
    }

    private int[] ParseTime(int time)
    {
        int[] times = new int[3];
        
        int seconds = time % 60;
        time /= 60;
        int minutes = time % 60;
        time /= 60;
        int hours = time % 60;

        times[0] = hours;
        times[1] = minutes;
        times[2] = seconds;

        return times;
    }
}
