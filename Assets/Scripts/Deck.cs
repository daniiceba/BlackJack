using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;
    public Text userPoints;
    public Text probGanaDealer;
    public Text probPasarse;
    public Text probRango;

    public List<GameObject> cards = new List<GameObject>();

    public int[] values = new int[52];
    int cardIndex = 0;


    private void Awake()
    {
        //hecho
        InitCardValues();

    }

    private void Start()

    {
        //hecho
        ShuffleCards();
        StartGame();
    }

    private void InitCardValues()
    {
        //hecho
        for (int i = 0; i < 52; i++)
        {
            if ((i + 1) % 13 == 0)
            {
                values[i] = 10;
            }
            else
            {
                if ((i + 1) % 13 > 10)
                {
                    values[i] = 10;
                }
                else if ((i + 1) % 13 == 1)
                {
                    values[i] = 11;
                }
                else
                {
                    values[i] = (i + 1) % 13;
                }

            }

        }


    }

    private void ShuffleCards()
    {
        //hecho

        CardHand barajaDealer = dealer.GetComponent<CardHand>();


        List<int> baraja = new List<int>();


        for (int i = 0; i < 52; i++)
        {
            int nRand = Random.Range(0, 52);

            if (!baraja.Contains(nRand))
            {
                addCard(i, nRand);
            }
            else
            {
                while (!baraja.Contains(nRand))
                {
                    if (nRand > 52) nRand = 0;
                    addCard(i, nRand++);

                }
            }

        }
    }

    private void addCard(int i, int nRand)
    {
        //hecho
        faces[nRand] = faces[i];
        values[nRand] = values[i];
    }

    void StartGame()
    {
        //hecho
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();

            if (player.GetComponent<CardHand>().points == 21)
            {
                finalMessage.text = "Has Ganado ;P";
            }

            else if (dealer.GetComponent<CardHand>().points == 21)
            {

                finalMessage.text = "Has Perdido ;P";
            }

        }
        CalculateProbabilities();
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
        v * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
        v * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         v* - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */


        probGanaDealer.text = ganaDealer().ToString();
        probPasarse.text = Pasarse().ToString();
        probRango.text = entreElRango().ToString();
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]);
        cardIndex++;
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]);
        userPoints.text = player.GetComponent<CardHand>().points.ToString();
        cardIndex++;


    }

    public void Hit()
    {
        //hecho



        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */
        if (player.GetComponent<CardHand>().points > 21)
        {
            hitButton.interactable = false;
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);
            finalMessage.text = "Has Perdido ;P";
        }
        CalculateProbabilities();
    }

    public void Stand()
    {


        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        if (player.GetComponent<CardHand>().cards.Count == 1)
        {
            dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        }

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */
        while (dealer.GetComponent<CardHand>().points <= 16)
        {
            PushDealer();
        }
        dealer.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().ToggleFace(true);

        if (player.GetComponent<CardHand>().points > dealer.GetComponent<CardHand>().points && player.GetComponent<CardHand>().points < 22)
        {
            finalMessage.text = "Has ganado !!!";
        }
        else if (dealer.GetComponent<CardHand>().points > 21)
        {
            finalMessage.text = "Has ganado !!!";
        }
        else
        {
            finalMessage.text = "Has perdido";
        }


    }

    public void PlayAgain()
    {
        //hecho
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }

    public float ganaDealer()
    {

        int casosFavorables = 0;
        int[] cartasMesa = new int[3];

        int puntuacionJugador = player.GetComponent<CardHand>().points;
        int cartaDealer = dealer.GetComponent<CardHand>().cards[1].GetComponent<CardModel>().value;
        int diferencia = puntuacionJugador - cartaDealer;

        cartasMesa[0] = player.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().value;
        cartasMesa[1] = player.GetComponent<CardHand>().cards[1].GetComponent<CardModel>().value;
        cartasMesa[2] = cartaDealer;

        if (diferencia < 0)
        {
            return 0;
        }

        for (int i = diferencia + 1; i < 12; i++)
        {
            int contadorCartas = 0;

            if (i == cartasMesa[0])
            {
                contadorCartas++;
            }
            if (i == cartasMesa[1])
            {
                contadorCartas++;
            }
            if (i == cartasMesa[2])
            {
                contadorCartas++;
            }

            if (i != 10)
                casosFavorables += (4 - contadorCartas);

            if (i == 10)
            {
                casosFavorables += (16 - contadorCartas);
            }
        }

        float probabilidad = (float)casosFavorables / 49;
        probabilidad = 1 - probabilidad;
        probabilidad *= 100;


        return probabilidad;
    }
    public float Pasarse()
    {
        int casosFavorables = 0;
        int cartasRepartidas = player.GetComponent<CardHand>().cards.Count + 1;
        int[] cartasMesa = new int[cartasRepartidas];

        int puntuacionJugador = player.GetComponent<CardHand>().points;
        int cartaDealer = dealer.GetComponent<CardHand>().cards[1].GetComponent<CardModel>().value;

        cartasMesa[0] = player.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().value;
        cartasMesa[1] = player.GetComponent<CardHand>().cards[1].GetComponent<CardModel>().value;
        cartasMesa[2] = cartaDealer;

        if (puntuacionJugador == 21)
        {

            return 100;
        }

        int diferencia = 21 - puntuacionJugador;

        if (diferencia > 11)
        {

            return 0;
        }

        for (int i = 1; i < diferencia + 1; i++)
        {
            int contadorCartas = 0;

            if (i == cartasMesa[0])
            {
                contadorCartas++;
            }
            if (i == cartasMesa[1])
            {
                contadorCartas++;
            }
            if (i == cartasMesa[2])
            {
                contadorCartas++;
            }

            if (i != 10)
            {
                casosFavorables += (4 - contadorCartas);
            }
            else
            {
                casosFavorables += (16 - contadorCartas);
            }

        }


        float probabilidad = (float)casosFavorables / 49;
        probabilidad = 1 - probabilidad;
        probabilidad *= 100;
        return probabilidad;
    }
    public float entreElRango()
    {
        int casosFavorables = 0;
        int puntuacionJugador = player.GetComponent<CardHand>().points;
        int cartasRepartidas = player.GetComponent<CardHand>().cards.Count + 1;
        int[] cartasMesa = new int[cartasRepartidas];

        for (int i = 0; i < cartasRepartidas - 2; i++)
        {
            cartasMesa[i] = player.GetComponent<CardHand>().cards[i].GetComponent<CardModel>().value;
        }

        cartasMesa[cartasRepartidas - 1] = player.GetComponent<CardHand>().cards[1].GetComponent<CardModel>().value;
        int cartaMinima = 17 - puntuacionJugador;
        int cartaMaxima = 21 - puntuacionJugador;

        if (cartaMinima <= 0)
        {
            cartaMinima = 1;
        }

        if (cartaMaxima <= 0)
        {

            return 0;
        }

        if (cartaMinima >= 11)
        {
            return 100;
        }

        for (int i = cartaMinima; i < cartaMaxima + 1; i++)
        {
            int contador = 0;
            for (int j = 0; j < cartasMesa.Length; j++)
            {
                if (i == cartasMesa[j])
                {
                    contador++;
                }
            }
            if (i != 10)
            {
                casosFavorables += (4 - contador);
            }
            else
            {
                casosFavorables += (16 - contador);
            }
        }


        float probabilidad = (float)casosFavorables / 49;
        probabilidad *= 100;
        return probabilidad;
    }
}
