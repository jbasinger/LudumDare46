using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class EventLogEntry : MonoBehaviour
{

  TextMeshProUGUI entryText;

  void Awake()
  {
    entryText = GetComponent<TextMeshProUGUI>();
  }
  
  public void SetEntry(string evt)
  {
    entryText.text = evt;
  }

}
