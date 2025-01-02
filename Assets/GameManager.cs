using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameManager : MonoBehaviour
{
    public GameObject prefabCarta;
    public Transform pivot;
    public List<Material> materialiCarte;
    public List<Material> materialiDaDare;
    public Camera myCamera;
    public Carta primaCartaScoperta;
    public Carta secondaCartaScoperta;
    public AudioClip suonoCarteUguali;
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
        CreaGiocoDelMemory();
        audioSource= GetComponent<AudioSource>();
    }

    private void CreaGiocoDelMemory()
    {
        foreach (Material material in materialiCarte)
        {
            materialiDaDare.Add(material);
            materialiDaDare.Add(material);
        }

        ShuffleMe(materialiDaDare);


        for (int riga = 0; riga < 4; riga++)
        {
            for (int colonna = 0; colonna < 5; colonna++)
            {
                GameObject oggettoCreato = Instantiate(prefabCarta);
                oggettoCreato.transform.position = pivot.position + new Vector3(colonna * 10, -riga * 15, 0);
                oggettoCreato.GetComponent<MeshRenderer>().material = materialiDaDare[riga * 5 + colonna];
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (primaCartaScoperta != null && secondaCartaScoperta != null) { return; }
        if (Input.GetMouseButtonDown(0))
        {
            Ray raggioDaSparare = myCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit informazioniSulRaggio;
            bool hoColpitoQualcosa = Physics.Raycast(raggioDaSparare, out informazioniSulRaggio);
            if (hoColpitoQualcosa)
            {
                Carta cartaColpita = informazioniSulRaggio.collider.gameObject.GetComponent<Carta>();
                if (cartaColpita != null)
                {

                    cartaColpita.GiraCarta();
                    if (primaCartaScoperta == null)
                    {
                        primaCartaScoperta = cartaColpita;
                    }
                    else
                    {
                        if (primaCartaScoperta != cartaColpita)
                        {

                            secondaCartaScoperta = cartaColpita;
                            Invoke(nameof(ControllaSeHoScopertoDueCarteUguali), 1);
                            
                            //ControllaSeHoScopertoDueCarteUguali(); errore perche se la chiaami due volte da bug
                        }
                        else
                        {

                            cartaColpita.GiraCarta();
                        }
                    }
                }
            }
        }


    }

    private void ControllaSeHoScopertoDueCarteUguali()
    {
        if (primaCartaScoperta.GetComponent<MeshRenderer>().sharedMaterial == secondaCartaScoperta.GetComponent<MeshRenderer>().sharedMaterial)
        {
            DestroyImmediate(primaCartaScoperta.gameObject);
            DestroyImmediate(secondaCartaScoperta.gameObject);
            audioSource.PlayOneShot(suonoCarteUguali, 1);
            if (FindObjectsOfType<Carta>().Length == 0)
            {
                
                Invoke("Victory", 1f);
                
            }
        }
        else
        {
            

            
            primaCartaScoperta.GiraCarta();
            secondaCartaScoperta.GiraCarta();
            
           
            


        }
       
        primaCartaScoperta = null;
        secondaCartaScoperta = null;
    }

    public void Victory()
    { SceneManager.LoadScene(2);
        
    }
    
}
