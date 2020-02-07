using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [HideInInspector] public bool fade = false;
    [HideInInspector] public string scene;
    [HideInInspector] public static string currentScene;
    [HideInInspector] public bool teleport;
    public Color loadToColor = Color.black;
    public double increment;

    public GameObject yourCash;
    public GameObject[] expText = new GameObject[6], lvlText = new GameObject[6];

    static bool exists;

    void Start()
    {
        if (!exists)
        {
            exists = true;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    void Update()
    {        
        if (!PlayerEngine.MaxLevel(StrengthSkill.strLvl))
            expText[0].GetComponent<Text>().text = StrengthSkill.strExp + " / " + PlayerEngine.FindNextLevelExpRequirement(StrengthSkill.strLvl + 1);        
        else
            expText[0].GetComponent<Text>().text = StrengthSkill.strExp + " / " + PlayerEngine.FindNextLevelExpRequirement(StrengthSkill.strLvl);

        if (!PlayerEngine.MaxLevel(DefenseSkill.defLvl))
            expText[1].GetComponent<Text>().text = DefenseSkill.defExp + " / " + PlayerEngine.FindNextLevelExpRequirement(DefenseSkill.defLvl + 1);
        else
            expText[1].GetComponent<Text>().text = DefenseSkill.defExp + " / " + PlayerEngine.FindNextLevelExpRequirement(DefenseSkill.defLvl);

        if (!PlayerEngine.MaxLevel(PlayerHealth.pHealthLevel))
            expText[2].GetComponent<Text>().text = PlayerHealth.pHealthExp + " / " + PlayerEngine.FindNextLevelExpRequirement(PlayerHealth.pHealthLevel + 1);
        else
            expText[2].GetComponent<Text>().text = PlayerHealth.pHealthExp + " / " + PlayerEngine.FindNextLevelExpRequirement(PlayerHealth.pHealthLevel);

        if (!PlayerEngine.MaxLevel(MagicSkill.magLvl))
            expText[3].GetComponent<Text>().text = MagicSkill.magExp + " / " + PlayerEngine.FindNextLevelExpRequirement(MagicSkill.magLvl + 1);
        else
            expText[3].GetComponent<Text>().text = MagicSkill.magExp + " / " + PlayerEngine.FindNextLevelExpRequirement(MagicSkill.magLvl);

        if (!PlayerEngine.MaxLevel(MiningSkill.mineLvl))
            expText[4].GetComponent<Text>().text = MiningSkill.mineExp + " / " + PlayerEngine.FindNextLevelExpRequirement(MiningSkill.mineLvl + 1);
        else
            expText[4].GetComponent<Text>().text = MiningSkill.mineExp + " / " + PlayerEngine.FindNextLevelExpRequirement(MiningSkill.mineLvl);

        if (!PlayerEngine.MaxLevel(CraftingSkill.craftLvl))
            expText[5].GetComponent<Text>().text = CraftingSkill.craftExp + " / " + PlayerEngine.FindNextLevelExpRequirement(CraftingSkill.craftLvl + 1);
        else
            expText[5].GetComponent<Text>().text = CraftingSkill.craftExp + " / " + PlayerEngine.FindNextLevelExpRequirement(CraftingSkill.craftLvl);

        lvlText[0].GetComponent<Text>().text = "Lv. " + StrengthSkill.strLvl;
        lvlText[1].GetComponent<Text>().text = "Lv. " + DefenseSkill.defLvl;
        lvlText[2].GetComponent<Text>().text = "Lv. " + PlayerHealth.pHealthLevel;
        lvlText[3].GetComponent<Text>().text = "Lv. " + MagicSkill.magLvl;
        lvlText[4].GetComponent<Text>().text = "Lv. " + MiningSkill.mineLvl;
        lvlText[5].GetComponent<Text>().text = "Lv. " + CraftingSkill.craftLvl;

        yourCash.GetComponent<Text>().text = PlayerEngine.pCash.ToString();  
    }

    public void whichScene(string sceneName)
    {
        scene = sceneName;
        if (currentScene == scene)
        {
            Debug.Log("You are already in the village!");
            return;
        }

        currentScene = sceneName;
    }

    public void StartFade(bool teleportClicked)
    {
        fade = true;
        teleport = teleportClicked;
    }

    void OnGUI()
    {
        if (fade && teleport)
        {
            //Player disappears, particles
            Initiate.Fade(scene, loadToColor, 0.5f);
            fade = false; teleport = false;
        }
        else if (fade)
        {
            Initiate.Fade(scene, loadToColor, 0.5f);
            fade = false;
        }
    }

    public void EnlargeButton(GameObject button)
    {
        button.gameObject.GetComponent<RectTransform>().sizeDelta += new Vector2((float)increment, (float)increment);
    }

    public void RevertButton(GameObject button)
    {
        button.gameObject.GetComponent<RectTransform>().sizeDelta -= new Vector2((float)increment, (float)increment);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
