using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boton : MonoBehaviour
{
    public static Boton instancia;

    string[] colors = { "BotonRed", "BotonBlue", "BotonYellow", "BotonGreen", "BotonFucsia" };
        
    void Awake()
    {
        instancia = this;
    }

    public void _Click()
    {
        if (gameObject.GetComponentInChildren<TextMeshProUGUI>().text == "") gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "0";
        
        int i = int.Parse(gameObject.GetComponentInChildren<TextMeshProUGUI>().text);
        i++;

        foreach (var item in Tablero.Botones)
        {
            if (item.GetComponent<Button>().interactable == true)
            {
                item.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            }
            else
            {
                item.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(140, 140, 140, 140);
            }
        }

        if (i > 5)
        {
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
        else
        {
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
        }

        ComprobarGrupo();
        ComprobarFila();
        ComprobarColumna();
        ComprobarSolucion(ComprobarGrupo(), ComprobarColumna(), ComprobarFila());
    }

    public bool ComprobarGrupo()
    {
        foreach (var item in Tablero.Botones)
        {
            if (item.GetComponent<Button>().interactable == true)
            {
                item.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
            }
            else
            {
                item.GetComponentInChildren<TextMeshProUGUI>().color = new Color32(140, 140, 140, 255);
            }
        }

        foreach (var color in colors)
        {
            foreach (var boton in Tablero.Botones)
            {
                if (color == boton.name.Substring(0, boton.name.Length - 1))
                {
                    GameObject nombreBoton = boton;

                    foreach (var item in Tablero.Botones)
                    {
                        if (color == item.name.Substring(0, item.name.Length - 1))
                        {
                            if (nombreBoton.name != item.name)
                            {
                                if(nombreBoton.GetComponentInChildren<TextMeshProUGUI>().text != "" &&
                                    item.GetComponentInChildren<TextMeshProUGUI>().text != "")
                                {
                                    if(nombreBoton.GetComponentInChildren<TextMeshProUGUI>().text ==
                                        item.GetComponentInChildren<TextMeshProUGUI>().text)
                                    {
                                        if (Tablero.AyudaOn)
                                        {
                                            nombreBoton.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                                            item.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                                        }
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return true;
    }
        
    public bool ComprobarFila()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject nombreBotonFilas = Tablero.FilasColumnas[i, j];

                if(nombreBotonFilas.GetComponentInChildren<TextMeshProUGUI>().text != "")
                {
                    for (int h = 0; h < 5; h++)
                    {
                        if(nombreBotonFilas.name != Tablero.FilasColumnas[i, h].name)
                        {
                            if(nombreBotonFilas.GetComponentInChildren<TextMeshProUGUI>().text ==
                                Tablero.FilasColumnas[i, h].GetComponentInChildren<TextMeshProUGUI>().text)
                            {
                                if (Tablero.AyudaOn)
                                {
                                    nombreBotonFilas.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                                    Tablero.FilasColumnas[i, h].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                                }
                                return false;
                            }
                        }
                    }
                }
            }
        }
        return true;
    }

    public bool ComprobarColumna()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject nombreBotonColumnas = Tablero.FilasColumnas[j, i];

                if (nombreBotonColumnas.GetComponentInChildren<TextMeshProUGUI>().text != "")
                {
                    for (int h = 0; h < 5; h++)
                    {
                        if (nombreBotonColumnas.name != Tablero.FilasColumnas[h, i].name)
                        {
                            if (nombreBotonColumnas.GetComponentInChildren<TextMeshProUGUI>().text ==
                                Tablero.FilasColumnas[h, i].GetComponentInChildren<TextMeshProUGUI>().text)
                            {
                                if (Tablero.AyudaOn)
                                {
                                    nombreBotonColumnas.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                                    Tablero.FilasColumnas[h, i].GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
                                }
                                return false;
                            }
                        }
                    }
                }
            }
        }

        return true;
    }

    void ComprobarSolucion(bool _comprobarGrupo, bool _comprobarColumna, bool _comprobarFila)
    {
        bool terminado = true;
        foreach (GameObject item in Tablero.Botones)
        {
            if (item.GetComponentInChildren<TextMeshProUGUI>().text == "")
            {
                terminado = false;
                break;
            }
            
            //if (item.GetComponentInChildren<TextMeshProUGUI>().color != Color.black)
            //{
            //    terminado = false;
            //    break;
            //}
        }

        if (terminado && _comprobarGrupo && _comprobarColumna && _comprobarFila)
        {
            foreach (GameObject item in Tablero.Botones)
            {
                item.GetComponent<Button>().interactable = false;
            }

            Tablero.TxtCorrecto.SetActive(true);
        }
    }
}
