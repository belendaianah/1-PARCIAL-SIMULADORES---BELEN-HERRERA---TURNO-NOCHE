﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class SokobanGameManager : MonoBehaviour
{
    Nivel nivel, nivelAux;
    GameObject casillero, casilleroTarget, pared, jugador, bloque;
    List<Vector2> posOcupadasEsperadasCasillerosTarget;
    //Stack pilaTablerosAnteriores;
    List<Tablero> tablerosAnteriores = new List<Tablero>(); 

    string orientacionJugador;
    string nombreNivelActual = "Nivel1";
    bool estoyDeshaciendo = false;

    private void Start()
    {
        casillero = SokobanLevelManager.instancia.dameLstPrefabsSokoban().Find(x => x.name == "Casillero");
        casilleroTarget = SokobanLevelManager.instancia.dameLstPrefabsSokoban().Find(x => x.name == "CasilleroTarget");
        pared = SokobanLevelManager.instancia.dameLstPrefabsSokoban().Find(x => x.name == "Pared");
        jugador = SokobanLevelManager.instancia.dameLstPrefabsSokoban().Find(x => x.name == "Jugador");
        bloque = SokobanLevelManager.instancia.dameLstPrefabsSokoban().Find(x => x.name == "Bloque");
        CargarNivel(nombreNivelActual);
    }

    private void CargarNivel(string nombre)
    {
        nivel = SokobanLevelManager.instancia.dameNivel(nombre);
        posOcupadasEsperadasCasillerosTarget = nivel.Tablero.damePosicionesObjetos("CasilleroTarget");
        InstanciadorPrefabs.instancia.graficarCasilleros(nivel.Tablero, casillero);
        InstanciadorPrefabs.instancia.graficarCasillerosTarget(nivel.Tablero, casilleroTarget);
        InstanciadorPrefabs.instancia.graficarObjetosTablero(nivel.Tablero, SokobanLevelManager.instancia.dameLstPrefabsSokoban());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            orientacionJugador = "derecha";
            mover();
        }
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            orientacionJugador = "arriba";
            mover();
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            orientacionJugador = "abajo";
            mover();
        }
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            orientacionJugador = "izquierda";
            mover();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            estoyDeshaciendo = true;
            mover();
        }

    }

    private void mover()
    {
        if (ChequearVictoria(nivel.Tablero))
        {
            Debug.Log("Gané");
        }
        else
        {
            if (estoyDeshaciendo == false)
            {
                Tablero tablAux = new Tablero(nivel.Tablero.casilleros.GetLength(0), nivel.Tablero.casilleros.GetLength(1));
                tablAux.setearObjetos(casillero, nivel.Tablero.damePosicionesObjetos("Casillero"));
                tablAux.setearObjetos(casilleroTarget, nivel.Tablero.damePosicionesObjetos("CasilleroTarget"));
                tablAux.setearObjetos(bloque, nivel.Tablero.damePosicionesObjetos("Bloque"));
                tablAux.setearObjetos(pared, nivel.Tablero.damePosicionesObjetos("Pared"));
                tablAux.setearObjetos(jugador, nivel.Tablero.damePosicionesObjetos("Jugador"));

                tablerosAnteriores.Add(tablAux);
                //pilaTablerosAnteriores.Push(tablAux);               
                

                Debug.Log("Tablero" + tablerosAnteriores.Count);


                Vector2 posicionJugador = new Vector2(nivel.Tablero.damePosicionObjeto("Jugador").x, nivel.Tablero.damePosicionObjeto("Jugador").y);
                GameObject objProximo, objProximoProximo;

                if (orientacionJugador == "izquierda" || orientacionJugador == "abajo")
                {
                    objProximo = nivel.Tablero.dameObjeto(posicionJugador, orientacionJugador, -1);
                    objProximoProximo = nivel.Tablero.dameObjeto(posicionJugador, orientacionJugador, -2);
                }
                else
                {
                    objProximo = nivel.Tablero.dameObjeto(posicionJugador, orientacionJugador, 1);
                    objProximoProximo = nivel.Tablero.dameObjeto(posicionJugador, orientacionJugador, 2);
                }

                if (objProximo != null && objProximo.CompareTag("casillero"))
                {
                    if (orientacionJugador == "izquierda" || orientacionJugador == "abajo")
                    {
                        nivel.Tablero.setearObjeto(casillero, posicionJugador);
                        nivel.Tablero.setearObjeto(jugador, posicionJugador, orientacionJugador, -1);
                    }
                    else if (orientacionJugador == "derecha" || orientacionJugador == "arriba")
                    {
                        nivel.Tablero.setearObjeto(casillero, posicionJugador);
                        nivel.Tablero.setearObjeto(jugador, posicionJugador, orientacionJugador, 1);
                    }

                }
                else
                {
                    if (objProximo != null && objProximo.CompareTag("bloque") && objProximoProximo != null && objProximoProximo.CompareTag("bloque"))
                    {
                        Debug.Log("doble caja");
                    }

                    else if (objProximo != null && objProximo.CompareTag("bloque") && objProximoProximo != null)
                    {
                        if (orientacionJugador == "izquierda" || orientacionJugador == "abajo")
                        {
                            nivel.Tablero.setearObjeto(jugador, posicionJugador, orientacionJugador, -1);
                            nivel.Tablero.setearObjeto(casillero, posicionJugador);
                            nivel.Tablero.setearObjeto(bloque, posicionJugador, orientacionJugador, -2);
                        }
                        else if (orientacionJugador == "derecha" || orientacionJugador == "arriba")
                        {
                            nivel.Tablero.setearObjeto(jugador, posicionJugador, orientacionJugador, 1);
                            nivel.Tablero.setearObjeto(casillero, posicionJugador);
                            nivel.Tablero.setearObjeto(bloque, posicionJugador, orientacionJugador, 2); ;
                        }
                    }
                }
                InstanciadorPrefabs.instancia.graficarObjetosTablero(nivel.Tablero, SokobanLevelManager.instancia.dameLstPrefabsSokoban());
            }
            else
            {
                if (tablerosAnteriores.Count > 0)
                {
                    Tablero tableroAnterior = tablerosAnteriores.Last<Tablero>();


                    InstanciadorPrefabs.instancia.graficarObjetosTablero(tableroAnterior, SokobanLevelManager.instancia.dameLstPrefabsSokoban());

                    nivel.Tablero = tableroAnterior;

                    tablerosAnteriores.Remove(tableroAnterior);
                }                

                estoyDeshaciendo = false;
            }
        }
    }

    private bool SonIgualesLosVectores(Vector2 v1, Vector2 v2)
    {
        return (v1.x == v2.x && v1.y == v2.y);
    }

    private bool ChequearVictoria(Tablero tablero)
    {
        List <Vector2> posicionesCasillerosTarget  = nivel.Tablero.damePosicionesObjetos("CasilleroTarget");

        Debug.Log("Contador target: " + posicionesCasillerosTarget.Count);

        if(posicionesCasillerosTarget.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

