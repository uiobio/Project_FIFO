using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public bool IsPaused = false;

    [System.NonSerialized] public GameObject MusicManager;

    [Header("Currency")]
    public int Currency;

    [Header("UI")]
    public float PlayerHealth = -1;
    public float PlayerMaxHealth = 20;
    [SerializeField] private GameObject mainUIPrefab;
    [SerializeField] private GameObject musicManagerPrefab;
    [SerializeField] private GameObject upgradePrefab;
    [SerializeField] private ProgressBar healthBar;
    [SerializeField] private TMP_Text currencyText;
    public GameObject ParentUI;
    private GameObject upgradesUI;
    private GameObject pauseMenuUI;
    private GameObject healthBarUI;
    private Button resumeButton;
    private Button pauseButton;
    private Button quitButton;
    private Button menuButton;
    private static PatternUIManager patternUIManager;

    [Header("Enemy Spawning")]
    public float EnemySpawnWarningTime;
    public int CurrentRoom;
    private EnemySpawning enemySpawner;

    [Header("Patterns")]
    // The longest an element pattern can be
    public const int MAX_PATTERN_LEN = 5;
    public PatternFuncs PatternAbilityManager;
    public List<int> PatternRecord = new();
    public Transform LeftPoint; // Used for pattern functions that require a leftmost point in the room, like Damage Sweep
    public Transform RightPoint; // Used for pattern functions that require a rightmost point in the room, like Damage Sweep
    [SerializeField] private (int, int) currentPattern = (-1, -1);
    [System.NonSerialized] public List<string> ElementTypes = new() { "Earth", "Fire", "Ice", "Wind" };
    [System.NonSerialized] public List<Color> ElementTypeColors = new() { Color.green, Color.red, Color.blue, Color.cyan };
    [System.NonSerialized] public List<List<(int, string, Action)>> Patterns = new();
    
    [Header("Upgrades")]
    public const int MAX_PLAYER_UPGRADES = 5; // Most upgrades the player can have at once
    public float UpgradeIconUnplugOffset; // how far the upgrade UI icons "unplug" when you select 
    public int[] PlayerHeldUpgradeIds = new int[MAX_PLAYER_UPGRADES];
    public List<GameObject> PlayerHeldUpgradeIcons = new(); // The UI icon gameObjects for the upgrades that the player currently has
    [System.NonSerialized] public List<Upgrade> Upgrades = new();
    [System.NonSerialized] public List<Upgrade> PlayerHeldUpgrades = new();
    [System.NonSerialized] public int CurrentlyHoveredUpgradeIndex = 0; // The upgrade the player is hovering over when paused
    [System.NonSerialized] public bool IsHoveringUpgradeIcon = false; // If the player is currently hovering an upgrade icon
    [System.NonSerialized] public int CurrentlySelectedUpgradeIndex = 0;  // The upgrade the player clicks on when replacing/discarding
    [System.NonSerialized] public bool IsCurrentlySelectingUpgrade = false; // If the player is currently selecting an upgrade to replace
    [System.NonSerialized] public bool IsCurrentlySelectingRecycle = false; // If the player is currently selecting an upgrade to discard

    // Upgrade modifiers indicate the amount of effect an upgrade actually applies during runtime
    public int precisionUpgradeModifier = 0;
    public int hardwareAccelUpgradeModifier = 0;
    public int twoBirdsUpgradeModifier = 0;
    public int fortifiedUpgradeModifier = 0;
    public int bootUpUpgradeModifier = 15;
    public int spiceLifeUpgradeModifier = 0;
    public int gitRestoreUpgradeModifier = 0;
    public int bloodthirstyUpgradeModifier = 5;
    public int greedyUpgradeModifier = 0;
    public int thornsUpgradeModifier = 0;

    // Upgrade values are the fixed amount that the upgrade should affect by
    public int precisionUpgradeModifierValue = 3;
    public int hardwareAccelUpgradeModifierValue = 10;
    public int twoBirdsUpgradeModifierValue = 10;
    public int fortifiedUpgradeModifierValue = 10;
    public int bootUpUpgradeModifierValue = 10;
    public int spiceLifeUpgradeModifierValue = 1;
    public int gitRestoreUpgradeModifierValue = 10;
    public int bloodthirstyUpgradeModifierValue = 1;
    public int greedyUpgradeModifierValue = 5;
    public int thornsUpgradeModifierValue = 10;

    //-------------------------------------Instantiation----------------------------------------
    private void Awake() //Makes levelmanager callable in any script: LevelManager.Instance.[]
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Instantiate the main UI
            ParentUI = Instantiate(mainUIPrefab);
            ParentUI.name = "UI";
            ParentUI.transform.Find("MainCanvas").GetComponent<Canvas>().sortingOrder = 1000;
            DontDestroyOnLoad(ParentUI);

            // Get HUD elements to keep them up to date
            healthBarUI = ParentUI.transform.Find("MainCanvas/Healthbar").gameObject;
            healthBar = ParentUI.GetComponent<ProgressBar>();
            currencyText = ParentUI.transform.Find("MainCanvas/Healthbar/Currency").GetComponent<TMP_Text>();

            // Assign the upgradesUI gameobject for easier access
            upgradesUI = ParentUI.transform.Find("MainCanvas/Upgrades").gameObject;

            // Assign the pauseMenuUI gameobject for easier access
            pauseMenuUI = ParentUI.transform.Find("MainCanvas/PauseMenu").gameObject;

            // Get Buttons for PauseMenu
            resumeButton = pauseMenuUI.transform.Find("ResumeButton").GetComponent<Button>();
            pauseButton = pauseMenuUI.transform.Find("PauseButton").GetComponent<Button>();
            quitButton = pauseMenuUI.transform.Find("QuitButton").GetComponent<Button>();
            menuButton = pauseMenuUI.transform.Find("MenuButton").GetComponent<Button>();
        }
        else
        {
            Destroy(gameObject); // Prevents duplicates on scene reload
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    { 
        CurrentRoom = 0;
        enemySpawner = gameObject.GetComponent<EnemySpawning>();

        // Give self 0 coins to set HUD currency
        GainCoin(0);
        MusicManager = Instantiate(musicManagerPrefab);

        // FIXME: Setting up the Patterns List should be move to gameconstants when one exists.
        // Create lists for all of the Patterns
        List<(int, string, Action)> Len1Patterns = new();
        List<(int, string, Action)> Len2Patterns = new() {
            (11, "Pair", PatternAbilityManager.StartSpeedBoost) 
        };
        List<(int, string, Action)> Len3Patterns = new() {
            (121, "Sandwich", PatternAbilityManager.DamageSweep), 
            (111, "Three of a kind", PatternAbilityManager.DamageSweep)
        };
        List<(int, string, Action)> Len4Patterns = new() {
            (1221, "Big Sandwich", PatternAbilityManager.DamageSweep), 
            (1111, "Four of a kind", PatternAbilityManager.DamageSweep), 
            (4321, "Rainbow", PatternAbilityManager.DamageSweep), 
            (2211, "Two Pair", PatternAbilityManager.DamageSweep), 
            (1321, "Mini Club", PatternAbilityManager.DamageSweep)
        };
        List<(int, string, Action)> Len5Patterns = new() {
            (12121, "Big Mac", PatternAbilityManager.DamageSweep), 
            (11111, "Five of a kind", PatternAbilityManager.DamageSweep), 
            (14321, "Club Sandwich", PatternAbilityManager.DamageSweep), 
            (22211, "Full House", PatternAbilityManager.DamageSweep), 
            (12321, "Double Decker", PatternAbilityManager.DamageSweep), 
            (11211, "Fat Sandwich", PatternAbilityManager.DamageSweep)
        };

        // Add all of the Patterns to the Patterns double list
        Patterns.Add(Len1Patterns);
        Patterns.Add(Len2Patterns);
        Patterns.Add(Len3Patterns);
        Patterns.Add(Len4Patterns);
        Patterns.Add(Len5Patterns);

        resumeButton.onClick.AddListener(ResumeGame);
        pauseButton.onClick.AddListener(PauseGame);
        quitButton.onClick.AddListener(QuitGame);
        menuButton.onClick.AddListener(GoToMainMenu);

        pauseMenuUI.SetActive(false); // Ensure menu is hidden at start

        // FIXME: add to game_constants
        // Add all upgrades to the Upgrade list; move to game_constants when one exists
        // See the constructor for the Upgrade class in 'UpgradeManager.cs' to find detailed info about parameters.
        Upgrades.Add(new Upgrade("Precision", "Deal +[X] extra Damage on every hit (currently adding [N] Damage)", (float)precisionUpgradeModifierValue, (float)precisionUpgradeModifier, "", 0, 5, "Assets/Sprites/Upgrades/whetstone.png", "Assets/Sprites/Upgrades/whetstoneVert.png", "red"));
        Upgrades.Add(new Upgrade("Hardware Acceleration", "Increase dash range by [X]%", (float)hardwareAccelUpgradeModifierValue, (float)hardwareAccelUpgradeModifier, "", 0, 5, "Assets/Sprites/Upgrades/hardwareAccel.png", "Assets/Sprites/Upgrades/hardwareAccelVert.png", "purple"));
        Upgrades.Add(new Upgrade("Two Birds", "Your attacks hit twice, second attack does [X]% and also applies on-hit effects", (float)twoBirdsUpgradeModifierValue, (float)twoBirdsUpgradeModifier, "", 0, 5, "Assets/Sprites/Upgrades/twobirds.png", "Assets/Sprites/Upgrades/twobirdsVert.png", "red"));
        Upgrades.Add(new Upgrade("Fortified", "Enemy projectiles deal [X]% less Damage", (float)fortifiedUpgradeModifierValue, (float)fortifiedUpgradeModifier, "", 0, 5, "Assets/Sprites/Upgrades/fortify.png", "Assets/Sprites/Upgrades/fortifyVert.png", "green"));
        Upgrades.Add(new Upgrade("Boot Up", "Gain a [X]% speed boost for the first [N] sec of each room", (float)bootUpUpgradeModifierValue, (float)bootUpUpgradeModifier, "", 0, 5, "Assets/Sprites/Upgrades/bootUp.png", "Assets/Sprites/Upgrades/bootUpVert.png", "purple"));
        Upgrades.Add(new Upgrade("Spice of Life", "Gain [X]% additional Damage for each unique combo used this run", (float)spiceLifeUpgradeModifierValue, (float)spiceLifeUpgradeModifierValue, "", 0, 5, "Assets/Sprites/Upgrades/spiceOfLife.png", "Assets/Sprites/Upgrades/spiceOfLifeVert.png", "red"));
        Upgrades.Add(new Upgrade("git restore", "When entering a new non-shop room, restore [X]% of max health", (float)gitRestoreUpgradeModifierValue, (float)gitRestoreUpgradeModifier, "", 0, 5, "Assets/Sprites/Upgrades/gitRestore.png", "Assets/Sprites/Upgrades/gitRestoreVert.png", "green"));
        Upgrades.Add(new Upgrade("Bloodthirsty", "Gain [X] health upon killing [N] enemies", (float)bloodthirstyUpgradeModifierValue, (float)bloodthirstyUpgradeModifier, "", 0, 5, "Assets/Sprites/Upgrades/bloodthirsty.png", "Assets/Sprites/Upgrades/bloodthirstVert.png", "red"));
        Upgrades.Add(new Upgrade("Greedy", "Gain [X]% more gold from enemy kills", (float)greedyUpgradeModifierValue, (float)greedyUpgradeModifier, "", 0, 5, "Assets/Sprites/Upgrades/chipMagnet.png", "Assets/Sprites/Upgrades/chipMagnetVert.png", "yellow"));
        Upgrades.Add(new Upgrade("Thorns", "When you take Damage, deal [X]% to the enemy that hit you", (float)thornsUpgradeModifierValue, (float)thornsUpgradeModifier, "", 0, 5, "Assets/Sprites/Upgrades/thorns.png", "Assets/Sprites/Upgrades/thornsVert.png", "green"));

        // Id should always = index in Upgrades list
        for (int i = 0; i < Upgrades.Count; i++)
        {
            Upgrades[i].Id = i;
        }

        // Sets which upgrades the player has based on the Id array
        SetPlayerHeldUpgradesFromIds();

        // Instantiate the UI icons of those held upgrades
        InstantiateIcons();
        ParentUI.transform.Find("MainCanvas/Upgrades/LineBL").gameObject.SetActive(false);
        ParentUI.transform.Find("MainCanvas/Upgrades/LineTL").gameObject.SetActive(false);
        ParentUI.transform.Find("MainCanvas/Upgrades/LineTR").gameObject.SetActive(false);
        ParentUI.transform.Find("MainCanvas/Upgrades/LineBR").gameObject.SetActive(false);
        ParentUI.transform.Find("MainCanvas/Upgrades/HoverSquare").gameObject.SetActive(false);
        ParentUI.transform.Find("MainCanvas/Upgrades/Label").gameObject.SetActive(false);
    }

    // Instantiates upgrade UI icons according to the PlayerHeldUpgrades list
    private void InstantiateIcons()
    {
        for (int i = 0; i < PlayerHeldUpgrades.Count; i++)
        {
            GameObject uiIcon = Instantiate(upgradePrefab);
            uiIcon.name = uiIcon.name + "_" + i.ToString();
            uiIcon.GetComponent<UpgradeManager>().Upgrade = PlayerHeldUpgrades[i];
            uiIcon.GetComponent<UpgradeManager>().UpgradeIndex = i;
            uiIcon.GetComponent<UpgradeManager>().Upgrade.UIOrShopItem = "UI";
            uiIcon.GetComponent<UpgradeManager>().CreateGameObjects();
            PlayerHeldUpgradeIcons.Add(uiIcon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Press 'P' to toggle pause
        {
            TogglePause();
        }
    }

    // Dummy function to test key input
    void Dummy()
    {
        Debug.Log("Dummy key pressed");
    }

    //---------------------------------------Functions for Currency-----------------------------------
    public void GainCoin(int val)
    {
        Currency += val;
        currencyText.text = Currency.ToString() + " CHIPS";
    }

    //--------------------------------------Functions for Player Health------------------------------------
    public void SetMaxHealth(float val)
    {
        PlayerMaxHealth = val;
    }

    public void SetHealth(float val)
    {
        PlayerHealth = val;
        healthBar.SetProgress(PlayerHealth/PlayerMaxHealth);
    }

    public void ResetPlayerHealth()
    {
        SetHealth(PlayerMaxHealth);
    }

    //----------------------------------------Functions for Patterns------------------------------------
    public static void AddPatternUI(GameObject patternManager)
    {
        patternUIManager = patternManager.GetComponent<PatternUIManager>();
    }

    public void UpdatePattern(int type) {
        // Adds a type to the pattern record. Should be called whenever an enemy is killed.
        // This then checks the Pattern Record to see if any Patterns have occurred.
        AddToPattern(type);
        ((int, int), (int, int)) ActivePattern = CheckPatterns();
        (int, int) success = ActivePattern.Item1;
        (int, int) subPattern = ActivePattern.Item2;
        string pattern = "";
        if (success.Item1 != -1)
        {
            pattern = Patterns[success.Item1][success.Item2].Item2;
        }

        currentPattern = success;
        if (patternUIManager != null)
        {
            patternUIManager.UpdatePatternName(pattern);
            patternUIManager.UpdateQueueColors(subPattern.Item1, subPattern.Item2);
        }
    }

    private void AddToPattern(int type)
    {
        Debug.Log($"Add to Pattern {type}");
        // Add the passed type to the PatternRecord
        PatternRecord.Add(type);
        if (PatternRecord.Count > MAX_PATTERN_LEN)
        {
            PatternRecord.Remove(PatternRecord[0]);
        }
    }

    private int TypeToChar()
    {
        return TypeToChar(PatternRecord.Count, 0);
    }

    private int TypeToChar(int start, int end)
    {
        // Translates the 5 most recent slain enemy ElementTypes to a 5 int number to compare with patterns
        int ret = 0;
        int counter = 1;

        Dictionary<int, int> Translations = new();
        // Iterate from most recent to oldest of saved ElementTypes
        for (int i = start; i >= end; i--)
        {
            int t = PatternRecord[i];
            if (!Translations.ContainsKey(t))
            {
                Translations.Add(t, counter);
                counter++;
            }
            ret += (int)(Mathf.Pow(10, start - i) * Translations[t]);
        }
        return ret;
    }

    private ((int, int), (int, int)) CheckPatterns()
    {
        return CheckPatterns(-1, PatternRecord.Count - 1, 0);
    }

    private ((int, int), (int, int)) CheckPatterns(int seqIn, int start, int end)
    {
        if (seqIn == 0) { return ((-1, -1), (-1, -1)); }
        int seq = seqIn;
        if (seqIn == -1)
        {
            if (start < end)
            {
                return ((-1, -1), (-1, -1));
            }
            seq = TypeToChar(start, end);
        }

        int l = start - end;
        int s = PatternRecord.Count - 1;
        // Loop through all patterns of size and smaller
        for (int i = 0; i < Patterns[l].Count; i++)
        {
            if (Patterns[l][i].Item1 == seq)
            {
                // Found Matching Pattern! Return the name
                return ((l, i), (start, end));
            }
        }
        // Go to smaller pattern if no pattern found in that list.
        // Left
        ((int, int), (int, int)) left = CheckPatterns(-1, start, end + 1);
        (int, int) subLeft = left.Item1;
        // Right
        ((int, int), (int, int)) right = CheckPatterns(-1, start - 1, end);
        (int, int) subRight = right.Item1;

        if (subLeft.Item1 == subRight.Item1)
        {
            return subLeft.Item2 > subRight.Item2 ? left : right;
        }
        return subLeft.Item1 > subRight.Item1 ? left : right;
    }

    public void UsePattern()
    {
        // Use the current Pattern's ability
        string patternName = "";
        if (currentPattern.Item1 != -1)
        {
            // Update the name for debugging and use ability
            patternName = Patterns[currentPattern.Item1][currentPattern.Item2].Item2;
            int len = currentPattern.Item1;
            int index = currentPattern.Item2;
            Patterns[len][index].Item3();
        }
        Debug.Log($"Using {currentPattern}: {patternName}");
        // Reset the pattern state
        currentPattern = (-1, -1);
        ClearPatternQueue();
        patternUIManager.ClearQueue();
    }

    private void ClearPatternQueue()
    {
        PatternRecord.RemoveAll(c => true);
    }

    // Sets the leftmost and rightmost points of the room - used for pattern func sweeps
    public void SetLeftPoint(Transform t)
    {
        LeftPoint = t;
    }

    public void SetRightPoint(Transform t)
    {
        RightPoint = t;        
    }

    //-------------------------------------Functions for Pause Menu-------------------------------------
    public void TogglePause()
    {
        if (!IsPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        IsPaused = true;
        pauseMenuUI.SetActive(true); // Show menu
        MusicManager.GetComponent<MusicManager>().AudioSource.Pause(); // Pause Music
        Time.timeScale = 0f; // Pause game
    }

    private void ResumeGame()
    {
        IsPaused = false;
        IsHoveringUpgradeIcon = false;
        pauseMenuUI.SetActive(false); // Hide menu
        MusicManager.GetComponent<MusicManager>().AudioSource.UnPause(); // Unpause Music
        Time.timeScale = 1f; // Resume game
    }

    private void QuitGame()
    {
        Time.timeScale = 1f; // Reset before quitting
        Application.Quit(); // Works only in a built game

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stops play mode in the Unity Editor
        #endif
    }

    private void GoToMainMenu()
    {
        Time.timeScale = 1f; // Reset before loading new scene
        IsPaused = false;
        pauseMenuUI.SetActive(false); // Hide menu
        SceneManager.LoadScene("Menu Room"); // Replace with actual Main Menu scene name
    }

    //---------------------------------Functions for Buying & Selling Upgrades------------------------------------
    // Adds upgrades to the PlayerHeldUpgrades list based on the array of upgrade Ids
    public void SetPlayerHeldUpgradesFromIds()
    {
        for (int i = 0; i < MAX_PLAYER_UPGRADES; i++)
        {
            if (PlayerHeldUpgradeIds[i] != -1)
            {
                PlayerHeldUpgrades.Add(Upgrades[PlayerHeldUpgradeIds[i]]);
                ApplyUpgradeModifiers(Upgrades[PlayerHeldUpgradeIds[i]]);
            }
        }


    }

    // Adds an upgrade to the player's held list
    // Returns true if the upgrade successfully adds to the list
    public bool AddPlayerUpgrade(Upgrade upgrade, GameObject shop)
    {
        // If the upgrade is already held by the player, then...
        if (Array.IndexOf(PlayerHeldUpgradeIds, upgrade.Id) > -1 && upgrade.Cost <= Currency && !IsCurrentlySelectingUpgrade)
        {
            // Increment the upgrade modifier by some specified amount (i.e. "level up" the upgrade)
            ApplyUpgradeModifiers(upgrade);
            GainCoin(-1 * upgrade.Cost);
            return true;
        }
        // Otherwise, if the player's max upgrade slots would be exceeded by adding this upgrade, then...
        else if (PlayerHeldUpgrades.Count + 1 > MAX_PLAYER_UPGRADES && upgrade.Cost <= Currency && !IsCurrentlySelectingUpgrade)
        {
            // Start the coroutine prompt to select/confirm/replace a currently held upgrade
            ReplacePlayerUpgrade(upgrade, shop);
            // Return false while the above function runs its coroutine, so the upgrade doesn't yet disappear from the ShopItem.
            return false;
        }
        // Otherwise (if the upgrade is not held by the player AND the max upgrade slots would not be exceeded by adding this upgrade), then...
        else if (upgrade.Cost <= Currency && !IsCurrentlySelectingUpgrade)
        {
            // Find the first empty slot in the PlayerHeldUpgradeIds array
            int index = 0;
            for (int i = 0; i < 5; i++)
            {
                if (PlayerHeldUpgradeIds[i] == -1)
                {
                    index = i;
                    break;
                }
            }
            ApplyUpgradeModifiers(upgrade);

            // Put the upgrade in the first empty slots of the PlayerHeldUpgradeIds array and PlayerHeldUpgrades list
            PlayerHeldUpgradeIds[index] = upgrade.Id;
            PlayerHeldUpgrades.Add(upgrade);

            // UI is updated accordingly
            GameObject upgradeUIIcon = Instantiate(upgradePrefab);
            upgradeUIIcon.SetActive(false);
            upgrade.UIOrShopItem = "UI";
            upgradeUIIcon.GetComponent<UpgradeManager>().Upgrade = upgrade;
            upgradeUIIcon.GetComponent<UpgradeManager>().UpgradeIndex = index;
            upgradeUIIcon.GetComponent<UpgradeManager>().CreateGameObjects();
            upgradeUIIcon.SetActive(true);

            // UI icon is added to the PlayerHeldUpgradeIcons list
            PlayerHeldUpgradeIcons.Add(upgradeUIIcon);
            GainCoin(-1 * upgrade.Cost);
            return true;
        }
        else { 
            // U ARE TOO BROKE TO AFFORD UPGRADE
            return false;
        }
    }
    // Functionality: Starts a coroutine defined in PlayerInputManager that prompts the player to select and confirm which upgrade they want to swap
    //                Player can leave the radius if they choose not to confirm and this effectively cancels the coroutine.
    // Params:
    //  Upgrade newUpgrade: the upgrade you want to replace the one in the player list with
    //  GameObject shop: the ShopItem that the incoming upgrade is correspondent with
    private void ReplacePlayerUpgrade(Upgrade newUpgrade, GameObject shop)
    {
        StartCoroutine(PlayerInputManager.Instance.SelectAndConfirmReplace(newUpgrade, shop));
    }

    // Functionality: Starts a coroutine defined in PlayerInputManager that prompts the player to select and confirm which upgrade they want to recycle
    //                Player can leave the radius if they choose not to confirm and this effectively cancels the coroutine.
    public void RecyclePlayerUpgrade(GameObject trashcan)
    {
        StartCoroutine(PlayerInputManager.Instance.SelectAndConfirmRecycle(trashcan));
    }

    // Called within the coroutine SelectAndConfirmReplace in PlayerInputManager (structured this way to keep all player inputs inside that script... it's a bit clunky but it works)
    // Updates all the necessary lists that track the player's currently held upgrades
    // Updates the UI with the new upgrade's icon and destroys the old upgrade's icon.
    public void SwapOutUpgrade(Upgrade newUpgrade, GameObject shop, Vector2 originalSlotPosition)
    {
        RemoveUpgradeModifiers(PlayerHeldUpgrades[CurrentlySelectedUpgradeIndex]);
        PlayerHeldUpgrades[CurrentlySelectedUpgradeIndex] = newUpgrade;
        ApplyUpgradeModifiers(newUpgrade);
        PlayerHeldUpgradeIds[CurrentlySelectedUpgradeIndex] = newUpgrade.Id;

        GameObject upgradeUIIcon = Instantiate(upgradePrefab);
        upgradeUIIcon.SetActive(false);
        newUpgrade.UIOrShopItem = "UI";
        upgradeUIIcon.GetComponent<UpgradeManager>().Upgrade = newUpgrade;
        upgradeUIIcon.GetComponent<UpgradeManager>().UpgradeIndex = CurrentlySelectedUpgradeIndex;
        upgradeUIIcon.GetComponent<UpgradeManager>().CreateGameObjects();
        upgradeUIIcon.SetActive(true);

        Destroy(PlayerHeldUpgradeIcons[CurrentlySelectedUpgradeIndex].GetComponent<UpgradeManager>().UpgradeUIIcon);
        Destroy(PlayerHeldUpgradeIcons[CurrentlySelectedUpgradeIndex]);
        

        Debug.Log("Replacing upgrade: " + PlayerHeldUpgradeIcons[CurrentlySelectedUpgradeIndex].GetComponent<UpgradeManager>().Upgrade.Name + " (Upgrade slot index " + CurrentlySelectedUpgradeIndex + ")");
        PlayerHeldUpgradeIcons[CurrentlySelectedUpgradeIndex] = upgradeUIIcon;
        upgradeUIIcon.GetComponent<UpgradeManager>().UpgradeUIIcon.GetComponent<RectTransform>().anchoredPosition = originalSlotPosition;
        GainCoin(-1 * newUpgrade.Cost);
        shop.GetComponent<ShopItem>().DestroyChildren();
    }

    // Called within the coroutine SelectAndConfirmRecycle in PlayerInputManager (structured this way to keep all player inputs inside that script... it's a bit clunky but it works)
    // Updates all lists that track the player's currently held upgrades
    // Updates the UI with the new upgrade's icon and destroys the old upgrade's icon
    // Moves all UI icons up to fill the gap left by the old upgrade
    public void RemoveUpgrade(GameObject trashcan) 
    {
        // Modifiers go poof
        RemoveUpgradeModifiers(PlayerHeldUpgrades[CurrentlySelectedUpgradeIndex]);
        Upgrade upgrade = PlayerHeldUpgrades[CurrentlySelectedUpgradeIndex];

        // Trashcan sprite is the same as the recycled upgrade color
        if (upgrade.Color == "green")
        {
            trashcan.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = trashcan.GetComponent<Trashcan>().TrashcanGreen;
        }
        else if (upgrade.Color == "purple")
        {
            trashcan.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = trashcan.GetComponent<Trashcan>().TrashcanPurple;
        }
        else if (upgrade.Color == "red") {
            trashcan.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = trashcan.GetComponent<Trashcan>().TrashcanRed;
        }
        else if (upgrade.Color == "yellow")
        {
            trashcan.transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = trashcan.GetComponent<Trashcan>().TrashcanYellow;
        }

        // Remove the upgrade data
        PlayerHeldUpgrades.RemoveAt(CurrentlySelectedUpgradeIndex);

        // Destroy the UI gameObject and remove it form the list
        Destroy(PlayerHeldUpgradeIcons[CurrentlySelectedUpgradeIndex].GetComponent<UpgradeManager>().UpgradeUIIcon);
        Destroy(PlayerHeldUpgradeIcons[CurrentlySelectedUpgradeIndex]);
        PlayerHeldUpgradeIcons.RemoveAt(CurrentlySelectedUpgradeIndex);

        // Move all UI icons up to fill the gap left by the old upgrade
        for (int i = CurrentlySelectedUpgradeIndex; i < PlayerHeldUpgradeIcons.Count; ++i)
        {
            PlayerHeldUpgradeIcons[i].GetComponent<UpgradeManager>().UpgradeUIIcon.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 108);
            PlayerHeldUpgradeIcons[i].GetComponent<UpgradeManager>().UpgradeIndex = i;
        }

        // Shift all Ids left by one, and set the last Id to -1
        PlayerHeldUpgradeIds[4] = -1;
        for (int i = CurrentlySelectedUpgradeIndex; i < 4; i++)
        {
            PlayerHeldUpgradeIds[i] = PlayerHeldUpgradeIds[i + 1];     
        }
        
        // Deactivate the label
        trashcan.GetComponent<Trashcan>().UILabel.SetActive(false);
    }

    // Called when upgrades are added to the player held upgrades list, essentially applies the effects of upgrades.
    // POST-MVP: implement more effects
    private void ApplyUpgradeModifiers(Upgrade upgrade) 
    {
        switch (upgrade.Name) 
        {
            case "Precision":
                precisionUpgradeModifier += precisionUpgradeModifierValue;
                upgrade.N = precisionUpgradeModifier;
                break;
            case "Hardware Acceleration":
                hardwareAccelUpgradeModifier += hardwareAccelUpgradeModifierValue;
                upgrade.N = hardwareAccelUpgradeModifier;
                break;
            case "Two Birds":
                twoBirdsUpgradeModifier += twoBirdsUpgradeModifierValue;
                upgrade.N = twoBirdsUpgradeModifier;
                break;
            case "Fortified":
                fortifiedUpgradeModifier += fortifiedUpgradeModifierValue;
                upgrade.N = fortifiedUpgradeModifier;
                break;
            case "Boot Up":
                bootUpUpgradeModifier += bootUpUpgradeModifierValue;
                upgrade.N = bootUpUpgradeModifier;
                break;
            case "Spice of Life":
                spiceLifeUpgradeModifier += spiceLifeUpgradeModifierValue;
                upgrade.N = spiceLifeUpgradeModifier;
                break;
            case "Git Restore":
                gitRestoreUpgradeModifier += gitRestoreUpgradeModifierValue;
                upgrade.N = gitRestoreUpgradeModifier;
                break;
            case "Bloodthirsty":
                bloodthirstyUpgradeModifier += bloodthirstyUpgradeModifierValue;
                upgrade.N = bloodthirstyUpgradeModifier;
                break;
            case "Greedy":
                greedyUpgradeModifier += greedyUpgradeModifierValue;
                upgrade.N = greedyUpgradeModifier;
                break;
            case "Thorns":
                thornsUpgradeModifier += thornsUpgradeModifierValue;
                upgrade.N = thornsUpgradeModifier;
                break;
            default:
                Debug.Log("Modifier not found for upgrade with name: \"" + upgrade.Name + "\"");
                break;
        }
        upgrade.UpdateDesc();
        if (ShopRoomSetup.Instance != null) 
        {
            ShopRoomSetup.Instance.UpdateShopItemLabel(upgrade);
        }
    }

    // Called when upgrades are removed from the player held upgrades list, essentially removes the effects of upgrades.
    // POST-MVP: implement more effects
    private void RemoveUpgradeModifiers(Upgrade upgrade) 
    {
        switch (upgrade.Name)
        {
            case "Precision":
                precisionUpgradeModifier = 0;
                upgrade.N = precisionUpgradeModifier;
                break;
            case "Hardware Acceleration":
                hardwareAccelUpgradeModifier = 0;
                upgrade.N = hardwareAccelUpgradeModifier;
                break;
            case "Two Birds":
                twoBirdsUpgradeModifier = 0;
                upgrade.N = twoBirdsUpgradeModifier;
                break;
            case "Fortified":
                fortifiedUpgradeModifier = 0;
                upgrade.N = fortifiedUpgradeModifier;
                break;
            case "Boot Up":
                bootUpUpgradeModifier = 0;
                upgrade.N = bootUpUpgradeModifier;
                break;
            case "Spice of Life":
                spiceLifeUpgradeModifier= 0;
                upgrade.N = spiceLifeUpgradeModifier;
                break;
            case "Git Restore":
                gitRestoreUpgradeModifier = 0;
                upgrade.N = gitRestoreUpgradeModifier;
                break;
            case "Bloodthirsty":
                bloodthirstyUpgradeModifier = 0;
                upgrade.N = bloodthirstyUpgradeModifier;
                break;
            case "Greedy":
                greedyUpgradeModifier = 0;
                upgrade.N = greedyUpgradeModifier;
                break;
            case "Thorns":
                thornsUpgradeModifier = 0;
                upgrade.N = thornsUpgradeModifier;
                break;
            default:
                Debug.Log("Modifier not found for upgrade with name: \"" + upgrade.Name + "\"");
                break;
        }
        upgrade.UpdateDesc();
        if (ShopRoomSetup.Instance != null)
        {
            ShopRoomSetup.Instance.UpdateShopItemLabel(upgrade);
        }
    }

    //---------------------------------Functions for Tracking # of Rooms Visited------------------------------------
    public void IncRoom()
    {
        Debug.Log("NEXT ROOM!");
        CurrentRoom++;
    }

    public void ResetRoom()
    {
        Debug.Log("RESET ROOM!");
        CurrentRoom = 0;
        patternUIManager.ClearQueue();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"In room {CurrentRoom}");
        if(enemySpawner != null){
            enemySpawner.GenerateEnemies(CurrentRoom);
        }
    }
}

