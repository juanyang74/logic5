using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tablero : MonoBehaviour
{
    public GameObject prefabObjeto;
    public static List<GameObject> Botones = new List<GameObject>();

    //Color[] colores = { new Color(255, 0, 0), new Color(0, 80, 255), new Color(255, 255, 0), new Color(0, 255, 0), new Color(255, 0, 255) };
    //string[] CT = {"0000122201234412334133441"};

    Color R = new Color(255, 0, 0);
    Color B = new Color(0, 80, 255);
    Color Y = new Color(255, 255, 0);
    Color G = new Color(0, 255, 0);
    Color F = new Color(255, 0, 255);
    string[] colorTablero = { "RRRRBYYYRBYGFFBYGGFBGGFFB", "YYYRRBYYRFBGRRFBGGFFBBGGF",
                                "BYYYGBBYFGBRYFGBRRFGRRFFG"};
    int numeroTablero = 0;

    int rojos, azules, amarillos, verdes, fucsias;

    public static GameObject[,] FilasColumnas;

    public string[] combinacionPosible;

    string[] colors = { "BotonRed", "BotonBlue", "BotonYellow", "BotonGreen", "BotonFucsia" };

    public Button BtNuevaCombinacion, BtMostrarSolucion, BtAvanzar, BtRetroceder;
    public static GameObject TxtCorrecto;

    public Toggle toggleAyuda;
    public static bool AyudaOn = true;

    private void Awake()
    {
        numeroTablero = Random.Range(0, colorTablero.Length);
        TxtCorrecto = GameObject.Find("TxtCorrecto");
    }

    void Start()
    {
        //Debug.Log(numeroTablero);
        TxtCorrecto.SetActive(false);

        Botones.Clear();
        FilasColumnas = new GameObject[5, 5];
        rojos = 0;
        azules = 0;
        amarillos = 0;
        verdes = 0;
        fucsias = 0;

        aleatorioSalidos = new int[120];

        int cuenta = 0;

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject boton = Instantiate(prefabObjeto, new Vector2(transform.position.x + (j * 50),
                    transform.position.y + (i * 50)), Quaternion.identity);
                boton.transform.SetParent(transform);

                //boton.GetComponent<Image>().color = colores[int.Parse(CT[0].Substring(cuenta, 1))];
                //boton.name = "Grupo" + CT[0].Substring(cuenta, 1);

                if (colorTablero[numeroTablero].Substring(cuenta, 1) == "R")
                {
                    boton.GetComponent<Image>().color = R;
                    boton.name = "BotonRed" + rojos;
                    rojos++;
                }
                else if (colorTablero[numeroTablero].Substring(cuenta, 1) == "B")
                {
                    boton.GetComponent<Image>().color = B;
                    boton.name = "BotonBlue" + azules;
                    azules++;
                }
                else if (colorTablero[numeroTablero].Substring(cuenta, 1) == "Y")
                {
                    boton.GetComponent<Image>().color = Y;
                    boton.name = "BotonYellow" + amarillos;
                    amarillos++;
                }
                else if (colorTablero[numeroTablero].Substring(cuenta, 1) == "G")
                {
                    boton.GetComponent<Image>().color = G;
                    boton.name = "BotonGreen" + verdes;
                    verdes++;
                }
                else if (colorTablero[numeroTablero].Substring(cuenta, 1) == "F")
                {
                    boton.GetComponent<Image>().color = F;
                    boton.name = "BotonFucsia" + fucsias;
                    fucsias++;
                }

                Botones.Add(boton);
                cuenta++;
            }
        }

        int columna = 0;
        int fila = 0;

        for (int i = 0; i < 25; i++)
        {
            FilasColumnas[fila, columna] = Botones[i];
            columna++;
            if (columna > 4)
            {
                columna = 0;
                fila++;
            }
        }

        AleatoriosSinRepetir();
    }    

    int[] aleatorios;
    int[] aleatorioSalidos = new int [120];
    string n;

    void AleatoriosSinRepetir()
    {
        aleatorios = new int[5];

        int numeroAleatorio = Random.Range(1, 6);
        aleatorios[0] = numeroAleatorio;

        for (int i = 1; i < aleatorios.Length; i++)
        {
            while (aleatorios.Contains(numeroAleatorio))
            {
                numeroAleatorio = Random.Range(1, 6);
            }
                        
            aleatorios[i] = numeroAleatorio;
        }

        n = "";
        for (int i = 0; i < aleatorios.Length; i++)
        {
            n += aleatorios[i];
        }

        if (aleatorioSalidos.Contains(int.Parse(n)))
        {
            AleatoriosSinRepetir();
            return;
        }

        CrearJuego();
    }

    int pasadasAsumibles, pasadasExtremas;
    void CrearJuego()
    {
        BtNuevaCombinacion.interactable = false;
        BtMostrarSolucion.interactable = false;
        BtAvanzar.interactable = false;
        BtRetroceder.interactable = false;

        int fila = 0;
        for (int i = 0; i < Botones.Count; i++)
        {
            if(Botones[i].GetComponentInChildren<TextMeshProUGUI>().text == "")
            {
                for (int j = 0; j < 5; j++)
                {
                    Botones[i + j].GetComponentInChildren<TextMeshProUGUI>().text = aleatorios[j].ToString();
                }
                fila = i;
                break;
            }
        }
                
        if (pasadasExtremas > 5)
        {
            for (int i = 0; i < Botones.Count; i++)
            {
                Botones[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            //Debug.Log("Han pasado más de 8 intentos EXTREMOS");
            pasadasExtremas = 0;
            pasadasAsumibles = 0;
            aleatorioSalidos = new int[120];
            AleatoriosSinRepetir();
            return;
        }

        pasadasAsumibles++;
        if (pasadasAsumibles > 70)
        {
            for (int i = fila; i < fila + 5; i++)
            {
                Botones[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            
            for (int i = fila - 5; i < fila; i++)
            {
                Botones[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            //Debug.Log("Han pasado más de 80 intentos ASUMIBLES");
            pasadasExtremas++;
            pasadasAsumibles = 0;
            aleatorioSalidos = new int[120];
            AleatoriosSinRepetir();
            return;
        }
                
        if(!Boton.instancia.ComprobarGrupo())
        {
            for (int i = fila; i < fila + 5; i++)
            {
                Botones[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            }

            aleatorioSalidos[pasadasAsumibles] = (int.Parse(n));
            AleatoriosSinRepetir();
            return;
        }
                
        if(!Boton.instancia.ComprobarColumna())
        {
            for (int i = fila; i < fila + 5; i++)
            {
                Botones[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            }

            aleatorioSalidos[pasadasAsumibles] = (int.Parse(n));
            AleatoriosSinRepetir();
            return;
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (FilasColumnas[i, j].GetComponentInChildren<TextMeshProUGUI>().text == "")
                {
                    if (i < 4)
                    {
                        aleatorioSalidos = new int[120];
                        AleatoriosSinRepetir();
                        return;
                    }                    
                }
            }
        }
        UltimaFila();        
    }

    int[] valores = { 1, 2, 3, 4, 5 };
    void UltimaFila()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int h = 0; h < valores.Length; h++)
            {
                bool numeroFaltante = true;
                for (int j = 0; j < 4; j++)
                {
                    if(valores[h] == int.Parse(FilasColumnas[j, i].GetComponentInChildren<TextMeshProUGUI>().text))
                    {
                        numeroFaltante = false;
                    } 
                }

                if (numeroFaltante)
                {
                    if (!ComprobarNumero(valores[h].ToString(), i))
                    {
                        StartCoroutine(VolverEmpezar());
                        return;
                    }
                }
            }
        }

        //Una vez que encuentra una combinación posible la guardamos y construimos el juego...
        combinacionPosible = new string[25];
        int cuenta = 0;
        foreach (GameObject item in FilasColumnas)
        {
            combinacionPosible[cuenta] = item.GetComponentInChildren<TextMeshProUGUI>().text;
            //item.GetComponentInChildren<TextMeshProUGUI>().text = "";
            cuenta++;
        }

        //BtNuevaCombinacion.interactable = true;
        //BtMostrarSolucion.interactable = true;
        //BtAvanzar.interactable = true;
        //BtRetroceder.interactable = true;
        MostrarInicio();
    }

    bool ComprobarNumero(string h, int i)
    {
        FilasColumnas[4, i].GetComponentInChildren<TextMeshProUGUI>().text = h;
        if (!Boton.instancia.ComprobarGrupo())
        {
            for (int t = 0; t < Botones.Count; t++)
            {
                Botones[t].GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            return false;
        }

        if (!Boton.instancia.ComprobarColumna())
        {
            for (int t = 0; t < Botones.Count; t++)
            {
                Botones[t].GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            return false;
        }          

        return true;
    }

    IEnumerator VolverEmpezar()
    {
        yield return new WaitForSeconds(0.5f);
        NuevaCombinacion();
    } 

    void NuevaCombinacion()
    {
        pasadasExtremas = 0;
        pasadasAsumibles = 0;
        aleatorioSalidos = new int[120];
        AleatoriosSinRepetir();
    }

    void MostrarInicio()
    {
        int ultimoColorAleatorio = Random.Range(0, 5);
        int n = 0;
        if (ultimoColorAleatorio != aleatorios[4] - 1) n = 1;

        for (int j = 0; j < aleatorios.Length - n; j++)
        {
            int casillaNoEliminada = Random.Range(0, 5);
            for (int i = 0; i < Botones.Count; i++)
            {
                if(colors[aleatorios[j] - 1] == Botones[i].name.Substring(0,Botones[i].name.Length - 1))
                {
                    if (int.Parse(Botones[i].name.Substring(Botones[i].name.Length - 1, 1)) != casillaNoEliminada)
                    {
                        Botones[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                    }
                }
            }
        }
        
        bool casillaOcupada = false;

        if(n == 1)
        {
            for (int i = 0; i < Botones.Count; i++)
            {
                if (Botones[i].name.Substring(0, Botones[i].name.Length - 1) == colors[aleatorios[4] - 1])
                {
                    Botones[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
            }

            for (int i = 0; i < Botones.Count; i++)
            {
                if (Botones[i].name.Substring(0, Botones[i].name.Length - 1) == colors[aleatorios[ultimoColorAleatorio] - 1])
                {
                    if(int.Parse(Botones[i].name.Substring(Botones[i].name.Length - 1, 1)) == ultimoColorAleatorio)
                    {
                        if(Botones[i].GetComponentInChildren<TextMeshProUGUI>().text == "")
                        {
                            Botones[i].GetComponentInChildren<TextMeshProUGUI>().text = combinacionPosible[i];
                        }
                        else
                        {
                            casillaOcupada = true;
                        }
                    }
                }
            }
        }

        if (casillaOcupada)
        {
            int numero = Random.Range(0, 25);
            while (Botones[numero].GetComponentInChildren<TextMeshProUGUI>().text != "")
            {
                numero = Random.Range(0, 25);
            }
            Botones[numero].GetComponentInChildren<TextMeshProUGUI>().text = combinacionPosible[numero];
        }

        foreach (GameObject item in Botones)
        {
            if(item.GetComponentInChildren<TextMeshProUGUI>().text != "")
            {
                item.GetComponent<Button>().interactable = false;
                item.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(140, 140, 140, 255);
            }
        }

        BtNuevaCombinacion.interactable = true;
        BtMostrarSolucion.interactable = true;
        BtAvanzar.interactable = true;
        BtRetroceder.interactable = true;
    }
    
    public void _BtNuevaCombinacion()
    {
        for (int t = 0; t < Botones.Count; t++)
        {
            Botones[t].GetComponentInChildren<TextMeshProUGUI>().text = "";
            Botones[t].GetComponent<Button>().interactable = true;
        }

        TxtCorrecto.SetActive(false);
        
        NuevaCombinacion();
    }

    Color colorComprobacion = new Color(140, 140, 140);
    public void _BtMostrarSolucion()
    {
        int cuenta = 0;
        foreach (GameObject item in FilasColumnas)
        {
            item.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            item.GetComponent<Button>().interactable = false;
            item.GetComponentInChildren<TextMeshProUGUI>().text = combinacionPosible[cuenta];
            cuenta++;
        }
    }
        
    public void _BtAvanzar()
    {
        aleatorioSalidos = new int[120];

        numeroTablero++;
        if (numeroTablero == colorTablero.Length) numeroTablero = 0;

        foreach (GameObject item in Botones)
        {
            Destroy(item.gameObject, 0.2f);
        }

        Start();
    }

    public void _BtRetroceder()
    {
        aleatorioSalidos = new int[120];

        numeroTablero--;        
        if (numeroTablero <= -1) numeroTablero = colorTablero.Length - 1;

        foreach (GameObject item in Botones)
        {
            Destroy(item.gameObject, 0.2f);
        }

        Start();
    }   
    
    IEnumerator EmpezarPartida()
    {
        yield return new WaitForSeconds(0.1f);
        Start();
    }

    public void _ToggleAyuda()
    {
        AyudaOn = toggleAyuda.isOn;
    }
}
