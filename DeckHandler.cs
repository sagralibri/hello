using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class DeckHandler : MonoBehaviour
{
    private manager manager;
    int partitionLinesDeck = manager.drawnMax;
    public GameObject empty;
    public Technique Knives;
    public Technique TwinKnives;
    public Technique WorldEnd;
    public Technique FuckYou;
    public GameObject cardPrefab;
    public GameObject treasurePrefab;
    public Treasure testTreasure;
    int partitionLinesTreasure = manager.treasureMax;
    List<GameObject> removing = new List<GameObject>();
    // how tf do i set these automatically
    int topDeckBound = 550;
    int bottomDeckBound = -670;
    public static List<GameObject> partitionsDeck = new List<GameObject>();
    public static List<GameObject> partitionsTreasure = new List<GameObject>();

    int normalizex = 1280;
    int normalizey = 720;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("started");
        CreatePartitionObjects();
        manager.drawnCards.Add(Knives);
        manager.drawnCards.Add(WorldEnd);
        manager.drawnCards.Add(TwinKnives);
        manager.drawnCards.Add(WorldEnd);
        manager.drawnCards.Add(TwinKnives);
        manager.drainableDeck.Add(WorldEnd);
        manager.drainableDeck.Add(FuckYou);
        manager.treasures.Add(testTreasure);
        manager.treasures.Add(testTreasure);
        manager.treasures.Add(testTreasure);
        manager.treasures.Add(testTreasure);
        manager.treasures.Add(testTreasure);
        SetToIndexDeck();


        if (manager.DiscardSelected == null)
            manager.DiscardSelected = new UnityEvent();
        manager.DiscardSelected.AddListener(DiscardSelected);

        if (manager.NewTreasure == null)
            manager.NewTreasure = new UnityEvent();
        manager.NewTreasure.AddListener(RefreshCards);

        manager.NewTreasure.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if(partitionsDeck.Count != partitionLinesDeck)
        {
        CreatePartitionObjects();
        SetToIndexDeck();
        }
    }

    void DiscardSelected()
    {
        if(manager.drawnCards.Count > 1 && manager.discards > 0)
        {
        int selected = -1;
        int i = 0;
        foreach (GameObject drawnCard in manager.drawnCardObjects)
        {
            if (drawnCard.GetComponent<Knives>().selected == true)
            {
                selected = i;
            }
            i++;
        }
        if (selected != -1)
        {
        manager.discards -= 1;
        manager.drawnCards.RemoveAt(selected);
        Debug.Log("Removal Successful");
        DrawRandCard();
        Debug.Log("Drawing Successful");
        RefreshCards();
        Debug.Log("Refreshing Successful");
        manager.UpdateDiscardCounter.Invoke();
        }
        }
        else
        {
            Debug.Log("You cannot discard your last card, or not enough discards.");
        }
    }

    public void RefreshCards()
    {
        SetToIndexDeck();
        CreatePartitionObjects();
        Debug.Log("RefreshCards functional");
    }

    public void SetToIndexDeck()
    {
        int i = 0;
        foreach (GameObject objeect in manager.drawnCardObjects)
        {
            removing.Add(objeect);
        }
        foreach (GameObject objeect in manager.drawnTreasureObjects)
        {
            removing.Add(objeect);
        }
        for (int index = 0; index < removing.Count; index++)
        {
            manager.drawnCardObjects.Remove(removing[index]);
            Destroy(removing[index]);
        }
        foreach (Technique cardd in manager.drawnCards)
        {
            GameObject currentpartition = partitionsDeck[i];
            Vector2 pos = new Vector3(-normalizex, -normalizey, 0) + currentpartition.transform.position;
            GameObject card = Instantiate(cardPrefab, pos, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            card.GetComponent<Knives>().usedTechnique = cardd;
            manager.drawnCardObjects.Add(card);
            i++;
        }
        i = 0;
        foreach (Treasure treasure in manager.treasures)
        {
            GameObject currentpartition = partitionsTreasure[i];
            Vector2 pos = new Vector3(-normalizex, -normalizey, 0) + currentpartition.transform.position;
            GameObject newTreasure = Instantiate(treasurePrefab, pos, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            newTreasure.GetComponent<TreasureScript>().usedTreasure = treasure;
            manager.drawnTreasureObjects.Add(newTreasure);
            i++;
        }


        Debug.Log("SetToIndexDeck functional");
    }



    public void CreatePartitionObjects()
    {
        int modBoundRight = -210;
        int modBoundLeft = -900;
        int partitionLinesDeck = manager.drawnMax;
        int posy = -(topDeckBound - ((bottomDeckBound - topDeckBound)/2));
        foreach (GameObject partition in partitionsDeck)
        {
            removing.Add(partition);
        }
        foreach (GameObject partition in partitionsTreasure)
        {
            removing.Add(partition);
        }
        for (int index = 0; index < removing.Count; index++)
        {
            partitionsDeck.Remove(removing[index]);
            Destroy(removing[index]);
        }
        for (int i = 1; i <= partitionLinesDeck; i++)
        {
            Debug.Log(((float)i-(float)1) / ((float)partitionLinesDeck-(float)1));
            Debug.Log(partitionLinesDeck);

            float posx = (float)modBoundLeft + (((float)modBoundRight - (float)modBoundLeft)*(((float)i-(float)1) / ((float)partitionLinesDeck-(float)1)));
            GameObject clone = Instantiate(empty, new Vector2(posx+normalizex, -580+normalizey), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            partitionsDeck.Add(clone);
        }
        modBoundRight = 1156;
        modBoundLeft = 431;
        for (int i = 1; i <= partitionLinesTreasure; i++)
        {
            Debug.Log(((float)i-(float)1) / ((float)partitionLinesTreasure-(float)1));
            Debug.Log(partitionLinesTreasure);

            float posx = (float)modBoundLeft + (((float)modBoundRight - (float)modBoundLeft)*(((float)i-(float)1) / ((float)partitionLinesTreasure-(float)1)));
            GameObject clone = Instantiate(empty, new Vector2(posx+normalizex, -580+normalizey), Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
            partitionsTreasure.Add(clone);
        }
        Debug.Log("CreatePartitionObjects functional");
    }


    public void DrawCardfromDeck(int drawncard)
    {
        if (manager.drainableDeck.Count > 0 && manager.drawnCards.Count != manager.drawnMax)
        {
        Technique card = manager.drainableDeck[drawncard];
        manager.drainableDeck.Remove(card);
        manager.drawnCards.Add(card);
        }
    }

    public void DrawAll()
    {
        while (manager.drainableDeck.Count > 0 && manager.drawnCards.Count != manager.drawnMax)
        {
            DrawRandCard();
        }
    }

    public void DrawRandCard()
    {
        if (manager.drainableDeck.Count > 0 && manager.drawnCards.Count != manager.drawnMax)
        {
        int random = UnityEngine.Random.Range(0, manager.drainableDeck.Count);
        DrawCardfromDeck(random);
        }
    
    }
}
