using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_manager : MonoBehaviour
{
    public static Level_manager instance;

    //FIXME: Add this list to a game_constants file
    List<string> types = new List<string>() {"Earth", "Fire", "Ice", "Wind"};
    int Max_pattern_len = 3;

    List<string> Pattern_record = new List<string>();

    //FIXME: Add this to a game_constants file
    List<(int, string)> Patterns = new List<(int, string)>() {
        (121, "Sandwich"), (0, "Reduce"),
        (11, "Pair")
    };
    List<int> Pattern_bounds = new List<int>();

    private void Awake() //Makes levelmanager callable in any script: Level_manager.instance.[]
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //record the index of each 0 in Patterns
        Pattern_bounds.Add(-1); //length 1 doesn't exist
        for (int i = 0; i < Patterns.Count; i++){
            if(Patterns[i].Item1 == 0){
                Pattern_bounds.Add(i-1);
            }
        }
        Pattern_bounds.Add(Patterns.Count-1); //Check all for max length


        // Print to Check
        print("PB<");
        for (int i = 0; i < Pattern_bounds.Count; i++){
            print(Pattern_bounds[i]);
        }
        print(">");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Dummy")){
            Dummy();
        }

        // Key inputs for testing patterns- feel free to delete/ignore
        if(Input.GetKeyDown("1")){
            AddToPattern("Earth");
        }
        if(Input.GetKeyDown("2")){
            AddToPattern("Fire");
        }
        if(Input.GetKeyDown("3")){
            AddToPattern("Ice");
        }
        if(Input.GetKeyDown("4")){
            AddToPattern("Wind");
        }
    }

    void Dummy(){
        Debug.Log("Dummy key pressed");
    }

    void AddToPattern(string type){
        //Add the passed type to the pattern_record
        Pattern_record.Add(type);

        if(Pattern_record.Count > Max_pattern_len){
            Pattern_record.Remove(Pattern_record[0]);
        }

        int temp = TypeToChar();
    }

    int TypeToChar(){
        //Translates the 5 most recent slain enemy types to a 5 int number to compare with patterns
        int ret = 0;
        int counter = 1;

        int c = Pattern_record.Count;
        Dictionary <string, int> Translations = new Dictionary<string, int>();
        //Iterate from most recent to oldest of saved types
        for (int i = c-1; i >= 0; i--){
            string t = Pattern_record[i];
            if(!Translations.ContainsKey(t)){
                Translations.Add(t, counter);
                counter++;
            }
            ret += (int)(Mathf.Pow(10, i) * Translations[t]);
        }
        print(ret);
        CheckPatterns(ret);
        return ret;
    }

    int CheckPatterns(int Seq){
        int s = Pattern_bounds[Pattern_record.Count];
        //Loop through all patterns of size and smaller
        for (int i=s; i>=0; i--){ //FIXME: bounds of this
            if (Patterns[i].Item1 == 0){
                //If hit a bound, reduce seq by 1
                Seq = (int)(Seq/10);
            }
            else if (Patterns[i].Item1 == Seq){
                //Found Matching Pattern! Print the name
                print(Patterns[i].Item2);
                return 1;
            }
        }

        print("None");
        return -1;
    }
}
