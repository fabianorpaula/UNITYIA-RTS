using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldadoIA : MonoBehaviour {

    /// <summary>
    /// Dados de Velocidade Para Randomizar o Personagem
    /// </summary>
    public int MeuLife = 0;
    public int MeuDano = 0;
    public int MeuVisao = 0;
    public int MeuAlcance = 0;
    public int MeuVelocidade = 0;
    public int Assassinatos = 0;


    //OS ESTADOS
    //Descansar - Ronda - Segue - Ataque
    public enum Ordens { Descansa, Ronda, Segue, Ataque, Morto };
    public enum Vontades { Anda, Procura, Coleta, Entrega, Volta, Busca};
    

    //GameObjesc
    //Olho para fazer campo de visão
    public GameObject Olho;
    //Pontos de deslocamento
    public GameObject PontoA;
    public GameObject PontoB;
    //Recurso para receber
    private GameObject Recurso;
    public GameObject Casa;
    //Disparo
    //prefab bala
    public GameObject capsula;
    //Ponto de Tiro
    public GameObject pontodetiro;
    //Caminhos
    public List<GameObject> Caminhos;
    public List<GameObject> Posicoes;
    public int destino_c = 0;
    /// <summary>
    /// As ordens
    /// </summary>
    public Ordens minhas_ordens;
    public Vontades minhas_vontades;
    
    //Ações Animadas
    private Actions Acoes;
    /// <summary>
    /// Dano
    private Dano dano;
    /// </summary>
    /// 

    private int bolsa_geral = 0;
    private int bolsa_madeira = 0;
    private int bolsa_carne = 0;
    private int bolsa_ouro = 0;




    public bool pos = false;
    //Para soldado Andar
    private NavMeshAgent Campones;


    //Tempo para ficar parado ou andar
    public float tempo = 0;
    //Tempo para atirar
    public float tempo_at = 0;

    public int vontadeDecoleta = 0;
    

	// Use this for initialization
	void Start () {
        //Time.timeScale = 10;
        int total_pos = Posicoes.Count - 1;
        destino_c = Random.Range(0, total_pos);
        
        transform.position = Posicoes[destino_c].transform.position;
        //Destroy(Posicoes[destino_c]);

        minhas_vontades = Vontades.Procura;
        
        Campones = GetComponent<NavMeshAgent>();

        ///Chamar Aram
        //GetComponent<PlayerController>().SetArsenal("Rifle");

        Acoes = GetComponent<Actions>();
        //Acoes.Stay();
        //Acoes.Aiming();

        ///Dano
        dano = GetComponent<Dano>();
    }
	
	// Update is called once per frame
	void Update () {
        
        SeguirVontades();
        //Verifica se Personagem ainda esta vivo
        if(dano.InformarStatus())
        {
            //minhas_ordens = Ordens.Morto;
        }
	}

    void SeguirVontades()
    {
        if (minhas_vontades == Vontades.Procura)
        {
            //Para buscar inimigo
            Buscar();
            //Tempo andando
            Campones.speed = 5 + MeuVelocidade;
            tempo += Time.deltaTime;
           
            int destinof = destino_c + 1;
            // transform.position = Vector3.MoveTowards(this.transform.position, PontoA.transform.position, 0.5f);
            Campones.SetDestination(Caminhos[destinof].transform.position);
            if (Vector3.Distance(transform.position, Caminhos[destinof].transform.position) < 2)
            {
                destino_c = destino_c + 1;
                if (destino_c >= Caminhos.Count - 1)
                {
                    destino_c = 0;
                }
            }

        }
        if(minhas_vontades == Vontades.Busca)
        {
            Campones.speed = 5 + MeuVelocidade;
            Campones.SetDestination(Recurso.transform.position);
            //Debug.Log(Vector3.Distance(transform.position, Recurso.transform.position));
            if (Vector3.Distance(transform.position, Recurso.transform.position) < 3)
            {
                minhas_vontades = Vontades.Coleta;
                
                
            }
        }
        if(minhas_vontades == Vontades.Coleta)
        {
            tempo_at += Time.deltaTime;
            if (tempo_at > 1)
            {
                if (bolsa_geral <= 10)
                {
                    if (Recurso.gameObject.tag == "Arvore")
                    {
                        bolsa_madeira++;
                    }
                    if (Recurso.gameObject.tag == "Carne")
                    {
                        bolsa_carne++;
                    }
                    if (Recurso.gameObject.tag == "Ouro")
                    {
                        bolsa_ouro++;
                    }
                    bolsa_geral++;
                   //Debug.Log("Coletou+1"+bolsa_carne);
                }else
                {
                    //Debug.Log("QUERO VOLTAR");
                    minhas_vontades = Vontades.Volta;
                    
                }
            }
        }
        if(minhas_vontades == Vontades.Volta)
        {
            Campones.speed = 5 + MeuVelocidade;
            Campones.SetDestination(Casa.transform.position);
            if (Vector3.Distance(transform.position, Casa.transform.position) < 3)
            {
                minhas_vontades = Vontades.Entrega;

            }
        }
        if (minhas_vontades == Vontades.Entrega)
        {
            tempo_at += Time.deltaTime;
            if (tempo_at > 1)
            {
                if (bolsa_geral > 0)
                {
                    Debug.Log("Depositou");
                    if (bolsa_madeira > 0)
                    {
                        bolsa_madeira--;
                        bolsa_geral--;
                        Casa.GetComponent<Vila>().DepositaMadeira(1);
                    }
                    if (bolsa_carne > 0)
                    {
                        bolsa_carne--;
                        bolsa_geral--;
                        Casa.GetComponent<Vila>().DepositaCarne(1);
                    }
                    if (bolsa_ouro > 0)
                    {
                        bolsa_ouro--;
                        bolsa_geral--;
                        Casa.GetComponent<Vila>().DepositaOuro(1);
                    }

                }else
                {
                    minhas_vontades = Vontades.Busca;
                }
            }
        }


    }
    /*
    void CumprirOrdens()
    {
        if(minhas_ordens == Ordens.Descansa)
        {
            //Para Buscar inimigo
            Buscar();
            //Tempo parado
            tempo += Time.deltaTime;
            Campones.speed = 0;
            if (tempo > 10)
            {
                tempo = 0;
                minhas_ordens = Ordens.Ronda;
                //Correr
                Acoes.Run();
            }
            
        }
        if(minhas_ordens == Ordens.Ronda)
        {
            //Para buscar inimigo
            Buscar();
            //Tempo andando
            Campones.speed = 5 + MeuVelocidade;
            tempo += Time.deltaTime;
            if(tempo > 30)
            {
                tempo = 0;
                minhas_ordens = Ordens.Descansa;
                //Parar
                Acoes.Stay();
            }
            
            
                int destinof = destino_c + 1;
                // transform.position = Vector3.MoveTowards(this.transform.position, PontoA.transform.position, 0.5f);
                Campones.SetDestination(Caminhos[destinof].transform.position);
                if (Vector3.Distance(transform.position, Caminhos[destinof].transform.position) < 2)
                {
                    destino_c = destino_c + 1;
                    if(destino_c >= Caminhos.Count-1)
                    {
                        destino_c = 0;
                    }
                }
            
        }
        //Ordem de Seguir
        if(minhas_ordens == Ordens.Segue)
        {
            Campones.speed = 5 + MeuVelocidade;
            Campones.SetDestination(Recurso.transform.position);
            if(Vector3.Distance(transform.position, Recurso.transform.position) < 5)
            {
                minhas_ordens = Ordens.Ataque;
                Acoes.Aiming();
                
            }
            
        }

        //Ordem de Ataque
        if(minhas_ordens == Ordens.Ataque)
        {
            //Campones para para atirar
            Campones.speed = 0;
            //Mira no Recurso
            transform.LookAt(Recurso.transform.position);
            //Se o Recurso Aumenta a distância muda de estado
            if (Vector3.Distance(transform.position, Recurso.transform.position) > (6+MeuAlcance))
            {
                //Volta a seguir
                minhas_ordens = Ordens.Segue;
                //Corre
                Acoes.Run();

            }else
            {
                tempo_at += Time.deltaTime;
                if (tempo_at > 1)
                {
                    Atirar();
                    tempo_at = 0;
                }
            }

            ///Se o Recurso estiver Morto - Muda o estado
            if (Recurso.GetComponent<Dano>().InformarStatus())
            {
                minhas_ordens = Ordens.Ronda;
                Acoes.Run();
                Assassinatos++;
                Destroy(Recurso, 5f);
            }
            

        }

        //Morre
        if (minhas_ordens == Ordens.Morto)
        {
            Campones.speed = 0;
            Destroy(this.gameObject, 1f);
        }

        }

    */

    public void DesejoColetar(int devecoletar)
    {
        vontadeDecoleta = devecoletar;
        minhas_vontades = Vontades.Procura;
    }

    ////Visão -> PARADO - RONDA
    void Buscar()
    {
        if(Olho.GetComponent<Visao>().Avistou(vontadeDecoleta) == true)
        {
            minhas_vontades = Vontades.Busca;
            Recurso = Olho.GetComponent<Visao>().Recurso;
        }


    }

    


    }
