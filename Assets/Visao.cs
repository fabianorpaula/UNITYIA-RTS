using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visao : MonoBehaviour {

    /// <summary>
    /// Game Object Empty Na Frente da Cabeça do Jogador, com uma animação de movimentação
    /// </summary>


    public GameObject Recurso;
    public GameObject Campones;

    //0 = todos
    //1 = madeira
    //2 = carne
    //3 = ouro
  

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        

    }
    public bool Avistou(int vontade)
    {
        RaycastHit hit;
        int alcance = 10 + Campones.GetComponent<SoldadoIA>().MeuVisao;
        if (Physics.Raycast(transform.position, transform.forward, out hit, alcance))
        {
            //Debug.DrawLine(transform.position, hit.point, Color.red);
            //Debug.Log(hit.collider.gameObject.tag);
            if (vontade != 0 || vontade == 0)
            {
                if (vontade == 1 || vontade == 0)
                {
                    if (hit.collider.gameObject.tag == "Arvore")
                    {

                        Recurso = hit.collider.gameObject;
                        //Informar que avistou algo

                        return true;
                    }
                    return false;
                }
                if (vontade == 2 || vontade == 0)
                {
                    if (hit.collider.gameObject.tag == "Carne")
                    {

                        Recurso = hit.collider.gameObject;
                        //Informar que avistou algo
                        return true;
                    }
                    return false;
                }
                if (vontade == 3 || vontade == 0)
                {
                    if (hit.collider.gameObject.tag == "Ouro")
                    {

                        Recurso = hit.collider.gameObject;
                        //Informar que avistou algo
                        return true;
                    }
                    return false;
                }
                return false;
            }
            else
            {
                return false;
            }
        }else
        {
            return false;
        }
    }
}
