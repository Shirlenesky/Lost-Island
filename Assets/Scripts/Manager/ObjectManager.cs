using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private Dictionary<ItemName, bool> itemAvailableDict = new Dictionary<ItemName, bool>();
    private Dictionary<string, bool> interactiveStateDict = new Dictionary<string, bool>();


    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.UpdateUIEvent += OnUpdateUIEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.UpdateUIEvent -= OnUpdateUIEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    
    private void OnBeforeSceneUnloadEvent()
    {
        foreach (var item in FindObjectsOfType<Item>())
        {
            if (!itemAvailableDict.ContainsKey(item.ItemName))
            {
                itemAvailableDict.Add(item.ItemName, true);
            }
        }

        foreach (var item in FindObjectsOfType<Interactive>())
        {
            if(interactiveStateDict.ContainsKey(item.name))
            {
                interactiveStateDict[item.name] = item.isDone;
            }
            else
            {
                interactiveStateDict.Add(item.name, item.isDone);
            }
        }
    }
    private void OnAfterSceneLoadedEvent()
    {
        foreach(var item in FindObjectsOfType<Item>())
        {
            if(!itemAvailableDict.ContainsKey(item.ItemName))
            {
                itemAvailableDict.Add(item.ItemName, true);
            }
            else
            {
                item.gameObject.SetActive(itemAvailableDict[item.ItemName]);
            }
        }

        foreach (var item in FindObjectsOfType<Interactive>())
        {
            if (interactiveStateDict.ContainsKey(item.name))
            {
                item.isDone = interactiveStateDict[item.name];
            }
            else
            {
                interactiveStateDict.Add(item.name, item.isDone);
            }
        }
    }
    private void OnStartNewGameEvent(int gameWeek)
    {
        itemAvailableDict.Clear();
        interactiveStateDict.Clear();
    }



    private void OnUpdateUIEvent(ItemDetails itemDetails, int arg2)
    {
        if(itemDetails != null)
        {
            itemAvailableDict[itemDetails.itemName] = false;
        }
    }
}
