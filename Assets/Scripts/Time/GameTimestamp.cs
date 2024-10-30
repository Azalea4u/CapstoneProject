using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class GameTimestamp : MonoBehaviour
{
    public TMPro.TextMeshProUGUI dateText;

    public int hour = 1;
    public int day = 1;
    public int month = 1;
    public int year = 0;

    public int daysGrown = 0;

    public GameTimestamp(int hour, int day, int month, int year)
    {
        this.hour = hour;
        this.day = day;
        this.month = month;
        this.year = year;
    }

    public GameTimestamp(GameTimestamp timestamp)
    {
        this.hour = timestamp.hour;
        this.day = timestamp.day;
        this.month = timestamp.month;
        this.year = timestamp.year;
    }

    public string date
    {
        get
        {
            return "Year " + year + "\nMonth " + month + " | Day " + day;
        }
        set
        {
            dateText.text = value;
        }
    }

    public void UpdateClock()
    {
        hour++;

        if (hour >= 24)
        {
            hour = 1;
            day++;
        }

        if (day >= 30)
        {
            day = 1;
            month++;
        }

        if (month >= 12)
        {
            month = 1;
            year++;
        }

        date = "Year " + year + "\nMonth " + month + " | Day " + day;
    }

    // Concert Days to Hours
    public static int DaysToHours(int days)
    {
        return days * 24;
    }

    // Convert Months to Days
    public static int MonthsToDays(int months)
    {
        return months * 30;
    }

    // Convert Years to Months
    public static int YearsToMonths(int years)
    {
        return years * 12;
    }

}
