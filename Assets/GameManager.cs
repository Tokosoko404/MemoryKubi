using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform pivot;
    public List<Material> cardMaterial;
    public List<Material> materialsToGive;
    public Camera myCamera;
    public Card firstCardUncovered;
    public Card secondCardUncovered;
    public AudioClip sameCardSound;
    //public AudioClip suonoCarteDiverse;
    //public AudioClip suonoCarteFinite;
    AudioSource audioSource;

    public void ShuffleMe<T>(IList<T> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;

        for (int i = list.Count - 1; i > 1; i--)
        {
            int rnd = random.Next(i + 1);

            T value = list[rnd];
            list[rnd] = list[i];
            list[i] = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MadeMemoryGame();
        audioSource= GetComponent<AudioSource>();
    }

    private void MadeMemoryGame()
    {
        foreach (Material material in cardMaterial)
        {
            materialsToGive.Add(material);
            materialsToGive.Add(material);
        }

        ShuffleMe(materialsToGive);


        for (int line = 0; line < 4; line++)
        {
            for (int column = 0; column < 5; column++)
            {
                GameObject newObject = Instantiate(cardPrefab);
                newObject.transform.position = pivot.position + new Vector3(column * 10, -line * 15, 0);
                newObject.GetComponent<MeshRenderer>().material = materialsToGive[line * 5 + column];
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (firstCardUncovered != null && secondCardUncovered != null) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = myCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayInformation;
            bool hitSomething = Physics.Raycast(ray, out rayInformation);
            if (hitSomething)
            {
                Card hitCard = rayInformation.collider.gameObject.GetComponent<Card>();
                if (hitCard != null)
                {

                    hitCard.TurnCard();
                    if (firstCardUncovered == null)
                    {
                        firstCardUncovered = hitCard;
                    }
                    else
                    {
                        if (firstCardUncovered != hitCard)
                        {

                            secondCardUncovered = hitCard;
                            Invoke(nameof(CheckIfTheCardAreIdentical), 1);
                            
                            //ControllaSeHoScopertoDueCarteUguali(); errore perche se la chiaami due volte da bug
                        }
                        else
                        {

                            hitCard.TurnCard();
                        }
                    }
                }
            }
        }


    }

    private void CheckIfTheCardAreIdentical()
    {
        if (firstCardUncovered.GetComponent<MeshRenderer>().sharedMaterial ==   secondCardUncovered.GetComponent<MeshRenderer>().sharedMaterial)
        {
            DestroyImmediate(firstCardUncovered.gameObject);
            DestroyImmediate(secondCardUncovered.gameObject);
            audioSource.PlayOneShot(sameCardSound, 1);
            if (FindObjectsOfType<Card>().Length == 0)
            {
                
                Invoke("Victory", 1f);
                
            }
        }
        else
        {
            

            
            firstCardUncovered.TurnCard();
            secondCardUncovered.TurnCard();
            
           
            


        }
       
        firstCardUncovered = null;
        secondCardUncovered = null;
    }

    public void Victory()
    { SceneManager.LoadScene(2);
        
    }
    
}
