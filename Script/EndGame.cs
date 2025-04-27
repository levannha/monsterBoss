using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndGame : MonoBehaviour
{
    public static EndGame Instance { get; set; }
    [SerializeField] private TextMeshProUGUI total_construction_blue;
    [SerializeField] private TextMeshProUGUI trained_personnel_blue;
    [SerializeField] private TextMeshProUGUI destroy_construction_blue;
    [SerializeField] private TextMeshProUGUI enemy_eliminated_blue;
    [SerializeField] private TextMeshProUGUI total_construction_red;
    [SerializeField] private TextMeshProUGUI trained_personnel_red;
    [SerializeField] private TextMeshProUGUI destroy_construction_red;
    [SerializeField] private TextMeshProUGUI enemy_eliminated_red;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private Image transcript;
    [SerializeField] private TextMeshProUGUI time;
    private int time_second = 0;
    private int time_minute = 0;
    private bool win_key = false;
    private int win_chip = 0;
    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        InvokeRepeating("UpdateTimeGame", 1f, 1f);
    }
    public void UpdateTimeGame()
    {

        time_second++;
        if (time_second == 60)
        {
            time_second = 0;
            time_minute++;
        }
        string x = "";
        if (time_minute <10)
        {
            x += "0" + time_minute;
        } else
        {
            x += time_minute;
        }
        if (time_second<10)
        {
           x  += ":0" + time_second;
        } else
        {
            x += ":" + time_second;
        }
        time.text = x;
    }
    public void CheckBlue()
    {
       if (ResourceManager.Instance.GetConstruction()==0&& ResourceManager.Instance.GetMilitary_strength()==0 && !GameManager.Instance.GetIsMain()&& ResourceManager.Instance.GetMinerals()<500) 
        {
            // thua
            StartCoroutine(End(false));
        }
    }
    public void CheckRed()
    {
        if (ResourceBoss.Instance.GetConstruction() == 0 && ResourceBoss.Instance.GetMilitary_strength() == 0 && !MainBoss.Instance.GetIsMain() && ResourceBoss.Instance.GetMinerals() < 500)
        {
            // thắng
            StartCoroutine(End(true));
        }
    }
    public IEnumerator End(bool win)
    {
        yield return new WaitForSeconds(5f);
        total_construction_blue.text = "Công trình đã xây: " + ResourceManager.Instance.GetTotal_construction();
        trained_personnel_blue.text = "Quân đã huấn luyện: " + ResourceManager.Instance.GetTrained_personnel();
        destroy_construction_blue.text = "Công trình đã phá huỷ: " + ResourceManager.Instance.GetDestroy_construction();
        enemy_eliminated_blue.text = "Quân địch đã tiêu diệt: " + ResourceManager.Instance.GetTrained_personnel();
        total_construction_red.text = "Công trình đã xây: " + ResourceBoss.Instance.GetTotal_construction();
        trained_personnel_red.text = "Quân đã huấn luyện: " + ResourceBoss.Instance.GetTrained_personnel();
        destroy_construction_red.text = "Công trình đã phá huỷ: " + ResourceBoss.Instance.GetDestroy_construction();
        enemy_eliminated_red.text = "Quân địch đã tiêu diệt: " + ResourceBoss.Instance.GetTrained_personnel();
        if (win)
        {
            winText.text = "Chiến thắng";
            winText.color = Color.cyan;
        } else
        {
            winText.text = "Thất bại";
            winText.color = Color.red;
        }
        transcript.gameObject.SetActive(true);
        win_key = win;
        win_chip = ResourceManager.Instance.GetTotal_construction() + ResourceManager.Instance.GetTrained_personnel() + ResourceManager.Instance.GetDestroy_construction() + ResourceManager.Instance.GetTrained_personnel();

        }
    public void Save()
    {
        MainData data = SaveGameManager.Instance.LoadGame();
        data.chip +=win_chip;
        if(win_key)
        {
            data.key+=1;
        }
        SaveGameManager.Instance.SaveGame(data);
    }
    public void BackToMenu()
    {
        Save();
        SceneManager.LoadScene("Menu");
    }
}
