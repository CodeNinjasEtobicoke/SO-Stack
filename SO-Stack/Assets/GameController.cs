using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("Cube Object")]
    public GameObject currentCube;
    [Header("Last Cube Object")]
    public GameObject lastCube;
    [Header("Text Object")]
    public Text text;
    [Header("Current Level")]
    public int Level;
    [Header("Boolean")]
    public bool Done;

    // Start is called before the first frame update
    void Start()
    {
        Newblock();
    }

    // Update is called once per frame
    void Update()
    {
        if (Done)
        {
            return;
        }

        var time = Mathf.Abs(Time.realtimeSinceStartup % 2f - 1f);
        var pos1 = lastCube.transform.position + Vector3.up * 10f;
        var pos2 = pos1 + ((Level % 2 == 0) ? Vector3.left : Vector3.forward) * 120;
       
        if(Level % 2 == 0)
        {
            currentCube.transform.position = Vector3.Lerp(pos2, pos1, time);
        }
        else
        {
            currentCube.transform.position = Vector3.Lerp(pos1, pos2, time);
        }

        //SENA! Originally, in this code here you wrote Input.GetMouseButton(0);
        //The Input.GetMouseButton(0) gets called multiple times as long as you hold the mouse click (which in this case is like 3-4 times when you just click it normally...)
        //The Input.GetMouseButtonDown(0) in the other hand only gets called 'Once' when you click the mouse regardless of how long you hold it for.
        //So the problem was, instead of calling the NewBlock() only once when you click, the function gets called 3 times in a single click and reduces the next block spawn 3-times into the negative...
        //At which point the 'GameOver' condition gets triggered.
        if (Input.GetMouseButtonDown(0))
        {
            Newblock();
        }
    }

    void Newblock()
    {
        if (lastCube != null)
        {
            currentCube.transform.position = new Vector3(
                Mathf.Round(currentCube.transform.position.x),
                currentCube.transform.position.y,
                Mathf.Round(currentCube.transform.position.z)
                );

            currentCube.transform.localScale = new Vector3(
                lastCube.transform.localScale.x - Mathf.Abs(currentCube.transform.position.x - lastCube.transform.position.x),
                lastCube.transform.localScale.y,
                lastCube.transform.localScale.z - Mathf.Abs(currentCube.transform.position.z - lastCube.transform.position.z)
                );

            currentCube.transform.position = Vector3.Lerp(currentCube.transform.position, lastCube.transform.position, 0.5f) + Vector3.up * 5f;

            //This is the game over sequence
            if (currentCube.transform.localScale.x <= 0f || currentCube.transform.localScale.z <= 0f)
            {
                Done = true;
                text.gameObject.SetActive(true);
                text.text = "Final Score: " + Level;
                StartCoroutine(X());
                return;
            }
        }

        lastCube = currentCube;
        currentCube = Instantiate(lastCube);
        currentCube.name = Level + "";
        currentCube.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((Level / 100f) % 1f, 1f, 1f));
        Level++;
        Camera.main.transform.position = currentCube.transform.position + new Vector3(100, 100, 100);
        Camera.main.transform.LookAt(currentCube.transform.position);
    }

    IEnumerator X()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("SampleScene");
    }
}
