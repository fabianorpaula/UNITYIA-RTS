using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vila : MonoBehaviour {

    public int total_de_carne = 50;
    public int total_de_madeira = 50;
    public int total_de_ouro = 50;

    private float tempo;
    private float tempodecisao;
    private float espera = 0;


    public List<SoldadoIA> Camponeses;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Tempo();
        Morte();
        Necessidades();
    }

    void Tempo()
    {
        tempo += Time.deltaTime;
        if (tempo > 5)
        {
            tempo = 0;
            total_de_carne = total_de_carne - Camponeses.Count;
           
        }
    }


    void Necessidades()
    {
        tempodecisao += Time.deltaTime;
        if (tempodecisao > espera)
        {
            if (total_de_carne < (Camponeses.Count * 50))
            {
                Debug.Log("CAÇOU");
                for (int i = 0; i < Camponeses.Count; i++)
                {
                    Camponeses[i].DesejoColetar(2);
                    
                }
                espera = 5000;
                tempodecisao = 0;
            }
            
        }
    }

    void Morte()
    {
        if(total_de_carne <= 0)
        {
            Debug.Log("MORREU");
            Time.timeScale = 0;
            //Debug.Log("MORREU");
        }
    }


    public void DepositaCarne(int adicao)
    {
        total_de_carne = total_de_carne + adicao;
    }
    public void DepositaMadeira(int adicao)
    {
        total_de_madeira = total_de_madeira + adicao;
    }
    public void DepositaOuro(int adicao)
    {
        total_de_ouro = total_de_ouro + adicao;
    }


}
