using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEditor.iOS;

public class Cell : MonoBehaviour
{
    public RectTransform panelTransform;  
    public GameObject frontSide;         
    public GameObject backSide;          
    public float flipDuration = 0.5f;
    public TextMeshProUGUI marker;
    private bool isFlipped = false;
    private bool isFlipping = false;
    public string cellID;
    
    public void OnPanelClick()
    {
        if (!IsInvoking("FlipPanelCoroutine"))
        {
            StartCoroutine(FlipPanelCoroutine(isFlipped ? 0 : 180, !isFlipped, () =>
            {
                GamePlayManager.Instance.CurrentMove(this); 
            }));
        }
    }


    public void Initialise(string id)
    {
        marker.text = id;
        cellID = id;
    }
    

    public void Reset()
    {
        if (!IsInvoking("FlipPanelCoroutine"))
        {
            StartCoroutine(FlipPanelCoroutine(0, false,null)); // Reset to front side
        }
    }

    int counter = 0;
    // Generic function for flipping or resetting the panel
    private IEnumerator FlipPanelCoroutine(float targetRotation, bool flipState, System.Action onComplete)
    {
        GetComponent<Button>().interactable = !flipState;
        float elapsedTime = 0f;
        Quaternion startRotation = panelTransform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, targetRotation, 0);

        while (elapsedTime < flipDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsedTime / flipDuration);
            panelTransform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        panelTransform.rotation = endRotation;

        // Toggle visibility based on the target flip state
        frontSide.SetActive(!flipState);
        backSide.SetActive(flipState);
        isFlipped = flipState;
        onComplete?.Invoke();
    }
}