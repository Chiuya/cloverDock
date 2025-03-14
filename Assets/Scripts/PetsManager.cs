using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetsManager : MonoBehaviour
{
    public GameObject player;
    public GameObject petPrefab;
    private bool isFishing = false;
    private List<Pet> pets;
    private Pet activePet;
    private Sprite blackCatSprite, raccoonSprite, dogSprite;
    private RuntimeAnimatorController blackCatAnimator, raccoonAnimator, dogAnimator;

    void Awake() {
        initSprites();
        initAnimators();
        LoadPetsFromJSON();
    }
    // Start is called before the first frame update
    void Start()
    {
        LoadActivePetFromJSON();
        //resetPets(); //TEST PURPOSES
    }

    private void initSprites() {
        blackCatSprite = Resources.Load<Sprite>("Pets/Black Cat");
        raccoonSprite = Resources.Load<Sprite>("Pets/Raccoon");
        dogSprite = Resources.Load<Sprite>("Pets/Dog");
    }

    private void initAnimators() {
        blackCatAnimator = Resources.Load<RuntimeAnimatorController>("Animations/Black Cat");
        raccoonAnimator = Resources.Load<RuntimeAnimatorController>("Animations/Raccoon");
        dogAnimator = Resources.Load<RuntimeAnimatorController>("Animations/Dog");
    }

    private void LoadPetsFromJSON()
    {
        // Load the JSON data from the persistent data path
        string filePath = Application.persistentDataPath + "/pets.json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            pets = JsonUtility.FromJson<Pets>(jsonData).petList;
            //Debug.Log("pets.json successfully read");
        } else
        {
            resetPets();
        }
    }

    private void resetPets() {
        //Debug.Log("no pets inventory saved, creating pets");
        pets = new List<Pet>{
            new Pet("Black Cat", "It keeps following me. How purr-sistent!", 10000, false),
            new Pet("Dog", "I've made a fur-ever friend!", 12500, false),
            new Pet("Raccoon", "One man's trash panda is another man's best friend.", 15000, false)
        };
        SavePetsToJSON();
    }

    public void SavePetsToJSON()
    {
        string jsonData = JsonUtility.ToJson(new Pets(pets));
        string filePath = Application.persistentDataPath + "/pets.json";
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    public int getNumPets() {
        return pets.Count;
    }

    public string getPetName(int index) {
        //Debug.Log("getting index of petname at: " + index);
        return pets[index].petName;
    }

    public int getPetValue(int index) {
        return pets[index].value;
    }

    public bool isPetBought(int index) {
        return pets[index].isBought;
    }

    public bool isPetBoughtByName(string name) {
        Pet tar = pets.Find(pets => pets.petName == name);
        if (tar != null) {
            //Debug.Log("pet isBought: " + name + " is " + tar.isBought);
            return tar.isBought;
        } else {
            //Debug.Log("invalid petName for isBought: " + name);
            return false;
        }
    }

    public void setPetBought(string name) {
        Pet tar = pets.Find(pet => pet.petName == name);
        if (tar != null) {
            tar.isBought = true;
            //Debug.Log("pet is bought: " + name);
        } else {
            Debug.Log("pet purchase failed: " + name);
        }
        SavePetsToJSON();
    }

    public Sprite getPetSprite(string petName)
    {
        Sprite ans;
        switch(petName) 
        {
        case "Black Cat":
            ans = blackCatSprite;
            break;
        case "Raccoon":
            ans = raccoonSprite;
            break;
        case "Dog":
            ans = dogSprite;
            break;
        default:
            ans = blackCatSprite;
            Debug.Log("misspelled pet name for sprite");
            break;
        }
        return ans;
    }

    private RuntimeAnimatorController getPetAnimator(string name) {
        RuntimeAnimatorController ans;
        switch(name) 
        {
        case "Black Cat":
            ans = blackCatAnimator;
            break;
        case "Raccoon":
            ans = raccoonAnimator;
            break;
        case "Dog":
            ans = dogAnimator;
            break;
        default:
            ans = blackCatAnimator;
            Debug.Log("misspelled pet name for animator");
            break;
        }
        return ans;
    }

    public string getPetDescription(string name) {
        Pet tar = pets.Find(pets => pets.petName == name);
        if (tar != null) {
            return tar.description;
        } else {
            Debug.Log("invalid petName for description: " + name);
            return "";
        }
    }

    ///////////////////////ACTIVE PET FUNCTIONS///////////////////
    
    private void LoadActivePetFromJSON()
    {
        // Load the JSON data from the persistent data path
        string filePath = Application.persistentDataPath + "/activePet.json";
        if (System.IO.File.Exists(filePath))
        {
            string jsonData = System.IO.File.ReadAllText(filePath);
            activePet = JsonUtility.FromJson<Pet>(jsonData);
            spawnPet();
            //Debug.Log("activeBait.json successfully read");
        } else
        {
            //Debug.Log("no activeBait saved, creating null");
            resetActivePet();
        }
    }

    public void resetActivePet() {
        activePet = null;
        spawnPet();
        SaveActivePetToJSON();
    }
    
    private void SaveActivePetToJSON()
    {
        string jsonData = JsonUtility.ToJson(activePet);
        string filePath = Application.persistentDataPath + "/activePet.json";
        System.IO.File.WriteAllText(filePath, jsonData);
    }

    private void despawnPet() {
        foreach(Transform child in this.transform) {
            Destroy(child.gameObject);
        }
    }

    public bool isPetActive() {
        return (activePet != null);
    }

    public string getActivePetName() {
        if (activePet != null) {
            return activePet.petName;
        } else {
            Debug.Log("no pet active!");
            return "";
        }
    }

    public void setActivePet(string name) {
        Pet tar = pets.Find(pets => pets.petName == name);
        if (tar != null) {
            //Debug.Log("set active pet: " + name);
            activePet = tar;
            spawnPet();
            SaveActivePetToJSON();
        } else {
            Debug.Log("invalid petName for setActivePet: " + name);
        }
    }

    public void setIsFishing(bool _isFishing) {
        isFishing = _isFishing;
        if (_isFishing) {
            despawnPet();
        }
    }

    public void spawnPet() {
        if (isFishing) {
            Debug.Log("fishing! can't spawn pet");
            return;
        }
        if (activePet != null) {
            despawnPet();
            GameObject petToSpawn = Instantiate(petPrefab);
            petToSpawn.transform.position = player.transform.position;
            petToSpawn.transform.SetParent(this.transform);
            //petToSpawn.transform.localScale = Vector3.Scale(player.transform.localScale, new Vector3(0.25f, 0.25f, 1.0f));
            petToSpawn.gameObject.GetComponent<SpriteRenderer>().sprite = getPetSprite(activePet.petName);
            petToSpawn.gameObject.GetComponent<Animator>().runtimeAnimatorController = getPetAnimator(activePet.petName);
        } else {
            despawnPet();
        }
    }


}

[System.Serializable]
public class Pet
{
    public string petName;
    public string description;
    public int value;
    public bool isBought;

    public Pet(string _petName, string _description, int _value, bool _isBought)
    {
        petName = _petName;
        description = _description;
        value = _value;
        isBought = _isBought;
    }
}

[System.Serializable]
public class Pets
{
    public List<Pet> petList;

    public Pets(List<Pet> _petList)
    {
        petList = _petList;
    }
}