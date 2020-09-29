using UnityEngine;
using System.Collections.Generic;


public class SokobanLevelManager : MonoBehaviour
{
    public GameObject casillero;
    public GameObject casilleroTarget;
    public GameObject jugador;
    public GameObject bloque;
    public GameObject pared;


    public static SokobanLevelManager instancia;

    [SerializeField] private Texture2D mapTexture;
    
    const string Jugador = "42A5F5";
    const string Bloque = "F06292";
    const string Casillero = "EEDA5F";
    const string Pared = "E65100";
    const string CasilleroTarget = "4CAF50";

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public List<GameObject> dameLstPrefabsSokoban()
    {
        List<GameObject> lstPrefabsSokoban = new List<GameObject>();
        lstPrefabsSokoban.Add(casillero);
        lstPrefabsSokoban.Add(casilleroTarget);
        lstPrefabsSokoban.Add(jugador);
        lstPrefabsSokoban.Add(pared);
        lstPrefabsSokoban.Add(bloque);
        
        return lstPrefabsSokoban;
    }
                
    private Tablero dameTablero(int x, int y)
    {
        Tablero tablero = new Tablero(x, y);

        for (int i = 0; i < tablero.casilleros.GetLength(0); i++)
        {
            for (int j = 0; j < tablero.casilleros.GetLength(1); j++)
            {
                tablero.setearObjeto(casillero, new Vector2(i, j));
            }
        }

        return tablero;
    }

    public Nivel dameNivel(string nombre)
    {
        return SokobanLevelManager.instancia.dameNiveles().Find(x => x.Nombre == nombre);
    }

    private List<Nivel> dameNiveles()
    {
        List<Nivel> lstNiveles = new List<Nivel>();
        lstNiveles.Add(new Nivel("Nivel1", SokobanLevelManager.instancia.dameTableroNivel1()));
        return lstNiveles;
    }

    private Tablero dameTableroNivel1()
    {
        Tablero tablero = SokobanLevelManager.instancia.dameTablero(8, 8);
        
        for(int x = 0; x < mapTexture.width; x++)
        {
            for(int y = 0; y < mapTexture.height; y++)
            {
                mapTexture.GetPixel(x,y);
                Color color = mapTexture.GetPixel(x, y);

                string hex = ColorUtility.ToHtmlStringRGB(color);
                Debug.Log(hex);
                
                switch (hex)
                {
                    case Jugador:
                        Debug.Log("Entro jugador");
                        tablero.setearObjeto(jugador, new Vector2(x, y));
                        break;
                    case Bloque:
                        Debug.Log("Entro bloque");
                        tablero.setearObjeto(bloque, new Vector2(x,y));
                        break;
                    case Casillero:
                        Debug.Log("Entro casilleros");
                        tablero.setearObjeto(casillero, new Vector2(x, y));
                        break;
                    case Pared:
                        Debug.Log("Entro pared");
                        tablero.setearObjeto(pared, new Vector2(x, y));
                        break;
                    case CasilleroTarget:
                        Debug.Log("Entro cas target");
                        tablero.setearObjeto(casilleroTarget, new Vector2(x, y));
                        break;
                }
            }
        }
        return tablero;
    }
}


